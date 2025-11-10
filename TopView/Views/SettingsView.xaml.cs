using TopView.Core.ViewModels;

namespace TopView.Views;

public partial class SettingsView : ContentView
{
    public SettingsView()
	{
		InitializeComponent();

		this.BindingContext = ServiceHelper.GetService<ISettingsPageViewModel>();
	}
}