using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class PriceList
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [ForeignKey("Vehicle")]
        public int VehicleId;
        public Vehicle Vehicle { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public double Price { get; set; }
    }
}