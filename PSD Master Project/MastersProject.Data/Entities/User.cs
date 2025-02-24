using System;
using System.ComponentModel;
namespace MastersProject.Data.Entities;
using System.ComponentModel.DataAnnotations;

    public enum Role { admin, optometrist, staff, researcher }

    public class User
    {
        [Key]
        public int UId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy:MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }
        public int Age => (int)(DateTime.Now - DoB).TotalDays / 365;
        public string MobileNumber { get; set; }
        public string HomeNumber { get; set; }
        public Role Role { get; set; }

    }

