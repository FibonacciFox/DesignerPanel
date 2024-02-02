using Avalonia.Interactivity;

namespace DesignerControlApp;

public class SelectedEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Returns or sets a value indicating whether the target control is selected for editing.
    /// </summary>
    public bool IsSelected { get; set; }

    public SelectedEventArgs()
    {
        
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectedEventArgs"/> class.
    /// </summary>
    /// <param name="routedEvent">The routed event associated with these event args.</param>
    public SelectedEventArgs(RoutedEvent? routedEvent)
    {
        RoutedEvent = routedEvent;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RoutedEventArgs"/> class.
    /// </summary>
    /// <param name="routedEvent">The routed event associated with these event args.</param>
    /// <param name="source">The source object that raised the routed event.</param>
    // public SelectedEventArgs(RoutedEvent? routedEvent, AdornerTargetControl source)
    // {
    //     RoutedEvent = routedEvent;
    //     Source = source;
    //     IsSelected = source.IsSelected;
    // }
}