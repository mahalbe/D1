using ISas.Entities.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISas.Web.Models
{
    public interface IAlert_EventManager
    {
        List<Alert_EventManagerModel> GetAlert_EventList();
    }
}
