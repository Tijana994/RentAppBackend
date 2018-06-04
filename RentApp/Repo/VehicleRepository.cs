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

        public DbContext ApplicationDbContext
        {
            get { return Context as DbContext; }
        }
    }
}