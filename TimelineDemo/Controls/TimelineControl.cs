using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace TimelineDemo.Controls;

public class TimelineControl : Control
{
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var rect = new Rect(new Point(), Bounds.Size);
        context.DrawRectangle(Brushes.LightGray, null, rect);
    }
}
