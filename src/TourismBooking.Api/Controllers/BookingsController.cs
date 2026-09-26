using Microsoft.AspNetCore.Mvc;
using TourismBooking.Application.DTOs;
using TourismBooking.Application.Interfaces;

namespace TourismBooking.Api.Controllers
{
    [ApiController]
    [Route("api/reservas")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Crea una nueva reserva validando capacidad y disponibilidad.
        /// </summary>
        /// <param name="dto">DTO con los datos de la reserva a crear.</param>
        /// <returns> La reserva creada.</returns>
     
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var result = await _bookingService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Obtiene los detalles de una reserva por su ID.
        /// </summary>
        /// <param name="id">ID de la reserva a obtener.</param>
        /// <returns>Los detalles de la reserva.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookingService.GetByIdAsync(id);
            if (result == null) return NotFound(new { Message = $"No se encontró la reserva con ID {id}." });
            return Ok(result);
        }

        /// <summary>
        /// Obtiene una lista de reservas filtradas por fecha, estado o experiencia.
        /// </summary>
        /// <param name="filter">DTO con los criterios de filtrado.</param>
        /// <returns>La lista de reservas que cumplen con los criterios de filtrado.</returns>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] BookingFilterDto filter)
        {
            var list = await _bookingService.GetFilteredAsync(filter);
            return Ok(list);
        }

        /// <summary>
        /// Cancela una reserva existente por su ID.
        /// </summary>
        /// <param name="id">ID de la reserva a cancelar.</param>
        /// <returns>Un mensaje indicando el resultado de la operación.</returns>
        [HttpPatch("{id:int}/cancelacion")]
        public async Task<IActionResult> Cancel(int id)
        {
            var success = await _bookingService.CancelAsync(id);
            if (!success) return NotFound(new { Message = $"No se encontró la reserva con ID {id}." });
            return NoContent();
        }
    }
}
