using System.ComponentModel.DataAnnotations;
using MastersProject.Data.Entities;
using MastersProject.Data.Repositories;
using Microsoft.VisualBasic;

namespace MastersProject.Data.Services
{

    public static class Seeder
    {

        public static void Seed(IUserService usvc, IPatientService psvc)
        {
            usvc.Initialise();
            // add users
            usvc.AddUser("Admin", "Istrator1", "admin1@mail.com", "admin1", "Test Admin Address1", "Male", new DateTime(1980,1,1), "0771111111", "0281111111", Role.admin);
            usvc.AddUser("Admin", "Istrator2", "admin2@mail.com", "admin2", "Test Admin Address2", "Female", new DateTime(1981,1,2), "0771111112", "0281111112", Role.admin);

            usvc.AddUser("Optom", "Etrist1", "optom1@mail.com", "optom1", "Test Optom Address 1", "Male", new DateTime(1980,2,2), "0781111111", "0281112222", Role.optometrist);
            usvc.AddUser("Optom", "Etrist2", "optom2@mail.com", "optom2", "Test Optom Address 2", "Female", new DateTime(1981,3,3), "0781111112", "0281112223", Role.optometrist);
            usvc.AddUser("Optom", "Etrist3", "optom3@mail.com", "optom3", "Test Optom Address 3", "Female", new DateTime(1982,4,4), "0781111113", "0281112224", Role.optometrist);

            usvc.AddUser("Staff", "Member1", "staff1@mail.com", "staff1", "Test Staff Address 1", "Female", new DateTime(2000,1,2), "0751112222", "0282223333", Role.staff);
            usvc.AddUser("Staff", "Member2", "staff2@mail.com", "staff2", "Test Staff Address 2", "Male", new DateTime(2001,2,2), "0751112223", "0282223334", Role.staff);
            usvc.AddUser("Staff", "Member3", "staff3@mail.com", "staff3", "Test Staff Address 3", "Female", new DateTime(1991,3,2), "0751112224", "0282223335", Role.staff);
            usvc.AddUser("Staff", "Member4", "staff4@mail.com", "staff4", "Test Staff Address 4", "Female", new DateTime(1970,4,5), "0751112225", "0282223336", Role.staff);
            usvc.AddUser("Staff", "Member5", "staff5@mail.com", "staff5", "Test Staff Address 5", "Female", new DateTime(1980,2,5), "0751112226", "0282223337", Role.staff); 

            usvc.AddUser("Re", "Search", "research@mail.com", "research", "Test Rsearch Address1", "Male", new DateTime(1980,1,1), "0771111111", "0281111111", Role.researcher);


            var px1 = psvc.AddPatient( new Patient { Forename = "Patient", Surname = "Test1", Email = "patient1@mail.com", Address = "Test Px Address 1",
                            Gender = "Female", Dob = new DateTime(1950,1,1), Mobile = "0771111111", HomeNumber = "0281111111", 
                            GPName = "Dr McTest", HandCNum = "3333333333", GPAddress = "GP Test Address 1", PatientType = "NHS", Opt = Opt.optIn });

            var px2 = psvc.AddPatient( new Patient { Forename = "Patient2", Surname = "Test2", Email = "patient2@mail.com", Address = "Test Px Address 2",
                            Gender = "Male", Dob = new DateTime(1950,2,2), Mobile = "0771111122", HomeNumber = "0281111122", 
                            GPName = "Dr McTest2", HandCNum = "3333333333", GPAddress = "GP Test Address 2", PatientType = "NHS", Opt = Opt.optIn });

            var px3 = psvc.AddPatient( new Patient { Forename = "Patient3", Surname = "Test2", Email = "patient2@mail.com", Address = "Test Px Address 2",
                            Gender = "Male", Dob = new DateTime(2010,2,2), Mobile = "0771111122", HomeNumber = "0281111122", 
                            GPName = "Dr McTest2", HandCNum = "3333333333", GPAddress = "GP Test Address 2", PatientType = "NHS", Opt = Opt.optIn });
            
            var px4 = psvc.AddPatient( new Patient { Forename = "Patient3", Surname = "Test2", Email = "patient2@mail.com", Address = "Test Px Address 2",
                            Gender = "Male", Dob = new DateTime(2011,2,2), Mobile = "0771111122", HomeNumber = "0281111122", 
                            GPName = "Dr McTest2", HandCNum = "3333333333", GPAddress = "GP Test Address 2", PatientType = "NHS", Opt = Opt.optIn });

            
            var t1 = psvc.AddTriage (px1.Pid, "store", "july", "here", "pain", "2 days", "yes", "yes", 10, "yes", "yes",
                                     "yes", "yes", "yes", "dailies", "yes", "yes", "yes", "yes", "yes", "yes", "Michael", "sight test needed");

            var t2 = psvc.AddTriage (px1.Pid, "store", "july", "here", "pain", "2 days", "yes", "yes", 10, "yes", "yes",
                                     "yes", "yes", "yes", "dailies", "yes", "yes", "yes", "yes", "yes", "yes", "Michael", "sight test needed");

            var t3 = psvc.AddTriage (px2.Pid, "store", "july", "here", "pain", "2 days", "yes", "yes", 10, "yes", "yes",
                                     "yes", "yes", "yes", "dailies", "yes", "yes", "yes", "yes", "yes", "yes", "Michael", "sight test needed");

            var t4 = psvc.AddTriage (px2.Pid, "store", "july", "here", "pain", "2 days", "yes", "yes", 10, "yes", "yes",
                                     "yes", "yes", "yes", "dailies", "yes", "yes", "yes", "yes", "yes", "yes", "Michael", "sight test needed");

            
            var st1 = psvc.AddSightTest (px1.Pid, "history", "GH", "OH", "occupation", "driver", "smoker", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 4, 4, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15, 
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes","+", 1.00,"+", 1.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);

            var st2 = psvc.AddSightTest (px1.Pid, "history", "GH", "OH", "occupation", "driver", "smoker", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 4, 4, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15, 
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes","+", 1.00,"+", 1.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);

            var st3 = psvc.AddSightTest (px2.Pid, "history", "GH", "OH", "occupation", "driver", "no", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15, 
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes","-", 1.00, "-", 1.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);
            
            var st4 = psvc.AddSightTest (px2.Pid, "history", "GH", "OH", "occupation", "driver", "no", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 8, 8, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15,
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes","+", 1.00, "+", 1.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);

            var st5 = psvc.AddSightTest (px3.Pid, "history", "GH", "OH", "occupation", "driver", "smoker", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15,
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes", "+", 4.00, "+", 4.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);

            var st6 = psvc.AddSightTest (px4.Pid, "history", "GH", "OH", "occupation", "driver", "smoker", "+", 2.25, "+", 2.50, 1.25, 1.00, 180, 180, 0.00, 0.00, 
                                        "6/6", "6/6", "6/6", "6/5", "6/5", "6/5", "n5", "n5", "n5", "n5", "n5", "n5", "ortho", "ortho", "ortho", "ortho", 
                                        "OMBNotes", "AdditionalTests", "+", 1.00, "+", 1.25, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "+", 1.25, "+", 1.50, 1.00, 1.00, 10, 15,
                                        "+", 2.00, "+", 2.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "RefractionNotes", "6/5", "6/5", "6/5", "FinalOMBDist", "FinalOMBNear", 
                                        "FinalOMBNotes", "+", 3.00, "+", 3.00, 1.00, 1.00, 5, 5, 2.00, 2.00, "no", "DrugUsed", 14.30, "PupilExam", 
                                        "ACExam", 22.2, 22.1, "IOPInstrument", 14.30, "Adnexa", "LidsLashes", "Conjunctiva", "Cornea", "Lens", "Vitreous",
                                        "OpticNerve", "Macula", "Vessels", "Periphery", "AdditionalTestsFinal", "Advice", "Recommendations", "Referrals",
                                        12);
            
        }
          
        
    }
}
