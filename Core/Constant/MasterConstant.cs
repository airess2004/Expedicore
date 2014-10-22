using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constant
{ 
    public partial class MasterConstant
    {
        public class VesselType
        {
            public const string VesselFeeder = "FEEDER";
            public const string VesselConnecting = "CONNECTING";
            public const string VesselMother = "MOTHER";
        }

        public class Cost
        {
            public const int Sea = 1;
            public const int Air = 2; 
        }

        public class MStBillOfLading
        {
            public const int Sea = 1;
            public const int Air = 2;
        }

        public class ContactType
        {
            public const int Agent = 1;
            public const int Shipper = 2;
            public const int Consignee = 3;
            public const int SSLine = 4;
            public const int IATA = 5;
            public const int EMKL = 6;
            public const int Depo = 7;
            public const int RebateShipper = 8;
            public const int RebateConsignee = 9;
        }

        public class ContactStatus
        {
            public const string PT = "PT";
            public const string Other = "OT";
            public const string CV = "CV"; 
        }

        public class JobType
        {
            public const int SeaExport = 1;
            public const int SeaImport = 2;
            public const int AirExport = 3;
            public const int AirImport = 4;
            public const int EMKLSeaExport = 5;
            public const int EMKLSeaImport = 6;
            public const int EMKLAirExport = 7;
            public const int EMKLAirImport = 8;
            public const int EMKLDomestic = 9;
        }
    }
}
