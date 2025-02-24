using MastersProject.Data.Entities;
namespace MastersProject.Web.Models;
using System.ComponentModel.DataAnnotations;
public class RSearchViewModel
{

    [Required]
    public DateTime StartDate { get; set; } 
    [Required]
    public DateTime EndDate { get; set; } 
    public RxType? RxType { get; set; }
    public IEnumerable<Patient> Patients { get; set; }
    
    
}