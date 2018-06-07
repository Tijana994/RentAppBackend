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
using Microsoft.AspNet.Identity.Owin;
using RentApp.Models.Entities;

using RentApp.Repo;

namespace RentApp.Controllers
{
    [RoutePrefix("api/AppUser")]
    public class AppUsersController : ApiController
    {
        private IUnitOfWork db { get; set; }

        public AppUsersController(IUnitOfWork db)
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


        // GET: api/AppUsers 
        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize]
        public IEnumerable<AppUser> GetAllUsers()
        {
            return db.AppUsers.GetAll();
        }

        // GET: api/AppUsers/5[HttpGet]
        [HttpGet]
        [Route("GetAppUser/{username}")]
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult GetAppUser(string username)
        {
            bool isAdmin = UserManager.IsInRole(User.Identity.Name, "Admin");//User.Identity.Name => Username Identity User-a! UserManager trazi po njegovom username-u, i onda poredi! 
            var user = db.AppUsers.FirstOrDefault(u => u.Username == User.Identity.Name);//Vadimo iz Identity baze po username-u Identity User-a, koji u sebi sadrzi AppUser-a!
            if (isAdmin || (user != null && user.Username.Equals(username)))//Ako korisnik nije admin, i nije AppUser koji trazi podatke o sebi, nije autorizovan!
            {
                AppUser appUser = db.AppUsers.FirstOrDefault(x => x.Username.Equals(username));
                if (appUser == null)
                {
                    return NotFound();
                }
                appUser.LoggedIn = true;
                db.SaveChanges();
                return Ok(appUser);
            }
            
            return Unauthorized();
        }

        // PUT: api/AppUsers/5
        [HttpPut]
        [Authorize]
        [Route("PutAppUser/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppUser(int id, AppUser appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appUser.Id)
            {
                return BadRequest();
            }

            db.AppUsers.Update(appUser);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (!AppUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/AppUsers
        [HttpPost]
        [Route("PostAppUser")]
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult PostAppUser(AppUser appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppUsers.Add(appUser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appUser.Id }, appUser);
        }

        // DELETE: api/AppUsers/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("DeleteAppUser")]
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult DeleteAppUser(int id)
        {
            AppUser appUser = db.AppUsers.Get(id);
            if (appUser == null)
            {
                return NotFound();
            }

            db.AppUsers.Remove(appUser);
            db.SaveChanges();

            return Ok(appUser);
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("Promotion")]
        public IHttpActionResult Promotion(int id,bool service)
        {

            var user = db.AppUsers.Get(id);

            if (user != null)
            {
                return BadRequest("User doesn`t exist");
            }

            user.CreateService = service;

            try
            {
               
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                return BadRequest("Somebody already change user");
            }



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

        private bool AppUserExists(int id)
        {
            return db.AppUsers.Find(e => e.Id == id).Count() > 0;
        }
    }
}