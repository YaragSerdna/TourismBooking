using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Domain.Enums;

namespace TourismBooking.Application.DTOs
{
    public record CreateExperienceDto(
        string Name,
        string Description,
        string Destination,
        decimal PricePerPerson,
        int MaxCapacity
    );

    public record UpdateExperienceDto(
        string Name,
        string Description,
        string Destination,
        decimal PricePerPerson,
        int MaxCapacity
    );

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
