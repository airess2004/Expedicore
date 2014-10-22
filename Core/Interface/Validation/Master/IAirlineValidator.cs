using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IAirlineValidation
    { 
        Airline VCreateObject(Airline airline, IAirlineService _airlineService);
        Airline VUpdateObject(Airline airline, IAirlineService _airlineService);
    }
}
