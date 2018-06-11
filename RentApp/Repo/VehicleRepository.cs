
using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Repo
{
    public class VehicleRepository: Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(DbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        protected RADBContext ApplicationDbContext { get { return Context as RADBContext; } }

        public IEnumerable<Vehicle> GetAll(int pageIndex, int pageSize)
        {
            
            return ApplicationDbContext.Vehicles.OrderBy(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}