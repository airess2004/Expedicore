using Core.DomainModel;
using Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class ExpedicoEntities : DbContext
    {
        public ExpedicoEntities() : base("Expedico")
        {
            Database.SetInitializer<ExpedicoEntities>(new DropCreateDatabaseIfModelChanges<ExpedicoEntities>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            #region Master
            modelBuilder.Configurations.Add(new AirlineMapping());
            modelBuilder.Configurations.Add(new AirportMapping());
            modelBuilder.Configurations.Add(new CityLocationMapping());
            modelBuilder.Configurations.Add(new ContactMapping());
            modelBuilder.Configurations.Add(new ContactTypeMapping());
            modelBuilder.Configurations.Add(new ContinentMapping());
            modelBuilder.Configurations.Add(new CostMapping());
            modelBuilder.Configurations.Add(new CountryLocationMapping());
            modelBuilder.Configurations.Add(new GroupContactMapping());
            modelBuilder.Configurations.Add(new GroupMapping());
            modelBuilder.Configurations.Add(new JobMapping());
            modelBuilder.Configurations.Add(new MstBillOfLadingMapping());
            modelBuilder.Configurations.Add(new OfficeMapping());
            modelBuilder.Configurations.Add(new PortMapping());
            modelBuilder.Configurations.Add(new VesselMapping()); 
            #endregion

            #region Transaction
            modelBuilder.Configurations.Add(new BillOfLadingMapping());
            modelBuilder.Configurations.Add(new DeliveryOrderMapping());
            modelBuilder.Configurations.Add(new EstimateProfitLossDetailMapping());
            modelBuilder.Configurations.Add(new EstimateProfitLossMapping());
            modelBuilder.Configurations.Add(new NoticeOfArrivalMapping());
            modelBuilder.Configurations.Add(new SeaContainerMapping());
            modelBuilder.Configurations.Add(new ShipmentAdviceMapping());
            modelBuilder.Configurations.Add(new ShipmentInstructionMapping()); 
            #endregion


            #region UserRole
            modelBuilder.Configurations.Add(new AccessUserMapping());
            modelBuilder.Configurations.Add(new AccountUserMapping());
            modelBuilder.Configurations.Add(new MenuUserMapping()); 
            #endregion


            base.OnModelCreating(modelBuilder);
        }

        #region Master
        public DbSet<Airline> AirLines { get; set; }
        public DbSet<Airport> AirPorts { get; set; }
        public DbSet<CityLocation> CityLocations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<Continent> Continents { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<CountryLocation> CountryLocations { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupContact> GroupContacts { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<MstBillOfLading> MstBillOfLadings { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Vessel> Vessels { get; set; } 
        #endregion

        #region Transcantion
        public DbSet<BillOfLading> BillOfLaddings { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public DbSet<EstimateProfitLoss> EstimateProfitLosses { get; set; }
        public DbSet<EstimateProfitLossDetail> EstimateProfitLossDetails { get; set; }
        public DbSet<NoticeOfArrival> NoticeOfArrivals { get; set; }
        public DbSet<SeaContainer> SeaContainers { get; set; }
        public DbSet<ShipmentAdvice> ShipmentAdvices { get; set; }
        public DbSet<ShipmentInstruction> ShipmenetInstructions { get; set; }
        public DbSet<ShipmentOrder> ShipmentOrders { get; set; }
        public DbSet<ShipmentOrderRouting> ShipmentOrdersRoutings { get; set; }
        public DbSet<TelexRelease> TelexReleases { get; set; }
        #endregion


        #region UserRole
        public DbSet<AccessUser> AccessUser { get; set; }
        public DbSet<AccountUser> AccountUser { get; set; }
        public DbSet<MenuUser> MenuUser { get; set; } 
        #endregion



    }
}
