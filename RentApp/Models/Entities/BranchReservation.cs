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
        [Key, Column(Order = 0)]
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("Reservation")]
        public int ReservationId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Reservation Reservation { get; set; }

        public bool Reception { get; set; }
        
    }
}