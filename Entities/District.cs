using System;
using System.Collections.Generic;

namespace MobileAPIS4.Entities
{
    public partial class District
    {
        public District()
        {
            TransporationTaskDestinationDistricts = new HashSet<TransporationTask>();
            TransporationTaskStartDistricts = new HashSet<TransporationTask>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<TransporationTask> TransporationTaskDestinationDistricts { get; set; }
        public virtual ICollection<TransporationTask> TransporationTaskStartDistricts { get; set; }
    }
}
