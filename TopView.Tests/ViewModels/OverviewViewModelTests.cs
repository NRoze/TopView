using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopView.Common.Infrastructure;
using TopView.Model.Models;
using TopView.Services.Interfaces;
using TopView.ViewModel;
using Xunit;

namespace TopView.Tests.ViewModels
{
	public class OverviewViewModelTests
	{
		[Fact]
		public async Task Update_SetsProperties()
		{
			// Arrange
			var balancePointRepo = new Mock<RepositoryCached<BalancePoint>>();
			var tradeRepo = new Mock<RepositoryCached<Trade>>();
			var accountsViewModel = new Mock<AccountsViewModel>(null, null, null).Object;

			balancePointRepo.Setup(r => r.GetAllAsync())
				.ReturnsAsync(new List<BalancePoint> { new BalancePoint { Time = DateTime.Now, Balance =1000 } });
            tradeRepo.Setup(r => r.GetAllAsync())
				.ReturnsAsync(new List<Trade> { new Trade { Realized =10 }, new Trade { Realized = -5 } });

			var vm = new OverviewViewModel(balancePointRepo.Object, tradeRepo.Object, accountsViewModel);

			// Act
			await vm.Update();

			// Assert
			Assert.NotNull(vm.BalancePoints);
			Assert.True(vm.SuccessRate >=0);
			Assert.True(vm.TotalTrades >=0);
		}

		[Fact]
		public async Task createIfNeeded_InitializesBalancePoints()
		{
			// Arrange
			var balancePointRepo = new Mock<RepositoryCached<BalancePoint>>();
			var tradeRepo = new Mock<RepositoryCached<Trade>>();
			var accountsViewModel = new Mock<AccountsViewModel>(null, null, null).Object;
			var balancePoints = new List<BalancePoint> { new BalancePoint { Id =1, Time = DateTime.Now, Balance =1000 } };
			balancePointRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(balancePoints);
			var vm = new OverviewViewModel(balancePointRepo.Object, tradeRepo.Object, accountsViewModel);

			// Act
			var method = typeof(OverviewViewModel).GetMethod("createIfNeeded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			await (Task)method.Invoke(vm, null);

			// Assert
			Assert.NotNull(vm.BalancePoints);
			Assert.Single(vm.BalancePoints);
		}
	}
}
