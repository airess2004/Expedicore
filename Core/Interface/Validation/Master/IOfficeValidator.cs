using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IOfficeValidation
    {
        Office VCreateObject(Office office, IOfficeService _officeService);
        Office VUpdateObject(Office office, IOfficeService _officeService);
    }
}
