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
    [RoutePrefix("api/Services")]
    public class ServicesController : ApiController
    {
        private IUnitOfWork db;

        public ServicesController(IUnitOfWork db)
        {
            this.db = db;
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
        [Route("PutService")]
        [ResponseType(typeof(void))]
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

            db.Services.Update(service);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

            db.Services.Add(service);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = service.Id }, service);
        }

        // DELETE: api/Services/5
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        [Route("DeleteService")]
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {
            Service service = db.Services.Get(id);
            if (service == null)
            {
                return NotFound();
            }

            db.Services.Remove(service);
            db.SaveChanges();

            return Ok(service);
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