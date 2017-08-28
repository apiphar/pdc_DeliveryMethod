using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Entities
{
    public class LogisticDbContext : DbContext
    {
        public LogisticDbContext(DbContextOptions<LogisticDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AFIApplicationNumber>().HasKey(entity => new
            {
                entity.AFIBranchCode,
                entity.Year,
            });

            modelBuilder.Entity<AppRoleMenuMapping>().HasKey(entity => new
            {
                entity.AppMenuName,
                entity.AppRoleName,
            });

            modelBuilder.Entity<CarType>().HasKey(entity => new
            {
                entity.Katashiki,
                entity.Suffix,
            });

            modelBuilder.Entity<Dwelling>().HasKey(entity => new
            {
                entity.LocationFrom,
                entity.LocationTo,
            });

            modelBuilder.Entity<Holiday>().HasKey(entity => new
            {
                entity.HolidayDate,
                entity.LocationCode,
            });

            modelBuilder.Entity<PortLocationResponsibility>().HasKey(entity => new
            {
                entity.PortLocationCode,
                entity.ResponsibleLocationCode,
            });

            modelBuilder.Entity<PreBookVesselLocationMapping>().HasKey(entity => new
            {
                entity.LocationCode,
                entity.ProcessMasterCode,
            });

            modelBuilder.Entity<PreDeliveryCenterDelivery>().HasKey(entity => new
            {
                entity.BranchCode,
                entity.LocationCode,
            });

            modelBuilder.Entity<ProcessDictionaryDetail>().HasKey(entity => new
            {
                entity.Ordering,
                entity.ProcessDictionaryId,
            });

            modelBuilder.Entity<ProcessHeadTemplateDetail>().HasKey(entity => new
            {
                entity.Ordering,
                entity.ProcessHeadTemplateCode,
            });

            modelBuilder.Entity<ProcessLeadTimeForLocation>().HasKey(entity => new
            {
                entity.LocationCode,
                entity.ProcessMasterCode,
            });

            modelBuilder.Entity<ProcessTailTemplateDetail>().HasKey(entity => new
            {
                entity.ProcessTailTemplateCode,
                entity.Ordering,
            });

            modelBuilder.Entity<ScratchConfiguration>().HasKey(entity => new
            {
                entity.BranchCode,
                entity.CarModelCode,
            });

            modelBuilder.Entity<SuratPengantarFakturDetail>().HasKey(entity => new
            {
                entity.NomorSuratPengantarFaktur,
                entity.VehicleId,
            });

            modelBuilder.Entity<VoyageNodeSourceDetail>().HasKey(entity => new
            {
                entity.VehicleId,
                entity.VoyageNodeSourceId,
            });
        }

        public virtual DbSet<AFIApplication> AFIApplication { get; set; }

        public virtual DbSet<AFIApplicationNumber> AFIApplicationNumber { get; set; }

        public virtual DbSet<AFIApplicationProcessEnum> AFIApplicationProcessEnum { get; set; }

        public virtual DbSet<AFIBranch> AFIBranch { get; set; }

        public virtual DbSet<AFICarType> AFICarType { get; set; }

        public virtual DbSet<AFIRegion> AFIRegion { get; set; }

        public virtual DbSet<AFIRegionRestriction> AFIRegionRestriction { get; set; }

        public virtual DbSet<AFISubmissionTypeEnum> AFISubmissionTypeEnum { get; set; }

        public virtual DbSet<AppMenu> AppMenu { get; set; }

        public virtual DbSet<AppRole> AppRole { get; set; }

        public virtual DbSet<AppRoleMenuMapping> AppRoleMenuMapping { get; set; }

        public virtual DbSet<AS400Cluster> AS400Cluster { get; set; }

        public virtual DbSet<AS400FrameNumber> AS400FrameNumber { get; set; }

        public virtual DbSet<Blob> Blob { get; set; }

        public virtual DbSet<Branch> Branch { get; set; }

        public virtual DbSet<BranchLocationMapping> BranchLocationMapping { get; set; }

        public virtual DbSet<BranchPricingComponent> BranchPricingComponent { get; set; }

        public virtual DbSet<Brand> Brand { get; set; }

        public virtual DbSet<BreakHourTemplate> BreakHourTemplate { get; set; }

        public virtual DbSet<BreakHourTemplateDetail> BreakHourTemplateDetail { get; set; }

        public virtual DbSet<CarModel> CarModel { get; set; }

        public virtual DbSet<CarSeries> CarSeries { get; set; }

        public virtual DbSet<CarType> CarType { get; set; }

        public virtual DbSet<CityForLeg> CityForLeg { get; set; }

        public virtual DbSet<CityForShipment> CityForShipment { get; set; }

        public virtual DbSet<CityLeg> CityLeg { get; set; }

        public virtual DbSet<CityLegCost> CityLegCost { get; set; }

        public virtual DbSet<Company> Company { get; set; }

        public virtual DbSet<CompanyPlafond> CompanyPlafond { get; set; }

        public virtual DbSet<CompanyPlafondMutation> CompanyPlafondMutation { get; set; }

        public virtual DbSet<Dealer> Dealer { get; set; }

        public virtual DbSet<DealerType> DealerType { get; set; }

        public virtual DbSet<DebitAdvice> DebitAdvice { get; set; }

        public virtual DbSet<DeliveryDriver> DeliveryDriver { get; set; }

        public virtual DbSet<DeliveryLeadTime> DeliveryLeadTime { get; set; }

        public virtual DbSet<DeliveryLeg> DeliveryLeg { get; set; }

        public virtual DbSet<DeliveryMethod> DeliveryMethod { get; set; }

        public virtual DbSet<DeliveryOrder> DeliveryOrder { get; set; }

        public virtual DbSet<DeliveryOrderDetail> DeliveryOrderDetail { get; set; }

        public virtual DbSet<DeliveryOrderDetailPriceComponent> DeliveryOrderDetailPriceComponent { get; set; }

        public virtual DbSet<DeliveryRequest> DeliveryRequest { get; set; }

        public virtual DbSet<DeliveryRequestTransitTypeEnum> DeliveryRequestTransitTypeEnum { get; set; }

        public virtual DbSet<DeliveryRequestTypeEnum> DeliveryRequestTypeEnum { get; set; }

        public virtual DbSet<DeliveryVendor> DeliveryVendor { get; set; }

        public virtual DbSet<DeliveryVendorVehicle> DeliveryVendorVehicle { get; set; }

        public virtual DbSet<Destination> Destination { get; set; }

        public virtual DbSet<DiscountConfiguration> DiscountConfiguration { get; set; }

        public virtual DbSet<Dwelling> Dwelling { get; set; }

        public virtual DbSet<EngineMaster> EngineMaster { get; set; }

        public virtual DbSet<ExteriorColor> ExteriorColor { get; set; }

        public virtual DbSet<FileJob> FileJob { get; set; }

        public virtual DbSet<Holiday> Holiday { get; set; }

        public virtual DbSet<InteriorColor> InteriorColor { get; set; }

        public virtual DbSet<Location> Location { get; set; }

        public virtual DbSet<LocationBreakHour> LocationBreakHour { get; set; }

        public virtual DbSet<LocationType> LocationType { get; set; }

        public virtual DbSet<LocationWorkHour> LocationWorkHour { get; set; }

        public virtual DbSet<PDILeadTime> PDILeadTime { get; set; }

        public virtual DbSet<PDILine> PDILine { get; set; }

        public virtual DbSet<PIOLine> PIOLine { get; set; }

        public virtual DbSet<PIOLineDetail> PIOLineDetail { get; set; }

        public virtual DbSet<Plant> Plant { get; set; }

        public virtual DbSet<PortLocationResponsibility> PortLocationResponsibility { get; set; }

        public virtual DbSet<PreBookVesselLocationMapping> PreBookVesselLocationMapping { get; set; }

        public virtual DbSet<PreDeliveryCenter> PreDeliveryCenter { get; set; }

        public virtual DbSet<PreDeliveryCenterDelivery> PreDeliveryCenterDelivery { get; set; }

        public virtual DbSet<PricingComponent> PricingComponent { get; set; }

        public virtual DbSet<PricingComponentLookup> PricingComponentLookup { get; set; }

        public virtual DbSet<PricingTypeEnum> PricingTypeEnum { get; set; }

        public virtual DbSet<ProcessDictionary> ProcessDictionary { get; set; }

        public virtual DbSet<ProcessDictionaryDetail> ProcessDictionaryDetail { get; set; }

        public virtual DbSet<ProcessHeadTemplate> ProcessHeadTemplate { get; set; }

        public virtual DbSet<ProcessHeadTemplateDetail> ProcessHeadTemplateDetail { get; set; }

        public virtual DbSet<ProcessHeadTemplateMapping> ProcessHeadTemplateMapping { get; set; }

        public virtual DbSet<ProcessLeadTimeByEnum> ProcessLeadTimeByEnum { get; set; }

        public virtual DbSet<ProcessLeadTimeForLocation> ProcessLeadTimeForLocation { get; set; }

        public virtual DbSet<ProcessMaster> ProcessMaster { get; set; }

        public virtual DbSet<ProcessTailTemplate> ProcessTailTemplate { get; set; }

        public virtual DbSet<ProcessTailTemplateDetail> ProcessTailTemplateDetail { get; set; }

        public virtual DbSet<ProcessTailTemplateMapping> ProcessTailTemplateMapping { get; set; }

        public virtual DbSet<Region> Region { get; set; }

        public virtual DbSet<SalesArea> SalesArea { get; set; }

        public virtual DbSet<Scratch> Scratch { get; set; }

        public virtual DbSet<ScratchConfiguration> ScratchConfiguration { get; set; }

        public virtual DbSet<ScratchHandOver> ScratchHandOver { get; set; }

        public virtual DbSet<Shift> Shift { get; set; }

        public virtual DbSet<SPULine> SPULine { get; set; }

        public virtual DbSet<SPULineDetail> SPULineDetail { get; set; }

        public virtual DbSet<SuratPengantarFaktur> SuratPengantarFaktur { get; set; }

        public virtual DbSet<SuratPengantarFakturDetail> SuratPengantarFakturDetail { get; set; }

        public virtual DbSet<Swapping> Swapping { get; set; }

        public virtual DbSet<UserMapping> UserMapping { get; set; }

        public virtual DbSet<Vehicle> Vehicle { get; set; }

        public virtual DbSet<VehicleHold> VehicleHold { get; set; }

        public virtual DbSet<VehicleRouting> VehicleRouting { get; set; }

        public virtual DbSet<VehicleVoyageStatusEnum> VehicleVoyageStatusEnum { get; set; }

        public virtual DbSet<Voyage> Voyage { get; set; }

        public virtual DbSet<VoyageNode> VoyageNode { get; set; }

        public virtual DbSet<VoyageNodeSource> VoyageNodeSource { get; set; }

        public virtual DbSet<VoyageNodeSourceDetail> VoyageNodeSourceDetail { get; set; }

        public virtual DbSet<VoyageStatusEnum> VoyageStatusEnum { get; set; }

        public virtual DbSet<WorkHourTemplate> WorkHourTemplate { get; set; }

        public virtual DbSet<WorkHourTemplateDetail> WorkHourTemplateDetail { get; set; }
    }
}