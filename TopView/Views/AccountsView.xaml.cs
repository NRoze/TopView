using TopView.ViewModels;
using TopView.Common.Infrastructure;

namespace TopView.Views;

public partial class AccountsView : ContentView
{
    private AccountsViewModel vm;
    public AccountsView()
    {
        InitializeComponent();

        vm = ServiceHelper.GetService<AccountsViewModel>();

        this.BindingContext = vm;
        vm.PropertyChanged += Vm_PropertyChanged;
    }

    private void Vm_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(vm.SelectedAccount))
        {
            if (vm.SelectedAccount?.Account?.IsOverview == true)
            {
                m_Overview.IsVisible = true;
                m_AccountView.IsVisible = false;
            }
            else
            {
                m_Overview.IsVisible = false;
                m_AccountView.IsVisible = true;
            }
        }
    }

    private void OnSettingsTapped(object sender, TappedEventArgs e)
    {
        if (sender is Label label)
        {
            vm.SettingsDisplayed = true;
        }
    }
}
