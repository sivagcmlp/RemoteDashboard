using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteDashboard.Models.BaseTypes;

namespace RemoteDashboard.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        int Add(T entity, string sqlQuery);
        int AddRange(IEnumerable<T> entities, bool persist = true);
    }
}
