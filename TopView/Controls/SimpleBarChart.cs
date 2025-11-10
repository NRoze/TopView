using TopView.Core.Models;

namespace TopView.Controls
{
    public class SimpleBarChart : ContentView
    {
        public static readonly BindableProperty DataProperty =
            BindableProperty.Create(
                nameof(Data),
                typeof(List<BalancePoint>),
                typeof(SimpleBarChart),
                new List<BalancePoint>(),
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((SimpleBarChart)bindable).DrawChart();
                });

        public List<BalancePoint> Data
        {
            get => (List<BalancePoint>)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        private readonly Grid _chartGrid;

        public SimpleBarChart()
        {
            _chartGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star }, // Bars
                    new RowDefinition { Height = GridLength.Auto }  // X-axis labels
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto } // Labels 
                }
            };

            Content = _chartGrid;
        }

        private void DrawChart()
        {
            _chartGrid.Children.Clear();

            if (Data == null || Data.Count == 0)
                return;

            double max = Data.Max(d => d.Balance);

            // Stack for bars
            var barsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Fill,
                Spacing = 5, // space between bars
                Padding = new Thickness(10, 0, 0, 0) // move right by 10
            };

            // X-axis labels
            var labelsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                Spacing = 5,
                Padding = new Thickness(10, 0, 0, 0) // same right shift
            };

            foreach (var point in Data)
            {
                double height = max == 0 ? 0 : 200 * (point.Balance / max);

                // Bar
                var bar = new BoxView
                {
                    Color = Colors.ForestGreen,
                    WidthRequest = 20, // thinner
                    HeightRequest = height,
                    VerticalOptions = LayoutOptions.End
                };

                barsLayout.Children.Add(bar);

                // Date label centered under bar
                var label = new Label
                {
                    Text = point.Time.ToString("MMM yy"),
                    FontSize = 10,
                    HorizontalTextAlignment = TextAlignment.Center,
                    WidthRequest = 20 // same width as bar for centering
                };

                labelsLayout.Children.Add(label);
            }

            // Y-axis scale
            var scaleLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Start,
                Spacing = 0,
            };

            int steps = 5;
            for (int i = steps; i >= 0; i--)
            {
                scaleLayout.Children.Add(new Label
                {
                    Text = (max * i / steps).ToString("N0"),
                    FontSize = 10,
                    VerticalTextAlignment = TextAlignment.End,
                    HorizontalTextAlignment = TextAlignment.End,
                    HeightRequest = 200.0 / steps
                });
            }

            // Combine Y-axis and bars
            var mainGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            //mainGrid.Add(scaleLayout);
            //mainGrid.Add(barsLayout, 1, 0);

            // Add to chart grid
            _chartGrid.Add(scaleLayout, 0, 0);
            _chartGrid.Add(barsLayout, 1, 0);
            _chartGrid.Add(labelsLayout, 1, 1);
        }
    }
}