using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ITelexReleaseValidation
    {
        TelexRelease VCreateObject(TelexRelease telexrelease, ITelexReleaseService _telexreleaseService);
        TelexRelease VUpdateObject(TelexRelease telexrelease, ITelexReleaseService _telexreleaseService);
    }
}
