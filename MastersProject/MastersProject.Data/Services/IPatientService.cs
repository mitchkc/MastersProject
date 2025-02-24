
using MastersProject.Data.Entities;

namespace  MastersProject.Data.Services
{

    public interface IPatientService
    {
        void Initialise();

        //----------- Patient management ---------------//
        IList<Patient> GetPatients();
        Paged<Patient> GetPatients(int page=1, int size=20, string orderBy="id", string direction="asc");
        Patient GetPatient(int pid);
        Patient GetPatientByEmail(string email);
        bool IsEmailAvailable(string email, int patientId);
        Patient AddPatient(string forename, string surname, string email, string address, string gender, DateTime dob, string mobile, string homenumber, string gpName, string gpAddress, string patientType, Opt role);
        Patient UpdatePatient(Patient patient);
        bool DeletePatient(int pid);
    
    }
}