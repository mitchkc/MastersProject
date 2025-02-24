

using MastersProject.Data.Entities;
using MastersProject.Data.Repositories;

namespace MastersProject.Data.Services
{
    public class PatientServiceDb : IPatientService
    {
        private readonly DatabaseContext ctx;

        public PatientServiceDb(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public void Initialise()
        {
            ctx.Initialise();
        }

        // ------------------------------- Patient Operations ----------------------------- //

        //retrieve list of patients 
        public IList<Patient> GetPatients()
        {
            return ctx.Patients.ToList();
        }

        //retrieve paged list of patients
        public Paged<Patient> GetPatients(int page = 1, int size = 10, string orderBy = "pid", string direction = "asc")
         {          
            var query = (orderBy.ToLower(),direction.ToLower()) switch
            {
                ("pid","asc")     => ctx.Patients.OrderBy(r => r.Pid),
                ("pid","desc")    => ctx.Patients.OrderByDescending(r => r.Pid),
                ("forename","asc")   => ctx.Patients.OrderBy(r => r.Forename),
                ("forename","desc")  => ctx.Patients.OrderByDescending(r => r.Forename),
                ("surname", "asc") => ctx.Patients.OrderBy(r => r.Surname),
                ("surname", "desc") => ctx.Patients.OrderByDescending(r => r.Surname),
                ("age", "asc") => ctx.Patients.OrderBy(r => r.Age),
                ("age", "desc") => ctx.Patients.OrderByDescending( r => r.Age),
                _                => ctx.Patients.OrderBy(r => r.Pid)
            };

            return query.ToPaged(page,size,orderBy,direction);
        }

        // Retrive Patient by Id 
        public Patient GetPatient(int id)
        {
            return ctx.Patients.FirstOrDefault(s => s.Pid == id);
        }

        // Add a new Patient checking a Patient with same email does not exist - further check for name+DoB?
        public Patient AddPatient(string forename, string surname, string email, string address, string gender, DateTime dob, string mobile, string homeNumber, string gpname, string gpaddress, string pxtype, Opt opt)
        {     
            var existing = GetPatientByEmail(email);
            if (existing != null)
            {
                return null;
            } 

            var patient = new Patient
            {            
                Forename = forename,
                Surname = surname,
                Email = email,
                Address = address,
                Gender = gender,
                Dob = dob,
                Mobile = mobile,
                HomeNumber = homeNumber,
                GPName = gpname,
                GPAddress = gpaddress,
                PatientType = pxtype,
                Opt = opt             
            };
            ctx.Patients.Add(patient);
            ctx.SaveChanges();
            return patient; // return newly added Patient 
        }

        // Delete the Patient identified by Id returning true if deleted and false if not found
        public bool DeletePatient(int id)
        {
            var s = GetPatient(id);
            if (s == null)
            {
                return false;
            }
            ctx.Patients.Remove(s);
            ctx.SaveChanges();
            return true;
        }

        // Update the Patient with the details in updated 
        public Patient UpdatePatient(Patient updated)
        {
            // verify the Patient exists
            var Patient = GetPatient(updated.Pid);
            if (Patient == null)
            {
                return null;
            }
            // verify email address is registered or available to this Patient
            if (!IsEmailAvailable(updated.Email, updated.Pid))
            {
                return null;
            }
            // update the details of the Patient retrieved and save
            Patient.Forename = updated.Forename;
            Patient.Surname = updated.Surname;
            Patient.Email = updated.Email;            
            Patient.Address = updated.Address;
            Patient.Gender = updated.Gender;
            Patient.Dob = updated.Dob;
            Patient.Mobile = updated.Mobile;
            Patient.HomeNumber = updated.HomeNumber; 
            Patient.GPName = updated.GPName;
            Patient.GPAddress = updated.GPAddress;
            Patient.PatientType = updated.PatientType;
            Patient.Opt = updated.Opt; 

            ctx.SaveChanges();          
            return Patient;
        }

        // Find a patient with specified email address
        public Patient GetPatientByEmail(string email)
        {
            return ctx.Patients.FirstOrDefault(u => u.Email == email);
        }

        // Verify if email is available or registered to specified patient
        public bool IsEmailAvailable(string email, int patientId)
        {
            return ctx.Patients.FirstOrDefault(u => u.Email == email && u.Pid != patientId) == null;
        }

        public IList<Patient> GetPatientsQuery(Func<Patient,bool> q)
        {
            return ctx.Patients.Where(q).ToList();
        }
    }
}