﻿using RentApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentApp.Repo
{
    public interface IVehicleRepository: IRepository<Vehicle>
    {
        // to do
        IEnumerable<Vehicle> GetAll(int pageIndex, int pageSize, string manuName, string modelName, string year, int fromPrice, int toPrice, string type, int serviceId);
    }
}
