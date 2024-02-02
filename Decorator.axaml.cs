using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace DesignerPanel;

public partial class Decorator : UserControl
{
    /// <summary>
    /// Defines the <see cref="IsSelected"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<Decorator, bool>(nameof(IsSelected), defaultValue: true);

    /// <summary>
    /// Returns or sets a value indicating whether the target control is selected for editing.
    /// </summary>
    public bool IsSelected
    {
        get { return GetValue(IsSelectedProperty); }
        set
        {
            SetValue(IsSelectedProperty, value);
        }
    }
    
    public Decorator()
    {
        InitializeComponent();
    }
    
    public Decorator(Control targetControl , Canvas layer)
    {
        InitializeComponent();
        _targetControl = targetControl;
        _layer = layer;
       
        _targetControl.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            //А тут вернется Stack Panel
              Console.WriteLine(sender);
            
            e.Handled = true;
        }, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        
        _targetControl.AddHandler(
            TappedEvent,
            (sender, e) =>
            {
                //Тут все ок
                Console.WriteLine(sender);
                e.Handled = true;
            },
            routes:  RoutingStrategies.Tunnel | RoutingStrategies.Bubble );
        
       // _targetControl.AddHandler(PointerReleasedEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble );
        //_targetControl.AddHandler(PointerMovedEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        //_targetControl.AddHandler(PointerEnteredEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        //_targetControl.AddHandler(KeyUpEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        //_targetControl.AddHandler(KeyDownEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        
        //Cобытие возникает, когда контрол был размещен и его размеры были определены.
        _targetControl.LayoutUpdated += TargetControlOnLayoutUpdated;
        
        //Resize
        // Добавляем обработчики событий для LeftCenterAnchor и RightCenterAnchor обязательно RoutingStrategies.Tunnel 
        LeftCenterAnchor.AddHandler(PointerPressedEvent,AnchorOnPointerPressed, RoutingStrategies.Tunnel  );
        LeftCenterAnchor.AddHandler(PointerMovedEvent,LeftCenterAnchorOnPointerMoved, RoutingStrategies.Tunnel  );
        LeftCenterAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased,RoutingStrategies.Tunnel);
        
        RightCenterAnchor.AddHandler(PointerPressedEvent,AnchorOnPointerPressed, RoutingStrategies.Tunnel  );
        RightCenterAnchor.AddHandler(PointerMovedEvent,RightCenterAnchorOnPointerMoved, RoutingStrategies.Tunnel  );
        RightCenterAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased,RoutingStrategies.Tunnel);
        //
    }
    
    
    private void AnchorOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _isResizing = true;
        _isDraggable = true;
        _startSizePoint = e.GetCurrentPoint((Visual?)_targetControl.Parent);
    }
    
    private void LeftCenterAnchorOnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isResizing)
        {
            var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
            var delta = _startSizePoint.Position.X - point.Position.X;
            _targetControl.Width += delta;
            _startSizePoint = point;
            Canvas.SetLeft(_targetControl, Canvas.GetLeft(_targetControl) - delta);
        }
    }
    
    private void RightCenterAnchorOnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isResizing)
        {
            var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
            var delta = point.Position.X - _startSizePoint.Position.X;
            _targetControl.Width += delta;
            _startSizePoint = point;
        }
    }
    
    private void AnchorOnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isResizing = false;
        _isDraggable = true;
    }

    private void TargetControlOnLayoutUpdated(object? sender, EventArgs e)
    {
        Width = _targetControl.Bounds.Width;
        _targetControl.Width = _targetControl.Bounds.Width;
        
        Height = _targetControl.Bounds.Height;
        _targetControl.Height = _targetControl.Bounds.Height;
        
        //Получаем позицию _targetControl относительно Layer
        var relativePositionToParent = _targetControl.TranslatePoint(new Point(0, 0), _layer);
        
        Canvas.SetLeft(this, relativePositionToParent.Value.X );
        Canvas.SetTop(this, relativePositionToParent.Value.Y );
        
    }

    
    private Control _targetControl;
    private Control _layer;
    private bool _isResizing;
    private bool _isDraggable;
    private PointerPoint _startSizePoint;
    private PointerPoint _startPoint;
}