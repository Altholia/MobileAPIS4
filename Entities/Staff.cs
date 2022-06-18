using System;
using System.Collections.Generic;

namespace MobileAPIS4.Entities
{
    public partial class Staff
    {
        public Staff()
        {
            TransporationTasks = new HashSet<TransporationTask>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Photo { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<TransporationTask> TransporationTasks { get; set; }
    }
}
