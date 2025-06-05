using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;

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

        // GET: api/LanguageTests - все тесты
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Languagetest>>> GetLanguageTests()
        {
            return await _context.Languagetests.ToListAsync();
        }

        // GET: api/LanguageTests/quiz - только тесты типа quiz
        [HttpGet("quiz")]
        public async Task<ActionResult<IEnumerable<LanguageTestWithQuestions>>> GetQuizTests()
        {
            try
            {
                var testsWithQuestions = await _context.Languagetests
                    .Where(t => t.Type == "quiz")
                    .Include(t => t.TestQuestions)
                    .Select(t => new LanguageTestWithQuestions
                    {
                        Test = t,
                        Questions = t.TestQuestions.Select(q => new TestQuestion
                        {
                            Id = q.Id,
                            TestId = q.TestId,
                            Question = q.Question,
                            Answer = q.Answer,
                            QuestionType = q.QuestionType,
                            Options = q.Options ?? new List<string>() // Защита от null
                        }).ToList(),
                        Images = t.Testimages.ToList()
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return Ok(testsWithQuestions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    details = "Check if 'options' column exists in database"
                });
            }
        }

        // GET: api/LanguageTests/5 - конкретный тест с вопросами
        [HttpGet("{id}")]
        public async Task<ActionResult<LanguageTestWithQuestions>> GetLanguageTest(int id)
        {
            var test = await _context.Languagetests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            var questions = await _context.TestQuestions
                .Where(q => q.TestId == id)
                .ToListAsync();

            var result = new LanguageTestWithQuestions
            {
                Test = test,
                Questions = questions
            };

            return result;
        }

        // GET: api/LanguageTests/5/results - результаты пользователя по тесту
        [HttpGet("{testId}/results/{userId}")]
        public async Task<ActionResult<Usertestresult>> GetUserTestResult(int testId, int userId)
        {
            var result = await _context.Usertestresults
                .FirstOrDefaultAsync(r => r.TestId == testId && r.UserId == userId);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // POST: api/LanguageTests/results - сохранение результата теста
        [HttpPost("results")]
        public async Task<ActionResult<Usertestresult>> PostTestResult(Usertestresult result)
        {
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
        public List<TestQuestion> Questions { get; set; }
        public List<Testimage> Images { get; set; }
    }

}