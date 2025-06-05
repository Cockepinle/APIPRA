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
        // POST: api/tests/results
        [Authorize]
        [HttpPost("results")]
        public async Task<ActionResult<TestResultResponseDto>> SubmitTestResult([FromBody] TestResultDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0) return Unauthorized();

                // Проверяем существование теста и вопросов
                var test = await _context.Languagetests
                    .Include(t => t.TestQuestions)
                    .FirstOrDefaultAsync(t => t.Id == request.TestId);

                if (test == null) return NotFound("Test not found");

                // Создаем запись о результате теста
                var testResult = new Usertestresult
                {
                    UserId = userId,
                    TestId = request.TestId,
                    Score = 0,
                    CompletedAt = DateTime.UtcNow
                };

                _context.Usertestresults.Add(testResult);
                await _context.SaveChangesAsync();

                // Обрабатываем каждый ответ
                foreach (var answer in request.Answers)
                {
                    var question = test.TestQuestions.FirstOrDefault(q => q.Id == answer.QuestionId);
                    if (question == null) continue;

                    bool isCorrect = question.Answer.Equals(answer.Answer, StringComparison.OrdinalIgnoreCase);

                    if (isCorrect) testResult.Score++;

                    _context.UserAnswers.Add(new UserAnswer
                    {
                        UserTestResultId = testResult.Id,
                        QuestionId = answer.QuestionId,
                        UserAnswerText = answer.Answer,
                        IsCorrect = isCorrect
                    });
                }

                await _context.SaveChangesAsync();

                return Ok(new TestResultResponseDto
                {
                    Id = testResult.Id,
                    Score = testResult.Score,
                    TotalQuestions = test.TestQuestions.Count,
                    CompletedAt = testResult.CompletedAt.Value
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting test results");
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
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (userId == 0) return Unauthorized();

                var results = await _context.Usertestresults
                    .Where(r => r.UserId == userId && r.TestId != null)
                    .Include(r => r.Test)
                    .Select(r => new TestResultDetailDto
                    {
                        Id = r.Id,
                        TestId = r.TestId,
                        TestName = r.Test != null ? r.Test.Name : "Unknown Test",
                        Score = r.Score,
                        CompletedAt = r.CompletedAt ?? DateTime.UtcNow
                    })
                    .ToListAsync();

                return Ok(results);
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
        [Authorize]
        [HttpGet("results/{resultId}")]
        public async Task<ActionResult<TestResultDetailsDto>> GetTestResultDetails(int resultId)
        {
            var result = await _context.Usertestresults
                .Include(r => r.Test)
                .Include(r => r.UserAnswers)
                    .ThenInclude(ua => ua.Question)
                .FirstOrDefaultAsync(r => r.Id == resultId);

            if (result == null) return NotFound();

            return new TestResultDetailsDto
            {
                Id = result.Id,
                TestId = result.TestId,
                TestName = result.Test?.Name ?? "Unknown Test",
                Score = result.Score,
                TotalQuestions = result.UserAnswers.Count,
                CompletedAt = result.CompletedAt ?? DateTime.MinValue,
                Questions = result.UserAnswers.Select(ua => new QuestionResultDto
                {
                    QuestionId = ua.QuestionId,
                    QuestionText = ua.Question.Question,
                    CorrectAnswer = ua.Question.Answer,
                    UserAnswer = ua.UserAnswerText,
                    IsCorrect = ua.IsCorrect
                }).ToList()
            };
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
       /* [Authorize]
        [HttpPost("Submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([FromBody] TestResultDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state: {Errors}",
                        string.Join(", ", ModelState.Values.SelectMany(v => v.Errors)));
                    return BadRequest(ModelState);
                }

                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (userId == 0) return Unauthorized();

                // Проверка существования теста и вопросов
                var test = await _context.Languagetests
                    .Include(t => t.TestQuestions)
                    .FirstOrDefaultAsync(t => t.Id == request.TestId);

                if (test == null) return NotFound("Тест не найден");

                // Подсчёт баллов
                int score = 0;
                foreach (var answer in request.Answers)
                {
                    var question = test.TestQuestions.FirstOrDefault(q => q.Id == answer.QuestionId);
                    if (question != null && question.Answer.Equals(answer.Answer, StringComparison.OrdinalIgnoreCase))
                    {
                        score++;
                    }
                }

                // Сохранение результата
                var result = new Usertestresult
                {
                    UserId = userId,
                    TestId = request.TestId,
                    Score = score,
                    CompletedAt = DateTime.UtcNow
                };

                _context.Usertestresults.Add(result);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    testId = request.TestId,
                    score,
                    totalQuestions = test.TestQuestions.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении теста");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
       */
    }
}