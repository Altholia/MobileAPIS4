using System;
using System.Collections.Generic;

namespace MobileAPIS4.Entities
{
    public partial class City
    {
        public City()
        {
            Districts = new HashSet<District>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
