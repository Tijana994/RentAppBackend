using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Rate
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required]
        public int Point { get; set; }
        [Required]
        [StringLength(400)]
        public string Comment { get; set; }

        [Required]
        [ForeignKey("AppUser")]
        public int AppUserId { get; set; } //FK
        public AppUser AppUser { get; set; }

        [Required]
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}