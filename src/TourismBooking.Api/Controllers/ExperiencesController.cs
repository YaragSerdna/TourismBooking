using Microsoft.AspNetCore.Mvc;
using TourismBooking.Application.DTOs;
using TourismBooking.Application.Interfaces;

namespace TourismBooking.Api.Controllers
{
    [ApiController]
    [Route("api/experiencias")]
    public class ExperiencesController : ControllerBase
    {
        private readonly IExperienceService _experienceService;

        public ExperiencesController(IExperienceService experienceService)
        {
            _experienceService = experienceService;
        }

        /// <summary>
        /// Crea una nueva experiencia turística.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExperienceDto dto)
        {
            var result = await _experienceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Consulta el listado de experiencias activas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActive()
        {
            var list = await _experienceService.GetActiveExperiencesAsync();
            return Ok(list);
        }

        /// <summary>
        /// Consulta el detalle de una experiencia por su ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _experienceService.GetByIdAsync(id);
            if (result == null) return NotFound(new { Message = $"No se encontró la experiencia con ID {id}." });
            return Ok(result);
        }

        /// <summary>
        /// Actualiza una experiencia turística existente.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExperienceDto dto)
        {
            var result = await _experienceService.UpdateAsync(id, dto);
            if (result == null) return NotFound(new { Message = $"No se encontró la experiencia con ID {id}." });
            return Ok(result);
        }

        /// <summary>
        /// Inactiva lógicamente una experiencia.
        /// </summary>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Inactivate(int id)
        {
            var success = await _experienceService.InactivateAsync(id);
            if (!success) return NotFound(new { Message = $"No se encontró la experiencia con ID {id}." });
            return NoContent();
        }
    }
}
