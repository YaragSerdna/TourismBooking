using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Domain.Enums;

namespace TourismBooking.Application.DTOs
{
    /// <summary>
    /// DTO para crear una nueva experiencia turística.
    /// </summary>
    /// <param name="Name">Nombre de la experiencia turística.</param>
    /// <param name="Description">Descripción de la experiencia turística.</param>
    /// <param name="Destination">Destino de la experiencia turística.</param>
    /// <param name="PricePerPerson">Precio por persona.</param>
    /// <param name="MaxCapacity">Capacidad máxima de la experiencia turística.</param>
    public record CreateExperienceDto(
        string Name,
        string Description,
        string Destination,
        decimal PricePerPerson,
        int MaxCapacity
    );
    
    /// <summary>
    /// DTO para actualizar una experiencia turística existente.
    /// </summary>
    /// <param name="Name">Nombre de la experiencia turística.</param>
    /// <param name="Description">Descripción de la experiencia turística.</param>
    /// <param name="Destination">Destino de la experiencia turística.</param>
    /// <param name="PricePerPerson">Precio por persona.</param>
    /// <param name="MaxCapacity">Capacidad máxima de la experiencia turística.</param>
    public record UpdateExperienceDto(
        string Name,
        string Description,
        string Destination,
        decimal PricePerPerson,
        int MaxCapacity
    );

    /// <summary>
    /// DTO para la respuesta de una experiencia turística.
    /// </summary>
    /// <param name="Id">ID de la experiencia turística.</param>
    /// <param name="Name">Nombre de la experiencia turística.</param>
    /// <param name="Description">Descripción de la experiencia turística.</param>
    /// <param name="Destination">Destino de la experiencia turística.</param>
    /// <param name="PricePerPerson">Precio por persona.</param>
    /// <param name="MaxCapacity">Capacidad máxima de la experiencia turística.</param>
    /// <param name="Status">Estado de la experiencia turística.</param>
    /// <param name="CreatedAt">Fecha de creación de la experiencia turística.</param>
    /// <param name="UpdatedAt">Fecha de actualización de la experiencia turística.</param>
    public record ExperienceResponseDto(
        int Id,
        string Name,
        string Description,
        string Destination,
        decimal PricePerPerson,
        int MaxCapacity,
        ExperienceStatus Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
