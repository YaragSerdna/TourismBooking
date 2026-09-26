using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Domain.Enums;

namespace TourismBooking.Application.DTOs
{
    /// <summary>
    /// DTO para crear una nueva reserva.
    /// </summary>
    /// <param name="ExperienceId">ID de la experiencia turística.</param>
    /// <param name="ActivityDate">Fecha de la actividad.</param>
    /// <param name="PassengerCount">Número de pasajeros.</param>
    /// <param name="CustomerName">Nombre del cliente.</param>
    /// <param name="CustomerEmail">Correo electrónico del cliente.</param>
    public record CreateBookingDto(
        int ExperienceId,
        DateTime ActivityDate,
        int PassengerCount,
        string CustomerName,
        string CustomerEmail
    );

    /// <summary>
    /// DTO para la respuesta de una reserva.
    /// </summary>
    /// <param name="Id">ID de la reserva.</param>
    /// <param name="ExperienceId">ID de la experiencia turística.</param>
    /// <param name="ExperienceName">Nombre de la experiencia turística.</param>
    /// <param name="ActivityDate">Fecha de la actividad.</param>
    /// <param name="PassengerCount">Número de pasajeros.</param>
    /// <param name="CustomerName">Nombre del cliente.</param>
    /// <param name="CustomerEmail">Correo electrónico del cliente.</param>
    /// <param name="TotalAmount">Monto total de la reserva.</param>
    /// <param name="Status">Estado de la reserva.</param>
    /// <param name="CreatedAt">Fecha de creación de la reserva.</param>
    /// <param name="UpdatedAt">Fecha de actualización de la reserva.</param>
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

    /// <summary>
    /// DTO para filtrar reservas.
    /// </summary>
    /// <param name="ActivityDate">Fecha de la actividad.</param>
    /// <param name="Status">Estado de la reserva.</param>
    /// <param name="ExperienceId">ID de la experiencia turística.</param>
    public record BookingFilterDto(
        DateTime? ActivityDate,
        BookingStatus? Status,
        int? ExperienceId
    );
}
