using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;
using System.Text.Json;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public TestsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/tests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestDto>>> GetTests()
        {
            try
            {
                var tests = await _context.Languagetests
                    .Include(t => t.Testimages)
                    .AsNoTracking()
                    .ToListAsync();

                var result = new List<TestDto>();
                foreach (var test in tests)
                {
                    var image = test.Testimages?.FirstOrDefault();
                    if (image == null) continue;

                    string testType = "standard";
                    if (!string.IsNullOrEmpty(image.Metadata))
                    {
                        try
                        {
                            var metadata = JsonSerializer.Deserialize<TestMetadata>(image.Metadata);
                            testType = metadata?.TestType ?? "standard";
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON parse error: {ex.Message}");
                            testType = "standard";
                        }
                    }

                    result.Add(new TestDto
                    {
                        Id = test.Id,
                        Name = test.Name,
                        Description = image.Metadata ?? string.Empty,
                        ImageUrl = image.ImageUrl,
                        TestType = testType
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/tests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestDetailDto>> GetTest(int id)
        {
            try
            {
                var test = await _context.Languagetests
                    .Include(t => t.Testimages)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (test == null)
                    return NotFound();

                var testImage = test.Testimages?.FirstOrDefault();
                if (testImage == null)
                    return NotFound();

                var questions = new List<QuestionDto>();
                string testType = "standard";
                string description = "Нет описания";

                try
                {
                    if (!string.IsNullOrEmpty(testImage.Metadata))
                    {
                        var metadata = JsonSerializer.Deserialize<TestMetadata>(testImage.Metadata);
                        if (metadata != null)
                        {
                            testType = metadata.TestType ?? "standard";
                            questions = metadata.Questions?.Select(q => new QuestionDto
                            {
                                Question = q.Question,
                                QuestionType = q.QuestionType,
                                Answer = q.Answer
                            }).ToList() ?? new List<QuestionDto>();

                            description = metadata.Questions?.FirstOrDefault()?.Question ?? "Нет описания";
                        }
                    }
                }
                catch (JsonException)
                {
                    // Если metadata не JSON, оставляем дефолтные значения
                }

                return new TestDetailDto
                {
                    Id = test.Id,
                    Name = test.Name,
                    Description = description,
                    ImageUrl = testImage.ImageUrl,
                    Questions = questions,
                    TestType = testType
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTest: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/tests/results
        [Authorize]
        [HttpPost("results")]
        public async Task<ActionResult<TestResultResponseDto>> SubmitTestResult([FromBody] TestResultDto resultDto)
        {
            try
            {
                var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

                // Рассчитываем score на основе ответов
                var correctAnswers = await _context.TestQuestions
                    .Where(q => q.TestId == resultDto.TestId)
                    .ToListAsync();

                int score = 0;
                foreach (var userAnswer in resultDto.Answers)
                {
                    var question = correctAnswers.FirstOrDefault(q => q.Id == userAnswer.QuestionId);
                    if (question != null && question.Answer == userAnswer.Answer)
                    {
                        score++;
                    }
                }

                var result = new Usertestresult
                {
                    UserId = userId,
                    TestId = resultDto.TestId,
                    Score = score,
                    CompletedAt = DateTime.UtcNow
                };

                _context.Usertestresults.Add(result);
                await _context.SaveChangesAsync();

                var testName = await _context.Languagetests
                    .Where(t => t.Id == resultDto.TestId)
                    .Select(t => t.Name)
                    .FirstOrDefaultAsync();

                return Ok(new TestResultResponseDto
                {
                    Id = result.Id,
                    TestName = testName ?? "Unknown Test",
                    Score = score,
                    CompletedAt = result.CompletedAt
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/tests/results/5
        [Authorize]
        [HttpGet("results/{id}")]
        public async Task<ActionResult<TestResultDetailDto>> GetTestResult(int id)
        {
            var result = await _context.Usertestresults
                .Include(r => r.Test)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (result == null)
                return NotFound();

            return new TestResultDetailDto
            {
                Id = result.Id,
                TestName = result.Test?.Name ?? "Unknown Test",
                Score = result.Score,
                CompletedAt = result.CompletedAt
            };
        }

        // GET: api/tests/user/results
        [Authorize]
        [HttpGet("user/results")]
        public async Task<ActionResult<IEnumerable<TestResultDetailDto>>> GetUserResults()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            return await _context.Usertestresults
                .Where(r => r.UserId == userId)
                .Include(r => r.Test)
                .Select(r => new TestResultDetailDto
                {
                    Id = r.Id,
                    TestName = r.Test.Name,
                    Score = r.Score,
                    CompletedAt = r.CompletedAt
                })
                .ToListAsync();
        }
    }
}