using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ICostValidation
    {
        Cost VCreateObject(Cost cost, ICostService _costService);
        Cost VUpdateObject(Cost cost, ICostService _costService);
    }
}
