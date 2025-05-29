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
    [Produces("application/json")] // Явно указываем тип ответа
    public class HousingarticletypesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public HousingarticletypesController(PostgresContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все типы жилищных статей
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Housingarticletype>))]
        public async Task<ActionResult<IEnumerable<Housingarticletype>>> Get()
        {
            return await _context.Housingarticletypes.ToListAsync();
        }

        /// <summary>
        /// Получить тип жилищной статьи по ID
        /// </summary>
        /// <param name="id">ID типа статьи</param>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Housingarticletype))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Housingarticletype>> GetById(int id)
        {
            var type = await _context.Housingarticletypes.FindAsync(id);
            if (type == null)
                return NotFound();

            return type;
        }

        /// <summary>
        /// Создать новый тип жилищной статьи
        /// </summary>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Housingarticletype))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Housingarticletype>> Create([FromBody] HousingarticletypeCreateDto dto)
        {
            var type = new Housingarticletype { Name = dto.Name };

            _context.Housingarticletypes.Add(type);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
        }

        /// <summary>
        /// Обновить тип жилищной статьи
        /// </summary>
        /// <param name="id">ID типа статьи</param>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] HousingarticletypeUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var type = await _context.Housingarticletypes.FindAsync(id);
            if (type == null)
                return NotFound();

            type.Name = dto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Удалить тип жилищной статьи
        /// </summary>
        /// <param name="id">ID типа статьи</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _context.Housingarticletypes.FindAsync(id);
            if (type == null)
                return NotFound();

            _context.Housingarticletypes.Remove(type);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

  
    // DTO для обновления
   
}