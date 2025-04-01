using CinemaWebSite.Models;
using CinemaWebSite.Models.ViewModels;
using CinemaWebSite.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CinemaWebSite.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole>roleManager,IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._roleManager = roleManager;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName: "SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole(roleName: "Admin"));
                await _roleManager.CreateAsync(new IdentityRole(roleName: "Customer"));
                await _roleManager.CreateAsync(new IdentityRole(roleName: "Cinema"));
            }

            return View(new RegisterVM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                    EmailConfirmed = registerVM.EmailConfirmed,
                    PhoneNumber = registerVM.PhoneNumber
                };

                var result = await userManager.CreateAsync(applicationUser, registerVM.Password);

                if (result.Succeeded)
                {
                    var userId = applicationUser.Id;
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                    var returnUrl = Url.Content("~/");

                    var callbackUrl = Url.Action(
                        "ConfirmEmail",                         
                        "Account",
                        new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl }, 
                        protocol: Request.Scheme);              

                    await emailSender.SendEmailAsync(registerVM.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                   
                    await signInManager.SignInAsync(applicationUser, isPersistent: false);
                    var roleResult = await userManager.AddToRoleAsync(applicationUser, "Customer");
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to assign role.");
                    }
                    if (applicationUser.EmailConfirmed)
                    {

                        return RedirectToAction("Index", "Home", new { area = "Customer" });

                    }
                    else
                    {
                        return View(registerVM);
                    }

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registerVM);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            ViewBag.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
              var user= await userManager.FindByEmailAsync(loginVM.Email);

                if (user != null)
                {
                  bool result= await userManager.CheckPasswordAsync(user,loginVM.Password);
                    if (result)
                    {
                       await signInManager.SignInAsync(user, loginVM.RememberMe);
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("Password","Password do not match");

                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "can not find your Email");

                }
            }
            return View(loginVM);
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Try signing in with an external login
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl ?? "/");
            }

            // If the user cannot log in, try finding them by email
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // Create a new user if they do not exist
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email
                    };
                    var createUserResult = await userManager.CreateAsync(user);
                    if (!createUserResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Error creating user.");
                        return RedirectToAction(nameof(Login));
                    }
                }

                // Ensure the external login is linked
                var existingLogins = await userManager.GetLoginsAsync(user);
                var hasGoogleLogin = existingLogins.Any(l => l.LoginProvider == info.LoginProvider);

                if (!hasGoogleLogin)
                {
                    var addLoginResult = await userManager.AddLoginAsync(user, info);
                    if (!addLoginResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Error linking external login.");
                        return RedirectToAction(nameof(Login));
                    }
                }

                // Sign in the user
                await signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl ?? "/");
            }

            return RedirectToAction(nameof(Login));
        }



        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }
        public IActionResult AccessDenied()
        { 
            return View(); 
        }





    }
}
