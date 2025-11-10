using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        private readonly IAccountRepository _accountRepo;
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
        public ICommand RemoveAccountCommand => new Command<AccountViewModel>(
                                    async (vm) => await removeAccount(vm));


        public AccountsViewModel(IAccountRepository repo, Func<Account, AccountViewModel> accountVmFactory)
        {
            _accountRepo = repo;
            _accountVmFactory = accountVmFactory;

            //RESET accounts!!
            //_repo.Reset();

            LoadAccounts();
        }

        private async Task createAccount(string name, bool isOverview = false)
        {
            Account newAccount = new Account { Name = name, IsOverview = isOverview };
            IAccountViewModel vm;

            await _accountRepo.AddAsync(newAccount);
            if (isOverview)
            {
                vm = ServiceHelper.GetService<OverviewViewModel>();
                vm.Account = newAccount;
                vm.Name = name;
            }
            else
            {
                vm = _accountVmFactory(newAccount);
            }

            Accounts.Add(vm);
            SelectedAccount = vm;
        }
        private async Task removeAccount(AccountViewModel accountVM)
        {
            if (accountVM != null && !accountVM.Account.IsOverview)
            {
                await _accountRepo.RemoveAsync(accountVM.Account);
                Accounts.Remove(accountVM);

                if (SelectedAccount == accountVM)
                    SelectedAccount = null;
            }
        }

        private async Task LoadAccounts()
        {
            var accounts = await _accountRepo.GetAccountsAsync();
            var trades = await _accountRepo.GetTradesAsync();

            foreach (var acc in accounts)
            {
                Debug.WriteLine($"{acc.Name}, {acc.Id}");
            }
            if (accounts.Count() == 0)
            {
                createAccount("Overview", true);
            }

            foreach (var account in accounts)
            {
                IAccountViewModel vm;


                if (account.IsOverview)
                {
                    vm = ServiceHelper.GetService<OverviewViewModel>();
                    vm.Account = account;
                    vm.Name = account.Name;
                }
                else
                {
                    account.Trades = new ObservableCollection<Trade>(trades.Where(t => t.AccountId == account.Id && !t.IsOver));
                    vm = _accountVmFactory(account);
                }
                    
                Accounts.Add(vm);
            }

            SelectedAccount = Accounts.FirstOrDefault();
        }

    }
}
