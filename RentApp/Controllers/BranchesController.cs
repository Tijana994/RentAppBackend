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
    [RoutePrefix("api/Branches")]
    public class BranchesController : ApiController
    {
        private IUnitOfWork db { get; set; }

        public BranchesController(IUnitOfWork db)
        {
            this.db = db;
        }

        // GET: api/Branches
        public IEnumerable<Branch> GetBranches()
        {
            return db.Branches.GetAll();
        }

        // GET: api/Branches/5
        [HttpGet]
        [Route("GetBranch/{id}")]
        [ResponseType(typeof(Branch))]
        public IHttpActionResult GetBranch(int id)
        {
            Branch branch = db.Branches.Get(id);
            if (branch == null)
            {
                return NotFound();
            }

            return Ok(branch);
        }

        // PUT: api/Branches/5
        [HttpPut]
        [Authorize(Roles = "Manager")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBranch(int id, Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != branch.Id)
            {
                return BadRequest();
            }

            db.Branches.Update(branch);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchExists(id))
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

        // POST: api/Branches
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("PostBranch")]
        [ResponseType(typeof(Branch))]
        public IHttpActionResult PostBranch(Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Branches.Add(branch);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = branch.Id }, branch);
        }

        // DELETE: api/Branches/5
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        [ResponseType(typeof(Branch))]
        public IHttpActionResult DeleteBranch(int id)
        {
            Branch branch = db.Branches.Get(id);
            if (branch == null)
            {
                return NotFound();
            }

            db.Branches.Remove(branch);
            db.SaveChanges();

            return Ok(branch);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BranchExists(int id)
        {
            return db.Branches.FirstOrDefault(e => e.Id == id) != null;
        }
    }
}