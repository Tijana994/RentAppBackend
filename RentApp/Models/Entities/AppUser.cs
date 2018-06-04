using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentApp.Models.Entities
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        [Required]
        [StringLength(40)]
        public string Surname { get; set; }
        [Required]
        [StringLength(40)]
        public string Contact { get; set; }
        [Required]
        [StringLength(40)]
        public string Username { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public bool Approved { get; set; }
        [Required]
        public bool LoggedIn { get; set; }
        [Required]
        public bool CreateService { get; set; }
        [StringLength(200)]
        public string Path { get; set; } //format : idKorisnika_Dokument


        public List<Service> Services { get; set; }
        public virtual ICollection<Rate> Rates { get; set; } //FK-s
        public virtual ICollection<Reservation> Reservations { get; set; } //FK-s
    }
}