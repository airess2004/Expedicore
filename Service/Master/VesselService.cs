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
    public class VesselService : IVesselService 
    {  
        private IVesselRepository _repository;
        private IVesselValidation _validator;

        public VesselService(IVesselRepository _vesselRepository, IVesselValidation _vesselValidation)
        {
            _repository = _vesselRepository;
            _validator = _vesselValidation;
        }

        public IQueryable<Vessel> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Vessel GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        
        public Vessel CreateObject(Vessel vessel)
        {
            vessel.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(vessel,this)))
            {
                vessel.MasterCode = _repository.GetLastMasterCode(vessel.OfficeId) + 1;
                vessel = _repository.CreateObject(vessel);
            }
            return vessel;
        }
         
        public Vessel UpdateObject(Vessel vessel)
        {
            if (isValid(_validator.VUpdateObject(vessel, this)))
            {
                vessel = _repository.UpdateObject(vessel);
            }
            return vessel;
        }
         
        public Vessel SoftDeleteObject(Vessel vessel)
        {
            vessel = _repository.SoftDeleteObject(vessel);
            return vessel;
        }

        public bool IsNameDuplicated(Vessel vessel)
        {
            return _repository.IsNameDuplicated(vessel);
        }


        public bool isValid(Vessel obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
