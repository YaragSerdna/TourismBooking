using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Application.DTOs;
using TourismBooking.Application.Interfaces;
using TourismBooking.Domain.Entities;
using TourismBooking.Domain.Enums;
using TourismBooking.Infrastructure.Persistence;

namespace TourismBooking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly TourismBookingDbContext _context;

        /// <summary>
        /// Constructor for BookingService, initializes the service with the provided database context.
        /// </summary>
        /// <param name="context">El contexto de base de datos para acceder a los datos.</param>
        public BookingService(TourismBookingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crea una nueva reserva en la base de datos.
        /// </summary>
        /// <param name="dto">El DTO con los datos para crear la reserva.</param>
        /// <returns>El DTO con los datos de la reserva creada.</returns>
        /// <exception cref="ArgumentException">Lanzado cuando los datos del DTO son inválidos.</exception>
        /// <exception cref="InvalidOperationException">Lanzado cuando se intenta crear una reserva con datos inválidos.</exception>
        public async Task<BookingResponseDto> CreateAsync(CreateBookingDto dto)
        {
            if (dto.PassengerCount <= 0)
            {
                throw new ArgumentException("La cantidad de pasajeros debe ser mayor a cero.");
            }

            if (dto.ActivityDate.Date < DateTime.UtcNow.Date)
            {
                throw new InvalidOperationException("No se aceptan reservas para fechas anteriores a la actual.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var experience = await _context.Experiences.FirstOrDefaultAsync(e => e.Id == dto.ExperienceId);

                if (experience == null || experience.Status == ExperienceStatus.Inactive)
                {
                    throw new InvalidOperationException("No se pueden realizar reservas para experiencias inactivas o inexistentes.");
                }

                var reservedCapacity = await _context.Bookings.Where(b => b.ExperienceId == dto.ExperienceId && b.ActivityDate == dto.ActivityDate.Date && b.Status == BookingStatus.Confirmed).SumAsync(b => b.PassengerCount);

                var availableCapacity = experience.MaxCapacity - reservedCapacity;

                if (dto.PassengerCount > availableCapacity)
                {
                    throw new InvalidOperationException($"La reserva supera la capacidad disponible. Cupos disponibles: {availableCapacity}.");
                }

                var totalAmount = experience.PricePerPerson * dto.PassengerCount;

                var booking = new Booking
                {
                    ExperienceId = dto.ExperienceId,
                    ActivityDate = dto.ActivityDate.Date,
                    PassengerCount = dto.PassengerCount,
                    CustomerName = dto.CustomerName,
                    CustomerEmail = dto.CustomerEmail,
                    TotalAmount = totalAmount,
                    Status = BookingStatus.Confirmed,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapToDto(booking, experience.Name);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Obtiene una reserva por su ID.
        /// </summary>
        /// <param name="id"> El ID de la reserva a obtener.</param>
        /// <returns> El DTO de la reserva obtenida o null si no se encuentra.</returns>
        public async Task<BookingResponseDto?> GetByIdAsync(int id)
        {
            var booking = await _context.Bookings.Include(b => b.Experience).AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

            return booking == null ? null : MapToDto(booking, booking.Experience.Name);
        }

        /// <summary>
        /// Obtiene una lista de reservas filtradas según los criterios proporcionados.
        /// </summary>
        /// <param name="filter"> El filtro para aplicar a la lista de reservas.</param>
        /// <returns> La lista de DTOs de las reservas obtenidas.</returns>
        public async Task<IEnumerable<BookingResponseDto>> GetFilteredAsync(BookingFilterDto filter)
        {
            var query = _context.Bookings.Include(b => b.Experience).AsNoTracking().AsQueryable();

            if (filter.ActivityDate.HasValue)
            {
                query = query.Where(b => b.ActivityDate == filter.ActivityDate.Value.Date);
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(b => b.Status == filter.Status.Value);
            }

            if (filter.ExperienceId.HasValue)
            {
                query = query.Where(b => b.ExperienceId == filter.ExperienceId.Value);
            }

            var list = await query.ToListAsync();
            return list.Select(b => MapToDto(b, b.Experience.Name));
        }

        /// <summary>
        /// Cancela una reserva existente por su ID.
        /// </summary>
        /// <param name="id">El ID de la reserva a cancelar.</param>
        /// <returns>True si la reserva fue cancelada, false en caso contrario.</returns>
        /// <exception cref="InvalidOperationException"> Se lanza cuando la reserva ya está cancelada.</exception>
        public async Task<bool> CancelAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;

            if (booking.Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("La reserva ya se encuentra cancelada.");
            }

            booking.Status = BookingStatus.Cancelled;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Mapea una entidad Booking a un DTO BookingResponseDto.
        /// </summary>
        /// <param name="b"> La entidad Booking a mapear.</param>
        /// <param name="experienceName"> El nombre de la experiencia asociada.</param>
        /// <returns> El DTO BookingResponseDto mapeado.</returns>
        private static BookingResponseDto MapToDto(Booking b, string experienceName) =>
            new(b.Id, b.ExperienceId, experienceName, b.ActivityDate, b.PassengerCount, b.CustomerName, b.CustomerEmail, b.TotalAmount, b.Status, b.CreatedAt, b.UpdatedAt);
    }
}
