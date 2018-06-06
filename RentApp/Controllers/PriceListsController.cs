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
    [RoutePrefix("api/PriceLists")]
    public class PriceListsController : ApiController
    {
        private IUnitOfWork db { get; set; }

        public PriceListsController(IUnitOfWork db)
        {
            this.db = db;

        }

        // GET: api/PriceLists/5
        [HttpGet]
        [Route("GetPriceList/{id}")]
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult GetPriceList(int id)
        {
            PriceList priceList = db.PriceLists.Get(id);
            if (priceList == null)
            {
                return NotFound();
            }

            return Ok(priceList);
        }

        // PUT: api/PriceLists/5
        [HttpPut]
        [Authorize(Roles = "Manager")]
        [Route("PutPriceList")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPriceList(int id, PriceList priceList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceList.Id)
            {
                return BadRequest();
            }

            db.PriceLists.Update(priceList);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceListExists(id))
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

        // POST: api/PriceLists
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("PostPriceList")]
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult PostPriceList(PriceList priceList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PriceLists.Add(priceList);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = priceList.Id }, priceList);
        }

        // DELETE: api/PriceLists/5
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        [Route("DeletePriceList")]
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult DeletePriceList(int id)
        {
            PriceList priceList = db.PriceLists.Get(id);
            if (priceList == null)
            {
                return NotFound();
            }

            db.PriceLists.Remove(priceList);
            db.SaveChanges();

            return Ok(priceList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceListExists(int id)
        {
            return db.PriceLists.Find(e => e.Id == id).Count() > 0;
        }
    }
}