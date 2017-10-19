using SecretsRUs.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SecretsRUs.Tests.Unit.Repositories
{
    public class IdentityRepositoryTests
    {
        [Fact]
        public void CanAddRecord()
        {
            var sut = new IdentityRepository("Fake connection string");

            var testValue = Guid.NewGuid().ToString();
            sut.Add(testValue);

            var values = sut.Read();

            Assert.Contains(values, x => x.Trim() == testValue);
        }
    }
}
