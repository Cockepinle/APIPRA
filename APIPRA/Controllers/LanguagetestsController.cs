using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;
using System.Text.Json;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageTestsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public LanguageTestsController(PostgresContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Languagetest>>> GetLanguageTests()
        {
            var tests = await _context.Languagetests
                .Include(t => t.TestQuestions)
                .Include(t => t.Testimages)
                .ToListAsync();

            return Ok(tests);
        }


[HttpGet("quiz")]
    public async Task<ActionResult<IEnumerable<object>>> GetQuizTests()
    {
        var tests = await _context.Languagetests
            .Where(t => t.Type == "quiz")
            .Include(t => t.Testimages)
            .ToListAsync();

        var results = new List<object>();

        foreach (var test in tests)
        {
            var questions = new List<object>();

            foreach (var img in test.Testimages)
            {
                if (string.IsNullOrEmpty(img.Metadata))
                    continue;

                try
                {
                    using JsonDocument doc = JsonDocument.Parse(img.Metadata);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("Questions", out JsonElement questionsElement))
                    {
                        foreach (var questionElem in questionsElement.EnumerateArray())
                        {
                            var question = new
                            {
                                Question = questionElem.GetProperty("Question").GetString(),
                                Answer = questionElem.GetProperty("Answer").GetString(),
                                Options = questionElem.GetProperty("Options").EnumerateArray().Select(o => o.GetString()).ToList(),
                                QuestionType = questionElem.TryGetProperty("QuestionType", out var qt) ? qt.GetString() : null
                            };
                            questions.Add(question);
                        }
                    }
                }
                catch (JsonException)
                {
                    // Логировать ошибку парсинга, если нужно
                    continue;
                }
            }

            results.Add(new
            {
                TestId = test.Id,
                TestName = test.Name,
                TestType = test.Type,
                Questions = questions,
                Images = test.Testimages.Select(i => i.ImageUrl).ToList()
            });
        }

        return Ok(results);
    }


    [HttpGet("{id}")]
        public async Task<ActionResult<LanguageTestWithQuestions>> GetLanguageTest(int id)
        {
            var test = await _context.Languagetests
                .Include(t => t.TestQuestions)
                .Include(t => t.Testimages)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (test == null)
                return NotFound();

            var result = new LanguageTestWithQuestions
            {
                Test = test,
                Questions = test.TestQuestions.ToList(),
                Images = test.Testimages.ToList()
            };

            return result;
        }

        [HttpGet("{testId}/results/{userId}")]
        public async Task<ActionResult<Usertestresult>> GetUserTestResult(int testId, int userId)
        {
            var result = await _context.Usertestresults
                .FirstOrDefaultAsync(r => r.TestId == testId && r.UserId == userId);

            if (result == null)
                return NotFound();

            return result;
        }

        [HttpPost("results")]
        public async Task<ActionResult<Usertestresult>> PostTestResult(Usertestresult result)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // При желании проверить существование теста и пользователя

            _context.Usertestresults.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserTestResult),
                new { testId = result.TestId, userId = result.UserId },
                result);
        }
    }

    public class LanguageTestWithQuestions
    {
        public Languagetest Test { get; set; }
        public List<TestQuestion> Questions { get; set; } = new List<TestQuestion>();
        public List<Testimage> Images { get; set; } = new List<Testimage>();
    }


}