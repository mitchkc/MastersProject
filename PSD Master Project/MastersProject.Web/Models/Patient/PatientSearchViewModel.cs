using MastersProject.Data.Entities;
using System.ComponentModel.DataAnnotations;
namespace MastersProject.Web.Models;

public class PatientSearchViewModel
{
    public IList<Patient> Patients { get; set; } = new List<Patient>();
    public string QueryFname { get; set; } = string.Empty;
    public string QuerySname { get; set; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:yyyy:MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime? Dob { get; set; }

    
}