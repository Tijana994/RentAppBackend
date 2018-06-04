using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Unity.Attributes;

namespace RentApp.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext applicationDbContext;

        public UnitOfWork(DbContext context)
        {
            applicationDbContext = context;
            
        }

        [Dependency]
        public IAppUserRepository AppUsers { get; set; }
        [Dependency]
        public IBranchRepository Branches { get; set; }
        [Dependency]
        public IBranchReservationRepository BranchReservations { get; set; }
        [Dependency]
        public IPicRepository Pics { get; set; }
        [Dependency]
        public IPriceListRepository PriceLists { get; set; }
        [Dependency]
        public IRateRepository Rates { get; set; }
        [Dependency]
        public IReservationRepository Reservations { get; set; }
        [Dependency]
        public IServiceRepository Services { get; set; }
        [Dependency]
        public ITypeOfVehicleRepository TypeOfVehicles { get; set; }
        [Dependency]
        public IVehicleRepository Vehicles { get; set; }

        public int SaveChanges()
        {
            return applicationDbContext.SaveChanges();
        }

        public void Dispose()
        {
            applicationDbContext.Dispose();
        }
    }
}