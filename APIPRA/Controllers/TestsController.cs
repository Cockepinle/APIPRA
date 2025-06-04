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
            var tests = await _context.Languagetests
                .Include(t => t.Testimages)
                .ToListAsync();

            return tests.Select(t => new TestDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Testimages.FirstOrDefault()?.Description ?? string.Empty,
                ImageUrl = t.Testimages.FirstOrDefault()?.ImageUrl,
                TestType = t.Testimages.FirstOrDefault() != null
                    ? JsonSerializer.Deserialize<TestMetadata>(t.Testimages.First().Description)?.TestType
                    : "standard"
            }).ToList();
        }

        // GET: api/tests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestDetailDto>> GetTest(int id)
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
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Options = q.Options,
                    CorrectAnswer = q.CorrectAnswer
                }).ToList(),
                TestType = metadata.TestType
            };
        }

        // POST: api/tests/results
        [Authorize]
        [HttpPost("results")]
        public async Task<ActionResult<TestResultResponseDto>> SubmitTestResult([FromBody] TestResultDto resultDto)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            var result = new Usertestresult
            {
                UserId = userId,
                TestId = resultDto.TestId,
                Score = resultDto.Score,
                CompletedAt = DateTime.UtcNow
            };

            _context.Usertestresults.Add(result);
            await _context.SaveChangesAsync();

            // Получаем название теста
            var testName = await _context.Languagetests
                .Where(t => t.Id == resultDto.TestId)
                .Select(t => t.Name)
                .FirstOrDefaultAsync();

            return new TestResultResponseDto
            {
                Id = result.Id,
                TestName = testName ?? "Unknown Test",
                Score = result.Score,
                CompletedAt = result.CompletedAt
            };
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