using SecretsRUs.Models;
using SecretsRUs.Services.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace SecretsRUs.Tests.Unit.Services.Identity
{
    public class CustomUserRoleStoreTests
    {
        [Fact]
        public async void AddToRoleAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.AddToRoleAsync(user, roleName, cancellationToken)
            );
        }

        [Fact]
        public async void CreateAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.CreateAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void DeleteAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.DeleteAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void FindByIdAsyncThrowsNotImplementedException()
        {
            var userId = Guid.NewGuid().ToString();
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.FindByIdAsync(userId, cancellationToken)
            );
        }

        [Fact]
        public async void FindByNameAsyncThrowsNotImplementedException()
        {
            var normalizedUserName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.FindByNameAsync(normalizedUserName, cancellationToken)
            );
        }

        [Fact]
        public async void GetNormalizedUserNameAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetNormalizedUserNameAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void GetRolesAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetRolesAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void GetUserIdAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetUserIdAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void GetUserNameAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetUserNameAsync(user, cancellationToken)
            );
        }

        [Fact]
        public async void GetUsersInRoleAsyncThrowsNotImplementedException()
        {
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.GetUsersInRoleAsync(roleName, cancellationToken)
            );
        }

        [Fact]
        public async void IsInRoleAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.IsInRoleAsync(user, roleName, cancellationToken)
            );
        }

        [Fact]
        public async void RemoveFromRoleAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var roleName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.RemoveFromRoleAsync(user, roleName, cancellationToken)
            );
        }

        [Fact]
        public async void SetNormalizedUserNameAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var normalizedName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.SetNormalizedUserNameAsync(user, normalizedName, cancellationToken)
            );
        }

        [Fact]
        public async void SetUserNameAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var userName = "TEST";
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.SetUserNameAsync(user, userName, cancellationToken)
            );
        }

        [Fact]
        public async void UpdateAsyncThrowsNotImplementedException()
        {
            var user = new ApplicationUser();
            var cancellationToken = new CancellationToken();
            var sut = new CustomUserRoleStore();

            await Assert.ThrowsAsync<NotImplementedException>(() =>
                sut.UpdateAsync(user, cancellationToken)
            );
        }

    }
}
