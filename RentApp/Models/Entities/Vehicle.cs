using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required]
        [StringLength(40)]
        public string Mark { get; set; }
        [Required]
        [StringLength(40)]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }

        [Required]
        [StringLength(40)]
        public string Description { get; set; }

        [Required]
        public bool Avaliable { get; set; }

        
        [ForeignKey("TypeOfVehicle")]
        public int TypeOfVehicleId { get; set; }
        public TypeOfVehicle TypeOfVehicle { get; set; }  //FK

        public List<Pic> Pics { get; set; }   //format jedne slike : idVozila_nazivSlike

        
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public virtual ICollection<PriceList> PriceLists { get; set; }
    }
}