using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WeHaveSecrets.Models;
using WeHaveSecrets.Services.Secrets;
using Microsoft.AspNetCore.Authorization;

namespace WeHaveSecrets.Controllers
{
    public class SecretsController : Controller
    {
        [Authorize]
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
        [Authorize]
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

        public IActionResult PublicShare(ISecretVault vault, int id)
        {
            if (vault == null) throw new ArgumentNullException("vault");

            var secret = vault.Get(id);

            if (secret == null)
            {
                return NotFound();
            }
            else
            {
                var vm = new SecretViewModel
                {
                    Id = secret.Id,
                    Key = secret.Key,
                    Value = secret.Value
                };

                return View(vm);
            }
            throw new NotImplementedException();
        }
    }
}