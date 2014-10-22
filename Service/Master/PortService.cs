using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Master
{
    public class PortService : IPortService 
    {  
        private IPortRepository _repository;
        private IPortValidation _validator;

        public PortService(IPortRepository _portRepository, IPortValidation _portValidation)
        {
            _repository = _portRepository;
            _validator = _portValidation;
        }

        public IQueryable<Port> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public Port GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
          
        public Port CreateObject(Port port,ICityLocationService _citylocationService)
        {
            port.Errors = new Dictionary<String, String>();
            if (!isValid(_validator.VCreateObject(port,this,_citylocationService)))
            {
                port.MasterCode = _repository.GetLastMasterCode(port.OfficeId) + 1;
                port = _repository.CreateObject(port);
            }
            return port;
        }

        public Port UpdateObject(Port port,ICityLocationService _citylocationService)
        {
            if (!isValid(_validator.VUpdateObject(port, this,_citylocationService)))
            {
                port = _repository.UpdateObject(port);
            }
            return port;
        }
         
        public Port SoftDeleteObject(Port port)
        {
            port = _repository.SoftDeleteObject(port);
            return port;
        }

        public bool IsNameDuplicated(Port port)
        {
            return _repository.IsNameDuplicated(port);
        }


        public bool isValid(Port obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
