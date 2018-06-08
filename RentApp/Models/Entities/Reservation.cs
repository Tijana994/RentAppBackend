using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required]
        public bool Expired { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        
        [ForeignKey("AppUser")]
        public int AppUserId { get; set; } //FK
        public AppUser AppUser { get; set; }
       
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; } //FK
        public Vehicle Vehicle { get; set; }

        public List<BranchReservation> BranchReservations { get; set; }

    }
}