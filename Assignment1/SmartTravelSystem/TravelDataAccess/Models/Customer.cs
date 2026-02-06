using System;
using System.Collections.Generic;

namespace TravelDataAccess.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Bookings = new HashSet<Booking>();
        }

        public int CustomerId { get; set; }
        public string Code { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public int? Age { get; set; }
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User";

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
