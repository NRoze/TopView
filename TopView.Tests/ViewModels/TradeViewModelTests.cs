using Moq;
using System.Threading.Tasks;
using TopView.Common.Infrastructure;
using TopView.Model.Models;
using TopView.Services.Interfaces;
using TopView.ViewModel;
using Xunit;

namespace TopView.Tests.ViewModels
{
	public class TradeViewModelTests
	{
		[Fact]
		public void UpdateFromModel_InitializesProperties()
		{
			// Arrange
			var trade = new Trade
			{
				Symbol = "AAPL",
				Price =150m,
				Quantity =2m,
				Cost =100m,
				Change =1m,
				ChangeP =0.5m,
				Realized =10m
			};

			var repo = new Mock<RepositoryCached<Trade>>();
			var vm = new TradeViewModel(repo.Object, trade);

			// Act
			// ctor calls UpdateFromModel, so nothing else required

			// Assert
			Assert.Equal(trade.Symbol, vm.Symbol);
			Assert.Equal(trade.Price, vm.Price);
			Assert.Equal(trade.Quantity, vm.Quantity);
			Assert.Equal(trade.Cost, vm.Cost);
			Assert.Equal(trade.Realized, vm.Realized);
			Assert.Equal(trade.Change, vm.Change);
			Assert.Equal(trade.ChangeP, vm.ChangeP);
		}

		[Fact]
		public void CalculatedProperties_WorkAsExpected()
		{
			// Arrange
			var trade = new Trade
			{
				Price =10m,
				Quantity =3m,
				Cost =4m
			};
			var repo = new Mock<RepositoryCached<Trade>>();
			var vm = new TradeViewModel(repo.Object, trade);

			// Act
			var value = vm.Value; // Price * Quantity
			var unrealized = vm.Unrealized; // Value - (Cost * Quantity)
			var unrealizedP = vm.UnrealizedP; // unrealized / (Cost * Quantity) *100

			// Assert
			Assert.Equal(30m, value);
			Assert.Equal(30m - (4m *3m), unrealized);
			Assert.Equal(unrealized / (4m *3m) *100m, unrealizedP);
		}

		[Fact]
		public void SettingProperties_InvokesRepositorySave()
		{
			// Arrange
			var trade = new Trade
			{
				Price =10m,
				Quantity =1m,
				Cost =5m
			};

			var repo = new Mock<RepositoryCached<Trade>>();
			repo.Setup(r => r.SaveAsync(It.IsAny<Trade>())).Returns(Task.CompletedTask);

			var vm = new TradeViewModel(repo.Object, trade);

			// Act
			vm.Cost =6m; // should call Commit -> SaveAsync
			vm.Price =11m;
			vm.Quantity =2m;
			vm.Realized =7m;
			vm.Change =0.5m;
			vm.ChangeP =1.2m;

			// Assert
			// Commit is called on each setter; verify SaveAsync was invoked at least once
			repo.Verify(r => r.SaveAsync(It.IsAny<Trade>()), Times.AtLeast(1));
		}
	}
}
