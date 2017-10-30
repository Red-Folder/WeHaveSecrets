using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretsRUs.Services;
using SecretsRUs.Models.Converters;
using SecretsRUs.Models;
using Microsoft.AspNetCore.Identity;
using SecretsRUs.Services.Secrets;

namespace SecretsRUs.Controllers
{
    public class SecretsController : Controller
    {
        public IActionResult Index([FromServices]ISecretVault vault)
        {
            if (vault == null) throw new ArgumentNullException("vault");

            var secrets = vault
                            .GetAll()
                            .Select(x => new SecretViewModelFromDomain(x))
                            .ToList();

            return View(secrets);
        }

        [HttpPost]
        public IActionResult Index([FromServices]ISecretVault vault, SecretViewModel secret)
        {
            if (vault == null) throw new ArgumentNullException("vault");

            if (ModelState.IsValid)
            {
                vault.Save(new SecretDomainFromViewModel(secret));
                return RedirectToAction(actionName: nameof(Index));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}