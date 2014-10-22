using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IAirportValidation
    {
        Airport VCreateObject(Airport airport, IAirportService _airportService, ICityLocationService _citylocationservice);
        Airport VUpdateObject(Airport airport, IAirportService _airportService, ICityLocationService _citylocationservice);
    }
}
