
using MastersProject.Data.Entities;
using MastersProject.Data.Services;
using MastersProject.Data.Security;
using MastersProject.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace MastersProject.Data.Services
{
    public class UserServiceDb : IUserService
    {
        private readonly DatabaseContext  ctx;
      
        public UserServiceDb(DatabaseContext ctx) 
        {
            this.ctx = ctx; 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        // ------------------ User Related Operations ------------------------

        // retrieve list of Users
        public IList<User> GetUsers()
        {
            return ctx.Users.ToList();
        }

        // retrieve paged list of users
        public Paged<User> GetUsers(int page = 1, int size = 10, string orderBy = "uid", string direction = "asc")
        {          
            var query = (orderBy.ToLower(),direction.ToLower()) switch
            {
                ("uid","asc")     => ctx.Users.OrderBy(r => r.Uid),
                ("uid","desc")    => ctx.Users.OrderByDescending(r => r.Uid),
                ("forename","asc")   => ctx.Users.OrderBy(r => r.Forename),
                ("forename","desc")  => ctx.Users.OrderByDescending(r => r.Forename),
                ("surname", "asc") => ctx.Users.OrderBy(r => r.Surname),
                ("surname", "desc") => ctx.Users.OrderByDescending(r => r.Surname),
                ("email","asc")  => ctx.Users.OrderBy(r => r.Email),
                ("email","desc") => ctx.Users.OrderByDescending(r => r.Email),
                ("age", "asc") => ctx.Users.OrderBy(r => r.Age),
                ("age", "desc") => ctx.Users.OrderByDescending( r => r.Age),
                _                => ctx.Users.OrderBy(r => r.Uid)
            };

            return query.ToPaged(page,size,orderBy,direction);
        }

        // Retrive User by Id 
        public User GetUser(int id)
        {
            return ctx.Users.FirstOrDefault(s => s.Uid == id);
        }

        // Add a new User checking a User with same email does not exist
        public User AddUser(string forename, string surname, string email, string password, string address, string gender, DateTime dob, string mobile, string homeNumber, Role role)
        {     
            var existing = GetUserByEmail(email);
            if (existing != null)
            {
                return null;
            } 

            var user = new User
            {            
                Forename = forename,
                Surname = surname,
                Email = email,
                Password = Hasher.CalculateHash(password), // can hash if required 
                Address = address,
                Gender = gender,
                DoB = dob,
                MobileNumber = mobile,
                HomeNumber = homeNumber,
                Role = role              
            };
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user; // return newly added User
        }

        // Delete the User identified by Id returning true if deleted and false if not found
        public bool DeleteUser(int id)
        {
            var s = GetUser(id);
            if (s == null)
            {
                return false;
            }
            ctx.Users.Remove(s);
            ctx.SaveChanges();
            return true;
        }

        // Update the User with the details in updated 
        public User UpdateUser(User updated)
        {
            // verify the User exists
            var User = GetUser(updated.Uid);
            if (User == null)
            {
                return null;
            }
            // verify email address is registered or available to this user
            if (!IsEmailAvailable(updated.Email, updated.Uid))
            {
                return null;
            }
            // update the details of the User retrieved and save
            User.Forename = updated.Forename;
            User.Surname = updated.Surname;
            User.Email = updated.Email;
            User.Password = Hasher.CalculateHash(updated.Password); 
            User.Address = updated.Address;
            User.Gender = updated.Gender;
            User.DoB = updated.DoB;
            User.MobileNumber = updated.MobileNumber;
            User.HomeNumber = updated.HomeNumber; 
            User.Role = updated.Role; 

            ctx.SaveChanges();          
            return User;
        }

        // Find a user with specified email address
        public User GetUserByEmail(string email)
        {
            return ctx.Users.FirstOrDefault(u => u.Email == email);
        }

        // Verify if email is available or registered to specified user
        public bool IsEmailAvailable(string email, int userId)
        {
            return ctx.Users.FirstOrDefault(u => u.Email == email && u.Uid != userId) == null;
        }

        public IList<User> GetUsersQuery(Func<User,bool> q)
        {
            return ctx.Users.Where(q).ToList();
        }

        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
            //return (user != null && user.Password == password ) ? user: null;
        }

         public string ForgotPassword(string email)
        {
            var user = ctx.Users.FirstOrDefault(u => u.Email == email);
            if (user != null) {
                // invalidate any previous tokens
                ctx.ForgotPasswords
                    .Where(t => t.Email == email && t.ExpiresAt > DateTime.Now).ToList()
                    .ForEach(t => t.ExpiresAt = DateTime.Now);
                var f = new ForgotPassword { Email = email };
                ctx.ForgotPasswords.Add(f);
                ctx.SaveChanges();
                return f.Token;
            }
            return null;
        }
        
        public User ResetPassword(string email, string token, string password)
        {
            // find user by email
            var user = ctx.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) 
            {
                return null; // user not found
            }
            // find valid reset token for user
            var reset = ctx.ForgotPasswords
                           .FirstOrDefault(t => t.Email == email && t.Token == token && t.ExpiresAt > DateTime.Now);
            if (reset == null) 
            {
                return null; // reset token invalid
            }

            // valid token and user so update password, invalidate the token and return the user           
            reset.ExpiresAt = DateTime.Now;
            user.Password = Hasher.CalculateHash(password);
            ctx.SaveChanges();
            return user;
        }

        public IList<string> GetValidPasswordResetTokens() {
            // return non expired tokens
            return ctx.ForgotPasswords.Where(t => t.ExpiresAt > DateTime.Now)
                                      .Select(t => t.Token)
                                      .ToList();
        }
   
    }
}