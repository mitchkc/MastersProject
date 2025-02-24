using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MastersProject.Data.Entities;
using System.Diagnostics.Contracts;

namespace MastersProject.Web.Models.User;
public class ProfileViewModel
{
    public int UId { get; set; }

    [Required]
    public string Forename { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [Remote(action: "VerifyEmailAvailable", controller: "User", AdditionalFields = nameof(UId))]
    public string Email { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string Gender { get; set; }

    [DisplayFormat(DataFormatString = "{0:yyyy:MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime DoB { get; set; }

    public string MobileNumber { get; set; }

    public string HomeNumber { get; set; }

    public Role Role { get; set; }

}
