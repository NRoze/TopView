using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Principal;
using System.Windows.Input;
using TopView.Core.Data;
using TopView.Core.Infrastructure;
using TopView.Core.Models;
using TopView.Core.Services;
using TopView.Core.ViewModel;

namespace TopView.Core.ViewModels
{
    public class AccountsViewModel : BaseNotify
    {
        private readonly IAccountRepository _repo;
        private readonly Func<Account, AccountViewModel> _accountVmFactory;

        public ObservableCollection<IAccountViewModel> Accounts { get; } = new ObservableCollection<IAccountViewModel>();

        private IAccountViewModel? _selectedAccount;
        public IAccountViewModel? SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                if (_selectedAccount != value)
                {
                    _selectedAccount = value;
                    _selectedAccount?.Update();
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddAccountCommand => new Command(_ => createAccount("New Account"));
        public ICommand RemoveAccountCommand => new Command<AccountViewModel>(removeAccount);


        public AccountsViewModel(IAccountRepository repo, Func<Account, AccountViewModel> accountVmFactory)
        {
            _repo = repo;
            _accountVmFactory = accountVmFactory;

            //RESET accounts!!
            _repo.Reset();

            LoadAccounts();
        }

        private async void createAccount(string name, bool isOverview = false)
        {
            Account newAccount = new Account { Name = name, IsOverview = isOverview };
            IAccountViewModel vm;

            if (isOverview)
            {
                vm = ServiceHelper.GetService<OverviewViewModel>();
                vm.Account = newAccount;
                vm.Name = name;
            }
            else
            {
                vm = _accountVmFactory(newAccount);

                await _repo.AddAsync(newAccount);
            }

            Accounts.Add(vm);
            SelectedAccount = vm;
        }
        private async void removeAccount(AccountViewModel accountVM)
        {
            if (accountVM != null && !accountVM.Account.IsOverview)
            {
                await _repo.RemoveAsync(accountVM.Account);
                Accounts.Remove(accountVM);

                if (SelectedAccount == accountVM)
                    SelectedAccount = null;
            }
        }

        private async void LoadAccounts()
        {
            var accounts = await _repo.GetAccountsAsync();
            var trades = await _repo.GetTradesAsync();

            if (accounts.Count() == 0)
            {
                createAccount("Overview", true);
            }

            foreach (var account in accounts)
            {
                account.Trades = new ObservableCollection<Trade>(trades.Where(t => t.AccountId == account.Id && t.Quantity != 0).ToList());
                Accounts.Add(_accountVmFactory(account));
            }

            SelectedAccount = Accounts.FirstOrDefault();
        }

    }
}
