using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Application.DTOs;

namespace TourismBooking.Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateAsync(CreateBookingDto dto);
        Task<BookingResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<BookingResponseDto>> GetFilteredAsync(BookingFilterDto filter);
        Task<bool> CancelAsync(int id);
    }
}
