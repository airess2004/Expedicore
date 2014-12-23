using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IVatValidation
    { 
        Vat VCreateObject(Vat vat, IVatService _vatService);
        Vat VUpdateObject(Vat vat, IVatService _vatService);
    }
}
