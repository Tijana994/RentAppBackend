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
    [RoutePrefix("api/Reservations")]
    public class ReservationsController : ApiController
    {
        private IUnitOfWork db { get; set; }

        public ReservationsController(IUnitOfWork db)
        {
            this.db = db;

        }

        // GET: api/Reservations
        [HttpGet]
        [Route("GetAllReservations")]
        [ResponseType(typeof(Reservation))]
        public IEnumerable<Reservation> GetReservations()
        {
            return db.Reservations.GetAll();
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserReservation/{id}")]
        [ResponseType(typeof(Reservation))]
        public IEnumerable<Reservation> GetUserReservation(int id)
        {
            List<Reservation> reservatons = new List<Reservation>();

            


            foreach (var reservation in db.Reservations.Find(x => x.AppUserId == id))
            {
                if (reservation.EndDate <= DateTime.Now)
                {
                    reservation.Expired = true;
                }
                reservation.BranchReservations = new List<BranchReservation>();
                reservation.BranchReservations.Add(db.BranchReservations.FirstOrDefault(x => x.ReservationId == reservation.Id && x.Reception == true));
                reservation.BranchReservations.Add(db.BranchReservations.FirstOrDefault(x => x.ReservationId == reservation.Id && x.Reception == false));
                reservation.Vehicle = db.Vehicles.Get(reservation.VehicleId);
                reservatons.Add(reservation);
            }

            db.SaveChanges();

            return reservatons;
            
        }

        // GET: api/Reservations/5
        [HttpGet]
        [Route("GetReservation/{id}")]
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult GetReservation(int id)
        {
            Reservation reservation = db.Reservations.Get(id);
            if (reservation == null)
            {
                return NotFound();
            }
            reservation.BranchReservations = new List<BranchReservation>();
            var pom1 = db.BranchReservations.FirstOrDefault(x => x.ReservationId == reservation.Id && x.Reception == true);
            //reservation.BranchReservations.Add(pom1);
            var pom2 = db.BranchReservations.FirstOrDefault(x => x.ReservationId == reservation.Id && x.Reception == false);
            //reservation.BranchReservations.Add(pom2);
            reservation.Vehicle = db.Vehicles.Get(reservation.VehicleId);

            return Ok(reservation);
        }

        // PUT: api/Reservations/5
        [HttpPut]
        [Authorize]
        [Route("PutReservation/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReservation(int id, Reservation reservation)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DateTime pomday = reservation.StartDate;
            pomday.AddDays(1);
            if (pomday <= DateTime.Now)
            {
                return BadRequest("24");
            }

            if (id != reservation.Id)
            {
                return BadRequest();
            }

            if (!free(reservation, true))
            {
                return BadRequest();
            }

            if (!db.Vehicles.FirstOrDefault(x => x.Id == reservation.VehicleId).Available)
            {
                return BadRequest();
            }

            double pricePerHour = GetPrice(reservation.VehicleId);
            TimeSpan timeSpan = reservation.EndDate - reservation.StartDate;
            reservation.TotalPrice = timeSpan.TotalHours * pricePerHour;

            BranchReservation reception = new BranchReservation();
            reception.BranchId = reservation.BranchReservations.ElementAt(0).BranchId;
            reception.Branch = db.Branches.Get(reception.BranchId);
            reception.Reception = true;

            BranchReservation returnto = new BranchReservation();
            returnto.BranchId = reservation.BranchReservations.ElementAt(1).BranchId;
            returnto.Branch = db.Branches.Get(returnto.BranchId);
            returnto.Reception = false;

            reception.ReservationId = id;
  
    
            returnto.ReservationId = id;

            reservation.BranchReservations.Clear();


            if (!db.BranchReservations.Any(x => x.BranchId == reception.BranchId && x.ReservationId == reception.ReservationId && x.Reception == true))
            {
                BranchReservation pom = db.BranchReservations.FirstOrDefault(x => x.ReservationId == reception.ReservationId && x.Reception == true);
                db.BranchReservations.Remove(pom);
                db.BranchReservations.Add(reception);
                reservation.BranchReservations.Add(pom);
            }
            else
            {
                reservation.BranchReservations.Add(db.BranchReservations.FirstOrDefault(x => x.BranchId == reception.BranchId && x.ReservationId == reception.ReservationId && x.Reception == true));
            }

            if (!db.BranchReservations.Any(x => x.BranchId == returnto.BranchId && x.ReservationId == returnto.ReservationId && x.Reception == false))
            {
                BranchReservation pom = db.BranchReservations.FirstOrDefault(x => x.ReservationId == returnto.ReservationId && x.Reception == false);
                db.BranchReservations.Remove(pom);
                db.BranchReservations.Add(returnto);
                reservation.BranchReservations.Add(pom);
            }
            else
            {
                reservation.BranchReservations.Add(db.BranchReservations.FirstOrDefault(x => x.BranchId == returnto.BranchId && x.ReservationId == returnto.ReservationId && x.Reception == false));
            }


            db.Reservations.Update(reservation);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest("Somebody already change this");
            }

            return Ok(reservation);
        }

        private bool free(Reservation reservation, bool edit = false)
        {

            foreach (var item in db.Reservations.Find(x => x.VehicleId == reservation.VehicleId))
            {
                if (edit && item.Id == reservation.Id)
                {
                    continue;
                }
                if (reservation.StartDate <= item.StartDate)
                {
                    if (reservation.EndDate >= item.StartDate && item.EndDate >= reservation.EndDate)
                    {
                        return false;
                    }
                    else if (reservation.EndDate >= item.EndDate)
                    {
                        return false;
                    }

                }
                else
                {
                    if (reservation.EndDate <= item.EndDate)
                    {
                        return false;
                    }
                    else if (reservation.StartDate < item.EndDate && reservation.EndDate >= item.EndDate)
                    {
                        return false;
                    }
                }

               

            }
            return true;

        }

        [HttpPost]
        [Authorize]
        [Route("CheckReservation")]
        public IHttpActionResult CheckReservation(Reservation reservation)
        {
            if (!free(reservation))
            {
                return BadRequest();
            }

            double pricePerHour = GetPrice(reservation.VehicleId);
            TimeSpan timeSpan = reservation.EndDate - reservation.StartDate;
            double price = timeSpan.TotalHours * pricePerHour;
            return Ok(price);
        }

        [HttpPost]
        [Authorize]
        [Route("CheckReservationEdit")]
        public IHttpActionResult CheckReservationEdit(Reservation reservation)
        {
            DateTime pom = reservation.StartDate;
            pom.AddDays(1);
            if (pom <= DateTime.Now)
            {
                return BadRequest("24");
            }
            if (!free(reservation,true))
            {
                return BadRequest();
            }

            double pricePerHour = GetPrice(reservation.VehicleId);
            TimeSpan timeSpan = reservation.EndDate - reservation.StartDate;
            double price = timeSpan.TotalHours * pricePerHour;
            return Ok(price);
        }



        private double GetPrice(int id)
        {
            Vehicle vehicle = db.Vehicles.Get(id);
            if (vehicle == null)
            {
                return 0;
            }


            PriceList price = null;

            foreach (var item in vehicle.PriceLists)
            {
                if (item.StartDate < DateTime.Now && item.EndDate > DateTime.Now)
                {
                    price = item;
                }
            }

            if (price == null)
            {
                price = vehicle.PriceLists.Last();
            }



            return price.Price;
        }

        // POST: api/Reservations
        [HttpPost]
        [Authorize]
        [Route("PostReservation")]
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult PostReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!free(reservation))
            {
                return BadRequest();
            }

            reservation.Vehicle = db.Vehicles.Get(reservation.VehicleId);

            double pricePerHour = GetPrice(reservation.VehicleId);
            TimeSpan timeSpan = reservation.EndDate - reservation.StartDate;
            reservation.TotalPrice = timeSpan.TotalHours * pricePerHour;


            BranchReservation reception = new BranchReservation();
            reception.BranchId = reservation.BranchReservations.ElementAt(0).BranchId;
            reception.Branch = db.Branches.Get(reception.BranchId);
            reception.Reception = true;

            BranchReservation returnto = new BranchReservation();
            returnto.BranchId = reservation.BranchReservations.ElementAt(1).BranchId;
            returnto.Branch = db.Branches.Get(returnto.BranchId);
            returnto.Reception = false;

            reservation.BranchReservations.Clear();

            db.Reservations.Add(reservation);
            db.SaveChanges();

            reception.ReservationId = reservation.Id;
            reception.Reservation = reservation;
            returnto.ReservationId = reservation.Id;
            returnto.Reservation = reservation;

            db.BranchReservations.Add(reception);
            db.BranchReservations.Add(returnto);

            db.SaveChanges();

            reservation.BranchReservations.Add(db.BranchReservations.FirstOrDefault(x => x.ReservationId == reservation.Id && x.Reception == true));
            reservation.BranchReservations.Add(db.BranchReservations.FirstOrDefault(x => x.ReservationId == reservation.Id && x.Reception == false)); 

            return Ok(reservation);
        }

        // DELETE: api/Reservations/5
        [HttpDelete]
        [Authorize]
        [Route("DeleteReservation/{id}")]
        [ResponseType(typeof(Reservation))]
        public IHttpActionResult DeleteReservation(int id)
        {
            Reservation reservation = db.Reservations.Get(id);
            DateTime pomday = reservation.StartDate;
            pomday.AddDays(1);
            if (pomday <= DateTime.Now)
            {
                return BadRequest("24");
            }

            if (reservation == null)
            {
                return NotFound();
            }

            if (!db.Vehicles.FirstOrDefault(x => x.Id == reservation.VehicleId).Available)
            {
                return BadRequest();
            }

            BranchReservation br = db.BranchReservations.FirstOrDefault(x => x.Reception == true && x.ReservationId == reservation.Id);
            BranchReservation br2 = db.BranchReservations.FirstOrDefault(x => x.Reception == false && x.ReservationId == reservation.Id);

            db.BranchReservations.Remove(br);
            db.BranchReservations.Remove(br2);

            db.Reservations.Remove(reservation);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            

            return Ok(reservation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservationExists(int id)
        {
            return db.Reservations.Find(e => e.Id == id).Count() > 0;
        }
    }
}