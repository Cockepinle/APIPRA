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
    public class HousingdocumentsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public HousingdocumentsController(PostgresContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все документы по жилью
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Housingdocument>))]
        public async Task<ActionResult<IEnumerable<Housingdocument>>> GetHousingDocuments()
        {
            return await _context.Housingdocuments.ToListAsync();
        }

        /// <summary>
        /// Получить документ по ID
        /// </summary>
        /// <param name="id">ID документа</param>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Housingdocument))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Housingdocument>> GetHousingDocument(int id)
        {
            var document = await _context.Housingdocuments.FindAsync(id);

            if (document == null)
                return NotFound();

            return document;
        }

        /// <summary>
        /// Создать новый документ
        /// </summary>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Housingdocument))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Housingdocument>> CreateHousingDocument([FromBody] HousingdocumentCreateDto dto)
        {
            var document = new Housingdocument
            {
                Name = dto.Name,
                FileUrl = dto.FileUrl
            };

            _context.Housingdocuments.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetHousingDocument),
                new { id = document.Id },
                document);
        }

        /// <summary>
        /// Обновить документ
        /// </summary>
        /// <param name="id">ID документа</param>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateHousingDocument(
            int id,
            [FromBody] HousingdocumentUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var document = await _context.Housingdocuments.FindAsync(id);
            if (document == null)
                return NotFound();

            document.Name = dto.Name;
            document.FileUrl = dto.FileUrl;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Удалить документ
        /// </summary>
        /// <param name="id">ID документа</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteHousingDocument(int id)
        {
            var document = await _context.Housingdocuments.FindAsync(id);
            if (document == null)
                return NotFound();

            _context.Housingdocuments.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // DTO для создания документа
   

    // DTO для обновления документа
  
}