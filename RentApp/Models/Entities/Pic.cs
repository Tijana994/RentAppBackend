using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Pic
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [StringLength(200)]
        public string Path { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        
       
    }
}