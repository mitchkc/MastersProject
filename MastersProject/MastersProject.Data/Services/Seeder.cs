using MastersProject.Data.Entities;

namespace MastersProject.Data.Services

{
    public static class Seeder
    {
        // use this class to seed the database with dummy test data using an IUserService 
        public static void Seed(IUserService svc)
        {
            IUserService usvc = new UserServiceDb();
            svc = new 


            // seeder destroys and recreates the database - NOT to be called in production!!!
            svc.Initialise();

            // add users
            svc.AddUser("Admin", "Istrator1", "admin1@mail.com", "admin1", "Test Admin Address1", "Male", new DateTime(10-10-1985), "0771111111", "0281111111", Role.admin);
            svc.AddUser("Admin", "Istrator2", "admin2@mail.com", "admin2", "Test Admin Address2", "Female", new DateTime(01-01-1981), "0771111112", "0281111112", Role.admin);

            svc.AddUser("Optom", "Etrist1", "optom1@mail.com", "optom1", "Test Optom Address 1", "Male", new DateTime(02-02-1980), "0781111111", "0281112222", Role.optometrist);
            svc.AddUser("Optom", "Etrist2", "optom2@mail.com", "optom2", "Test Optom Address 2", "Female", new DateTime(03-03-1981), "0781111112", "0281112223", Role.optometrist);
            svc.AddUser("Optom", "Etrist3", "optom3@mail.com", "optom3", "Test Optom Address 3", "Female", new DateTime(04-04-1982), "0781111113", "0281112224", Role.optometrist);

            svc.AddUser("Staff", "Member1", "staff1@mail.com", "staff1", "Test Staff Address 1", "Female", new DateTime(01-02-2000), "0751112222", "0282223333", Role.staff);
            svc.AddUser("Staff", "Member2", "staff2@mail.com", "staff2", "Test Staff Address 2", "Male", new DateTime(02-02-2001), "0751112223", "0282223334", Role.staff);
            svc.AddUser("Staff", "Member3", "staff3@mail.com", "staff3", "Test Staff Address 3", "Female", new DateTime(03-02-1991), "0751112224", "0282223335", Role.staff);
            svc.AddUser("Staff", "Member4", "staff4@mail.com", "staff4", "Test Staff Address 4", "Female", new DateTime(04-02-1970), "0751112225", "0282223336", Role.staff);
            svc.AddUser("Staff", "Member5", "staff5@mail.com", "staff5", "Test Staff Address 5", "Female", new DateTime(05-02-1980), "0751112226", "0282223337", Role.staff); 
        
            // optionally add some fake users
            // var faker = new Faker();
            // for(int i=1; i<=20; i++)
            // {
            //     var s = svc.AddUser(
            //         faker.Name.FullName(),
            //         faker.Internet.Email(),
            //         "password",
            //         Role.guest
            //     );
            // }


            // add patients 
            
        }
    }

}