using Core.DomainModel;
using Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Migrations;

namespace Data.Context
{
    public class ExpedicoEntities : DbContext
    {
        public ExpedicoEntities() : base("ExpedicoPPJK")
        {
            Database.SetInitializer<ExpedicoEntities>(new MigrateDatabaseToLatestVersion<ExpedicoEntities, Configuration>());
        }

        public void DeleteAllTables()
        {
            IList<String> tableNames = new List<String>();

            IList<String> userroleNames = new List<String>() { "MenuUser", "AccountUser", "AccessUser","Office"};

            IList<String> transactionNames = new List<String>()
                                        { "TruckOrder"};
            IList<String> MasterNames = new List<String>()
                                        { "Truck", "Contact", "Employee", "GroupEmployee", "ContainerYard", "Depo",
                                          "Office"};

            userroleNames.ToList().ForEach(x => tableNames.Add(x));
            transactionNames.ToList().ForEach(x => tableNames.Add(x));
            MasterNames.ToList().ForEach(x => tableNames.Add(x));

            foreach (var tableName in tableNames)
            {
               
                Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            #region Finance
            modelBuilder.Configurations.Add(new CashBankMapping());
            modelBuilder.Configurations.Add(new CashBankAdjustmentMapping());
            modelBuilder.Configurations.Add(new CashBankMutationMapping());
            modelBuilder.Configurations.Add(new CashMutationMapping());
            modelBuilder.Configurations.Add(new PayableMapping());
            modelBuilder.Configurations.Add(new PaymentVoucherDetailMapping());
            modelBuilder.Configurations.Add(new PaymentVoucherMapping());
            modelBuilder.Configurations.Add(new ReceiptVoucherDetailMapping());
            modelBuilder.Configurations.Add(new ReceiptVoucherMapping());
            modelBuilder.Configurations.Add(new ReceivableMapping());
            #endregion

            #region Master
            modelBuilder.Configurations.Add(new AirlineMapping());
            modelBuilder.Configurations.Add(new AirportMapping());
            modelBuilder.Configurations.Add(new CityLocationMapping());
            modelBuilder.Configurations.Add(new ContactMapping());
            modelBuilder.Configurations.Add(new ContactTypeMapping());
            modelBuilder.Configurations.Add(new ContainerYardMapping());
            modelBuilder.Configurations.Add(new ContinentMapping());
            modelBuilder.Configurations.Add(new CostMapping());
            modelBuilder.Configurations.Add(new CountryLocationMapping());
            modelBuilder.Configurations.Add(new DepoMapping());
            modelBuilder.Configurations.Add(new EmployeeMapping());
            modelBuilder.Configurations.Add(new ExchangeRateMapping());
            modelBuilder.Configurations.Add(new GroupContactMapping());
            modelBuilder.Configurations.Add(new GroupEmployeeMapping());
            modelBuilder.Configurations.Add(new GroupMapping());
            modelBuilder.Configurations.Add(new JobMapping());
            modelBuilder.Configurations.Add(new MstBillOfLadingMapping());
            modelBuilder.Configurations.Add(new OfficeMapping());
            modelBuilder.Configurations.Add(new PortMapping());
            modelBuilder.Configurations.Add(new VesselMapping()); 
            modelBuilder.Configurations.Add(new TruckMapping());
            modelBuilder.Configurations.Add(new VatMapping());
          
            #endregion

            #region Transaction
            modelBuilder.Configurations.Add(new BillOfLadingMapping());
            modelBuilder.Configurations.Add(new CashAdvanceDetailMapping());
            modelBuilder.Configurations.Add(new CashAdvanceMapping());
            modelBuilder.Configurations.Add(new CashSettlementMapping());
            modelBuilder.Configurations.Add(new DeliveryOrderMapping());
            modelBuilder.Configurations.Add(new EstimateProfitLossDetailMapping());
            modelBuilder.Configurations.Add(new EstimateProfitLossMapping());
            modelBuilder.Configurations.Add(new InvoiceDetailMapping());
            modelBuilder.Configurations.Add(new InvoiceMapping());
            modelBuilder.Configurations.Add(new NoticeOfArrivalMapping());
            modelBuilder.Configurations.Add(new PaymentRequestDetailMapping());
            modelBuilder.Configurations.Add(new PaymentRequestMapping());
            modelBuilder.Configurations.Add(new SeaContainerMapping());
            modelBuilder.Configurations.Add(new ShipmentAdviceMapping());
            modelBuilder.Configurations.Add(new ShipmentInstructionMapping());
            modelBuilder.Configurations.Add(new ShipmentOrderMapping());
            modelBuilder.Configurations.Add(new ShipmentOrderRoutingMapping());
            modelBuilder.Configurations.Add(new TemporaryPaymentJobMapping());
            modelBuilder.Configurations.Add(new TemporaryPaymentMapping());
            modelBuilder.Configurations.Add(new TemporaryReceiptJobMapping());
            modelBuilder.Configurations.Add(new TemporaryReceiptMapping());
            modelBuilder.Configurations.Add(new TruckOrderMapping());
            #endregion


            #region UserRole
            modelBuilder.Configurations.Add(new AccessUserMapping());
            modelBuilder.Configurations.Add(new AccountUserMapping());
            modelBuilder.Configurations.Add(new MenuUserMapping()); 
            #endregion


            base.OnModelCreating(modelBuilder);
        }

        #region Finance
        public DbSet<CashBankAdjustment> CashBankAdjustments { get; set; }
        public DbSet<CashBank> CashBanks { get; set; }
        public DbSet<CashBankMutation> CashBankMutations { get; set; }
        public DbSet<CashMutation> CashMutations { get; set; }
        public DbSet<Payable> Payables { get; set; }
        public DbSet<PaymentVoucherDetail> PaymentVoucherDetails { get; set; }
        public DbSet<PaymentVoucher> PaymentVouchers { get; set; }
        public DbSet<ReceiptVoucherDetail> ReceiptVoucherDetails { get; set; }
        public DbSet<ReceiptVoucher> ReceiptVouchers { get; set; }
        public DbSet<Receivable> Receivables { get; set; }
        #endregion

        #region Master
        public DbSet<Airline> AirLines { get; set; }
        public DbSet<Airport> AirPorts { get; set; }
        public DbSet<CityLocation> CityLocations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<ContainerYard> ContainerYards { get; set; }
        public DbSet<Continent> Continents { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<CountryLocation> CountryLocations { get; set; }
        public DbSet<Depo> Depos { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupContact> GroupContacts { get; set; }
        public DbSet<GroupEmployee> GroupEmployees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<MstBillOfLading> MstBillOfLadings { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Vat> Vat { get; set; } 
        public DbSet<Vessel> Vessels { get; set; }

        #endregion

        #region Transcantion
        public DbSet<BillOfLading> BillOfLaddings { get; set; }
        public DbSet<CashAdvanceDetail> CashAdvanceDetails { get; set; }
        public DbSet<CashAdvance> CashAdvances { get; set; }
        public DbSet<CashSettlement> CashSettlements { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public DbSet<EstimateProfitLoss> EstimateProfitLosses { get; set; }
        public DbSet<EstimateProfitLossDetail> EstimateProfitLossDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<NoticeOfArrival> NoticeOfArrivals { get; set; }
        public DbSet<OfficialReceipt> OfficialReceipts { get; set; }
        public DbSet<OfficialReceiptDetailBank> OfficialReceiptDetailBanks { get; set; }
        public DbSet<OfficialReceiptDetailInvoice> OfficialReceiptDetailInvoices { get; set; }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<PaymentRequestDetail> PaymentRequestDetails { get; set; }
        public DbSet<SeaContainer> SeaContainers { get; set; }
        public DbSet<ShipmentAdvice> ShipmentAdvices { get; set; }
        public DbSet<ShipmentInstruction> ShipmentInstructions { get; set; }
        public DbSet<ShipmentOrder> ShipmentOrders { get; set; }
        public DbSet<ShipmentOrderRouting> ShipmentOrdersRoutings { get; set; }
        public DbSet<TelexRelease> TelexReleases { get; set; }
        public DbSet<TemporaryPayment> TemporaryPayment { get; set; }
        public DbSet<TemporaryPaymentJob> TemporaryPaymentJobs { get; set; }
        public DbSet<TemporaryReceipt> TemporaryReceipts { get; set; }
        public DbSet<TemporaryReceiptJob> TemporaryReceiptJob { get; set; }
        public DbSet<TruckOrder> TruckOrders { get; set; } 

        #endregion


        #region UserRole
        public DbSet<AccessUser> AccessUser { get; set; }
        public DbSet<AccountUser> AccountUser { get; set; }
        public DbSet<MenuUser> MenuUser { get; set; } 
        #endregion



    }
}
