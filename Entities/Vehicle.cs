using System;
using System.Collections.Generic;

namespace MobileAPIS4.Entities
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            TaskAssigns = new HashSet<TaskAssign>();
        }

        public int Id { get; set; }
        public string PlateNumber { get; set; }
        public int BrandId { get; set; }
        public int Ton { get; set; }
        public int DriverId { get; set; }
        public string Photo { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
    }
}
