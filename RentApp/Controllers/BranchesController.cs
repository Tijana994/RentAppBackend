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
        [HttpGet]
        [Route("GetAllBranches")]
        [ResponseType(typeof(Branch))]
        public IEnumerable<Branch> GetBranches()
        {
            return db.Branches.GetAll();
        }

        [HttpGet]
        [Route("GetAllBrancesOfService/{id}")]
        [ResponseType(typeof(Branch))]
        public IEnumerable<Branch> GetAllBrancesOfService(int id)
        {
            List<Branch> list = new List<Branch>();

            if (db.Services.Any(x => x.Id == id))
            {
                return null;
            }

            foreach (var item in db.Branches.GetAll())
            {
                if (item.ServiceId == id)
                {
                    list.Add(item);
                }
            }


            return list;
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
        [Route("PutBranch/{id}")]
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
            catch (Exception ex)
            {
                return BadRequest("Somebody already changed Branch");
            }

            return Ok("Successfuly added new Branch");
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

            if (db.Branches.Any(x => x.Name == branch.Name))
            {
                return BadRequest("Name should be unique");
            }
            db.Branches.Add(branch);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                return BadRequest("Somebody already changed Branch");

            }
            
           

            return Ok(db.Branches.FirstOrDefault(x => x.Name==branch.Name));
        }

        // DELETE: api/Branches/5
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        [Route("DeleteBranch/{id}")]
        public IHttpActionResult DeleteBranch(int id)
        {
            Branch branch = db.Branches.Get(id);
            if (branch == null)
            {
                return BadRequest("Bad id");
            }

            try
            {
                db.Branches.Remove(branch);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest("Somebody already delete Branch");
            }
            

            return Ok("Successfuly deleted Branch");
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