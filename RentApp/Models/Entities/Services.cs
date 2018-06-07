using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(40)]
        public string Email { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        [StringLength(40)]
        public string Contact { get; set; }

        [ForeignKey("AppUser")]
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        
        [StringLength(200)]
        public string Path { get; set; } //format : idServisa_Logo
        [Required]
        public bool Approved { get; set; }
        public double AverageMark { get; set; }
        public List<Branch> Branches { get; set; }    //FK-s
        public List<Vehicle> Vehicles { get; set; }
        public List<Rate> Rates { get; set; }  //FK-s
    }
}