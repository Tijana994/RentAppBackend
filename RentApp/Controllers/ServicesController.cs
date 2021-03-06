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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using RentApp.Models.Entities;
using RentApp.Persistance;
using RentApp.Repo;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Services")]
    public class ServicesController : ApiController
    {
        private IUnitOfWork db;

        public ServicesController(IUnitOfWork db)
        {
            this.db = db;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: api/Services
        [HttpGet]
        [Route("GetAllServices")]
        [ResponseType(typeof(Service))]
        public IEnumerable<Service> GetServices()
        {
            return db.Services.GetAll();
        }

        // GET: api/Services/5
        [HttpGet]
        [Route("GetService/{id}")]
        [ResponseType(typeof(Service))]
        public IHttpActionResult GetService(int id)
        {
            Service service = db.Services.Get(id);
            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        // PUT: api/Services/5
        [HttpPut]
        [Authorize(Roles = "Manager")]
        [Route("PutService/{id}")]
        [ResponseType(typeof(Service))]
        public IHttpActionResult PutService(int id, Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != service.Id)
            {
                return BadRequest();
            }

            var user = db.AppUsers.FirstOrDefault(u => u.Username == User.Identity.Name);

            if (service.AppUserId != user.Id)
            {
                return BadRequest("Service doesn`t exists");
            }

            if (db.Services.Any(x => x.Name == service.Name && x.Id != service.Id))
            {
                return BadRequest("Name must be unique");
            }

            db.Services.Update(service);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                return BadRequest("Somebody already changed service");
            }

            return Ok(db.Services.Get(id));
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("ApproveService/{id}")]
        public IHttpActionResult ApproveService(int id, Service service)
        {

            if (!db.Services.AsNoTracking().Any(x => x.Id == id))
            {
                return BadRequest("Bad id");
            }

            db.Services.Get(id).Approved = service.Approved;

            try
            {
               
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest("Somebody already changed this service");
            }
            var userStore = new UserStore<RAIdentityUser>(new RADBContext());
            var userManager = new UserManager<RAIdentityUser>(userStore);
            AppUser userko = db.AppUsers.Get(db.Services.Get(id).AppUserId);
            string Email = userManager.FindByName(userko.Username).Email;
            HelperController.sendServiceConfirmationEmail(Email, db.Services.Get(id).Name);

            return Ok();
        }

        // POST: api/Services
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("PostService")]
        [ResponseType(typeof(Service))]
        public IHttpActionResult PostService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.Services.Any(x => x.Name == service.Name))
            {
                return BadRequest("Name already exists");
            }
            db.Services.Add(service);
            db.SaveChanges();
            Hubs.NotificationHub.SendNotification("New service created. To preview it, go to admin panel.");
            return Ok(db.Services.FirstOrDefault(x => x.Name == service.Name).Id);
        }

        // DELETE: api/Services/5
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        [Route("DeleteService/{id}")]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Get(id);
            if (service == null)
            {
                return BadRequest();
            }

            List<BranchReservation> pom = new List<BranchReservation>();
            List<Reservation> res = new List<Reservation>();
            List<Rate> rates = new List<Rate>();

            rates = db.Rates.Find(x => x.ServiceId == id).ToList();

            foreach (var branch in db.Branches.Find(x => x.ServiceId == id))
            {
                pom.AddRange(db.BranchReservations.Find(x => x.BranchId == branch.Id));
            }

            foreach (var item in db.Vehicles.Find(x => x.ServiceId == id))
            {
                res.AddRange(db.Reservations.Find(x => x.VehicleId == item.Id));
            }

            db.Rates.RemoveRange(rates);


            db.BranchReservations.RemoveRange(pom);

            db.Reservations.RemoveRange(res);

            db.Services.Remove(service);
            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }

            return Ok("Successfuly deleted service");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceExists(int id)
        {
            return db.Services.FirstOrDefault(e => e.Id == id) != null;
        }
    }
}