using MastersProject.Data.Entities;
namespace MastersProject.Web.Models;
using System.ComponentModel.DataAnnotations;

public class PatientViewModel
{
    public Patient Patient { get; set; }
    public int Pid { get; set; }
    public string Forename { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy:MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime Dob { get; set; }
    public int Age => (int)(DateTime.Now - Dob).TotalDays/365;
    public string Mobile { get; set; }
    public string HomeNumber { get; set; }
    public string GPName { get; set; }
    public string HandCNum { get; set; }
    public string GPAddress { get; set; }
    public string PatientType { get; set; }
    public Opt Opt { get; set; }
    public IList<Triage> Triages { get; set; } = new List<Triage>();
    public IList<SightTest> SightTests { get; set; } = new List<SightTest>();

    public string PrescriptionRE { get; set; }
    public string PrescriptionLE { get; set; }

}