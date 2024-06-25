using System;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;

namespace DesignerPanel
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddAttached(DesignPanel);
        }

        public void AddAttached(Panel panel)
        {
            foreach (var child in panel.Children)
            {
                AdornerPanel.Children.Add(new Decorator(child, AdornerPanel));

                if (child is Panel childPanel)
                {
                    AddAttached(childPanel);
                }
            }
        }

        private void ControlsListView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is TextBlock selectedItem)
            {
                Control newControl = selectedItem.Tag switch
                {
                    "Button" => new Button { Content = "New Button" },
                    "TextBox" => new TextBox { Text = "New TextBox" },
                    "Label" => new TextBlock { Text = "New Label" },
                    "StackPanel" => new StackPanel { Background = Brushes.LightGray, Width = 100, Height = 100 },
                    "DockPanel" => new DockPanel { Background = Brushes.LightGray, Width = 100, Height = 100 },
                    "Grid" => new Grid { Background = Brushes.LightGray, Width = 100, Height = 100 },
                    "ComboBox" => new ComboBox { ItemsSource = new[] { "Item1", "Item2" } },
                    "CheckBox" => new CheckBox { Content = "New CheckBox" },
                    "RadioButton" => new RadioButton { Content = "New RadioButton" },
                    "Slider" => new Slider(),
                    "ProgressBar" => new ProgressBar(),
                    "Image" => new Image { Source = new Avalonia.Media.Imaging.Bitmap(AssetLoader.Open(new Uri("avares://DesignerPanel/Assets/image.png"))), Width = 100, Height = 100 },
                    "Calendar" => new Calendar(){Width = 100, Height = 100 },
                    _ => throw new InvalidOperationException("Unknown control type")
                };

                // Set default position
                Canvas.SetLeft(newControl, 50);
                Canvas.SetTop(newControl, 50);

                // Add the new control to the DesignPanel and wrap it in a Decorator
                DesignPanel.Children.Add(newControl);
                AdornerPanel.Children.Add(new Decorator(newControl, AdornerPanel));

                // Deselect the item
                listBox.SelectedItem = null;
            }
        }
    }
}
