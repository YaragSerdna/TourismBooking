using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Domain.Enums;

namespace TourismBooking.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public int ExperienceId { get; set; }

        public DateTime ActivityDate { get; set; }

        public int PassengerCount { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Experience Experience { get; set; } = null!;
    }
}
