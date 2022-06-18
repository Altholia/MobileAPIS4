using System;
using System.Collections.Generic;

namespace MobileAPIS4.Entities
{
    public partial class TransporationTask
    {
        public TransporationTask()
        {
            TaskAssigns = new HashSet<TaskAssign>();
            TravellingRecords = new HashSet<TravellingRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StartDistrictId { get; set; }
        public string StartStreetAddress { get; set; }
        public int DestinationDistrictId { get; set; }
        public string DestinationStreetAddress { get; set; }
        public decimal Weight { get; set; }
        public DateTime RequiredCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public int VehicleTeamAdministrator { get; set; }
        public string Remark { get; set; }
        public int StatusId { get; set; }

        public virtual District DestinationDistrict { get; set; }
        public virtual District StartDistrict { get; set; }
        public virtual TaskStatus Status { get; set; }
        public virtual Staff VehicleTeamAdministratorNavigation { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
        public virtual ICollection<TravellingRecord> TravellingRecords { get; set; }
    }
}
