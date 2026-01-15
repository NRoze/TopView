using TopView.Services.Interfaces;
using TopView.Model.Models;
using TopView.Common.Infrastructure;

namespace TopView.ViewModel;

public partial class TradeViewModel : BaseNotify
{
    private readonly RepositoryCached<Trade> _repo;
    public Trade Trade { get; }

    public TradeViewModel(RepositoryCached<Trade> repo, Trade trade)
    {
        _repo = repo;
        Trade = trade;

        UpdateFromModel();
    }

    public void UpdateFromModel()
    {
        Realized = Trade.Realized;
        Quantity = Trade.Quantity;
        Price = Trade.Price;
        Change = Trade.Change;
        ChangeP = Trade.ChangeP;
        Cost = Trade.Cost;
    }

    public string Symbol => Trade.Symbol;
    public decimal Value => Trade.Price * Trade.Quantity;
    public decimal Unrealized => Value - (Trade.Cost * Trade.Quantity);
    public decimal UnrealizedP => Unrealized / (Trade.Cost * Trade.Quantity) * 100;

    private decimal _cost;
    public decimal Cost
    {
        get { return _cost; }
        set
        {
            if (_cost != value)
            {
                Trade.Cost = _cost = value;
                OnPropertyChanged();
                Commit();
            }
        }
    }

    public void Commit()
    {
        _repo.SaveAsync(Trade);
    }

    private decimal _realized;
    public decimal Realized
    {
        get { return _realized; }
        set
        {
            if (_realized != value)
            {
                Trade.Realized = _realized = value;
                OnPropertyChanged();
                Commit();
            }
        }
    }
    private decimal _quantity;
    public decimal Quantity
    {
        get { return _quantity; }
        set
        {
            if (_quantity != value)
            {
                Trade.Quantity = _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(Unrealized));
                Commit();
            }
        }
    }
    private decimal _price;
    public decimal Price
    {
        get { return _price; }
        set
        {
            if (_price != value)
            {
                Trade.Price = _price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(Unrealized));
                Commit();
            }
        }
    }

    private decimal _change;
    public decimal Change
    {
        get { return _change; }
        set
        {
            if (_change != value)
            {
                Trade.Change = _change =  value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ChangeP));
                OnPropertyChanged(nameof(Unrealized));
                Commit();
            }
        }
    }
    private decimal _changeP;
    public decimal ChangeP
    {
        get { return _changeP; }
        set
        {
            if (_changeP != value)
            {
                Trade.ChangeP = _changeP = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Change));
                OnPropertyChanged(nameof(Unrealized));
                Commit();
            }
        }
    }

    //public string DisplayChange => $"{Change:+0.##;-0.##;0} ({ChangeP:+0.##;-0.##;0}%)";
    //public string DisplayUnrealized => $"{Unrealized:+0.##;-0.##;0} ({UnrealizedP:+0.##;-0.##;0}%)";

}
