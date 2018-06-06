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
    public class TypeOfVehiclesController : ApiController
    {
        private IUnitOfWork db { get; set; }

        public TypeOfVehiclesController(IUnitOfWork db)
        {
            this.db = db;

        }

        // GET: api/TypeOfVehicles
        public IEnumerable<TypeOfVehicle> GetTypeOfVehicles()
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
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTypeOfVehicle(int id, TypeOfVehicle typeOfVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != typeOfVehicle.Id)
            {
                return BadRequest();
            }

            db.TypeOfVehicles.Update(typeOfVehicle);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeOfVehicleExists(id))
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

        // POST: api/TypeOfVehicles
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(TypeOfVehicle))]
        public IHttpActionResult PostTypeOfVehicle(TypeOfVehicle typeOfVehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TypeOfVehicles.Add(typeOfVehicle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = typeOfVehicle.Id }, typeOfVehicle);
        }

        // DELETE: api/TypeOfVehicles/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(TypeOfVehicle))]
        public IHttpActionResult DeleteTypeOfVehicle(int id)
        {
            TypeOfVehicle typeOfVehicle = db.TypeOfVehicles.Get(id);
            if (typeOfVehicle == null)
            {
                return NotFound();
            }

            db.TypeOfVehicles.Remove(typeOfVehicle);
            db.SaveChanges();

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