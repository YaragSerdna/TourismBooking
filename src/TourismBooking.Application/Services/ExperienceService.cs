using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public class ExperienceService : IExperienceService
    {
        private readonly TourismBookingDbContext _context;

        /// <summary>
        /// Constructor de la clase ExperienceService.
        /// </summary>
        /// <param name="context">El contexto de base de datos.</param>
        public ExperienceService(TourismBookingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crea una nueva experiencia en la base de datos.
        /// </summary>
        /// <param name="dto">El DTO con los datos de la experiencia a crear.</param>
        /// <returns>El DTO con los datos de la experiencia creada.</returns>
        /// <exception cref="ArgumentException">Lanzado cuando los datos del DTO son inválidos.</exception>
        public async Task<ExperienceResponseDto> CreateAsync(CreateExperienceDto dto)
        {
            if (dto.PricePerPerson <= 0)
            {
                throw new ArgumentException("El precio por persona debe ser mayor a cero.");
            }

            if (dto.MaxCapacity <= 0)
            {
                throw new ArgumentException("La capacidad máxima debe ser mayor a cero.");
            }

            var experience = new Experience
            {
                Name = dto.Name,
                Description = dto.Description,
                Destination = dto.Destination,
                PricePerPerson = dto.PricePerPerson,
                MaxCapacity = dto.MaxCapacity,
                Status = ExperienceStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();

            return MapToDto(experience);
        }

        /// <summary>
        /// Obtiene todas las experiencias activas de la base de datos.
        /// </summary>
        /// <returns>Una lista de DTOs con los datos de las experiencias activas.</returns>
        public async Task<IEnumerable<ExperienceResponseDto>> GetActiveExperiencesAsync()
        {
            return await _context.Experiences.AsNoTracking().Where(e => e.Status == ExperienceStatus.Active).Select(e => MapToDto(e)).ToListAsync();
        }

        /// <summary>
        /// Obtiene una experiencia por su ID.
        /// </summary>
        /// <param name="id">El ID de la experiencia a obtener.</param>
        /// <returns>El DTO con los datos de la experiencia, o null si no se encuentra.</returns>
        public async Task<ExperienceResponseDto?> GetByIdAsync(int id)
        {
            var experience = await _context.Experiences.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

            return experience == null ? null : MapToDto(experience);
        }

        /// <summary>
        /// Actualiza una experiencia existente en la base de datos.
        /// </summary>
        /// <param name="id">El ID de la experiencia a actualizar.</param>
        /// <param name="dto">El DTO con los datos actualizados de la experiencia.</param>
        /// <returns>El DTO con los datos de la experiencia actualizada, o null si no se encuentra.</returns>
        /// <exception cref="ArgumentException">Lanzado cuando los datos del DTO son inválidos.</exception>
        public async Task<ExperienceResponseDto?> UpdateAsync(int id, UpdateExperienceDto dto)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null) return null;

            if (dto.PricePerPerson <= 0 || dto.MaxCapacity <= 0)
            {
                throw new ArgumentException("Precio y capacidad deben ser mayores a cero.");
            }

            experience.Name = dto.Name;
            experience.Description = dto.Description;
            experience.Destination = dto.Destination;
            experience.PricePerPerson = dto.PricePerPerson;
            experience.MaxCapacity = dto.MaxCapacity;
            experience.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(experience);
        }

        /// <summary>
        /// Inactiva una experiencia existente en la base de datos.
        /// </summary>
        /// <param name="id">El ID de la experiencia a inactivar.</param>
        /// <returns>True si la experiencia fue inactivada, false si no se encontró.</returns>
        public async Task<bool> InactivateAsync(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null) return false;

            experience.Status = ExperienceStatus.Inactive;
            experience.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Mapea una entidad Experience a un DTO ExperienceResponseDto.
        /// </summary>
        /// <param name="e">La entidad Experience a mapear.</param>
        /// <returns>El DTO ExperienceResponseDto con los datos de la experiencia.</returns>
        private static ExperienceResponseDto MapToDto(Experience e) =>
            new(e.Id, e.Name, e.Description, e.Destination, e.PricePerPerson, e.MaxCapacity, e.Status, e.CreatedAt, e.UpdatedAt);
    }
}
