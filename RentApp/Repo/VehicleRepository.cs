
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

        public IEnumerable<Vehicle> GetAll(int pageIndex, int pageSize, string manuName, string modelName, string year, int fromPrice, int toPrice, string type, int serviceId)
        {
            List<Vehicle> firstList = ApplicationDbContext.Vehicles.ToList();
            if (!manuName.Equals("*")) firstList = firstList.Where(x => x.Mark.ToUpper().Equals(manuName.ToUpper())).ToList();
            if (!modelName.Equals("*")) firstList = firstList.Where(x => x.Model.ToUpper().Equals(modelName.ToUpper())).ToList();
            if (!year.Equals("*")) firstList = firstList.Where(x => x.Year.Equals(year)).ToList();
            if (serviceId != -1) firstList = firstList.Where(x => x.ServiceId == serviceId).ToList();

            List<Vehicle> returnList = firstList.Where(x =>
            price(x).Price >= fromPrice &&
            price(x).Price <= toPrice).ToList();

            if (type.Equals("All")) return returnList.OrderBy(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            else
            {
                var t = ApplicationDbContext.TypeOfVehicles.FirstOrDefault(x => x.Name.Equals(type));
                return returnList.Where(x => x.TypeOfVehicleId == t.Id).ToList().OrderBy(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        private PriceList price(Vehicle vehicle)
        {
            PriceList price = null;

            foreach (var item in vehicle.PriceLists)
            {
                if (item.StartDate < DateTime.Now && item.EndDate > DateTime.Now)
                {
                    price = item;
                }
            }

            if (price == null)
            {
                price = vehicle.PriceLists.Last();
            }

            return price;
        }
    }
}