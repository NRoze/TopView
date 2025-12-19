using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TopView.Common.Infrastructure;
using TopView.Services.Interfaces;
using TopView.ViewModel.Interfaces;

namespace TopView.ViewModel
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
            ResetDatabaseCommand = new RelayCommand(async (o) => await ResetDatabase());
            CloseSettingsCommand = new RelayCommand((o) => CloseSettings());
        }

        private void CloseSettings()
        { 
            _accoountsVM.SettingsDisplayed = false;
        }
        private async Task ResetDatabase()
        {
            //TBD
            //bool confirmed = await Application.Current.MainPage.DisplayAlert(
            //    "Confirm Reset",
            //    "This will erase all data. Are you sure?",
            //    "Yes",
            //    "No");

            //if (!confirmed)
            //    return;

            //await _accountRepo.Reset();
            //await _accoountsVM.LoadAccounts();
            //await Application.Current.MainPage.DisplayAlert("Done", "Database has been reset.", "OK");
            //CloseSettings();
        }
    }

}
