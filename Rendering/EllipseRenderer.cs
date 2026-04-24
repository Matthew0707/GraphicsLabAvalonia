
using Avalonia;
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

// Renderer for Ellipse shapes
public class EllipseRenderer : IShapeRenderer
{
    public bool CanRender(Shape shape)
    {
        return shape is Ellipse;
    }

    public void Render(DrawingContext context, Shape shape)
    {
        var ellipse = shape as Ellipse;
        if (ellipse == null) return;

        var pen = new Pen(Brushes.Black, 2);
        var rect = new Rect(ellipse.X, ellipse.Y, ellipse.Width, ellipse.Height);
        
        context.DrawEllipse(Brushes.LightPink, pen, rect);
    }
}