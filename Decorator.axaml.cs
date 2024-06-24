using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace DesignerPanel
{
    public partial class Decorator : UserControl
    {
        // Property to indicate if the control is selected for editing
        public static readonly StyledProperty<bool> IsSelectedProperty =
            AvaloniaProperty.Register<Decorator, bool>(nameof(IsSelected), defaultValue: false);

        public bool IsSelected
        {
            get => GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        // Fields for control and resizing logic
        private Control _targetControl;
        private Panel _layer;
        private bool _isResizing;
        private PointerPoint _startSizePoint;
        private string _currentAnchor;

        // Default constructor
        public Decorator()
        {
            InitializeComponent();
        }

        // Constructor with target control and layer
        public Decorator(Control targetControl, Panel layer)
        {
            InitializeComponent();
            _targetControl = targetControl;
            _layer = layer;

            // Add handlers for various pointer events
            AddPointerHandlers();

            // Add layout updated event handler
            _targetControl.LayoutUpdated += TargetControlOnLayoutUpdated;

            // Add resize handlers for anchors
            AddAnchorHandlers();
        }

        private void AddPointerHandlers()
        {
            _targetControl.AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel, true);
            _targetControl.AddHandler(PointerReleasedEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
            _targetControl.AddHandler(PointerMovedEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
            _targetControl.AddHandler(PointerEnteredEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
            _targetControl.AddHandler(KeyUpEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
            _targetControl.AddHandler(KeyDownEvent, (sender, e) => e.Handled = true, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        }

        private void AddAnchorHandlers()
        {
            TopLeftAnchor.AddHandler(PointerPressedEvent, AnchorOnPointerPressed, RoutingStrategies.Tunnel);
            TopLeftAnchor.AddHandler(PointerMovedEvent, TopLeftAnchorOnPointerMoved, RoutingStrategies.Tunnel);
            TopLeftAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased, RoutingStrategies.Tunnel);

            LeftCenterAnchor.AddHandler(PointerPressedEvent, AnchorOnPointerPressed, RoutingStrategies.Tunnel);
            LeftCenterAnchor.AddHandler(PointerMovedEvent, LeftCenterAnchorOnPointerMoved, RoutingStrategies.Tunnel);
            LeftCenterAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased, RoutingStrategies.Tunnel);

            BottomLeftAnchor.AddHandler(PointerPressedEvent, AnchorOnPointerPressed, RoutingStrategies.Tunnel);
            BottomLeftAnchor.AddHandler(PointerMovedEvent, BottomLeftAnchorOnPointerMoved, RoutingStrategies.Tunnel);
            BottomLeftAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased, RoutingStrategies.Tunnel);

            TopRightAnchor.AddHandler(PointerPressedEvent, AnchorOnPointerPressed, RoutingStrategies.Tunnel);
            TopRightAnchor.AddHandler(PointerMovedEvent, TopRightAnchorOnPointerMoved, RoutingStrategies.Tunnel);
            TopRightAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased, RoutingStrategies.Tunnel);

            RightCenterAnchor.AddHandler(PointerPressedEvent, AnchorOnPointerPressed, RoutingStrategies.Tunnel);
            RightCenterAnchor.AddHandler(PointerMovedEvent, RightCenterAnchorOnPointerMoved, RoutingStrategies.Tunnel);
            RightCenterAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased, RoutingStrategies.Tunnel);

            BottomRightAnchor.AddHandler(PointerPressedEvent, AnchorOnPointerPressed, RoutingStrategies.Tunnel);
            BottomRightAnchor.AddHandler(PointerMovedEvent, BottomRightAnchorOnPointerMoved, RoutingStrategies.Tunnel);
            BottomRightAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased, RoutingStrategies.Tunnel);

            TopCenterAnchor.AddHandler(PointerPressedEvent, AnchorOnPointerPressed, RoutingStrategies.Tunnel);
            TopCenterAnchor.AddHandler(PointerMovedEvent, TopCenterAnchorOnPointerMoved, RoutingStrategies.Tunnel);
            TopCenterAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased, RoutingStrategies.Tunnel);

            BottomCenterAnchor.AddHandler(PointerPressedEvent, AnchorOnPointerPressed, RoutingStrategies.Tunnel);
            BottomCenterAnchor.AddHandler(PointerMovedEvent, BottomCenterAnchorOnPointerMoved, RoutingStrategies.Tunnel);
            BottomCenterAnchor.AddHandler(PointerReleasedEvent, AnchorOnPointerReleased, RoutingStrategies.Tunnel);
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            Console.WriteLine(sender);

            // Iterate over children of the layer and deselect all decorators
            foreach (var child in _layer.Children)
            {
                if (child is Decorator decorator)
                {
                    decorator.IsSelected = false;
                }
            }

            IsSelected = true;
            e.Handled = true;
        }

        private void AnchorOnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            _isResizing = true;
            _startSizePoint = e.GetCurrentPoint((Visual?)_targetControl.Parent);
            _currentAnchor = ((Control)sender!).Name;
        }

        private void TopLeftAnchorOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isResizing)
            {
                var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
                var deltaX = _startSizePoint.Position.X - point.Position.X;
                var deltaY = _startSizePoint.Position.Y - point.Position.Y;
                double newWidth = Math.Max(0, (_targetControl.Width.Equals(double.NaN) ? _targetControl.DesiredSize.Width : _targetControl.Width) + deltaX);
                double newHeight = Math.Max(0, (_targetControl.Height.Equals(double.NaN) ? _targetControl.DesiredSize.Height : _targetControl.Height) + deltaY);
                _targetControl.Width = newWidth;
                _targetControl.Height = newHeight;
                Canvas.SetLeft(_targetControl, Canvas.GetLeft(_targetControl) - deltaX);
                Canvas.SetTop(_targetControl, Canvas.GetTop(_targetControl) - deltaY);
                _startSizePoint = point;
                Console.WriteLine($"Resizing TopLeft: New Width = {_targetControl.Width}, New Height = {_targetControl.Height}");
            }
        }

        private void LeftCenterAnchorOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isResizing)
            {
                var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
                var delta = _startSizePoint.Position.X - point.Position.X;
                double newWidth = Math.Max(0, (_targetControl.Width.Equals(double.NaN) ? _targetControl.DesiredSize.Width : _targetControl.Width) + delta);
                _targetControl.Width = newWidth;
                Canvas.SetLeft(_targetControl, Canvas.GetLeft(_targetControl) - delta);
                _startSizePoint = point;
                Console.WriteLine($"Resizing Left: New Width = {_targetControl.Width}");
            }
        }

        private void BottomLeftAnchorOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isResizing)
            {
                var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
                var deltaX = _startSizePoint.Position.X - point.Position.X;
                var deltaY = point.Position.Y - _startSizePoint.Position.Y;
                double newWidth = Math.Max(0, (_targetControl.Width.Equals(double.NaN) ? _targetControl.DesiredSize.Width : _targetControl.Width) + deltaX);
                double newHeight = Math.Max(0, (_targetControl.Height.Equals(double.NaN) ? _targetControl.DesiredSize.Height : _targetControl.Height) + deltaY);
                _targetControl.Width = newWidth;
                _targetControl.Height = newHeight;
                Canvas.SetLeft(_targetControl, Canvas.GetLeft(_targetControl) - deltaX);
                _startSizePoint = point;
                Console.WriteLine($"Resizing BottomLeft: New Width = {_targetControl.Width}, New Height = {_targetControl.Height}");
            }
        }

        private void TopRightAnchorOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isResizing)
            {
                var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
                var deltaX = point.Position.X - _startSizePoint.Position.X;
                var deltaY = _startSizePoint.Position.Y - point.Position.Y;
                double newWidth = Math.Max(0, (_targetControl.Width.Equals(double.NaN) ? _targetControl.DesiredSize.Width : _targetControl.Width) + deltaX);
                double newHeight = Math.Max(0, (_targetControl.Height.Equals(double.NaN) ? _targetControl.DesiredSize.Height : _targetControl.Height) + deltaY);
                _targetControl.Width = newWidth;
                _targetControl.Height = newHeight;
                Canvas.SetTop(_targetControl, Canvas.GetTop(_targetControl) - deltaY);
                _startSizePoint = point;
                Console.WriteLine($"Resizing TopRight: New Width = {_targetControl.Width}, New Height = {_targetControl.Height}");
            }
        }

        private void RightCenterAnchorOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isResizing)
            {
                var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
                var delta = point.Position.X - _startSizePoint.Position.X;
                double newWidth = Math.Max(0, (_targetControl.Width.Equals(double.NaN) ? _targetControl.DesiredSize.Width : _targetControl.Width) + delta);
                _targetControl.Width = newWidth;
                _startSizePoint = point;
                Console.WriteLine($"Resizing Right: New Width = {_targetControl.Width}");
            }
        }

        private void BottomRightAnchorOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isResizing)
            {
                var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
                var deltaX = point.Position.X - _startSizePoint.Position.X;
                var deltaY = point.Position.Y - _startSizePoint.Position.Y;
                double newWidth = Math.Max(0, (_targetControl.Width.Equals(double.NaN) ? _targetControl.DesiredSize.Width : _targetControl.Width) + deltaX);
                double newHeight = Math.Max(0, (_targetControl.Height.Equals(double.NaN) ? _targetControl.DesiredSize.Height : _targetControl.Height) + deltaY);
                _targetControl.Width = newWidth;
                _targetControl.Height = newHeight;
                _startSizePoint = point;
                Console.WriteLine($"Resizing BottomRight: New Width = {_targetControl.Width}, New Height = {_targetControl.Height}");
            }
        }

        private void TopCenterAnchorOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isResizing)
            {
                var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
                var delta = _startSizePoint.Position.Y - point.Position.Y;
                double newHeight = Math.Max(0, (_targetControl.Height.Equals(double.NaN) ? _targetControl.DesiredSize.Height : _targetControl.Height) + delta);
                _targetControl.Height = newHeight;
                Canvas.SetTop(_targetControl, Canvas.GetTop(_targetControl) - delta);
                _startSizePoint = point;
                Console.WriteLine($"Resizing Top: New Height = {_targetControl.Height}");
            }
        }

        private void BottomCenterAnchorOnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isResizing)
            {
                var point = e.GetCurrentPoint((Visual?)_targetControl.Parent);
                var delta = point.Position.Y - _startSizePoint.Position.Y;
                double newHeight = Math.Max(0, (_targetControl.Height.Equals(double.NaN) ? _targetControl.DesiredSize.Height : _targetControl.Height) + delta);
                _targetControl.Height = newHeight;
                _startSizePoint = point;
                Console.WriteLine($"Resizing Bottom: New Height = {_targetControl.Height}");
            }
        }

        private void AnchorOnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            _isResizing = false;
        }

        private void TargetControlOnLayoutUpdated(object? sender, EventArgs e)
        {
            // Update decorator size and position to match the target control
            Width = _targetControl.Bounds.Width;
            Height = _targetControl.Bounds.Height;

            var relativePositionToParent = _targetControl.TranslatePoint(new Point(0, 0), _layer);
            if (relativePositionToParent.HasValue)
            {
                Canvas.SetLeft(this, relativePositionToParent.Value.X);
                Canvas.SetTop(this, relativePositionToParent.Value.Y);
            }
        }
    }
}
