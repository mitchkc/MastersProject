using System.ComponentModel.DataAnnotations;

namespace MastersProject.Data.Entities;

public class Triage
{
    public string PxLocation { get; set; }
    public string LastST { get; set; }
    public string LastSTLocation { get; set; }
    public string PxDescript { get; set; }
    public string IssueStart { get; set; }
    public string HapBefore { get; set; }
    public string Pain { get; set; } 
    public int PainRating { get; set; }
    public int Redness { get; set; } // checkbox; checked = 1, not checked = 0
    public int Discharge { get; set; } // checkbox; checked = 1, not checked = 0
    public int Photosensitivity { get; set; } // checkbox; checked = 1, not checked = 0
    public int VisionAffected { get; set; } // checkbox; checked = 1, not checked = 0
    public int CLWearer { get; set; } // checkbox; checked = 1, not checked = 0
    public string CLWear { get; set; }  // if flashes checkbox selected open this box, if not leave greyed out?
    public int Flashes { get; set; } // checkbox; checked = 1, not checked = 0
    public string FlashesNew { get; set; } // if flashes checkbox selected open the following boxes, if not leave greyed out?
    public string FlashWorsening { get; set; }
    public int Floaters { get; set; } // checkbox; checked = 1, not checked = 0
    public string FloatersNew { get; set; }  // if floaters checkbox selected open the following boxes, if not leave greyed out?
    public string FloatersWorsening { get; set;}
    public string Optom { get; set; } //dropdown with Optoms populated from staff or free text?
    public string Advice { get; set; }
}