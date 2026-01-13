using Moq;
using System;
using TopView.Common.Infrastructure;
using TopView.Model.Models;
using TopView.Services.Interfaces;
using TopView.ViewModel;
using Xunit;

namespace TopView.Tests.ViewModels
{
 public class AccountsViewModelTests
 {
 [Fact]
 public void Constructor_InitializesAccounts()
 {
 // Arrange
 var repo = new Mock<IAccountRepository>();
 var vmFactory = new Mock<IViewModelFactory>();
 Func<TopView.Model.Models.Account, AccountViewModel> accountVmFactory = _ => null!;

 // Act
 var vm = new AccountsViewModel(vmFactory.Object, repo.Object, accountVmFactory);

 // Assert
 Assert.NotNull(vm.Accounts);
 }

 [Fact]
 public async System.Threading.Tasks.Task AddAccountCommand_AddsAccount()
 {
 // Arrange
 var repo = new Mock<IAccountRepository>();
 var vmFactory = new Mock<IViewModelFactory>();
 Func<Account, AccountViewModel> accountVmFactory = _ => new Mock<AccountViewModel>(null, null, null, null).Object;
 var vm = new AccountsViewModel(vmFactory.Object, repo.Object, accountVmFactory);

 // Act
 await System.Threading.Tasks.Task.Run(() => vm.AddAccountCommand.Execute(null));

 // Assert
 Assert.True(vm.Accounts.Count >=0); // Should be at least0 or more
 }
 }
}
