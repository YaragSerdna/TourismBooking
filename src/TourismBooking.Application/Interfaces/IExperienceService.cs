using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Application.DTOs;

namespace TourismBooking.Application.Interfaces
{
    public interface IExperienceService
    {
        Task<ExperienceResponseDto> CreateAsync(CreateExperienceDto dto);
        Task<IEnumerable<ExperienceResponseDto>> GetActiveExperiencesAsync();
        Task<ExperienceResponseDto?> GetByIdAsync(int id);
        Task<ExperienceResponseDto?> UpdateAsync(int id, UpdateExperienceDto dto);
        Task<bool> InactivateAsync(int id);
    }
}
