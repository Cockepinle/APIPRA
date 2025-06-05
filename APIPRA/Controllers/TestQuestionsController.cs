using APIPRA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/tests/{testId}/questions")]
    public class TestQuestionsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public TestQuestionsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/tests/{testId}/questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestQuestion>>> GetQuestions(int testId)
        {
            try
            {
                var questions = await _context.TestQuestions
                    .Where(q => q.TestId == testId)
                    .ToListAsync();

                return Ok(questions);
            }
            catch (Exception ex)
            {
                // Логировать ex
                return StatusCode(500, "Ошибка сервера при получении вопросов");
            }
        }


        // POST: api/tests/{testId}/questions
        [HttpPost]
        public async Task<ActionResult<TestQuestion>> PostQuestion(int testId, TestQuestion question)
        {
            question.TestId = testId;

            _context.TestQuestions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestionById), new { testId = testId, id = question.Id }, question);
        }

        // GET: api/tests/{testId}/questions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TestQuestion>> GetQuestionById(int testId, int id)
        {
            var question = await _context.TestQuestions.FindAsync(id);

            if (question == null || question.TestId != testId)
            {
                return NotFound();
            }

            return Ok(question);
        }


        // PUT: api/tests/{testId}/questions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int testId, int id, TestQuestion updatedQuestion)
        {
            if (id != updatedQuestion.Id || testId != updatedQuestion.TestId)
            {
                return BadRequest();
            }

            _context.Entry(updatedQuestion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TestQuestions.Any(q => q.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/tests/{testId}/questions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int testId, int id)
        {
            var question = await _context.TestQuestions.FindAsync(id);
            if (question == null || question.TestId != testId)
            {
                return NotFound();
            }

            _context.TestQuestions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
