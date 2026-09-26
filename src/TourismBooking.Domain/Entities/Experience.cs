using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourismBooking.Domain.Enums;

namespace TourismBooking.Domain.Entities
{
    public class Experience
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public decimal PricePerPerson { get; set; }

        public int MaxCapacity { get; set; }

        public ExperienceStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
