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
    public class ShipmentOrderRoutingService : IShipmentOrderRoutingService 
    {  
        private IShipmentOrderRoutingRepository _repository;
        private IShipmentOrderRoutingValidation _validator;

        public ShipmentOrderRoutingService(IShipmentOrderRoutingRepository _shipmentorderroutingRepository, IShipmentOrderRoutingValidation _shipmentorderroutingValidation)
        {
            _repository = _shipmentorderroutingRepository;
            _validator = _shipmentorderroutingValidation;
        }

        public IQueryable<ShipmentOrderRouting> GetQueryable()
        {  
            return _repository.GetQueryable();
        }

        public ShipmentOrderRouting GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ShipmentOrderRouting GetObjectByShipmentOrderId(int Id)
        {
            return _repository.GetObjectByShipmentOrderId(Id);
        }

        public IList<ShipmentOrderRouting> GetListByShipmentOrderId(int Id)
        { 
            return _repository.GetListByShipmentOrderId(Id);
        }

         
        public ShipmentOrderRouting CreateUpdateObject(ShipmentOrderRouting shipmentOrderRouting)
        {
            ShipmentOrderRouting existShipmentOrderRouting = GetQueryable().Where(x => x.ShipmentOrderId == shipmentOrderRouting.ShipmentOrderId && x.Id == shipmentOrderRouting.Id).FirstOrDefault();
            if (existShipmentOrderRouting == null)
            {
                ShipmentOrderRouting newShipmentOrderRouting = new ShipmentOrderRouting { 
                        AirportFromId = shipmentOrderRouting.AirportFromId,
                        AirportToId = shipmentOrderRouting.AirportToId,
                        CityId = shipmentOrderRouting.CityId,
                        OfficeId = shipmentOrderRouting.OfficeId,
                        ETD = shipmentOrderRouting.ETD,
                        FlightId = shipmentOrderRouting.FlightId,
                        FlightNo = shipmentOrderRouting.FlightNo,
                        PortId = shipmentOrderRouting.PortId,
                        ShipmentOrderId = shipmentOrderRouting.ShipmentOrderId,
                        VesselId = shipmentOrderRouting.VesselId,
                        VesselName = shipmentOrderRouting.VesselName,
                        VesselType = shipmentOrderRouting.VesselType,
                        Voyage = shipmentOrderRouting.Voyage,
                        CreatedById = shipmentOrderRouting.CreatedById,
                        MasterCode = _repository.GetLastMasterCode(shipmentOrderRouting.OfficeId) + 1,
                        CreatedAt = DateTime.Now,
                        Errors = new Dictionary<String, String>()
                };
                shipmentOrderRouting = CreateObject(newShipmentOrderRouting);
            }
            else
            {
                existShipmentOrderRouting.AirportFromId = shipmentOrderRouting.AirportFromId;
                existShipmentOrderRouting.AirportToId = shipmentOrderRouting.AirportToId;
                existShipmentOrderRouting.CityId = shipmentOrderRouting.CityId;
                existShipmentOrderRouting.OfficeId = shipmentOrderRouting.OfficeId;
                existShipmentOrderRouting.ETD = shipmentOrderRouting.ETD;
                existShipmentOrderRouting.FlightId = shipmentOrderRouting.FlightId;
                existShipmentOrderRouting.FlightNo = shipmentOrderRouting.FlightNo;
                existShipmentOrderRouting.PortId = shipmentOrderRouting.PortId;
                existShipmentOrderRouting.ShipmentOrderId = shipmentOrderRouting.ShipmentOrderId;
                existShipmentOrderRouting.VesselId = shipmentOrderRouting.VesselId;
                existShipmentOrderRouting.VesselName = shipmentOrderRouting.VesselName;
                existShipmentOrderRouting.VesselType = shipmentOrderRouting.VesselType;
                existShipmentOrderRouting.Voyage = shipmentOrderRouting.Voyage;
                existShipmentOrderRouting.UpdatedById = shipmentOrderRouting.UpdatedById;
                existShipmentOrderRouting.UpdatedAt = DateTime.Now;
                existShipmentOrderRouting.Errors = new Dictionary<String, String>();
                shipmentOrderRouting = UpdateObject(existShipmentOrderRouting);
            }
            shipmentOrderRouting = GetObjectById(shipmentOrderRouting.Id);
            return shipmentOrderRouting;
        }

        public ShipmentOrderRouting CreateObject(ShipmentOrderRouting shipmentorderrouting)
        {
            shipmentorderrouting.Errors = new Dictionary<String, String>();
            if (isValid(_validator.VCreateObject(shipmentorderrouting,this)))
            {
                shipmentorderrouting = _repository.CreateObject(shipmentorderrouting);
            }
            return shipmentorderrouting;
        }
         
        public ShipmentOrderRouting UpdateObject(ShipmentOrderRouting shipmentorderrouting)
        {
            if (isValid(_validator.VUpdateObject(shipmentorderrouting, this)))
            {
                shipmentorderrouting = _repository.UpdateObject(shipmentorderrouting);
            }
            return shipmentorderrouting;
        }
         
        public ShipmentOrderRouting SoftDeleteObject(ShipmentOrderRouting shipmentorderrouting)
        {
            shipmentorderrouting = _repository.SoftDeleteObject(shipmentorderrouting);
            return shipmentorderrouting;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
        public bool isValid(ShipmentOrderRouting obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }
    }
}
