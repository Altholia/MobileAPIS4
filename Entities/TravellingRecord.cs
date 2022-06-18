using System;
using System.Collections.Generic;

namespace MobileAPIS4.Entities
{
    public partial class TravellingRecord
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public DateTime RecordTime { get; set; }

        public virtual TransporationTask Task { get; set; }
    }
}
