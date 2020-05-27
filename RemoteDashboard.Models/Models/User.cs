using RemoteDashboard.Models.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDashboard.Models.Models
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
        public string PMI { get; set; }
        public string TimeZone { get; set; }
        public int Verified { get; set; }
        public string Department { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string LastClientVersion { get; set; }
        public string Language { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
    }
}
