﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WeHaveSecrets.Models;
using WeHaveSecrets.Models.Secrets;
using WeHaveSecrets.Services.Secrets;

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
            var users = _userManager.Users.ToArray();
            return View(users);
        }

        [HttpGet]
        public IActionResult SecretsFor()
        {
            return View(new SecretsForViewModel());
        }

        [HttpPost]
        public IActionResult SecretsFor(string userId)
        {
            var model = new SecretsForViewModel();
            if (ModelState.IsValid)
            {
                model.UserId = userId;

                _vault.UserId = userId;
                model.Secrets = _vault.GetAll()
                                    .Select(x => new SecretViewModel
                                    {
                                        Id = x.Id,
                                        Key = x.Key,
                                        Value = x.Value
                                    })
                                    .ToList(); ;
            }


            // If we got this far, something failed, redisplay form
            return View(model);
        }

    }
}