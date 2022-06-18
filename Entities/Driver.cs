using System;
using System.Collections.Generic;

namespace MobileAPIS4.Entities
{
    public partial class Driver
    {
        public Driver()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
