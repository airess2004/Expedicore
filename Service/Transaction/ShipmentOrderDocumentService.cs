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
    public class ShipmentOrderDocumentService : IShipmentOrderDocumentService 
    {  
        private IShipmentOrderDocumentRepository _repository;
        private IShipmentOrderDocumentValidation _validator;

        public ShipmentOrderDocumentService(IShipmentOrderDocumentRepository _shipmentOrderDocumentRepository, IShipmentOrderDocumentValidation _shipmentOrderDocumentValidation)
        {
            _repository = _shipmentOrderDocumentRepository;
            _validator = _shipmentOrderDocumentValidation;
        }

        public IQueryable<ShipmentOrderDocument> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public ShipmentOrderDocument GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ShipmentOrderDocument GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public IList<ShipmentOrderDocument> GetListByShipmentOrderId(int Id)
        { 
            return _repository.GetListByShipmentOrderId(Id);
        }

         
        public ShipmentOrderDocument CreateUpdateObject(ShipmentOrderDocument shipmentOrderRouting)
        {
            ShipmentOrderDocument existShipmentOrderDocument = GetQueryable().Where(x => x.ShipmentOrderId == shipmentOrderRouting.ShipmentOrderId && x.Id == shipmentOrderRouting.Id).FirstOrDefault();
            if (existShipmentOrderDocument == null)
            {
                ShipmentOrderDocument newShipmentOrderDocument = new ShipmentOrderDocument { 
                        OfficeId = shipmentOrderRouting.OfficeId,
                        ShipmentOrderId = shipmentOrderRouting.ShipmentOrderId,
                        DocumentName = shipmentOrderRouting.DocumentName,
                        Description = shipmentOrderRouting.Description,
                        CreatedById = shipmentOrderRouting.CreatedById,
                        CreatedAt = DateTime.Now,
                        Errors = new Dictionary<String, String>()
                };
                shipmentOrderRouting = CreateObject(newShipmentOrderDocument);
            }
            else
            {
                existShipmentOrderDocument.OfficeId = shipmentOrderRouting.OfficeId;
                existShipmentOrderDocument.ShipmentOrderId = shipmentOrderRouting.ShipmentOrderId;
                existShipmentOrderDocument.UpdatedById = shipmentOrderRouting.UpdatedById;
                existShipmentOrderDocument.UpdatedAt = DateTime.Now;
                existShipmentOrderDocument.Errors = new Dictionary<String, String>();
                shipmentOrderRouting = UpdateObject(existShipmentOrderDocument);
            }
            shipmentOrderRouting = GetObjectById(shipmentOrderRouting.Id);
            return shipmentOrderRouting;
        }

        public ShipmentOrderDocument CreateObject(ShipmentOrderDocument shipmentOrderDocument)
        {
            shipmentOrderDocument.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(shipmentOrderDocument,this)))
            {
                shipmentOrderDocument = _repository.CreateObject(shipmentOrderDocument);
            }
            return shipmentOrderDocument;
        }
         
        public ShipmentOrderDocument UpdateObject(ShipmentOrderDocument shipmentOrderDocument)
        {
            if (isValid(_validator.VUpdateObject(shipmentOrderDocument, this)))
            {
                shipmentOrderDocument = _repository.UpdateObject(shipmentOrderDocument);
            }
            return shipmentOrderDocument;
        }
         
        public ShipmentOrderDocument SoftDeleteObject(ShipmentOrderDocument shipmentOrderDocument)
        {
            shipmentOrderDocument = _repository.SoftDeleteObject(shipmentOrderDocument);
            return shipmentOrderDocument;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
        public bool isValid(ShipmentOrderDocument obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
