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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var result = await _bookingService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Consulta una reserva por su ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookingService.GetByIdAsync(id);
            if (result == null) return NotFound(new { Message = $"No se encontró la reserva con ID {id}." });
            return Ok(result);
        }

        /// <summary>
        /// Consulta reservas aplicando filtros por fecha, estado o experiencia.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] BookingFilterDto filter)
        {
            var list = await _bookingService.GetFilteredAsync(filter);
            return Ok(list);
        }

        /// <summary>
        /// Cancela una reserva existente y libera la capacidad ocupada.
        /// </summary>
        [HttpPatch("{id:int}/cancelacion")]
        public async Task<IActionResult> Cancel(int id)
        {
            var success = await _bookingService.CancelAsync(id);
            if (!success) return NotFound(new { Message = $"No se encontró la reserva con ID {id}." });
            return NoContent();
        }
    }
}
