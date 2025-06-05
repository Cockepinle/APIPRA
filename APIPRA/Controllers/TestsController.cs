using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

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
        // APIPRA/Controllers/TestsController.cs

        [HttpGet("{id}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetTestQuestions(int id)
        {
            try
            {
                // Вариант 1: Получение вопросов из таблицы TestQuestions
                var questions = await _context.TestQuestions
                    .Where(q => q.TestId == id)
                    .Select(q => new QuestionDto
                    {
                        Id = q.Id,
                        Question = q.Question,
                        QuestionType = q.QuestionType,
                        Answer = q.Answer,
                        Options = q.Options ?? new List<string>()
                    })
                    .ToListAsync();

                if (!questions.Any())
                    return NotFound();

                return Ok(questions);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting test questions");
                return StatusCode(500, "Internal server error");
            }
        }

        // Метод для сохранения результатов теста
        [HttpPost("{id}/results")]
        [Authorize]
        public async Task<ActionResult<TestResultDto>> SaveTestResult(int id, [FromBody] TestResultRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                // Проверяем существование теста
                var test = await _context.Languagetests.FindAsync(id);
                if (test == null)
                    return NotFound();

                // Сохраняем основной результат
                var result = new Usertestresult
                {
                    UserId = int.Parse(userId),
                    TestId = id,
                    Score = request.Score,
                    CompletedAt = DateTime.UtcNow
                };

                _context.Usertestresults.Add(result);
                await _context.SaveChangesAsync();

                // Сохраняем ответы на вопросы
                foreach (var answer in request.Answers)
                {
                    var userAnswer = new UserAnswer
                    {
                        UserTestResultId = result.Id,
                        QuestionId = answer.QuestionId,
                        UserAnswerText = answer.UserAnswer,
                        IsCorrect = answer.IsCorrect
                    };
                    _context.UserAnswers.Add(userAnswer);
                }

                await _context.SaveChangesAsync();

                return Ok(new TestResultDto
                {
                    TestId = id,
                    Score = request.Score,
                    TotalQuestions = request.Answers.Count,
                    CorrectAnswers = request.Answers.Count(a => a.IsCorrect)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving test result");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}