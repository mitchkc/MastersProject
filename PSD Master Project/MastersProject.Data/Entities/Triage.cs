using System.ComponentModel.DataAnnotations;
using System;

using System.ComponentModel.DataAnnotations.Schema;

namespace MastersProject.Data.Entities;

public class Triage
{
    [Key]
    public int TriageId { get; set;} // primary key
    
    public int PatientId { get; set;} // foreign key
    [ForeignKey("PatientId")]
    public Patient Patient { get; set;}
    public IList<Triage> Triages { get; set;} = new List<Triage>();
    public DateTime CreatedOn { get; set;} = DateTime.MinValue; //default
    public string PxLocation { get; set; }
    public string LastST { get; set; }
    public string LastSTLocation { get; set; }
    public string PxDescript { get; set; }
    public string IssueStart { get; set; }
    public string HapBefore { get; set; }
    public string Pain { get; set; } 
    public int PainRating { get; set; } =0;
    public string Redness { get; set; } 
    public string Discharge { get; set; } 
    public string Photosensitivity { get; set; } 
    public string VisionAffected { get; set; } 
    public string CLWearer { get; set; } 
    public string CLInfo { get; set; }  
    public string Flashes { get; set; } 
    public string FlashesNew { get; set; } 
    public string FlashWorsening { get; set; }
    public string Floaters { get; set; } 
    public string FloatersNew { get; set; }  
    public string FloatersWorsening { get; set;}
    public string Optom { get; set; } 
    [Required]
    public string Advice { get; set; }
}