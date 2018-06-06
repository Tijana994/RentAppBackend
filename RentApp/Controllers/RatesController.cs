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
    [RoutePrefix("api/Rates")]
    public class RatesController : ApiController
    {
        private IUnitOfWork db { get; set; }

        public RatesController(IUnitOfWork db)
        {
            this.db = db;

        }

        // GET: api/Rates/5
        [HttpGet]
        [Route("GetRate/{id}")]
        [ResponseType(typeof(Rate))]
        public IHttpActionResult GetRate(int id)
        {
            Rate rate = db.Rates.Get(id);
            if (rate == null)
            {
                return NotFound();
            }

            return Ok(rate);
        }

        // PUT: api/Rates/5
        [HttpPut]
        [Authorize]
        [Route("PutRate")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRate(int id, Rate rate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rate.Id)
            {
                return BadRequest();
            }

            db.Rates.Update(rate);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RateExists(id))
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

        // POST: api/Rates
        [HttpPost]
        [Authorize]
        [Route("PostRate")]
        [ResponseType(typeof(Rate))]
        public IHttpActionResult PostRate(Rate rate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rates.Add(rate);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = rate.Id }, rate);
        }

        // DELETE: api/Rates/5
        [HttpDelete]
        [Authorize]
        [Route("DeleteRate")]
        [ResponseType(typeof(Rate))]
        public IHttpActionResult DeleteRate(int id)
        {
            Rate rate = db.Rates.Get(id);
            if (rate == null)
            {
                return NotFound();
            }

            db.Rates.Remove(rate);
            db.SaveChanges();

            return Ok(rate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RateExists(int id)
        {
            return db.Rates.Find(e => e.Id == id).Count() > 0;
        }
    }
}