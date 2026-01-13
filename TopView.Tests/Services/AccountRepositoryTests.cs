using System;
using Moq;
using TopView.Model.Data;
using TopView.Services;
using TopView.Model.Models;
using Xunit;

namespace TopView.Tests.Services
{
    public class AccountRepositoryTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            // Arrange
            var dbMock = new Mock<AppDbContext>("fakepath");

            // Act
            var repo = new AccountRepository(dbMock.Object);

            // Assert
            Assert.NotNull(repo);
        }
    }
}
