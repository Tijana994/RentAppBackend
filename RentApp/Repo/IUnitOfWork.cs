using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Repo
{
    public interface IUnitOfWork : IDisposable
    {
        IAppUserRepository AppUsers { get; set; }
        IBranchRepository Branches { get; set;  }
        IBranchReservationRepository BranchReservations { get; set; }
        IPicRepository Pics { get; set; }
        IPriceListRepository PriceLists { get; set; }
        IRateRepository Rates { get; set; }
        IReservationRepository Reservations { get; set; }
        IServiceRepository Services { get; set;  }
        ITypeOfVehicleRepository TypeOfVehicles { get; set; }
        IVehicleRepository Vehicles { get; set; }
        int SaveChanges();
    }
}
