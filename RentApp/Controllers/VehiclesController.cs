using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RentApp.Models.Entities;
using RentApp.Persistance;
using RentApp.Repo;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Vehicles")]
    public class VehiclesController : ApiController
    {
        private IUnitOfWork db { get; set; }

        public VehiclesController(IUnitOfWork db)
        {
            this.db = db;

        }

        // GET: api/Vehicles
        [HttpGet]
        [Route("GetAllVehicles")]
        [ResponseType(typeof(Vehicle))]
        public IEnumerable<Vehicle> GetVehicles()
        {
            return db.Vehicles.GetAll();
        }

        // GET: api/Vehicles/5
        [HttpGet]
        [Route("GetVehicle/{id}")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult GetVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        // PUT: api/Vehicles/5
        [HttpPut]
        [Authorize(Roles = "Manager")]
        [Route("PutVehicle")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(int id, Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            db.Vehicles.Update(vehicle);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Vehicles
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("PostVehicle")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            db.Vehicles.Add(vehicle);
 
            db.SaveChanges();

            return Ok(vehicle);
        }

        // DELETE: api/Vehicles/5
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        [Route("DeleteVehicle/{id}")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult DeleteVehicle(int id)
        {
            Vehicle vehicle = db.Vehicles.Get(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();

            return Ok(vehicle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VehicleExists(int id)
        {
            return db.Vehicles.Find(e => e.Id == id).Count() > 0;
        }
    }
}