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
                    if (!string.IsNullOrEmpty(image.Description))
                    {
                        try
                        {
                            var metadata = JsonSerializer.Deserialize<TestMetadata>(image.Description);
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
                        Description = image.Description ?? string.Empty,
                        ImageUrl = image.ImageUrl,
                        TestType = testType
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex.Message); // для отладки
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

                if (test == null || test.Testimages.FirstOrDefault() == null)
                    return NotFound();

                var metadata = JsonSerializer.Deserialize<TestMetadata>(test.Testimages.First().Description);
                if (metadata == null)
                    return BadRequest("Invalid test format");

                return new TestDetailDto
                {
                    Id = test.Id,
                    Name = test.Name,
                    Description = test.Testimages.First().Description,
                    ImageUrl = test.Testimages.First().ImageUrl,
                    Questions = metadata.Questions.Select(q => new QuestionDto
                    {
                        Question = q.Question,
                        QuestionType = q.QuestionType,
                        Answer = q.Answer
                    }).ToList(),
                    TestType = metadata.TestType
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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