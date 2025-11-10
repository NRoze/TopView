using TopView.Core.Models;
using TopView.Core.ViewModels;

namespace TopView.Views;

public partial class ChartView : ContentView
{
    private OverviewViewModel ViewModel => BindingContext as OverviewViewModel;

    private bool _isRealData = true;
    public bool IsRealData
    {
        get => _isRealData;
        set
        {
            if (_isRealData != value)
            {
                _isRealData = value;
                OnPropertyChanged(nameof(IsRealData));
            }
                
            UpdateDataView();
        }
    }
    public ChartView()
	{
		InitializeComponent();

        BindingContextChanged += (s, e) => registerPropertyChanged();
    }

    private void registerPropertyChanged()
    {
        if (ViewModel != null)
        {
            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(OverviewViewModel.BalancePoints))
                {
                    UpdateDataView();
                }
            };
        }
    }

    public void UpdateDataView()
    {
        if (IsRealData && ViewModel != null)
        {
            MyChart.Data = ViewModel.BalancePoints;
        }
        else
        {
            setDemo();
        }
    }

    private void setDemo()
    {
        DateTime month = DateTime.Now;
        var balance = 1000.0;
        var data = new List<BalancePoint>();
        Random rand = new Random();

        for (int i = 0; i < 12; i++)
        {
            data.Add(new BalancePoint() { Time = month, Balance = balance });
            balance += rand.NextDouble() * 200 - 100;
            month = month.AddMonths(-1);
        }

        data.Reverse();
        MyChart.Data = data;
    }

    private void LoadRealData_Clicked(object sender, EventArgs e)
    {
        IsRealData = true;
    }
    private void LoadData_Clicked(object sender, EventArgs e)
    {
        IsRealData = false;
    }
}