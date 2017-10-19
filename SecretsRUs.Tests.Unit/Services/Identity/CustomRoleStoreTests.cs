using SecretsRUs.Models;
using SecretsRUs.Services.Identity;
using System;
using System.Threading;
using Xunit;

namespace SecretsRUs.Tests.Unit.Services.Identity
{
    public class CustomRoleStoreTests
    {
        [Fact]
        public async void CreateAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.CreateAsync(role, cancellationToken)
            );
        }

        [Fact]
        public async void DeleteAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.DeleteAsync(role, cancellationToken)
            );
        }

        [Fact]
        public async void FindByIdAsyncThrowsNotImplementedException()
        {
            var roleId = Guid.NewGuid().ToString();
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.FindByIdAsync(roleId, cancellationToken)
            );
        }

        [Fact]
        public async void FindByNameAsyncThrowsNotImplementedException()
        {
            var normalizedRoleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.FindByNameAsync(normalizedRoleName, cancellationToken)
            );
        }

        [Fact]
        public async void GetNormalizedRoleNameAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetNormalizedRoleNameAsync(role, cancellationToken)
            );
        }

        [Fact]
        public async void GetRoleIdAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetRoleIdAsync(role, cancellationToken)
            );
        }

        [Fact]
        public async void GetRoleNameAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetRoleNameAsync(role, cancellationToken)
            );
        }

        [Fact]
        public async void SetNormalizedRoleNameAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var normaliseRoleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.SetNormalizedRoleNameAsync(role, normaliseRoleName, cancellationToken)
            );
        }

        [Fact]
        public async void SetRoleNameAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.SetRoleNameAsync(role, roleName, cancellationToken)
            );
        }

        [Fact]
        public async void UpdateAsyncThrowsNotImplementedException()
        {
            var role = new ApplicationRole();
            var cancellationToken = new CancellationToken();
            var sut = new CustomRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.UpdateAsync(role, cancellationToken)
            );
        }
    }
}

