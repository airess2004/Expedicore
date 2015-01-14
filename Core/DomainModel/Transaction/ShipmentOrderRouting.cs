using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class ShipmentOrderRouting
    {
        public int Id { get; set; }
        public int ShipmentOrderId { get; set; }
        public int MasterCode { get; set; }
        public int OfficeId { get; set; }
        public int VesselId { get; set; }
        public string VesselName { get; set; }
        public string Voyage { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> PortId { get; set; }
        public Nullable<int> FlightId { get; set; }
        public string FlightNo { get; set; }
        public Nullable<int> AirportFromId { get; set; }
        public Nullable<int> AirportToId { get; set; }
        public Nullable<DateTime> ETD { get; set; }
        public string VesselType { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Office Office { get; set; }
        public virtual CityLocation City { get; set; }
        public virtual Port Port { get; set; }
        public virtual Airline Flight { get; set; }
        public virtual Airport AirPortForm { get; set; }
        public virtual Airport AirPortTo { get; set; }
         
        public virtual AccountUser CreatedBy { get; set; }
        public virtual AccountUser UpdatedBy { get; set; }
        public virtual ShipmentOrder ShipmentOrder { get; set; }


    }
}