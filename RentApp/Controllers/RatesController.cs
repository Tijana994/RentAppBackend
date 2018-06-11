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

        // GET: api/Rated
        [HttpGet]
        [Route("GetAllRates")]
        [ResponseType(typeof(Rate))]
        public IEnumerable<Rate> GetRates()
        {
            return db.Rates.GetAll();
        }


        [HttpGet]
        [Route("GetAllRatesUser/{id}")]
        [ResponseType(typeof(Rate))]
        public IEnumerable<Rate> GetAllRatesUser(int id)
        {
            return db.Rates.Find(x => x.AppUserId == id);
        }

        [HttpGet]
        [Route("GetAllRatesService/{id}")]
        [ResponseType(typeof(Rate))]
        public IEnumerable<Rate> GetAllRatesService(int id)
        {
            return db.Rates.Find(x => x.ServiceId == id);
        }

        [HttpGet]
        [Route("CanLeaveComment/{id}/{serviceId}")]
        public bool CanLeaveComment(int id, int serviceId)
        {
            return db.Reservations.Any(x => x.AppUserId == id &&  x.Expired == true  && x.Vehicle.ServiceId == serviceId) && !db.Rates.Any(x => x.AppUserId == id);
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
        [Route("PutRate/{id}")]
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
                return BadRequest();
            }

            List<int> grades = new List<int>();

            foreach (var item in db.Services.Get(rate.Id).Rates)
            {
                grades.Add(item.Point);
            }

            db.Services.Get(rate.Id).AverageMark = grades.Average();


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

            if (!db.Reservations.Any(x => x.AppUserId == rate.AppUserId && x.Expired == true && x.Vehicle.ServiceId == rate.ServiceId) && db.Rates.Any(x => x.AppUserId == rate.Id))
            {
                return BadRequest();
            }

            db.Rates.Add(rate);

            db.SaveChanges();

            List<int> grades = new List<int>();

            foreach (var item in db.Services.Get(rate.Id).Rates)
            {
                grades.Add(item.Point);
            }

            db.Services.Get(rate.Id).AverageMark = grades.Average();


            db.SaveChanges();

            return Ok(rate);
        }

        // DELETE: api/Rates/5
        [HttpDelete]
        [Authorize]
        [Route("DeleteRate/{id}")]
        [ResponseType(typeof(Rate))]
        public IHttpActionResult DeleteRate(int id)
        {
            Rate rate = db.Rates.Get(id);
            if (rate == null)
            {
                return NotFound();
            }

            db.Rates.Remove(rate);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            List<int> grades = new List<int>();

            foreach (var item in db.Services.Get(rate.Id).Rates)
            {
                grades.Add(item.Point);
            }

            db.Services.Get(rate.Id).AverageMark = grades.Average();


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            };

            return Ok();
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