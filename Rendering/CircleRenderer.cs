
using Avalonia;
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

// Renderer for Circle shapes
public class CircleRenderer : IShapeRenderer
{
    public bool CanRender(Shape shape)
    {
        return shape is Circle;
    }

    public void Render(DrawingContext context, Shape shape)
    {
        var circle = shape as Circle;
        if (circle == null) return;

        var pen = new Pen(Brushes.Black, 2);
        var rect = new Rect(circle.X, circle.Y, circle.Diameter, circle.Diameter);
        
        context.DrawEllipse(Brushes.LightBlue, pen, rect);
    }
}