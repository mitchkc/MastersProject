using System.ComponentModel.DataAnnotations;
using MastersProject.Data.Entities;

public class CreateSTViewModel
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



    // ---------------------------------------------- PREVIOUS ST --------------------------------- //

    public int? PrevSTId { get; set; } //primary key
    public DateTime? PrevCreatedOn { get; set; } = DateTime.MinValue; //default 
    
    public IList<SightTest> PrevSightTests { get; set; } = new List<SightTest>();
    public string PrevHistory { get; set; }
    public string PrevGeneralHealth { get; set; }
    public string PrevOphthalmicHistory { get; set; }
    public string PrevOccupation { get; set; }
    public string PrevDriver { get; set; }
    public string PrevSmoker { get; set; }
    public string PrevCurrentSignR { get; set; }
    public double? PrevCurrentSphereRE { get; set; }
    public string PrevCurrentSignL { get; set; }
    public double? PrevCurrentSphereLE { get; set; }
    public double? PrevCurrentCylRE { get; set; }
    public double? PrevCurrentCylLE { get; set; }
    public int? PrevCurrentAxisRE { get; set; }
    public int? PrevCurrentAxisLE { get; set; }
    public double? PrevCurrentAddRE { get; set; }
    public double? PrevCurrentAddLE { get; set; }
    public string PrevDistanceVisionRE { get; set; }
    public string PrevDistanceVisionLE { get; set; }
    public string PrevDistanceVisionBinoc { get; set; }
    public string PrevDistanceVARE { get; set; }
    public string PrevDistanceVALE { get; set; }
    public string PrevDistanceVABinoc { get; set; }
    public string PrevNearVABinoc { get; set; }
    public string PrevNearVARE { get; set; }
    public string PrevNearVALE { get; set; }
    public string PrevNearVisionBinoc { get; set; }
    public string PrevNearVisionRE { get; set; }
    public string PrevNearVisionLE { get; set; }
    public string PrevOMBNearUnaided { get; set; }
    public string PrevOMBNearAided { get; set; }
    public string PrevOMBDistAided { get; set; }
    public string PrevOMBDistUnaided { get; set; }
    public string PrevOMBNotes { get; set; }
    public string PrevAdditionalTests { get; set; }

    // ------------- Refraction ---------------//
    public string PrevAutoSignR { get; set; }
    public double? PrevAutoSphereRE { get; set; }
    public string PrevAutoSignL  { get; set;}
    public double? PrevAutoSphereLE { get; set; }
    public double? PrevAutorefractCylRE { get; set; }
    public double? PrevAutorefractCylLE { get; set; }
    public int? PrevAutorefractAxisRE { get; set; }
    public int? PrevAutorefractAxisLE { get; set; }
    public double? PrevAutorefractAddRE { get; set; }
    public double? PrevAutorefractAddLE { get; set; }
    public string PrevDropsUsed { get; set; }
    public string PrevRetSignR { get; set; }
    public double? PrevRetSphereRE { get; set; }
    public string PrevRetSignL { get; set; }
    public double? PrevRetSphereLE { get; set; }
    public double? PrevRetCylRE { get; set; }
    public double? PrevRetCylLE { get; set; }
    public int? PrevRetAxisRE { get; set; }
    public int? PrevRetAxisLE { get; set; }
    public string PrevSubjectSignR { get; set; }
    public double? PrevSubjectSphereRE { get; set; }
    public string PrevSubjectSignL { get; set; }
    public double? PrevSubjectSphereLE { get; set; }
    public double? PrevSubjectCylRE { get; set; }
    public double? PrevSubjectCylLE { get; set; }
    public int? PrevSubjectAxisRE { get; set; }
    public int? PrevSubjectAxisLE { get; set; }
    public double? PrevSubjectAddRE { get; set; }
    public double? PrevSubjectAddLE { get; set; }
    public string PrevRefractionNotes { get; set; }
    public string PrevFinalVARE { get; set; }
    public string PrevFinalVALE { get; set; }
    public string PrevFinalVABinoc  { get; set; }
    public string PrevFinalOMBDist { get; set; }
    public string PrevFinalOMBNear { get; set; }
    public string PrevFinalOMBNotes { get; set; }
    public string PrevFinalSphereSignRE { get; set; }
    public string PrevFinalSphereSignLE { get; set; }
    public double? PrevFinalSphereRE { get; set; }
    public double? PrevFinalSphereLE { get; set; }
    public double? PrevFinalCylRE { get; set; }
    public double? PrevFinalCylLE { get; set; }
    public int? PrevFinalAxisRE { get; set; }
    public int? PrevFinalAxisLE { get; set; }
    public double? PrevFinalAddRE { get; set; }
    public double? PrevFinalAddLE { get; set; }
    public string PrevAdditionalRefractionTests { get; set; }

// ---------------- Ocular Exam -------------- //
    
    public string PrevDilated { get; set; } // checkbox 
    public string PrevDrugUsed { get; set; }
    public double? PrevTimeDrugUsed { get; set; }
    public string PrevPupilExam { get; set; }
    public double? PrevIOPRE { get; set; }
    public double? PrevIOPLE { get; set; }
    public string PrevIOPInstrument { get; set; }
    public double? PrevIOPTime { get; set; }


    // --------- External -----------//
    public string PrevAdnexa { get; set; }
    public string PrevLidsLashes { get; set; }
    public string PrevConjunctiva { get; set; }
    public string PrevCornea { get; set; }


    // ------------- Internal ------------ //

    public string PrevLens { get; set; }
    public string PrevACExam { get; set; }
    public string PrevVitreous { get; set; }
    public string PrevOpticNerve { get; set; }
    public string PrevMacula { get; set; }
    public string PrevVessels { get; set; }
    public string PrevPeriphery { get; set; }


    // ---------------- End ---------------- //
    public string PrevAdditionalTestsFinal { get; set; }
    public string PrevAdvice { get; set; }
    public string PrevRecommendations { get; set; }
    public string PrevReferrals { get; set; }
    public int? PrevRecall { get; set; }
}