using System;
namespace MastersProject.Data.Entities
{
    public enum Role { admin, optometrist, staff }
    
    public class User
    {
        public int Uid { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateOnly DoB { get; set; }
        public string MobileNumber { get; set; }
        public string HomeNumber { get; set; }
        public Role Role { get; set; }

    }
}
