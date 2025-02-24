using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MastersProject.Data.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MastersProject.Web.Models.User;

public class RegisterViewModel
{
    [Required]
    public string Forename { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [Remote(action: "VerifyEmailAvailable", controller: "User")]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
    public string PasswordConfirm { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string Gender { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:yyyy:MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime DoB { get; set; }

    public string MobileNumber { get; set; }

    public string HomeNumber { get; set; }

    [Required]
    public Role Role { get; set; }


}
