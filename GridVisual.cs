using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace DesignerPanel;

public class GridVisual : Control
{
    public override void Render(DrawingContext context)
    {
        var pen = new Pen(Brushes.Black, 2);
        var size = Bounds.Size;

        for (var x = 0; x < size.Width; x += 8)
        {
            context.DrawLine(pen, new Point(x, 0), new Point(x, size.Height));
        }

        for (var y = 0; y < size.Height; y += 8)
        {
            context.DrawLine(pen, new Point(0, y), new Point(size.Width, y));
        }
    }
}
