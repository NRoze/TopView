using System.Windows.Input;
using TopView.Core.ViewModel;
using TopView.Core.ViewModels;

namespace TopView.Views;

public partial class AccountView : ContentView
{
    public AccountView()
	{
        InitializeComponent();
    }
    private void OnSubmitClicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(TransferAmount.Text, out _))
            TransferAmount.Text = string.Empty;
    }
}