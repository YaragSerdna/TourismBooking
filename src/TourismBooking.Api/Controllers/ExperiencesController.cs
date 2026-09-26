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
        /// <param name="dto">DTO con los datos de la experiencia a crear.</param>
        /// <returns>La experiencia creada.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExperienceDto dto)
        {
            var result = await _experienceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Obtiene la lista de experiencias turísticas activas.
        /// </summary>
        /// <returns>La lista de experiencias activas.</returns>
        [HttpGet]
        public async Task<IActionResult> GetActive()
        {
            var list = await _experienceService.GetActiveExperiencesAsync();
            return Ok(list);
        }

        /// <summary>
        /// Obtiene una experiencia turística por su ID.
        /// </summary>
        /// <param name="id">ID de la experiencia a obtener.</param>
        /// <returns>Los detalles de la experiencia.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _experienceService.GetByIdAsync(id);
            if (result == null) return NotFound(new { Message = $"No se encontró la experiencia con ID {id}." });
            return Ok(result);
        }

        /// <summary>
        /// Actualiza los detalles de una experiencia turística existente.
        /// </summary>
        /// <param name="id">ID de la experiencia a actualizar.</param>
        /// <param name="dto">DTO con los nuevos datos de la experiencia.</param>
        /// <returns>La experiencia actualizada.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExperienceDto dto)
        {
            var result = await _experienceService.UpdateAsync(id, dto);
            if (result == null) return NotFound(new { Message = $"No se encontró la experiencia con ID {id}." });
            return Ok(result);
        }

        /// <summary>
        /// Inactiva una experiencia turística existente.
        /// </summary>
        /// <param name="id">ID de la experiencia a inactivar.</param>
        /// <returns>Un mensaje indicando el resultado de la operación.</returns>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Inactivate(int id)
        {
            var success = await _experienceService.InactivateAsync(id);
            if (!success) return NotFound(new { Message = $"No se encontró la experiencia con ID {id}." });
            return NoContent();
        }
    }
}
