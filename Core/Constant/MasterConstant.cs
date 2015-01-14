﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constant
{ 
    public partial class MasterConstant
    {
        public class Status
        {
            public static string Cancel = "Cancel";
        }

        public class DebetCredit
        {
            public static string Debet = "D";
            public static string Credit = "C";
        }

        public class MutationStatus
        {
            public static int Addition = 1;
            public static int Deduction = 2;
        }


        public class SourceDocument
        {
            public static string CashBankMutation = "CashBankMutation";
            public static string CashBankAdjustment = "CashBankAdjustment";
            public static string PaymentRequest = "PaymentVoucher";
            public static string PaymentVoucher = "PaymentVoucher";
            public static string ReceiptVoucher = "ReceiptVoucher";
            public static string Invoice = "Invoice";
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
            public const int PPJK = 3;
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
            public const int PPJK = 10;
        }

        public class InvoiceTo
        {
            public const string InvoiceToAgent = "AG";
            public const string InvoiceToShipper = "SM";
            public const string InvoiceToConsignee = "CM";
        }

        public class Currency
        { 
            public const int USD = 0;
            public const int IDR = 1;
        }

        public class Print
        {
            public const string Fixed = "f";
            public const string Draft = "d";
        }
         
        public class Remarks
        {
            public const string SPPB = "SPPB";
            public const string BuatSP2 = "BUAT SP2";
            public const string DokOri = "DOK ORI";
            public const string Stripping = "STRIPPING";
            public const string H3 = "H+3";
            public const string PBM = "PBM";
            public const string Muat = "MUAT";
            public const string Kirim = "KIRIM";
        }
    }
}
