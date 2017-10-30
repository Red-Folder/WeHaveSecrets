﻿using SecretsRUs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsRUs.Repositories
{
    public interface ISecretsRepository
    {
        List<Secret> GetAll(string userId);
        Secret Add(string userId, Secret secret);
    }
}
