using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        [Required]
        [StringLength(60)]
        public string Address { get; set; }
        [Required]
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public List<BranchReservation> BranchReservations { get; set; }

    }
}