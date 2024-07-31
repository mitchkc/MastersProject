using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client.Advanced;

namespace MastersProject.Data.Entities;

public enum Opt { optIn, optOut}
public class Patient
{
    public int Pid { get; set; }
    public string Forename { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
    public DateOnly Dob { get; set; }
    public string Mobile { get; set; }
    public string HomeNumber { get; set; }
    public string GPName { get; set; }
    public string GPAddress { get; set; }
    public string PatientType { get; set; }
    public Role Opt { get; set; }
}