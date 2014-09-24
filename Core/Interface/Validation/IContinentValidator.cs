using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IContinentValidation
    {
        Continent VCreateObject(Continent airline, IContinentService _airlineService);
        Continent VUpdateObject(Continent airline, IContinentService _airlineService);
    }
}
