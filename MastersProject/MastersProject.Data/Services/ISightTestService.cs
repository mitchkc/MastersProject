

using MastersProject.Data.Entities;

namespace MastersProject.Data.Services
{
    public interface ISightTestService
    {
        void Initialise();

        // ---------------- Sight Test Management ----------------- //

        IList<SightTest> GetSightTests();
        Paged<SightTest> GetSightTests(int page=1, int size=20, string orderBy="stid", string direction="asc");
        SightTest GetSightTest(int stid);
        SightTest AddSightTest( int STId, DateTime CreatedOn, int PatientId, Patient Patient, string History, string GeneralHealth, string OphthalmicHistory, string Occupation, string Driver, 
                                int Sphere, int Cylinder, int Axis, int Addition, string DistanceVisionRE, string DistanceVisionLE, string DistanceVisionBinoc, string DistanceVARE, 
                                string DistanceVALE, string DistanceVABinoc, string NearVABinoc, string NearVARE, string NearVALE, string NearVisionBinoc, string NearVisionRE, string NearVisionLE, 
                                string OMBNearUnaided, string OMBNearAided, string OMBDistAided, string OMBDistUnaided, string OMBNotes, string AdditionalTests, int AutorefractSphereRE, 
                                int AutorefractSphereLE, int AutorefractCylRE, int AutorefractCylLE, int AutorefractAddRE, int AutorefractAddLE, int RetSphereRE, int RetSphereLE, int RetCylRE, 
                                int RetCylLE, int RetAxisRE, int RetAxisLE, int DropsUsed,  int SubjectSphereRE, int SubjectSphereLE, int SubjectCylRE, int SubjectCylLE, int SubjectAxisRE, 
                                int SubjectAxisLE, int SubjectAddRE, int SubjectAddLE, string RefractionNotes, string FinalVARE, string FinalVALE, string FinalVABinoc,  string FinalOMBDist, 
                                string FinalOMBNear, string FinalOMBNotes, int FinalSphereRE, int FinalSphereLE, int FinalCylRE, int FinalCylLE, int FinalAxisRE, int FinalAxisLE, int FinalAddRE, 
                                int FinalAddLE, int Dilated,  string DrugUsed, int TimeDrugUsed, string PupilExam, string ACExam, int IOPRE, int IOPLE, string IOPInstrument, int IOPTime, string Adnexa, 
                                string LidsLashes, string Conjunctiva, string Cornea, string Lens, string Vitreous, string OpticNerve, string Macula, string Vessels, string Periphery, 
                                string AdditionalTestsFinal, string Advice,string Recommendations, string Referrals, int Recall);
        bool DeleteSightTest(int stid);
    }
}