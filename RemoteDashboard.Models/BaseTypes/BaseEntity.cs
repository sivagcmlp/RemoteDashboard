using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDashboard.Models.BaseTypes
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
    }
}
