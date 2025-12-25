using TopView.Common.Infrastructure;
using TopView.ViewModel.Interfaces;

namespace TopView.Views;

public partial class SettingsView : ContentView
{
    public SettingsView()
	{
		InitializeComponent();

		this.BindingContext = ServiceHelper.GetService<ISettingsPageViewModel>();
	}
}