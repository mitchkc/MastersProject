
using Xunit;
using MastersProject.Data.Entities;
using MastersProject.Data.Services;

using Microsoft.EntityFrameworkCore;
using MastersProject.Data.Repositories;
using MastersProject.Data.Security;

namespace MastersProject.Test
{
    public class ServiceTests
    {
        private readonly IUserService service;
        private readonly IPatientService psvc;

        public ServiceTests()
        {
            // configure the data context options to use sqlite for testing
            var options = DatabaseContext.OptionsBuilder
                            .UseSqlite("Filename=test.db")
                            //.LogTo(Console.WriteLine)
                            .Options;

            // create service with new context
            service = new UserServiceDb(new DatabaseContext(options));
            psvc = new PatientServiceDb(new DatabaseContext(options));
            service.Initialise();
        }

        [Fact]
        public void GetUsers_WhenNoneExist_ShouldReturnNone()
        {
            // act
            var users = service.GetUsers();

            // assert
            Assert.Equal(0, users.Count);
        }

        [Fact]
        public void AddUser_When2ValidUsersAdded_ShouldCreate2Users()
        {
            // arrange
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);
            service.AddUser("optom", "test", "optom@mail.com", "optom", "optomAddress", "female", new DateTime(1990, 1, 1), "077", "028", Role.optometrist);

            // act
            var users = service.GetUsers();

            // assert
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public void GetPage1WithpageSize2_When3UsersExist_ShouldReturn2Pages()
        {
            // act
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);
            service.AddUser("optom", "test", "optom@mail.com", "optom", "optomAddress", "female", new DateTime(1990, 1, 1), "077", "028", Role.optometrist);
            service.AddUser("staff", "test", "staff@mail.com", "staff", "staffAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.staff);

            // return first page with 2 users per page
            var pagedUsers = service.GetUsers(1, 2);

            // assert
            Assert.Equal(2, pagedUsers.TotalPages);
        }

        [Fact]
        public void GetPage1WithPageSize2_When3UsersExist_ShouldReturnPageWith2Users()
        {
            // act
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);
            service.AddUser("optom", "test", "optom@mail.com", "optom", "optomAddress", "female", new DateTime(1990, 1, 1), "077", "028", Role.optometrist);
            service.AddUser("staff", "test", "staff@mail.com", "staff", "staffAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.staff);

            var pagedUsers = service.GetUsers(1, 2);

            // assert
            Assert.Equal(2, pagedUsers.Data.Count);
        }

        [Fact]
        public void GetPage1_When0UsersExist_ShouldReturn0Pages()
        {
            // act
            var pagedUsers = service.GetUsers(1, 2);

            // assert
            Assert.Equal(0, pagedUsers.TotalPages);
            Assert.Equal(0, pagedUsers.TotalRows);
            Assert.Empty(pagedUsers.Data);
        }

        [Fact]
        public void UpdateUser_WhenUserExists_ShouldWork()
        {
            // arrange
            var user = service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);

            // act
            user.Forename = "administrator";
            user.Email = "admin@mail.com";
            var updatedUser = service.UpdateUser(user);

            // assert
            Assert.Equal("administrator", updatedUser.Forename);
            Assert.Equal("admin@mail.com", updatedUser.Email);
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldWork()
        {
            // arrange
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);

            // act            
            var user = service.Authenticate("admin@mail.com", "admin");

            // assert
            Assert.NotNull(user);

        }

        [Fact]
        public void Login_WithInvalidCredentials_ShouldNotWork()
        {
            // arrange
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);

            // act      
            var user = service.Authenticate("admin@mail.com", "xxx");

