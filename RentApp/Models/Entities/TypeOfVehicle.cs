using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class TypeOfVehicle
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [Required]
        [StringLength(40)]
        public string Name{ get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}