using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace DesignerPanel;

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
}