            // assert
            Assert.Null(user);

        }

        [Fact]
        public void ForgotPasswordRequest_ForValidUser_ShouldGenerateToken()
        {
            // arrange
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);

            // act      
            var token = service.ForgotPassword("admin@mail.com");

            // assert
            Assert.NotNull(token);

        }

        [Fact]
        public void ForgotPasswordRequest_ForInValidUser_ShouldReturnNull()
        {
            // arrange

            // act      
            var token = service.ForgotPassword("admin@mail.com");

            // assert
            Assert.Null(token);

        }

        [Fact]
        public void ResetPasswordRequest_WithValidUserAndToken_ShouldReturnUser()
        {
            // arrange
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);
            var token = service.ForgotPassword("admin@mail.com");

            // act      
            var user = service.ResetPassword("admin@mail.com", token, "password");

            // assert
            Assert.NotNull(user);
            Assert.True(Hasher.ValidateHash(user.Password, "password"));
        }

        [Fact]
        public void ResetPasswordRequest_WithValidUserAndExpiredToken_ShouldReturnNull()
        {
            // arrange
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);
            var expiredToken = service.ForgotPassword("admin@mail.com");
            service.ForgotPassword("admin@mail.com");

            // act      
            var user = service.ResetPassword("admin@mail.com", expiredToken, "password");

            // assert
            Assert.Null(user);
        }

        [Fact]
        public void ResetPasswordRequest_WithInValidUserAndValidToken_ShouldReturnNull()
        {
            // arrange
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);
            var token = service.ForgotPassword("admin@mail.com");

            // act      
            var user = service.ResetPassword("unknown@mail.com", token, "password");

            // assert
            Assert.Null(user);
        }

        [Fact]
        public void ResetPasswordRequests_WhenAllCompleted_ShouldExpireAllTokens()
        {
            // arrange
            service.AddUser("admin", "test", "admin@mail.com", "admin", "adminAddress", "male", new DateTime(1990, 1, 1), "077", "028", Role.admin);
            service.AddUser("optom", "test", "optom@mail.com", "optom", "optomAddress", "female", new DateTime(1990, 1, 1), "077", "028", Role.optometrist);

            // create token and reset password - token then invalidated
            var token1 = service.ForgotPassword("admin@mail.com");
            service.ResetPassword("admin@mail.com", token1, "password");

            // create token and reset password - token then invalidated
            var token2 = service.ForgotPassword("optom@mail.com");
            service.ResetPassword("optom@mail.com", token2, "password");

            // act  
            // retrieve valid tokens 
            var tokens = service.GetValidPasswordResetTokens();

            // assert
            Assert.Empty(tokens);
        }




        // -------------------------------------- Patient Tests -------------------------------------- //

        [Fact]
        public void GetAllPatients_WhenNone_Return0()
        {
            //act
            var patients = psvc.GetPatients();
            var count = patients.Count();

            //assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void GetPatients_With2Added_ShouldReturn2()
        {
            // arrange       
            var p1 = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            }
            );
            var p2 = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test2",
                Email = "patient2@mail.com",
                Address = "Test Px Address 2",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            }
            );

            // act
            var patients = psvc.GetPatients();
            var count = patients.Count;

            // assert
            Assert.Equal(2, count);
        }

        [Fact]
        public void GetPatient_WhenNone_ReturnNull()
        {
            // act 
            var px = psvc.GetPatient(1);

            // assert
            Assert.Null(px);
        }

        [Fact]
        public void GetPatientById_WhenAdded_ReturnPatient()
        {
            // arrange 
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var px = psvc.GetPatient(p.Pid);

            // assert
            Assert.NotNull(px);
            Assert.Equal(p.Pid, px.Pid);
        }

        [Fact]
        public void GetPatient_WithSightTest_RetrieveBothPatientAndSightTests()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            psvc.AddSightTest(p.Pid, "history", "GH", "OH", "occupation", "driver", "smoker", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 4, 4, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15, 
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes","+", 1.00,"+", 1.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);

            // act      
            var px = psvc.GetPatient(p.Pid);

            // assert
            Assert.NotNull(px);
            Assert.Equal(1, px.SightTests.Count);
        }

        [Fact]
        public void GetPatient_WithTriage_RetrieveBothPatientAndTriage()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            psvc.AddTriage(p.Pid, "store", "july", "here", "pain", "2 days", "yes", "yes", 10, "yes", "yes",
                                     "yes", "yes", "yes", "dailies", "yes", "yes", "yes", "yes", "yes", "yes", "Michael", "sight test needed");

            // act      
            var px = psvc.GetPatient(p.Pid);

            // assert
            Assert.NotNull(px);
            Assert.Equal(1, px.Triages.Count);
        }


        //---------------- Add Patient Tests ---------------------

        [Fact]
        public void AddPatient_WhenValid_ShouldAddPatient()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act 
            var px = psvc.GetPatient(p.Pid);

            // assert 
            Assert.NotNull(px);

            // now assert that the properties were set properly
            Assert.Equal(p.Pid, p.Pid);
            Assert.Equal("Pat", p.Forename);
            Assert.Equal("Test1", p.Surname);
            Assert.Equal("patient1@mail.com", p.Email);
            Assert.Equal("Test Px Address 1", p.Address);
            Assert.Equal("Female", p.Gender);
            Assert.Equal(new DateTime(1950, 1, 1), p.Dob);
            Assert.Equal("0771111111", p.Mobile);
            Assert.Equal("0281111111", p.HomeNumber);
            Assert.Equal("Dr McTest", p.GPName);
            Assert.Equal("GP Test Address 1", p.GPAddress);
            Assert.Equal("NHS", p.PatientType);
            Assert.Equal(Opt.optIn, p.Opt);
        }


        //--------------- Update Patient Tests ------------------------

        [Fact]
        public void UpdatePatient_ThatExists_SetAllProperties()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act - copy and update all props excpet Id
            var px = psvc.UpdatePatient(new Patient
            {
                Pid = p.Pid,
                Forename = "Patient",
                Surname = "Test2",
                Email = "patient2@mail.com",
                Address = "Test Px Address 2",
                Gender = "Male",
                Dob = new DateTime(1950, 2, 2),
                Mobile = "0771111112",
                HomeNumber = "0281111112",
                GPName = "Dr McTest2",
                GPAddress = "GP Test Address 2",
                PatientType = "Priavte",
                Opt = Opt.optOut
            });

            //reload updated px
            var updated = psvc.GetPatient(p.Pid);

            // assert
            Assert.NotNull(updated);

            // now assert that the properties were set properly
            Assert.Equal(p.Pid, px.Pid);
            Assert.Equal(p.Forename, px.Forename);
            Assert.Equal(p.Surname, px.Surname);
            Assert.Equal(p.Email, px.Email);
            Assert.Equal(p.Address, px.Address);
            Assert.Equal(p.Gender, px.Gender);
            Assert.Equal(p.Dob, px.Dob);
            Assert.Equal(p.Mobile, px.Mobile);
            Assert.Equal(p.HomeNumber, px.HomeNumber);
            Assert.Equal(p.GPName, px.GPName);
            Assert.Equal(p.GPAddress, px.GPAddress);
            Assert.Equal(p.PatientType, px.PatientType);
            Assert.Equal(p.Opt, px.Opt);
        }


        //------------------ Delete Patient tests ----------------------

        [Fact]
        public void DeletePatient_ThatExists_ShouldReturnTrue()
        {
            // arrange 
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var deleted = psvc.DeletePatient(p.Pid);

            // attempt to retrieve deleted px
            var p1 = psvc.GetPatient(p.Pid);

            // assert
            Assert.True(deleted);
            Assert.Null(p1);
        }

        [Fact]
        public void DeletePatient_ThatDoesntExist_ShouldReturnFalse()
        {
            // act 	
            var deleted = psvc.DeletePatient(0);

            // assert
            Assert.False(deleted);
        }


        // ------------------ Patient Search Tests ----------------------- //

        [Fact]
        public void SearchPatientsForQuery_WhenMatchFound_ReturnPatient()
        {
            // arrange 
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var search = psvc.SearchPatients("p");

            // assert
            Assert.Equal(1, search.Count);
        }

        [Fact]
        public void SearchPatientForQuery_WhenNotFound_ReturnZero()
        {
            // arrange 
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var search = psvc.SearchPatients("b");

            // assert
            Assert.Equal(0, search.Count);
        }


        // -------------------------------------- ST Tests -------------------------------------------- //

        [Fact]
        public void CreateST_ForExistingPatient_ShouldBeCreated()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var st = psvc.AddSightTest(p.Pid, "history", "GH", "OH", "occupation", "driver", "smoker", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 4, 4, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15, 
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes","+", 1.00,"+", 1.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);

            // assert
            Assert.NotNull(st);
            Assert.Equal(p.Pid, st.PatientId);
        }

        [Fact]
        public void GetST_WhenExists_ShouldReturnSTAndPatient()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var st = psvc.AddSightTest(p.Pid, "history", "GH", "OH", "occupation", "driver", "smoker", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 4, 4, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15, 
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes","+", 1.00,"+", 1.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);

            // act
            var s = psvc.GetSightTest(st.PatientId);

            // assert
            Assert.NotNull(s);
            Assert.NotNull(s.Patient);
            Assert.Equal(p.Pid, s.Patient.Pid);
        }

        [Fact]
        public void GetST_WhenNoneExist_ShouldReturnFalse()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var sts = psvc.GetSightTests(p.Pid);
            var count = sts.Count();

            // assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void DeleteST_WhenExists_ShouldReturnTrue()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            var st = psvc.AddSightTest(p.Pid, "history", "GH", "OH", "occupation", "driver", "smoker", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 4, 4, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15, 
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes","+", 1.00,"+", 1.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);

            // act
            var deleted = psvc.DeleteSightTest(st.STId);

            // assert
            Assert.True(deleted);
        }

        [Fact]
        public void DeleteST_WhenNonExistant_ShouldReturnFalse()
        {
            // arrange

            // act
            var deleted = psvc.DeleteSightTest(1);

            // assert
            Assert.False(deleted);
        }


        // -------------------------------------- Triage Tests ---------------------------------------- //

        [Fact]
        public void CreateTriage_ForExistingPatient_ShouldBeCreated()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var t = psvc.AddTriage(p.Pid, "store", "july", "here", "pain", "2 days", "yes", "yes", 10, "yes", "yes",
                                     "yes", "yes", "yes", "dailies", "yes", "yes", "yes", "yes", "yes", "yes", "Michael", "sight test needed");

            // assert
            Assert.NotNull(t);
            Assert.Equal(p.Pid, t.PatientId);
        }

        [Fact]
        public void GetTriage_WhenExists_ShouldReturnTriageAndPatient()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var t = psvc.AddTriage(p.Pid, "store", "july", "here", "pain", "2 days", "yes", "yes", 10, "yes", "yes",
                                     "yes", "yes", "yes", "dailies", "yes", "yes", "yes", "yes", "yes", "yes", "Michael", "sight test needed");

            // act
            var all = psvc.GetTriage(t.PatientId);

            // assert
            Assert.NotNull(t);
            Assert.NotNull(t.Patient);
            Assert.Equal(p.Pid, t.Patient.Pid);
        }

        [Fact]
        public void GetTriage_WhenNoneExist_ShouldReturnFalse()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            // act
            var triage = psvc.GetTriages(p.Pid);
            var count = triage.Count();

            // assert
            Assert.Equal(0, count);
        }

        [Fact]
        public void DeleteTriage_WhenExists_ShouldReturnTrue()
        {
            // arrange
            var p = psvc.AddPatient(new Patient
            {
                Forename = "Pat",
                Surname = "Test1",
                Email = "patient1@mail.com",
                Address = "Test Px Address 1",
                Gender = "Female",
                Dob = new DateTime(1950, 1, 1),
                Mobile = "0771111111",
                HomeNumber = "0281111111",
                GPName = "Dr McTest",
                GPAddress = "GP Test Address 1",
                PatientType = "NHS",
                Opt = Opt.optIn
            });

            var t = psvc.AddTriage(p.Pid, "store", "july", "here", "pain", "2 days", "yes", "yes", 10, "yes", "yes",
                                     "yes", "yes", "yes", "dailies", "yes", "yes", "yes", "yes", "yes", "yes", "Michael", "sight test needed");

            // act
            var deleted = psvc.DeleteTriage(t.TriageId);

            // assert
            Assert.True(deleted);
        }

        [Fact]
        public void DeleteTriage_WhenNonExistant_ShouldReturnFalse()
        {
            // arrange

            // act
            var deleted = psvc.DeleteTriage(1);

            // assert
            Assert.False(deleted);
        }


    }
}
