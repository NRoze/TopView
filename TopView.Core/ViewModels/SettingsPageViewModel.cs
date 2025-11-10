using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TopView.Core.Infrastructure;
using TopView.Core.Services;

namespace TopView.Core.ViewModels
{
    public class SettingsPageViewModel : BaseNotify, ISettingsPageViewModel
    {
        private readonly IAccountRepository _accountRepo;
        private readonly AccountsViewModel _accoountsVM;

        public ICommand ResetDatabaseCommand { get; }
        public ICommand CloseSettingsCommand { get; }

        public SettingsPageViewModel(IAccountRepository accountRepo, AccountsViewModel accountsVM)
        {
            _accountRepo = accountRepo;
            _accoountsVM = accountsVM;
            ResetDatabaseCommand = new Command(async () => await ResetDatabase());
            CloseSettingsCommand = new Command(async () => await CloseSettings());
        }

        private async Task CloseSettings()
        { 
            _accoountsVM.SettingsDisplayed = false;
        }
        private async Task ResetDatabase()
        {
            bool confirmed = await Application.Current.MainPage.DisplayAlert(
                "Confirm Reset",
                "This will erase all data. Are you sure?",
                "Yes",
                "No");

            if (!confirmed)
                return;

            await _accountRepo.Reset();
            _accoountsVM.LoadAccounts();
            await Application.Current.MainPage.DisplayAlert("Done", "Database has been reset.", "OK");
            CloseSettings();
        }
    }

}
