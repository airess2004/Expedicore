using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TelexReleaseService : ITelexReleaseService 
    {  
        private ITelexReleaseRepository _repository;
        private ITelexReleaseValidation _validator;

        public TelexReleaseService(ITelexReleaseRepository _telexreleaseRepository, ITelexReleaseValidation _telexreleaseValidation)
        {
            _repository = _telexreleaseRepository;
            _validator = _telexreleaseValidation;
        }

        public IQueryable<TelexRelease> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public TelexRelease GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public TelexRelease GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public TelexRelease CreateUpdateObject(TelexRelease telexrelease)
        {
            TelexRelease newtelexrelease = this.GetObjectByShipmentOrderId(telexrelease.ShipmentOrderId);
            if (newtelexrelease == null)
            {
                telexrelease = this.CreateObject(telexrelease);
            }
            else
            {
                telexrelease = this.UpdateObject(telexrelease);
            }
            return telexrelease;
        }

        public TelexRelease CreateObject(TelexRelease telexrelease)
        {
            telexrelease.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(telexrelease,this)))
            {
                telexrelease = _repository.CreateObject(telexrelease);
            }
            return telexrelease;
        }
         
        public TelexRelease UpdateObject(TelexRelease telexrelease)
        {
            if (isValid(_validator.VUpdateObject(telexrelease, this)))
            {
                telexrelease = _repository.UpdateObject(telexrelease);
            }
            return telexrelease;
        }
         
        public TelexRelease SoftDeleteObject(TelexRelease telexrelease)
        {
            telexrelease = _repository.SoftDeleteObject(telexrelease);
            return telexrelease;
        }


        public bool isValid(TelexRelease obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
