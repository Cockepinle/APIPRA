using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly PostgresContext _context;
        private readonly ILogger<TestsController> _logger;

        public TestsController(PostgresContext context, ILogger<TestsController> logger)
        {
            _context = context;
            _logger = logger;
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
                            _logger.LogError(ex, "JSON parse error");
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
                _logger.LogError(ex, "Error getting tests");
                return StatusCode(500, "Internal server error");
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
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error parsing metadata");
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
                _logger.LogError(ex, "Error getting test");
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
                if (resultDto == null || resultDto.Answers == null)
                {
                    _logger.LogError("Invalid request data");
                    return BadRequest("Invalid request data");
                }

                var userId = int.Parse(User.FindFirst("UserId")?.Value);

                // Проверка существования теста
                var testExists = await _context.Languagetests.AnyAsync(t => t.Id == resultDto.TestId);
                if (!testExists) return NotFound("Test not found");

                // Получаем вопросы
                var questions = await _context.TestQuestions
                    .Where(q => q.TestId == resultDto.TestId)
                    .ToListAsync();

                // Подсчет баллов
                int score = resultDto.Answers
                    .Count(a => questions.Any(q => q.Id == a.QuestionId && q.Answer == a.Answer));

                // Сохранение результата
                var result = new Usertestresult
                {
                    UserId = userId,
                    TestId = resultDto.TestId,
                    Score = score,
                    CompletedAt = DateTime.UtcNow
                };

                _context.Usertestresults.Add(result);
                await _context.SaveChangesAsync();

                return Ok(new TestResultResponseDto
                {
                    Id = result.Id,
                    Score = score,
                    TotalQuestions = questions.Count,
                    CompletedAt = result.CompletedAt.Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving test result");
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: api/tests/{id}/questions
        [HttpGet("{id}/questions")]
        public async Task<ActionResult<IEnumerable<TestQuestion>>> GetTestQuestions(int id)
        {
            return await _context.TestQuestions
                .Where(q => q.TestId == id)
                .ToListAsync();
        }

        // GET: api/tests/user/results
        [Authorize]
        [HttpGet("user/results")]
        public async Task<ActionResult<IEnumerable<TestResultDetailDto>>> GetUserResults()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value);

                return await _context.Usertestresults
                    .AsNoTracking()
                    .Where(r => r.UserId == userId)
                    .Include(r => r.Test)
                    .Select(r => new TestResultDetailDto
                    {
                        Id = r.Id,
                        TestId = r.TestId ?? 0,  // Если null, подставит 0                        TestName = r.Test != null ? r.Test.Name : "Unknown Test",
                        Score = r.Score,
                        CompletedAt = r.CompletedAt ?? DateTime.MinValue
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user results");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{id}/info")]
        [AllowAnonymous]
        public async Task<ActionResult<TestInfo>> GetTestInfo(int id)
        {
            var test = await _context.Languagetests
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (test == null)
                return NotFound();

            return new TestInfo
            {
                Id = test.Id,
                Name = test.Name
            };
        }
        [Authorize]
        [HttpGet("{testId}/results/{userId}")]
        public async Task<ActionResult<TestResultDetailsDto>> GetTestResultDetails(int testId, int userId)
        {
            var result = await _context.Usertestresults
                .Include(r => r.Test)
                .FirstOrDefaultAsync(r => r.TestId == testId && r.UserId == userId);

            if (result == null)
                return NotFound();

            var questions = await _context.TestQuestions
                .Where(q => q.TestId == testId)
                .ToListAsync();

            return new TestResultDetailsDto
            {
                TestId = testId,
                TestName = result.Test?.Name ?? "Unknown Test",
                Score = result.Score,
                TotalQuestions = questions.Count,
                CompletedAt = result.CompletedAt ?? DateTime.MinValue,
                Questions = questions.Select(q => new QuestionResultDto
                {
                    QuestionId = q.Id,
                    QuestionText = q.Question,
                    CorrectAnswer = q.Answer
                }).ToList()
            };
        }
    }
}