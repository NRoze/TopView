using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Input;
using TopView.Common.Infrastructure;
using TopView.Model.Models;
using TopView.Services.Interfaces;
using TopView.ViewModel.Interfaces;

namespace TopView.ViewModel
{
    public class AccountsViewModel : BaseNotify
    {
        private readonly RepositoryCached<Account> _accountRepo;
        private readonly RepositoryCached<Trade> _tradesRepo;
        private readonly Func<Account, AccountViewModel> _accountVmFactory;
        private readonly IViewModelFactory _vmFactory;
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

        public ICommand AddAccountCommand => new RelayCommand(async _ => await CreateAccount("New Account"));
        public ICommand RemoveAccountCommand => new RelayCommand<AccountViewModel>(
                                    async (vm) => await RemoveAccount(vm));

        private bool settingsDisplayed = false;
        public bool SettingsDisplayed 
        {
            get => settingsDisplayed;
            set
            { 
                if (settingsDisplayed != value)
                {
                    settingsDisplayed = value;
                    OnPropertyChanged();
                }   
            }
        }

        public AccountsViewModel(
            IViewModelFactory vmFactory,
            RepositoryCached<Account> repo,
            RepositoryCached<Trade> tradesRepo,
            Func<Account, AccountViewModel> accountVmFactory)
        {
            _vmFactory = vmFactory;
            _accountRepo = repo;
            _tradesRepo = tradesRepo;
            _accountVmFactory = accountVmFactory;
            _ = LoadAccountsAsync();
        }

        internal async Task LoadAccountsAsync()
        {
            try
            {
                ClearData();
                await PopulateAccounts();
                SelectedAccount = Accounts.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load accounts: {ex.Message}");
            }
        }
        private async Task CreateAccount(string name, bool isOverview = false)
        {
            Account newAccount = new Account { Name = name, IsOverview = isOverview };
            IAccountViewModel vm;

            await _accountRepo.AddAsync(newAccount);
            if (isOverview)
            {
                vm = _vmFactory.Create<OverviewViewModel>();
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
        private async Task RemoveAccount(AccountViewModel accountVM)
        {
            if (accountVM != null && !accountVM.Account.IsOverview)
            {
                await _accountRepo.RemoveAsync(accountVM.Account);
                Accounts.Remove(accountVM);

                if (SelectedAccount == accountVM)
                    SelectedAccount = null;
            }
        }

        private async Task PopulateAccounts()
        {
            var accounts = await _accountRepo.GetAllAsync();
            var trades = await _tradesRepo.GetAllAsync();

            if (accounts?.Count() == 0)
            {
                await CreateAccount("Overview", true);
            }

            foreach (var account in accounts!)
            {
                IAccountViewModel vm;

                if (account.IsOverview)
                {
                    vm = _vmFactory.Create<OverviewViewModel>();
                    vm.Account = account;
                    vm.Name = account.Name;
                }
                else
                {
                    if (trades?.Count() > 0)
                    {
                        account.Trades = new ObservableCollection<Trade>(trades.Where(t => t.AccountId == account.Id && !t.IsOver));
                    }

                    vm = _accountVmFactory(account);
                }

                Accounts.Add(vm);
            }
        }

        private void ClearData()
        {
            Accounts.Clear();
            SelectedAccount = null;
        }
    }
}
