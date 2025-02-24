using System.ComponentModel.DataAnnotations;
using MastersProject.Data.Entities;

public class PatientCreateModel
{
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

    [Required]
    [DisplayFormat(DataFormatString = "{0:yyyy:MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime Dob { get; set; }
    public string Mobile { get; set; }
    public string HomeNumber { get; set; }

    [Required]
    public string GPName { get; set; }
    public string HandCNum { get; set; }

    [Required]
    public string GPAddress { get; set; }

    public string PatientType { get; set; }

    [Required]
    public Opt Opt { get; set; }

    public Patient ToPatient()
    {
        return new Patient
        {
            Pid = Pid,
            Forename = Forename,
            Surname = Surname,
            Email = Email,
            Address = Address,
            Gender = Gender,
            Mobile = Mobile,
            HomeNumber = HomeNumber,
            GPName = GPName,
            HandCNum = HandCNum,
            GPAddress = GPAddress,
            PatientType = PatientType,
            Opt = Opt

        };
    }
    public static PatientCreateModel FromPatient(Patient p)
    {
        return new PatientCreateModel
        {
            Pid = p.Pid,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            Gender = p.Gender,
            Mobile = p.Mobile,
            HomeNumber = p.HomeNumber,
            GPName = p.GPName,
            HandCNum = p.HandCNum,
            GPAddress = p.GPAddress,
            PatientType = p.PatientType,
            Opt = p.Opt
        };
    }
}