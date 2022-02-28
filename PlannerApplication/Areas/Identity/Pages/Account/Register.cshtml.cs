// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PlannerApplication.Data;
using PlannerApplication.HelpClasses;
using PlannerApplication.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using static System.Net.WebRequestMethods;

namespace PlannerApplication.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly PlannerContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            PlannerContext context,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Mail")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "Lösenordet måste vara minst 6 tecken, innehålla minst en siffra och ett special tecken")]
            [DataType(DataType.Password)]
            [Display(Name = "Lösenord")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
        
            [StringLength(100, ErrorMessage = "Lösenordet måste vara minst 6 tecken, innehålla minst en siffra och ett special tecken")]
            [DataType(DataType.Password)]
            [Display(Name = "Upprepa lösenord")]
            [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Förnamn")]
            public string firstName { get; set; }

            [Display(Name = "Efternamn")]
            public string lastName { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Telefon")]
            public string Phone { get; set; }

            [Display(Name = "Ålder")]
            public int Age{ get; set; }
            public string Longtitude { get; set; }
            public string Latitude { get; set; }
            public string Title { get; set; }
            public string ImageName { get; set; }
            [NotMapped]
            [Display(Name = "Profilbild")]
            public IFormFile ImageFile { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");



                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(Input.ImageFile.FileName);
                    string extension = Path.GetExtension(Input.ImageFile.FileName);
                    string FileName = fileName + DateTime.Now.ToString("yymmssfff") + extension; //

                    

                    string path = Path.Combine(wwwRootPath + "/ProfileImages/", FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await Input.ImageFile.CopyToAsync(fileStream);
                    }

                    using (var image = Image.Load(Input.ImageFile.OpenReadStream()))
                    {
                        //string newSize = Cropper.ResizeImage(image, 200, 200);
                        //string[] aSize = newSize.Split(',');
                        image.Mutate(h => h.Resize(200, 200));
                        image.Save(path);
                        TempData["message"] = "Bilden har laddats upp!";
                    }


                        planneruser us = new planneruser()
                        {
                            userID = userId,
                            Email = user.Email,
                            firstName = Input.firstName,
                            lastName = Input.lastName,
                            Phone = Input.Phone,
                            Age = Input.Age,
                            Longtitude = Input.Longtitude,
                            Latitude = Input.Latitude,
                            standardPicture = $"/ProfileImages/{FileName}"

                        };

                    await _context.planneruser.AddAsync(us);
                    await _context.SaveChangesAsync();

                    //imagemodel img = new imagemodel()
                    //{
                    //    ImageName = FileName,
                    //    Title = Input.firstName + Input.lastName + DateTime.Now.ToString("yymmssfff")
                    //};
                    //await _context.imagemodel.AddAsync(img);
                    //await _context.SaveChangesAsync();



                   
                  














                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
