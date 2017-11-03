﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SecretsRUs.Models;
using SecretsRUs.Services.Secrets;

namespace SecretsRUs.Controllers
{
    public class SecretsController : Controller
    {
        public IActionResult Index([FromServices]ISecretVault vault)
        {
            if (vault == null) throw new ArgumentNullException("vault");

            var vm = new SecretListAndNewViewModel
            {
                Secrets = GetList(vault),
                NewSecret = new NewSecretViewModel()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index([FromServices]ISecretVault vault, NewSecretViewModel vm)
        {
            if (vault == null) throw new ArgumentNullException("vault");

            if (ModelState.IsValid)
            {
                var domain = new Secret
                {
                    Key = vm.Key,
                    Value = vm.Value
                };
                vault.Save(domain);

                return RedirectToAction(actionName: nameof(Index));
            }
            else
            {
                var vm2 = new SecretListAndNewViewModel
                {
                    Secrets = GetList(vault),
                    NewSecret = vm 
                };
                return View(vm2);
            }
        }

        private List<SecretViewModel> GetList(ISecretVault vault)
        {
            return vault
                    .GetAll()
                    .Select(x => new SecretViewModel
                    {
                        Id = x.Id,
                        Key = x.Key,
                        Value = x.Value
                    })
                    .ToList();
        }
    }
}