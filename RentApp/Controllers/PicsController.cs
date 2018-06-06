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
    [RoutePrefix("api/Pics")]
    public class PicsController : ApiController
    {
        private IUnitOfWork db;

        public PicsController(IUnitOfWork db)
        {
            this.db = db;
        }

        // GET: api/Pics
        public IEnumerable<Pic> GetPics()
        {
            return db.Pics.GetAll();
        }

        // GET: api/Pics/5
        [HttpGet]
        [Route("GetPic/{id}")]
        [ResponseType(typeof(Pic))]
        public IHttpActionResult GetPic(int id)
        {
            Pic pic = db.Pics.Get(id);
            if (pic == null)
            {
                return NotFound();
            }

            return Ok(pic);
        }

        // PUT: api/Pics/5
        [HttpPut]
        [Authorize]
        [Route("PutPic")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPic(int id, Pic pic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pic.Id)
            {
                return BadRequest();
            }

            db.Pics.Update(pic);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PicExists(id))
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

        // POST: api/Pics
        [HttpPost]
        [Authorize]
        [Route("PostPic")]
        [ResponseType(typeof(Pic))]
        public IHttpActionResult PostPic(Pic pic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pics.Add(pic);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pic.Id }, pic);
        }

        // DELETE: api/Pics/5
        [HttpDelete]
        [Authorize]
        [Route("DeletePic")]
        [ResponseType(typeof(Pic))]
        public IHttpActionResult DeletePic(int id)
        {
            Pic pic = db.Pics.Get(id);
            if (pic == null)
            {
                return NotFound();
            }

            db.Pics.Remove(pic);
            db.SaveChanges();

            return Ok(pic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PicExists(int id)
        {
            return db.Pics.FirstOrDefault(e => e.Id == id) != null;
        }
    }
}