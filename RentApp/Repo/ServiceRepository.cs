using RentApp.Models.Entities;
using RentApp.Persistance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RentApp.Repo
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(DbContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public IEnumerable<Service> GetAll(int pageIndex, int pageSize)
        {
            return ApplicationDbContext.Services.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        protected  RADBContext ApplicationDbContext { get { return Context as RADBContext; } }
    }
}