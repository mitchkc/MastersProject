﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using MastersProject.Data.Entities;
using MastersProject.Data.Services;
using MastersProject.Data.Security;
using MastersProject.Web.Models.User;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

/**
 *  User Management Controller
 */
namespace MastersProject.Web.Controllers;

public class UserController : BaseController
{
    private readonly IConfiguration _config;
    private readonly IMailService _mailer;
    private readonly IUserService _svc;

    public UserController(IUserService svc, IConfiguration config, IMailService mailer)
    {
        _config = config;
        _mailer = mailer;
        _svc = svc;
    }

    // HTTP GET - Display Paged List of Users
    [Authorize(Roles = "admin")]
    public ActionResult Index(int page = 1, int size = 20, string order = "id", string direction = "asc")
    {
        var paged = _svc.GetUsers(page, size, order, direction);
        return View(paged);
    }

    // HTTP GET - Display Login page
    public IActionResult Login()
    {
        return View();
    }

    // HTTP POST - Login action
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind("Email,Password")] LoginViewModel m)
    {
        var user = _svc.Authenticate(m.Email, m.Password);
        // check if login was unsuccessful and add validation errors
        if (user == null)
        {
            ModelState.AddModelError("Email", "Invalid Login Credentials");
            ModelState.AddModelError("Password", "Invalid Login Credentials");
            return View(m);
        }

        // Login Successful, so sign user in using cookie authentication
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            BuildClaimsPrincipal(user)
        );

        Alert("Successfully Logged in", AlertType.success);

        return Redirect("/");
    }

    // HTTP GET - Display Register page
    [Authorize(Roles = "admin")]
    public IActionResult Register()
    {
        return View();
    }

    // HTTP POST - Register action
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult Register([Bind("Forename,Surname,Email,Password,PasswordConfirm,Address,Gender,Dob,MobileNumber,HomeNumber,Role")] RegisterViewModel m)
    {

        if (ModelState.IsValid)
        {
            var user = _svc.AddUser(m.Forename, m.Surname, m.Email, m.Password, m.Address, m.Gender, m.DoB, m.MobileNumber, m.HomeNumber, m.Role);
            Alert("Successfully Registered. Now login", AlertType.info);
            return RedirectToAction(nameof(Login));
        }

        return View(m);
    }
    // add user via service
    [Authorize(Roles = "admin")]
    public IActionResult Delete(int id)
    {
        var user = _svc.GetUser(id);

        if (user == null)
        {
            Alert($"User {id} could not be deleted..", AlertType.danger);
            return RedirectToAction(nameof(Index));
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteConfirm(int UId)
    {
        Console.WriteLine($"Id = {UId}");
        var delete = _svc.DeleteUser(UId);

        if (delete)
        {
            Alert("User deleted", AlertType.success);
        }
        else
        {
            Alert("User could not be deleted", AlertType.warning);
        }

        return RedirectToAction(nameof(Index));
    }


    // HTTP GET - Display Update profile page
    [Authorize(Roles = "admin")]
    public IActionResult UpdateProfile(int id)
    {
        // use BaseClass helper method to retrieve Id of signed in user 
        var user = _svc.GetUser(id);
        var profileViewModel = new ProfileViewModel
        {
            UId = user.UId,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            Address = user.Address,
            Gender = user.Gender,
            DoB = user.DoB,
            MobileNumber = user.MobileNumber,
            HomeNumber = user.HomeNumber,
            Role = user.Role
        };
        return View(profileViewModel);
    }

    // HTTP POST - Update profile action
    [Authorize(Roles = "admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateProfile([Bind("UId,Forename,Surname,Email,Address,Gender,Dob,MobileNumber,HomeNumber,Role")] ProfileViewModel m)
    {
        var user = _svc.GetUser(m.UId);
        // check if form is invalid and redisplay
        if (!ModelState.IsValid || user == null)
        {
            return View(m);
        }

        if(!_svc.IsEmailAvailable(m.Email, m.UId))
        {
            return View(m);
        }
        
        // update user details and call service
        user.Forename = m.Forename;
        user.Surname = m.Surname;
        user.Email = m.Email;
        user.Address = m.Address;
        user.Gender = m.Gender;
        user.DoB = m.DoB;
        user.MobileNumber = m.MobileNumber;
        user.HomeNumber = m.HomeNumber;
        user.Role = m.Role;
        var updated = _svc.UpdateUser(user);

        // check if error updating service
        if (updated == null)
        {
            Alert("There was a problem Updating. Please try again", AlertType.warning);
            return View(m);
        }

        Alert("Successfully Updated Account Details", AlertType.info);

        // sign the user in with updated details)
        

        return RedirectToAction("Index", "Home");
    }


    // HTTP GET - Display update password page
    [Authorize]
    public IActionResult UpdatePassword()
    {
        // use BaseClass helper method to retrieve Id of signed in user 
        var user = _svc.GetUser(User.GetSignedInUserId());
        if(user == null)
        {
            return RedirectToAction("Login");
        }
        var passwordViewModel = new PasswordViewModel
        {
            UId = user.UId,
            Password = user.Password,
            PasswordConfirm = user.Password,
        };
        return View(passwordViewModel);
    }

    // HTTP POST - Update Password action
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassword([Bind("UId,OldPassword,Password,PasswordConfirm")] PasswordViewModel m)
    {
        var user = _svc.GetUser(m.UId);
        if (!ModelState.IsValid || user == null)
        {
            return View(m);
        }
        // update the password
        user.Password = m.Password;
        // save changes      
        var updated = _svc.UpdateUser(user);
        if (updated == null)
        {
            Alert("There was a problem Updating the password. Please try again", AlertType.warning);
            return View(m);
        }

        Alert("Successfully Updated Password", AlertType.info);
        // sign the user in with updated details
        await SignInCookie(user);

        return RedirectToAction("Index", "Home");
    }

    // HTTP POST - Logout action
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }


    // HTTP GET - Display Forgot password page
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // HTTP POST - Forgot password action
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ForgotPassword([Bind("Email")] ForgotPasswordViewModel m)
    {
        var token = _svc.ForgotPassword(m.Email);
        if (token == null)
        {
            // No such account. Alert only for testing
            Alert("No account found", AlertType.warning);
            return RedirectToAction(nameof(Login));
        }

        // build reset password url and email html message
        var url = $"{Request.Scheme}://{Request.Host}/User/ResetPassword?token={token}&email={m.Email}";
        var message = @$" 
            <h3>Password Reset</h3>
            <a href='{url}'>
                {url}
            </a>
        ";

        // send email containing reset token
        if (!_mailer.SendMail("Password Reset Request", message, m.Email))
        {
            Alert("There was a problem sending a password reset email", AlertType.warning);
            return RedirectToAction(nameof(ForgotPassword));
        }

        Alert("Password Reset Token sent to your registered email account", AlertType.info);
        return RedirectToAction(nameof(ResetPassword));
    }

    // HTTP GET - Display Reset password page
    public IActionResult ResetPassword(string email, string token)
    {
        return View(new ResetPasswordViewModel { Email = email, Token = token });
    }


    // HTTP POST - ResetPassword action
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ResetPassword([Bind("Email,Password,Token")] ResetPasswordViewModel m)
    {
        // verify reset request
        var user = _svc.ResetPassword(m.Email, m.Token, m.Password);
        if (user == null)
        {
            Alert("Invalid Password Reset Request", AlertType.warning);
            return RedirectToAction(nameof(ResetPassword));
        }

        Alert("Password reset successfully", AlertType.success);
        return RedirectToAction(nameof(Login));
    }

    // HTTP GET - Display not authorised and not authenticated pages
    public IActionResult ErrorNotAuthorised() => View();
    public IActionResult ErrorNotAuthenticated() => View();

    // -------------------------- Helper Methods ------------------------------

    // Called by Remote Validation attribute on RegisterViewModel to verify email address is available
    [AcceptVerbs("GET", "POST")]
    public IActionResult VerifyEmailAvailable(string email, int UId)
    {
        bool avail = _svc.IsEmailAvailable(email, UId);
        Console.WriteLine($"user is in verify is: {UId}");
        // check if email is available, or owned by user with id 
        if (avail)
        {
            return Json(true);
        }
return Json($"A user with this email address {email} already exists.");
        
    }

    // Called by Remote Validation attribute on ChangePassword to verify old password
    [AcceptVerbs("GET", "POST")]
    public IActionResult VerifyPassword(string oldPassword)
    {
        // use BaseClass helper method to retrieve Id of signed in user 
        var id = User.GetSignedInUserId();
        // check if email is available, unless already owned by user with id
        var user = _svc.GetUser(id);
        if (user == null || !Hasher.ValidateHash(user.Password, oldPassword))
        {
            return Json($"Please enter current password.");
        }
        return Json(true);
    }

    // =========================== PRIVATE UTILITY METHODS ==============================

    // return a claims principle using the info from the user parameter
    private ClaimsPrincipal BuildClaimsPrincipal(User user)
    {
        // define user claims
        var claims = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Sid, user.UId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.Forename} {user.Surname}"),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        }, CookieAuthenticationDefaults.AuthenticationScheme);

        // build principal using claims
        return new ClaimsPrincipal(claims);
    }

    // Sign user in using Cookie authentication scheme
    private async Task SignInCookie(User user)
    {
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            BuildClaimsPrincipal(user)
        );
    }
}