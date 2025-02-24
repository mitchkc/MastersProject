

using System.Data.Common;
using MastersProject.Data.Entities;
using MastersProject.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MastersProject.Data.Services
{
    public class PatientServiceDb : IPatientService
    {
        private readonly DatabaseContext ctx;

        public PatientServiceDb(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public void Initialise()
        {
            ctx.Initialise();
        }

        // ------------------------------- Patient Operations ----------------------------- //

        //retrieve list of patients 
        public List<Patient> GetPatients(string orderBy = "forename", string direction = "asc")
        {
            var query = (orderBy.ToLower(), direction.ToLower()) switch
            {
                ("forename", "asc") => ctx.Patients.OrderBy(r => r.Forename),
                ("forename", "desc") => ctx.Patients.OrderByDescending(r => r.Forename),
                ("surname", "asc") => ctx.Patients.OrderBy(r => r.Surname),
                ("surname", "desc") => ctx.Patients.OrderByDescending(r => r.Surname),
                ("age", "asc") => ctx.Patients.OrderBy(r => r.Age),
                ("age", "desc") => ctx.Patients.OrderByDescending(r => r.Age),
                _ => ctx.Patients.OrderBy(r => r.Forename)
            };

            return query.ToList();
        }

        

        
        // Retrive Patient by Id 
        public Patient GetPatient(int id)
        {
            return ctx.Patients.FirstOrDefault(s => s.Pid == id);
        }


        public Patient AddPatient(Patient p)
        {
            var patient = new Patient
            {
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                Address = p.Address,
                Gender = p.Gender,
                Dob = p.Dob,
                Mobile = p.Mobile,
                HomeNumber = p.HomeNumber,
                GPName = p.GPName,
                HandCNum = p.HandCNum,
                GPAddress = p.GPAddress,
                PatientType = p.PatientType,
                Opt = p.Opt,
            };
            ctx.Patients.Add(patient);
            ctx.SaveChanges();
            return patient; // return newly added Patient 
        }

        // Delete the Patient identified by Id returning true if deleted and false if not found
        public bool DeletePatient(int id)
        {
            var s = GetPatient(id);
            if (s == null)
            {
                return false;
            }
            ctx.Patients.Remove(s);
            ctx.SaveChanges();
            return true;
        }

        // Update the Patient with the details in updated 
        public Patient UpdatePatient(Patient updated)
        {
            // verify the Patient exists
            var Patient = GetPatient(updated.Pid);
            if (Patient == null)
            {
                return null;
            }
            // verify email address is registered or available to this Patient
            if (!IsEmailAvailable(updated.Email, updated.Pid))
            {
                return null;
            }
            // update the details of the Patient retrieved and save
            Patient.Forename = updated.Forename;
            Patient.Surname = updated.Surname;
            Patient.Email = updated.Email;
            Patient.Address = updated.Address;
            Patient.Gender = updated.Gender;
            Patient.Dob = updated.Dob;
            Patient.Mobile = updated.Mobile;
            Patient.HomeNumber = updated.HomeNumber;
            Patient.GPName = updated.GPName;
            Patient.HandCNum = updated.HandCNum;
            Patient.GPAddress = updated.GPAddress;
            Patient.PatientType = updated.PatientType;
            Patient.Opt = updated.Opt;

            ctx.SaveChanges();
            return Patient;
        }

        // Verify if email is available or registered to specified patient
        public bool IsEmailAvailable(string email, int patientId)
        {
            return ctx.Patients.FirstOrDefault(u => u.Email == email && u.Pid != patientId) == null;
        }

        public IList<Patient> GetPatientsQuery(Func<Patient, bool> q)
        {
            return ctx.Patients.Where(q).ToList();
        }


        public IList<Patient> SearchPatients(string queryFname = null, string querySname = null, DateTime? dob = null)
        {
            queryFname = queryFname?.ToLower();
            querySname = querySname?.ToLower();

            var query = ctx.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(queryFname))
            {
                query = query.Where(p => p.Forename.ToLower().Contains(queryFname));
            }

            if (!string.IsNullOrEmpty(querySname))
            {
                query = query.Where(p => p.Surname.ToLower().Contains(querySname));
            }

            if (dob.HasValue)
            {
                query = query.Where(p => p.Dob == dob.Value);
            }



            return query.OrderBy(p => p.Forename).ToList();
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return ctx.Patients.Include( p => p.SightTests).ToList();
        }



        // -------------------------------- SightTest Operations -------------------------------------- //

        public IList<SightTest> GetSightTests(int pId)
        {
            var list = ctx.SightTests.Where(s => s.PatientId == pId).ToList();
            return list;
        }
        public SightTest GetLatestSTByPatient(int pid)
        {
            return ctx.SightTests.Where( st => st.PatientId == pid).OrderByDescending( st => st.CreatedOn).FirstOrDefault();
        }

        public Paged<SightTest> GetSightTests(int page = 1, int pageSize = 10, string orderBy = "stid", string direction = "asc")
        {
            var query = (orderBy.ToLower(), direction.ToLower()) switch
            {
                ("stid", "asc") => ctx.SightTests.OrderBy(r => r.STId),
                ("stid", "desc") => ctx.SightTests.OrderByDescending(r => r.STId),
                ("createdon", "asc") => ctx.SightTests.OrderBy(r => r.CreatedOn),
                ("createdon", "desc") => ctx.SightTests.OrderByDescending(r => r.CreatedOn),
                _ => ctx.SightTests.OrderBy(r => r.STId)
            };

            return query.ToPaged(page, pageSize, orderBy, direction);
        }

        public SightTest GetSightTest(int id)
        {
            return ctx.SightTests.FirstOrDefault(s => s.STId == id);
        }

        public SightTest AddSightTest(int patientId, string history, string generalHealth, string ophthalmicHistory, string occupation, string driver,
                                string smoker, string currentSignR, double currentSphereRE, string currentSignL, double currentSphereLE, double currentCylRE, double currentCylLE, int currentAxisRE, 
                                int currentAxisLE, double currentAddRE, double currentAddLE, string distanceVisionRE, string distanceVisionLE, string distanceVisionBinoc, string distanceVARE,
                                string distanceVALE, string distanceVABinoc, string nearVABinoc, string nearVARE, string nearVALE, string nearVisionBinoc, string nearVisionRE,
                                string nearVisionLE, string ombNearUnaided, string ombNearAided, string ombDistAided, string ombDistUnaided, string ombNotes,
                                string additionalTests, string autoSignR, double autoSphereRE, string autoSignL, double autoSphereLE, double autorefractCylRE,
                                double autorefractCylLE, int autorefractAxisRE, int autorefractAxisLE, double autorefractAddRE, double autorefractAddLE, string dropsUsed, string retSignR, 
                                double retSphereRE, string retSignL, double retSphereLE, double retCylRE, double retCylLE, int retAxisRE, int retAxisLE,  string subjectSignR, 
                                double subjectSphereRE, string subjectSignL, double subjectSphereLE, double subjectCylRE, double subjectCylLE, 
                                int subjectAxisRE, int subjectAxisLE, double subjectAddRE, double subjectAddLE, string refractionNotes, string finalVARE, string finalVALE, string finalVABinoc, 
                                string finalOMBDist, string finalOMBNear, string finalOMBNotes, string finalSphereSignRE, double finalSphereRE, string finalSphereSignLE, double finalSphereLE, 
                                double finalCylRE, double finalCylLE, int finalAxisRE, int finalAxisLE, double finalAddRE,
                                double finalAddLE, string dilated, string drugUsed, double timeDrugUsed, string pupilExam, string acExam, double iopRE, double iopLE, string iopInstrument, double iopTime, string adnexa,
                                string lidsLashes, string conjunctiva, string cornea, string lens, string vitreous, string opticNerve, string macula, string vessels, string periphery,
                                string additionalTestsFinal, string advice, string recommendations, string referrals, int recall)
        {
            var sightTest = new SightTest
            {
                CreatedOn = DateTime.Now,
                PatientId = patientId,
                History = history,
                GeneralHealth = generalHealth,
                OphthalmicHistory = ophthalmicHistory,
                Occupation = occupation,
                Driver = driver,
                Smoker = smoker,
                CurrentSignR = currentSignR,
                CurrentSphereRE = currentSphereRE,
                CurrentSignL = currentSignL,
                CurrentSphereLE = currentSphereLE,
                CurrentCylRE = currentCylRE,
                CurrentCylLE = currentCylLE,
                CurrentAxisRE = currentAxisRE,
                CurrentAxisLE = currentAxisLE,
                CurrentAddRE = currentAddRE,
                CurrentAddLE = currentAddLE,
                DistanceVisionRE = distanceVisionRE,
                DistanceVisionLE = distanceVisionLE,
                DistanceVisionBinoc = distanceVisionBinoc,
                DistanceVARE = distanceVARE,
                DistanceVALE = distanceVALE,
                DistanceVABinoc = distanceVABinoc,
                NearVABinoc = nearVABinoc,
                NearVARE = nearVARE,
                NearVALE = nearVALE,
                NearVisionBinoc = nearVisionBinoc,
                NearVisionRE = nearVisionRE,
                NearVisionLE = nearVisionLE,
                OMBNearUnaided = ombNearUnaided,
                OMBNearAided = ombNearAided,
                OMBDistAided = ombDistAided,
                OMBDistUnaided = ombDistUnaided,
                OMBNotes = ombNotes,
                AdditionalTests = additionalTests,
                AutoSignR = autoSignR,
                AutoSphereRE = autoSphereRE,
                AutoSignL = autoSignL,
                AutoSphereLE = autoSphereLE,
                AutorefractCylRE = autorefractCylRE,
                AutorefractCylLE = autorefractCylLE,
                AutorefractAxisRE = autorefractAxisRE,
                AutorefractAxisLE = autorefractAxisLE,
                AutorefractAddRE = autorefractAddRE,
                AutorefractAddLE = autorefractAddLE,
                DropsUsed = dropsUsed,
                RetSignR = retSignR,
                RetSphereRE = retSphereRE,
                RetSignL = retSignL,
                RetSphereLE = retSphereLE,
                RetCylRE = retCylRE,
                RetCylLE = retCylLE,
                RetAxisRE = retAxisRE,
                RetAxisLE = retAxisLE,
                SubjectSignR = subjectSignR,
                SubjectSphereRE = subjectSphereRE,
                SubjectSignL = subjectSignL,
                SubjectSphereLE = subjectSphereLE,
                SubjectCylRE = subjectCylRE,
                SubjectCylLE = subjectCylLE,
                SubjectAxisRE = subjectAxisRE,
                SubjectAxisLE = subjectAxisLE,
                SubjectAddRE = subjectAddRE,
                SubjectAddLE = subjectAddLE,
                RefractionNotes = refractionNotes,
                FinalVARE = finalVARE,
                FinalVALE = finalVALE,
                FinalVABinoc = finalVABinoc,
                FinalOMBDist = finalOMBDist,
                FinalOMBNear = finalOMBNear,
                FinalOMBNotes = finalOMBNotes,
                FinalSphereSignRE = finalSphereSignRE,
                FinalSphereRE = finalSphereRE,
                FinalSphereSignLE = finalSphereSignLE,
                FinalSphereLE = finalSphereLE,
                FinalCylRE = finalCylRE,
                FinalCylLE = finalCylLE,
                FinalAxisRE = finalAxisRE,
                FinalAxisLE = finalAxisLE,
                FinalAddRE = finalAddRE,
                FinalAddLE = finalAddLE,
                Dilated = dilated,
                DrugUsed = drugUsed,
                TimeDrugUsed = timeDrugUsed,
                PupilExam = pupilExam,
                ACExam = acExam,
                IOPRE = iopRE,
                IOPLE = iopLE,
                IOPInstrument = iopInstrument,
                IOPTime = iopTime,
                Adnexa = adnexa,
                LidsLashes = lidsLashes,
                Conjunctiva = conjunctiva,
                Cornea = cornea,
                Lens = lens,
                Vitreous = vitreous,
                OpticNerve = opticNerve,
                Macula = macula,
                Vessels = vessels,
                Periphery = periphery,
                AdditionalTestsFinal = additionalTestsFinal,
                Advice = advice,
                Recommendations = recommendations,
                Referrals = referrals,
                Recall = recall
            };
            ctx.SightTests.Add(sightTest);
            ctx.SaveChanges();
            return sightTest;
        }

        public bool DeleteSightTest(int id)
        {
            var s = GetSightTest(id);
            if (s == null)
            {
                return false;
            }
            ctx.SightTests.Remove(s);
            ctx.SaveChanges();
            return true;
        }



        // --------------------------------- Triage Operations --------------------------------------- //
        public IList<Triage> GetTriages(int pId)
        {
            var list = ctx.Triages.Where(t => t.PatientId == pId).ToList();
            return list;
        }

        public Paged<Triage> GetTriages(int page = 1, int pageSize = 10, string orderBy = "createdon", string direction = "asc")
        {
            var query = (orderBy.ToLower(), direction.ToLower()) switch
            {
                ("tid", "asc") => ctx.Triages.OrderBy(r => r.TriageId),
                ("tid", "desc") => ctx.Triages.OrderByDescending(r => r.TriageId),
                ("createdOn", "asc") => ctx.Triages.OrderBy(r => r.CreatedOn),
                ("createdOn", "desc") => ctx.Triages.OrderByDescending(r => r.CreatedOn),
                _ => ctx.Triages.OrderBy(r => r.CreatedOn)
            };
            return query.ToPaged(page, pageSize, orderBy, direction);
        }

        public Triage GetTriage(int id)
        {
            return ctx.Triages.FirstOrDefault(r => r.TriageId == id);
        }

        public Triage AddTriage(int pId, string pxLocation, string lastSt, string lastStLocation, string pxDescript, string issueStart,
                                string hapBefore, string pain, int painRating, string redness, string discharge, string photosens, string visionAffect,
                                string clWearer, string clInfo, string flashes, string flashesNew, string flashesWorsening, string floaters,
                                string floatersNew, string floatersWorsening, string optom, string advice)
        {
            var px = GetPatient(pId);
            if (px == null) return null;

            var triage = new Triage
            {
                PatientId = pId,
                CreatedOn = DateTime.Now,
                PxLocation = pxLocation,
                LastST = lastSt,
                LastSTLocation = lastStLocation,
                PxDescript = pxDescript,
                IssueStart = issueStart,
                HapBefore = hapBefore,
                Pain = pain,
                PainRating = painRating,
                Redness = redness,
                Discharge = discharge,
                Photosensitivity = photosens,
                VisionAffected = visionAffect,
                CLWearer = clWearer,
                CLInfo = clInfo,
                Flashes = flashes,
                FlashesNew = flashesNew,
                FlashWorsening = flashesWorsening,
                Floaters = floaters,
                FloatersNew = floatersNew,
                FloatersWorsening = floatersWorsening,
                Optom = optom,
                Advice = advice,
            };
            ctx.Triages.Add(triage);
            ctx.SaveChanges();
            return triage;
        }

        public bool DeleteTriage(int id)
        {
            var t = GetTriage(id);
            if (t == null)
            {
                return false;
            }
            ctx.Triages.Remove(t);
            ctx.SaveChanges();
            return true;
        }
    }
}