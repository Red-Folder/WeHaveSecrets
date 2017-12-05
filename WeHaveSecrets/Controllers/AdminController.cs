using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WeHaveSecrets.Models;
using WeHaveSecrets.Models.Secrets;
using WeHaveSecrets.Services.Secrets;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WeHaveSecrets.Models.AccountViewModels;
using WeHaveSecrets.Services.Identity;
using System.Threading;
using WeHaveSecrets.Services;

namespace WeHaveSecrets.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAdminVault _vault;

        public AdminController(UserManager<ApplicationUser> userManager, IAdminVault vault)
        {
            if (userManager == null) throw new ArgumentNullException("userManager");
            if (vault == null) throw new ArgumentNullException("vault");

            _userManager = userManager;
            _vault = vault;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SecretsFor()
        {
            return View(new SecretsForViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SecretsFor(string userName)
        {
            var model = new SecretsForViewModel();
            if (ModelState.IsValid)
            {
                model.UserName = userName;

                var user = await _userManager.FindByNameAsync(userName);

                if (user != null)
                {
                    _vault.UserId = user.Id;
                    model.Secrets = _vault.GetAll()
                                        .Select(x => new SecretViewModel
                                        {
                                            Id = x.Id,
                                            Key = x.Key,
                                            Value = x.Value
                                        })
                                        .ToList();
                }
            }


            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromServices]IUserPasswordStore<ApplicationUser> passwordStore, string userName, string newPassword)
        {
            var model = new ChangePasswordViewModel();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userName);
                var hashedPassword = _userManager.PasswordHasher.HashPassword(user, newPassword);

                var cancellationToken = new CancellationToken();
                await passwordStore.SetPasswordHashAsync(user, hashedPassword, cancellationToken);
                await passwordStore.UpdateAsync(user, cancellationToken);

                model.Updated = true;
            }

            return View(model);
        }

        public IActionResult BackupDatabase([FromServices]IDatabaseMaintenance databaseMaintenance)
        {
            var model = new BackupsViewModel();
            if (databaseMaintenance.Backup())
            {
                model.Successful = true;

                model.AvailableBackups = databaseMaintenance
                                            .Backups()
                                            .Select(x => new BackupViewModel
                                            {
                                                Url = x.Url,
                                                Created = x.Created
                                            })
                                            .OrderByDescending(x => x.Created)
                                            .ToList();
            }
            else
            {
                model.Successful = false;
                model.ErrorMessage = "Backup failed.  Please check logs.";
            }

            return View(model);
        }
    }
}