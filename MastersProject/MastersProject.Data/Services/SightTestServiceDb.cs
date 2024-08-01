

using MastersProject.Data.Entities;
using MastersProject.Data.Repositories;
using SQLitePCL;

namespace MastersProject.Data.Services
{
    public class SightTestServiceDb : ISightTestService
    {
        private readonly DatabaseContext ctx;

        public SightTestServiceDb(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public void Initialise()
        {
            ctx.Initialise();
        }

        // --------------- SightTest Operations ------------- //

        public IList<SightTest> GetSightTests()
        {
            return ctx.SightTests.ToList();
        }

        public Paged<SightTest> GetSightTests(int page = 1, int pageSize = 10, string orderBy = "stid", string direction = "asc")
        {
            var query = (orderBy.ToLower(), direction.ToLower()) switch
            {
                ("stid", "asc") => ctx.SightTests.OrderBy(r => r.STId),
                ("stid", "desc") => ctx.SightTests.OrderByDescending(r => r.STId),
                ("createdon", "asc") => ctx.SightTests.OrderBy(r => r.CreatedOn),
                ("createdon", "desc") => ctx.SightTests.OrderByDescending(r => r.CreatedOn),
                _               => ctx.SightTests.OrderBy(r => r.STId) 
            };

            return query.ToPaged(page,pageSize,orderBy,direction);
        }

        public SightTest GetSightTest(int id)
        {
            return ctx.SightTests.FirstOrDefault(s => s.STId == id);
        }

        public SightTest AddSightTest( int stid, DateTime createdOn, int patientId, Patient patient, string history, string generalHealth, string ophthalmicHistory, string occupation, string driver, 
                                int sphere, int cylinder, int axis, int addition, string distanceVisionRE, string distanceVisionLE, string distanceVisionBinoc, string distanceVARE, 
                                string distanceVALE, string distanceVABinoc, string nearVABinoc, string nearVARE, string nearVALE, string nearVisionBinoc, string nearVisionRE, string nearVisionLE, 
                                string ombNearUnaided, string ombNearAided, string ombDistAided, string ombDistUnaided, string ombNotes, string additionalTests, int autorefractSphereRE, 
                                int autorefractSphereLE, int autorefractCylRE, int autorefractCylLE, int autorefractAddRE, int autorefractAddLE, int retSphereRE, int retSphereLE, int retCylRE, 
                                int retCylLE, int retAxisRE, int retAxisLE, int dropsUsed,  int subjectSphereRE, int subjectSphereLE, int subjectCylRE, int subjectCylLE, int subjectAxisRE, 
                                int subjectAxisLE, int subjectAddRE, int subjectAddLE, string refractionNotes, string finalVARE, string finalVALE, string finalVABinoc,  string finalOMBDist, 
                                string finalOMBNear, string finalOMBNotes, int finalSphereRE, int finalSphereLE, int finalCylRE, int finalCylLE, int finalAxisRE, int finalAxisLE, int finalAddRE, 
                                int finalAddLE, int dilated,  string drugUsed, int timeDrugUsed, string pupilExam, string acExam, int iopRE, int iopLE, string iopInstrument, int iopTime, string adnexa, 
                                string lidsLashes, string conjunctiva, string cornea, string lens, string vitreous, string opticNerve, string macula, string vessels, string periphery, 
                                string additionalTestsFinal, string advice,string recommendations, string referrals, int recall)
        {
            var px = GetPatient(patientId);
            if (px == null) return null;

            var sightTest = new SightTest
            {
                STId = stid, CreatedOn = createdOn, PatientId = patientId, History = history, GeneralHealth = generalHealth,  OphthalmicHistory = ophthalmicHistory,
                Occupation = occupation, Driver = driver, Sphere = sphere, Cylinder = cylinder, Axis = axis, Addition = addition, DistanceVisionRE = distanceVisionRE,
                DistanceVisionLE = distanceVisionLE, DistanceVisionBinoc = distanceVisionBinoc, DistanceVARE = distanceVARE,
                DistanceVALE = distanceVALE, DistanceVABinoc = distanceVABinoc, NearVABinoc = nearVABinoc, NearVARE = nearVARE,
                NearVALE = nearVALE, NearVisionBinoc = nearVisionBinoc, NearVisionRE = nearVisionRE, NearVisionLE = nearVisionLE,
                OMBNearUnaided = ombNearUnaided, OMBNearAided = ombNearAided, OMBDistAided = ombDistAided, OMBDistUnaided = ombDistUnaided,
                OMBNotes = ombNotes, AdditionalTests = additionalTests, AutorefractSphereRE = autorefractSphereRE,
                AutorefractSphereLE = autorefractSphereLE, AutorefractCylRE = autorefractCylRE, AutorefractCylLE = autorefractCylLE,
                AutorefractAddRE = autorefractAddRE, AutorefractAddLE = autorefractAddLE, RetSphereRE = retSphereRE,
                RetSphereLE = retSphereLE, RetCylRE = retCylRE, RetCylLE = retCylLE, RetAxisRE = retAxisRE, RetAxisLE = retAxisLE,
                DropsUsed = dropsUsed, SubjectSphereRE = subjectSphereRE, SubjectSphereLE = subjectSphereLE, SubjectCylRE = subjectCylRE,
                SubjectCylLE = subjectCylLE, SubjectAxisRE = subjectAxisRE, SubjectAxisLE = subjectAxisLE, SubjectAddRE = subjectAddRE,
                SubjectAddLE = subjectAddLE, RefractionNotes = refractionNotes, FinalVARE = finalVARE, FinalVALE = finalVALE, FinalVABinoc = finalVABinoc,
                FinalOMBDist = finalOMBDist, FinalOMBNear = finalOMBNear, FinalOMBNotes = finalOMBNotes, FinalSphereRE = finalSphereRE,
                FinalSphereLE = finalSphereLE, FinalCylRE = finalCylRE, FinalCylLE = finalCylLE, FinalAxisRE = finalAxisRE,
                FinalAxisLE = finalAxisLE, FinalAddRE = finalAddRE,FinalAddLE = finalAddLE, Dilated = dilated, DrugUsed = drugUsed,
                TimeDrugUsed = timeDrugUsed, PupilExam = pupilExam, ACExam = acExam, IOPRE = iopRE, IOPLE = iopLE, IOPInstrument = iopInstrument, 
                IOPTime = iopTime, Adnexa = adnexa, LidsLashes = lidsLashes, Conjunctiva = conjunctiva, Cornea = cornea, Lens = lens,
                Vitreous = vitreous, OpticNerve = opticNerve, Macula = macula, Vessels = vessels, Periphery = periphery,
                AdditionalTestsFinal = additionalTestsFinal, Advice = advice, Recommendations = recommendations,Referrals = referrals,
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
    }
}