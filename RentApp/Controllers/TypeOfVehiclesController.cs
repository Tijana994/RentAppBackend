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
    [RoutePrefix("api/TypeOfVehicle")]
    public class TypeOfVehiclesController : ApiController
    {
        private IUnitOfWork db { get; set; }

        public TypeOfVehiclesController(IUnitOfWork db)
        {
            this.db = db;

        }



        // GET: api/TypeOfVehicles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAllTypeOfVehicles")]
        public IEnumerable<TypeOfVehicle> GetAllTypeOfVehicles()
        {
            return db.TypeOfVehicles.GetAll();
        }

        // GET: api/TypeOfVehicles/5
        [HttpGet]
        [Route("GetTypeOfVehicles/{id}")]
        [ResponseType(typeof(TypeOfVehicle))]
        public IHttpActionResult GetTypeOfVehicle(int id)
        {
            TypeOfVehicle typeOfVehicle = db.TypeOfVehicles.Get(id);
            if (typeOfVehicle == null)
            {
                return NotFound();
            }

            return Ok(typeOfVehicle);
        }

        // PUT: api/TypeOfVehicles/5
        [HttpPut]
        [Authorize(Roles ="Admin")]
        [Route("PutTypeOfVehicle/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTypeOfVehicle(int id, TypeOfVehicle type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /* if (db.TypeOfVehicles.Get(id) == null)
             {
                 return BadRequest();
             }*/

            if (db.TypeOfVehicles.AsNoTracking().Any(x => x.Name.Equals(type.Name) && x.Id != type.Id))
            {

                return BadRequest("This name is not unique");


            }





            try
            {
                //type.RowVersion = db.AppUsers.AsNoTracking().FirstOrDefault(x => x.Id == type.Id).RowVersion;
                db.TypeOfVehicles.Update(type);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                return BadRequest("Somebody change this type already");
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TypeOfVehicles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("PostTypeOfVehicle")]
        [ResponseType(typeof(TypeOfVehicle))]
        public IHttpActionResult PostTypeOfVehicle(TypeOfVehicle typeOfVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (db.TypeOfVehicles.Any(x => x.Name == typeOfVehicle.Name))
            {
                return BadRequest("Name already exists");
            }
            db.TypeOfVehicles.Add(typeOfVehicle);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest("Internal error, try again");
            }
           

            return Ok();
        }

        // DELETE: api/TypeOfVehicles/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("DeleteTypeOfVehicle/{id}")]
        [ResponseType(typeof(TypeOfVehicle))]
        public IHttpActionResult DeleteTypeOfVehicle(int id)
        {
            TypeOfVehicle typeOfVehicle = db.TypeOfVehicles.Get(id);
            if (typeOfVehicle == null)
            {
                return NotFound();
            }

            db.TypeOfVehicles.Remove(typeOfVehicle);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                return BadRequest("Somebody change this type already");
            }


            return Ok(typeOfVehicle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TypeOfVehicleExists(int id)
        {
            return db.TypeOfVehicles.Find(e => e.Id == id).Count() > 0;
        }
    }
}