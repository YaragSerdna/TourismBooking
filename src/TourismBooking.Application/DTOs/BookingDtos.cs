using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Domain.Enums;

namespace TourismBooking.Application.DTOs
{
    public record CreateBookingDto(
        int ExperienceId,
        DateTime ActivityDate,
        int PassengerCount,
        string CustomerName,
        string CustomerEmail
    );

    public record BookingResponseDto(
        int Id,
        int ExperienceId,
        string ExperienceName,
        DateTime ActivityDate,
        int PassengerCount,
        string CustomerName,
        string CustomerEmail,
        decimal TotalAmount,
        BookingStatus Status,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public record BookingFilterDto(
        DateTime? ActivityDate,
        BookingStatus? Status,
        int? ExperienceId
    );
}
