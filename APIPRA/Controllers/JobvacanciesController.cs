using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIPRA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace APIPRA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class JobvacanciesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public JobvacanciesController(PostgresContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все вакансии
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Jobvacancy>))]
        public async Task<ActionResult<IEnumerable<Jobvacancy>>> GetJobVacancies()
        {
            return await _context.Jobvacancies.ToListAsync();
        }

        /// <summary>
        /// Получить вакансию по ID
        /// </summary>
        /// <param name="id">ID вакансии</param>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Jobvacancy))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Jobvacancy>> GetJobVacancy(int id)
        {
            var vacancy = await _context.Jobvacancies.FindAsync(id);

            if (vacancy == null)
                return NotFound();

            return vacancy;
        }

        /// <summary>
        /// Создать новую вакансию
        /// </summary>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Jobvacancy))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Jobvacancy>> CreateJobVacancy([FromBody] JobvacancyCreateDto dto)
        {
            var vacancy = new Jobvacancy
            {
                Title = dto.Title,
                Description = dto.Description,
                EmployerName = dto.EmployerName,
                Location = dto.Location,
                ContactInfo = dto.ContactInfo
            };

            _context.Jobvacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetJobVacancy),
                new { id = vacancy.Id },
                vacancy);
        }

        /// <summary>
        /// Обновить вакансию
        /// </summary>
        /// <param name="id">ID вакансии</param>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateJobVacancy(
            int id,
            [FromBody] JobvacancyUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var vacancy = await _context.Jobvacancies.FindAsync(id);
            if (vacancy == null)
                return NotFound();

            vacancy.Title = dto.Title;
            vacancy.Description = dto.Description;
            vacancy.EmployerName = dto.EmployerName;
            vacancy.Location = dto.Location;
            vacancy.ContactInfo = dto.ContactInfo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Удалить вакансию
        /// </summary>
        /// <param name="id">ID вакансии</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteJobVacancy(int id)
        {
            var vacancy = await _context.Jobvacancies.FindAsync(id);
            if (vacancy == null)
                return NotFound();

            _context.Jobvacancies.Remove(vacancy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // DTO для создания вакансии
  

    // DTO для обновления вакансии
  
}