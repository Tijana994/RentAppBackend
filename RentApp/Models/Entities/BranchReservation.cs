using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RentApp.Models.Entities
{
    public class BranchReservation
    {
        [ForeignKey("Branch")]
        public int BranchId { get; set; }

        [ForeignKey("Reservation")]
        public int ReservationId { get; set; }

        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Reservation Reservation { get; set; }

        public bool Reception { get; set; }
        
    }
}