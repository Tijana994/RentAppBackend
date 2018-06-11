﻿using System;
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
using RentApp.Models;

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

        [HttpGet]
        [Route("Pagination/{pageNumber}/{pageSize}")]
        [ResponseType(typeof(Vehicle))]
        public ICollection<Vehicle> Pagination(int pageNumber, int pageSize)
        { 
            return db.Vehicles.GetAll(pageNumber,pageSize).ToList();
        }

        // PUT: api/Vehicles/5
        [HttpPut]
        [Authorize(Roles = "Manager")]
        [Route("PutVehicle/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVehicle(int id, Vehicle vehicle)
        {
            vehicle.Service = db.Services.Get(vehicle.ServiceId);
            vehicle.TypeOfVehicle = db.TypeOfVehicles.Get(vehicle.TypeOfVehicleId);
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

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
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetPrice/{id}")]
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult GetPrice(int id)
        {
            Vehicle vehicle = db.Vehicles.Get(id);
            if (vehicle == null)
            {
                return BadRequest();
            }


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



            return Ok(price);
        }


        // POST: api/Vehicles
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("PostVehicle")]
        [ResponseType(typeof(Vehicle))]
        public IHttpActionResult PostVehicle(Vehicle vehicle)
        {
            vehicle.Available = true;
            vehicle.Service = db.Services.Get(vehicle.ServiceId);
            vehicle.TypeOfVehicle = db.TypeOfVehicles.Get(vehicle.TypeOfVehicleId);
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

           
            
            db.Vehicles.Add(vehicle);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            

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
                return BadRequest("Bad id");
            }

            List<Reservation> reservations = new List<Reservation>();
            List<BranchReservation> branchReservations = new List<BranchReservation>();

            reservations.AddRange(db.Reservations.Find(x => x.VehicleId == id));


            foreach (var item in reservations)
            {
                branchReservations.AddRange(db.BranchReservations.Find(x => x.ReservationId == item.Id));
            }


            db.BranchReservations.RemoveRange(branchReservations);
            db.Reservations.RemoveRange(reservations);


            try
            {
                db.Vehicles.Remove(vehicle);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest("Somebody already delete Vehicle");
            }


            return Ok("Successfuly deleted Vehicle");
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