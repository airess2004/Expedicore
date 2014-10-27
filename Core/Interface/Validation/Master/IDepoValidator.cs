using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IDepoValidation
    {
        Depo VCreateObject(Depo depo, IDepoService _depoService);
        Depo VUpdateObject(Depo depo, IDepoService _depoService);
    }
}
