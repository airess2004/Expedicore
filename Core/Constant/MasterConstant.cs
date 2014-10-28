using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constant
{ 
    public partial class MasterConstant
    {

        public class MutationStatus
        {
            public static int Addition = 1;
            public static int Deduction = 2;
        }

        public class SourceDocument
        {
            public static string PaymentVoucherDetail = "PaymentVoucherDetail";
            public static string ReceiptVoucherDetail = "ReceiptVoucherDetail";
            public static string CustomPurchaseInvoiceDetail = "CustomPurchaseInvoiceDetail";
            public static string CashSalesInvoiceDetailDetail = "CashSalesInvoiceDetailDetail";
            public static string RetailSalesInvoiceDetail = "RetailSalesInvoiceDetail";
        }

        public class SourceDocumentType
        {
            public static string BarringOrder = "BarringOrder";
            public static string CashBankMutation = "CashBankMutation";
            public static string CashBankAdjustment = "CashBankAdjustment";
            public static string CoreIdentification = "CoreIdentification";
            public static string DeliveryOrder = "DeliveryOrder";
            public static string PaymentVoucher = "PaymentVoucher";
            public static string PurchaseOrder = "PurchaseOrder";
            public static string PurchaseReceival = "PurchaseReceival";
            public static string ReceiptVoucher = "ReceiptVoucher";
            public static string RecoveryOrder = "RecoveryOrder";
            public static string RecoveryOrderDetail = "RecoveryOrderDetail";
            public static string RetailPurchaseInvoice = "RetailPurchaseInvoice";
            public static string RetailSalesInvoice = "RetailSalesInvoice";
            public static string RollerWarehouseMutation = "RollerWarehouseMutation";
            public static string SalesOrder = "SalesOrder";
            public static string StockAdjustment = "StockAdjustment";
            public static string WarehouseMutationOrder = "WarehouseMutationOrder";
            public static string CashSalesInvoice = "CashSalesInvoice";
            public static string CashSalesReturn = "CashSalesReturn";
            public static string CustomPurchaseInvoice = "CustomPurchaseInvoice";
        }

        public class SourceDocumentDetailType
        {
            public static string BarringOrderDetail = "BarringOrderDetail";
            public static string CoreIdentificationDetail = "CoreIdentificationDetail";
            public static string DeliveryOrderDetail = "DeliveryOrderDetail";
            public static string PurchaseOrderDetail = "PurchaseOrderDetail";
            public static string PurchaseReceivalDetail = "PurchaseReceivalDetail";
            public static string RecoveryOrderDetail = "RecoveryOrderDetail";
            public static string RecoveryAccessoryDetail = "RecoveryAccessoryDetail";
            public static string RetailPurchaseInvoiceDetail = "RetailPurchaseInvoiceDetail";
            public static string RetailSalesInvoiceDetail = "RetailSalesInvoiceDetail";
            public static string RollerWarehouseMutationDetail = "RollerWarehouseMutationDetail";
            public static string SalesOrderDetail = "SalesOrderDetail";
            public static string StockAdjustmentDetail = "StockAdjustmentDetail";
            public static string WarehouseMutationOrderDetail = "WarehouseMutationOrderDetail";
            public static string CashSalesInvoiceDetail = "CashSalesInvoiceDetail";
            public static string CashSalesReturnDetail = "CashSalesReturnDetail";
            public static string CustomPurchaseInvoiceDetail = "CustomPurchaseInvoiceDetail";
        }

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
