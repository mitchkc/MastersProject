using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client.Advanced;

namespace MastersProject.Data.Entities;

public class SightTest
{
    public int STId { get; set; } //primary key
    public DateTime CreatedOn { get; set; } = DateTime.MinValue; //default 
    public int PatientId { get; set; } //foreign key
    public Patient Patient{ get; set; } //help with navigation, is this needed?
    public string History { get; set; }
    public string GeneralHealth { get; set; }
    public string OphthalmicHistory { get; set; }
    public string Occupation { get; set; }
    public string Driver { get; set; }
    public int Sphere { get; set; }
    public int Cylinder { get; set; }
    public int Axis { get; set; }
    public int Addition { get; set; }
    public string DistanceVisionRE { get; set; }
    public string DistanceVisionLE { get; set; }
    public string DistanceVisionBinoc { get; set; }
    public string DistanceVARE { get; set; }
    public string DistanceVALE { get; set; }
    public string DistanceVABinoc { get; set; }
    public string NearVABinoc { get; set; }
    public string NearVARE { get; set; }
    public string NearVALE { get; set; }
    public string NearVisionBinoc { get; set; }
    public string NearVisionRE { get; set; }
    public string NearVisionLE { get; set; }
    public string OMBNearUnaided { get; set; }
    public string OMBNearAided { get; set; }
    public string OMBDistAided { get; set; }
    public string OMBDistUnaided { get; set; }
    public string OMBNotes { get; set; }
    public string AdditionalTests { get; set; }
    public int AutorefractSphereRE { get; set; }
    public int AutorefractSphereLE { get; set; }
    public int AutorefractCylRE { get; set; }
    public int AutorefractCylLE { get; set; }
    public int AutorefractAddRE { get; set; }
    public int AutorefractAddLE { get; set; }
    public int RetSphereRE { get; set; }
    public int RetSphereLE { get; set; }
    public int RetCylRE { get; set; }
    public int RetCylLE { get; set; }
    public int RetAxisRE { get; set; }
    public int RetAxisLE { get; set; }
    public int DropsUsed { get; set; } // checkbox; checked = 1, not checked = 0
    public int SubjectSphereRE { get; set; }
    public int SubjectSphereLE { get; set; }
    public int SubjectCylRE { get; set; }
    public int SubjectCylLE { get; set; }
    public int SubjectAxisRE { get; set; }
    public int SubjectAxisLE { get; set; }
    public int SubjectAddRE { get; set; }
    public int SubjectAddLE { get; set; }
    public string RefractionNotes { get; set; }
    public string FinalVARE { get; set; }
    public string FinalVALE { get; set; }
    public string FinalVABinoc  { get; set; }
    public string FinalOMBDist { get; set; }
    public string FinalOMBNear { get; set; }
    public string FinalOMBNotes { get; set; }
    public int FinalSphereRE { get; set; }
    public int FinalSphereLE { get; set; }
    public int FinalCylRE { get; set; }
    public int FinalCylLE { get; set; }
    public int FinalAxisRE { get; set; }
    public int FinalAxisLE { get; set; }
    public int FinalAddRE { get; set; }
    public int FinalAddLE { get; set; }
    public int Dilated { get; set; } // checkbox; checked = 1, not checked = 0
    public string DrugUsed { get; set; }
    public int TimeDrugUsed { get; set; }
    public string PupilExam { get; set; }
    public string ACExam { get; set; }
    public int IOPRE { get; set; }
    public int IOPLE { get; set; }
    public string IOPInstrument { get; set; }
    public int IOPTime { get; set; }
    public string Adnexa { get; set; }
    public string LidsLashes { get; set; }
    public string Conjunctiva { get; set; }
    public string Cornea { get; set; }
    public string Lens { get; set; }
    public string Vitreous { get; set; }
    public string OpticNerve { get; set; }
    public string Macula { get; set; }
    public string Vessels { get; set; }
    public string Periphery { get; set; }
    // public Image Illustration - how do i store image in the Db?
    public string AdditionalTestsFinal { get; set; }
    public string Advice { get; set; }
    public string Recommendations { get; set; }
    public string Referrals { get; set; }
    public int Recall { get; set; }
}