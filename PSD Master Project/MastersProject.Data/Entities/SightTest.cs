using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client.Advanced;

namespace MastersProject.Data.Entities;

public class SightTest
{
    [Key]
    public int STId { get; set; } //primary key
    public int PatientId { get; set; } //foreign key
    public DateTime CreatedOn { get; set; } = DateTime.MinValue; //default 
    
    public Patient Patient{ get; set; } //for association of ST with Px
    public IList<SightTest> SightTests { get; set; } = new List<SightTest>();
    public string History { get; set; }
    public string GeneralHealth { get; set; }
    public string OphthalmicHistory { get; set; }
    public string Occupation { get; set; }
    public string Driver { get; set; }
    public string Smoker { get; set; }
    public string CurrentSignR { get; set; }
    public double CurrentSphereRE { get; set; }
    public string CurrentSignL { get; set; }
    public double CurrentSphereLE { get; set; }
    public double CurrentCylRE { get; set; }
    public double CurrentCylLE { get; set; }
    public int CurrentAxisRE { get; set; }
    public int CurrentAxisLE { get; set; }
    public double CurrentAddRE { get; set; }
    public double CurrentAddLE { get; set; }
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

    // ------------- Refraction ---------------//
    public string AutoSignR { get; set; }
    public double AutoSphereRE { get; set; }
    public string AutoSignL  { get; set;}
    public double AutoSphereLE { get; set; }
    public double AutorefractCylRE { get; set; }
    public double AutorefractCylLE { get; set; }
    public int AutorefractAxisRE { get; set; }
    public int AutorefractAxisLE { get; set; }
    public double AutorefractAddRE { get; set; }
    public double AutorefractAddLE { get; set; }
    public string DropsUsed { get; set; }
    public string RetSignR { get; set; }
    public double RetSphereRE { get; set; }
    public string RetSignL { get; set; }
    public double RetSphereLE { get; set; }
    public double RetCylRE { get; set; }
    public double RetCylLE { get; set; }
    public int RetAxisRE { get; set; }
    public int RetAxisLE { get; set; }
    public string SubjectSignR { get; set; }
    public double SubjectSphereRE { get; set; }
    public string SubjectSignL { get; set; }
    public double SubjectSphereLE { get; set; }
    public double SubjectCylRE { get; set; }
    public double SubjectCylLE { get; set; }
    public int SubjectAxisRE { get; set; }
    public int SubjectAxisLE { get; set; }
    public double SubjectAddRE { get; set; }
    public double SubjectAddLE { get; set; }
    public string RefractionNotes { get; set; }
    public string FinalVARE { get; set; }
    public string FinalVALE { get; set; }
    public string FinalVABinoc  { get; set; }
    public string FinalOMBDist { get; set; }
    public string FinalOMBNear { get; set; }
    public string FinalOMBNotes { get; set; }
    public string FinalSphereSignRE { get; set; }
    public string FinalSphereSignLE { get; set; }
    public double FinalSphereRE { get; set; }
    public double FinalSphereLE { get; set; }
    public double FinalCylRE { get; set; }
    public double FinalCylLE { get; set; }
    public int FinalAxisRE { get; set; }
    public int FinalAxisLE { get; set; }
    public double FinalAddRE { get; set; }
    public double FinalAddLE { get; set; }
    public string AdditionalRefractionTests { get; set; }

// ---------------- Ocular Exam -------------- //
    
    public string Dilated { get; set; } 
    public string DrugUsed { get; set; }
    public double TimeDrugUsed { get; set; }
    public string PupilExam { get; set; }
    public double IOPRE { get; set; }
    public double IOPLE { get; set; }
    public string IOPInstrument { get; set; }
    public double IOPTime { get; set; }


    // --------- External -----------//
    public string Adnexa { get; set; }
    public string LidsLashes { get; set; }
    public string Conjunctiva { get; set; }
    public string Cornea { get; set; }


    // ------------- Internal ------------ //

    public string Lens { get; set; }
    public string ACExam { get; set; }
    public string Vitreous { get; set; }
    public string OpticNerve { get; set; }
    public string Macula { get; set; }
    public string Vessels { get; set; }
    public string Periphery { get; set; }


    // ---------------- End ---------------- //
    public string AdditionalTestsFinal { get; set; }
    public string Advice { get; set; }
    public string Recommendations { get; set; }
    public string Referrals { get; set; }
    [Required]
    [Range(1,int.MaxValue, ErrorMessage = "Value must be greater than 0")]
    public int Recall { get; set; }
}