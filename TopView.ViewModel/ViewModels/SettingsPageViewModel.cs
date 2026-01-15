using System.Windows.Input;
using TopView.Common.Infrastructure;
using TopView.Model.Models;
using TopView.Services.Interfaces;
using TopView.ViewModel.Interfaces;

namespace TopView.ViewModel
{
    public class SettingsPageViewModel : BaseNotify, ISettingsPageViewModel
    {
        private readonly RepositoryCached<Account> _accountRepo;
        private readonly AccountsViewModel _accoountsVM;
        private readonly IDialogService _dialogService;

        public ICommand ResetDatabaseCommand { get; }
        public ICommand CloseSettingsCommand { get; }

        public SettingsPageViewModel(
            RepositoryCached<Account> accountRepo, AccountsViewModel accountsVM, IDialogService dialogService)
        {
            _accountRepo = accountRepo;
            _accoountsVM = accountsVM;
            _dialogService = dialogService;

            ResetDatabaseCommand = new RelayCommand(async (o) => await ResetDatabase());
            CloseSettingsCommand = new RelayCommand((o) => CloseSettings());
        }

        private void CloseSettings()
        { 
            _accoountsVM.SettingsDisplayed = false;
        }
        private async Task ResetDatabase()
        {
            bool result = await _dialogService.ConfirmAsync("Confirm Reset", "This will erase all data. Are you sure?");
            
            if (result)
            {
                await _accountRepo.Reset();
                await _accoountsVM.LoadAccountsAsync();
                await _dialogService.DisplayAsync("Done", "Database has been reset.");
                CloseSettings();
            }
        }
    }

}
