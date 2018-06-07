using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Repo
{
    public class AppUserRepository : Repository<AppUser>,IAppUserRepository
    {
        public AppUserRepository(DbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public DbContext ApplicationDbContext
        {
            get { return Context as RADBContext; }
        }

     
    }
}