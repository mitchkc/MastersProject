using MastersProject.Data.Entities;
using MastersProject.Data.Repositories;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;

namespace MastersProject.Data.Services;

public class DatabaseSeeder
{
    private readonly DatabaseContext _context;

    public DatabaseSeeder(DatabaseContext context)
    {
        _context = context;
    }

    public void seed()
    {
        var staffUsers = new List<User>
        {
        new User { Forename = "Admin", Surname = "Istrator1", Email = "admin1@mail.com", Password = "admin1", Address = "Test Admin Address1",
                   Gender = "Male", DoB = new DateTime(10 - 10 - 1985), MobileNumber = "0771111111", HomeNumber = "0281111111", Role = Role.admin },

        new User { Forename = "Admin", Surname = "Istrator2", Email = "admin2@mail.com", Password = "admin2", Address = "Test Admin Address2",
                    Gender = "Female", DoB = new DateTime(01 - 01 - 1981), MobileNumber = "0771111112", HomeNumber = "0281111112", Role = Role.admin },

        new User { Forename = "Optom", Surname = "Etrist1", Email = "optom1@mail.com", Password = "optom1", Address = "Test Optom Address 1",
                    Gender = "Male", DoB = new DateTime(02 - 02 - 1980), MobileNumber = "0781111111", HomeNumber = "0281112222", Role = Role.optometrist },

        new User { Forename = "Optom", Surname = "Etrist2", Email = "optom2@mail.com", Password = "optom2", Address = "Test Optom Address 2",
                    Gender = "Female", DoB = new DateTime(03 - 03 - 1981), MobileNumber = "0781111112", HomeNumber = "0281112223", Role = Role.optometrist },

        new User { Forename = "Optom", Surname = "Etrist3", Email = "optom3@mail.com", Password = "optom3", Address = "Test Optom Address 3",
                    Gender = "Female", DoB = new DateTime(04 - 04 - 1982), MobileNumber =  "0781111113", HomeNumber = "0281112224", Role = Role.optometrist },

        new User { Forename = "Staff", Surname = "Member1", Email = "staff1@mail.com", Password = "staff1", Address = "Test Staff Address 1",
                    Gender = "Female", DoB = new DateTime(01 - 02 - 2000), MobileNumber = "0751112222", HomeNumber = "0282223333", Role = Role.staff },

        new User { Forename = "Staff", Surname = "Member2", Email = "staff2@mail.com", Password = "staff2", Address = "Test Staff Address 2",
                    Gender = "Male", DoB = new DateTime(02 - 02 - 2001), MobileNumber = "0751112223", HomeNumber = "0282223334", Role = Role.staff },

        new User { Forename = "Staff", Surname = "Member3", Email = "staff3@mail.com", Password = "staff3", Address = "Test Staff Address 3",
                    Gender = "Female", DoB = new DateTime(03 - 02 - 1991), MobileNumber = "0751112224", HomeNumber = "0282223335", Role = Role.staff },

        new User { Forename = "Staff", Surname = "Member4", Email = "staff4@mail.com", Password = "staff4", Address = "Test Staff Address 4",
                    Gender = "Female", DoB = new DateTime(04 - 02 - 1970), MobileNumber = "0751112225", HomeNumber = "0282223336", Role = Role.staff },

        new User { Forename = "Staff", Surname = "Member5", Email = "staff5@mail.com", Password = "staff5", Address = "Test Staff Address 5",
                    Gender = "Female", DoB = new DateTime(05 - 02 - 1980), MobileNumber = "0751112226", HomeNumber = "0282223337", Role = Role.staff }
        };
        _context.Users.AddRange(staffUsers);
        _context.SaveChanges();

        var testPatients = new List<Patient>
        {
            var p1 = _context.Add { Forename = "Patient", Surname = "Test1", Email = "patient1@mail.com", Address = "Test Px Address 1",
                            Gender = "Female", Dob = new DateTime(01 - 01 - 1950), Mobile = "0771111111", HomeNumber = "0281111111", 
                            GPName = "Dr McTest", GPAddress = "GP Test Address 1", PatientType = "NHS", Opt = Opt.optIn },
            
            new Patient { Forename = "Patient", Surname = "Test2", Email = "patient2@mail.com", Address = "Test Px Address 2",
                            Gender = "Female", Dob = new DateTime(02 - 02 - 1952), Mobile = "0771111112", HomeNumber = "0281111112", 
                            GPName = "Dr McTest2", GPAddress = "GP Test Address 2", PatientType = "NHS", Opt = Opt.optIn },

            new Patient { Forename = "Patient", Surname = "Test3", Email = "patient3@mail.com", Address = "Test Px Address 3",
                            Gender = "Female", Dob = new DateTime(03 - 03 - 1953), Mobile = "0771111113", HomeNumber = "0281111113", 
                            GPName = "Dr McTest3", GPAddress = "GP Test Address 3", PatientType = "NHS", Opt = Opt.optIn },
            
            new Patient { Forename = "Patient", Surname = "Test4", Email = "patient4@mail.com", Address = "Test Px Address 4",
                            Gender = "Female", Dob = new DateTime(04 - 04 - 1954), Mobile = "0771111114", HomeNumber = "028111114", 
                            GPName = "Dr McTest4", GPAddress = "GP Test Address 4", PatientType = "NHS", Opt = Opt.optIn },

            new Patient { Forename = "Patient", Surname = "Test5", Email = "patient5@mail.com", Address = "Test Px Address 5",
                            Gender = "Female", Dob = new DateTime(05 - 05 - 1955), Mobile = "0771111115", HomeNumber = "0281111115", 
                            GPName = "Dr McTest5", GPAddress = "GP Test Address 5", PatientType = "NHS", Opt = Opt.optIn },

            new Patient { Forename = "Patient", Surname = "Test6", Email = "patient6@mail.com", Address = "Test Px Address 6",
                            Gender = "Female", Dob = new DateTime(06 - 06 - 1960), Mobile = "0771111116", HomeNumber = "0281111116", 
                            GPName = "Dr McTest6", GPAddress = "GP Test Address 6", PatientType = "NHS", Opt = Opt.optIn },

            new Patient { Forename = "Patient", Surname = "Test7", Email = "patient7@mail.com", Address = "Test Px Address 7",
                            Gender = "Female", Dob = new DateTime(07 - 07 - 1967), Mobile = "0771111117", HomeNumber = "0281111117", 
                            GPName = "Dr McTest7", GPAddress = "GP Test Address 7", PatientType = "NHS", Opt = Opt.optIn },

            new Patient { Forename = "Patient", Surname = "Test8", Email = "patient8@mail.com", Address = "Test Px Address 8",
                            Gender = "Female", Dob = new DateTime(08 - 08 - 1968), Mobile = "0771111118", HomeNumber = "0281111118", 
                            GPName = "Dr McTest8", GPAddress = "GP Test Address 8", PatientType = "NHS", Opt = Opt.optIn },

            new Patient { Forename = "Patient", Surname = "Test9", Email = "patient9@mail.com", Address = "Test Px Address 9",
                            Gender = "Female", Dob = new DateTime(09 - 09 - 1990), Mobile = "0771111119", HomeNumber = "0281111119", 
                            GPName = "Dr McTest9", GPAddress = "GP Test Address 9", PatientType = "Private", Opt = Opt.optIn },
        }
    }
}