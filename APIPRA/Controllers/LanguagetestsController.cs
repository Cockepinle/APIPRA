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
        public async Task<ActionResult<IEnumerable<LanguageTestWithQuestions>>> GetQuizTests()
        {
            try
            {
                var testsWithQuestions = await _context.Languagetests
                    .Where(t => t.Type == "quiz")
                    .Include(t => t.TestQuestions)
                    .Include(t => t.Testimages)
                    .Select(t => new LanguageTestWithQuestions
                    {
                        Test = t,
                        Questions = t.TestQuestions.Select(q => new TestQuestion
                        {
                            Id = q.Id,
                            TestId = q.TestId,
                            Question = q.Question,
                            Answer = q.Answer,
                            QuestionType = q.QuestionType
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