using System.ComponentModel.DataAnnotations;
using MastersProject.Data.Services;
using Microsoft.Identity.Client.Advanced;

namespace MastersProject.Data.Entities;

public enum Opt { optIn, optOut }
public enum RxType { myope, hyperope, antimetrope}
public class Patient
{
    [Key]
    public int Pid { get; set; }
    [Required]
    public string Forename { get; set; }
    [Required]
    public string Surname { get; set; }
    public string Email { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Gender { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy:MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime Dob { get; set; }
    public int Age => (int)(DateTime.Now - Dob).TotalDays / 365;

    public string FinalRxSignRE { get; set; }
    public string FinalRxSignLE { get; set; }
    public double FinalSphereRE { get; set; }
    public double FinalSphereLE { get; set; }
    public double FinalCylRE { get; set; }
    public double FinalCylLE { get; set; }
    public int FinalAxisRE { get; set; }
    public int FinalAxisLE { get; set; }
    public RxType rxType { get; set; }
    public string PrescriptionRE
    {
        get
        {
            var latestTest = SightTests.OrderByDescending(st => st.CreatedOn).FirstOrDefault();
            if (latestTest == null) return "No Prescription";

            return $"{latestTest.FinalSphereSignRE}{latestTest.FinalSphereRE:F2}/-{latestTest.FinalCylRE:F2}x{latestTest.FinalAxisRE}";
        }
    }

    public string PrescriptionLE
    {
        get
        {
            var latestTest = SightTests.OrderByDescending(st => st.CreatedOn).FirstOrDefault();
            if (latestTest == null) return "No Prescription";

            return $"{latestTest.FinalSphereSignLE}{latestTest.FinalSphereLE:F2}/-{latestTest.FinalCylLE:F2}x{latestTest.FinalAxisLE}";
        }
    }
    public IList<Patient> Patients { get; set; } = new List<Patient>();
    public string Mobile { get; set; }
    public string HomeNumber { get; set; }
    [Required]
    public string GPName { get; set; }
    public string HandCNum { get; set; }
    [Required]
    public string GPAddress { get; set; }
    public string PatientType { get; set; }
    public Opt Opt { get; set; }
    public IList<SightTest> SightTests { get; set; } = new List<SightTest>();
    public IList<Triage> Triages { get; set; } = new List<Triage>();

}