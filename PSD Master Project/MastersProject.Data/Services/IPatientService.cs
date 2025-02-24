
using MastersProject.Data.Entities;

namespace  MastersProject.Data.Services
{

    public interface IPatientService
    {
        void Initialise();

        //----------- Patient management ---------------//
        IEnumerable<Patient> GetAllPatients();
        List<Patient> GetPatients(string orderBy="forename", string direction="asc");
        Patient GetPatient(int pid);
        bool IsEmailAvailable(string email, int patientId);
        Patient AddPatient(Patient patient);
        Patient UpdatePatient(Patient patient);
        bool DeletePatient(int pid);

        IList<Patient> SearchPatients(string queryFname=null, string querySname=null, DateTime? dob = null);

        // ---------------- Sight Test Management ----------------- // 

        IList<SightTest> GetSightTests(int id);
        Paged<SightTest> GetSightTests(int page=1, int size=20, string orderBy="stid", string direction="asc");
        SightTest GetSightTest(int stid);
        SightTest AddSightTest( int PatientId, string History, string GeneralHealth, string OphthalmicHistory, string Occupation, string Driver, 
                                string smoker, string CurrentSignR, double CurrentSphereRE, string CurrentSignL, double CurrentSphereLE, double CurrentCylRE, double CurrentCylLE, int CurrentAxisRE, int CurrentAxisLE, 
                                double CurrentAddRE, double CurrentAddLE, string DistanceVisionRE, string DistanceVisionLE, string DistanceVisionBinoc, 
                                string DistanceVARE, string DistanceVALE, string DistanceVABinoc, string NearVABinoc, string NearVARE, 
                                string NearVALE, string NearVisionBinoc, string NearVisionRE, string NearVisionLE, string OMBNearUnaided, 
                                string OMBNearAided, string OMBDistAided, string OMBDistUnaided, string OMBNotes, string AdditionalTests, 
                                string AutoSignR, double AutoSphereRE, string AutoSignL, double AutoSphereLE, double AutorefractCylRE, double AutorefractCylLE, 
                                int AutorefractAxisRE, int AutorefractAxisLE, double AutorefractAddRE, double AutorefractAddLE,string DropsUsed, string RetSignR, double RetSphereRE, 
                                string RetSignL, double RetSphereLE, double RetCylRE, double RetCylLE, int RetAxisRE, int RetAxisLE,   
                                string SubjectiveSignR, double SubjectSphereRE, string SubjectiveSignL, double SubjectSphereLE, double SubjectCylRE, 
                                double SubjectCylLE, int SubjectAxisRE, int SubjectAxisLE, double SubjectAddRE, double SubjectAddLE, string RefractionNotes,
                                string FinalVARE, string FinalVALE, string FinalVABinoc,  string FinalOMBDist, string FinalOMBNear, 
                                string FinalOMBNotes, string FinalSphereSignRE, double FinalSphereRE, string FinalSphereSignLE, double FinalSphereLE, double FinalCylRE, double FinalCylLE, int FinalAxisRE, 
                                int FinalAxisLE, double FinalAddRE, double FinalAddLE, string Dilated,  string DrugUsed, double TimeDrugUsed, string PupilExam, 
                                string ACExam, double IOPRE, double IOPLE, string IOPInstrument, double IOPTime, string Adnexa, 
                                string LidsLashes, string Conjunctiva, string Cornea, string Lens, string Vitreous, string OpticNerve, 
                                string Macula, string Vessels, string Periphery, string AdditionalTestsFinal, 
                                string Advice,string Recommendations, string Referrals, int Recall);
        bool DeleteSightTest(int stid);
        SightTest GetLatestSTByPatient(int pid);


        // ----------------- Triage Management ------------- // 
 
        IList<Triage> GetTriages(int id);
        Paged<Triage> GetTriages(int page=1, int size=20, string orderBy="id", string direction="asc");
        Triage GetTriage(int triageId);
        Triage AddTriage(int pId, string pxLocation, string lastSt, string lastStLocation, string pxDescript, string issueStart, string hapBefore, string pain, 
                         int painRating, string redness, string discharge, string photosens, string visionAffect, string clWearer, string clInfo, string flashes, string flashesNew, 
                         string flashesWorsening, string floaters, string floatersNew, string floatersWorsening, string optom, string advice);
        bool DeleteTriage(int triageId) ;
    
    }
}