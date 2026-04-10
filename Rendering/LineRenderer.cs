// GraphicsLabAvalonia/Rendering/LineRenderer.cs
using Avalonia;
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

// Renderer for Line shapes
public class LineRenderer : IShapeRenderer
{
    public bool CanRender(Shape shape)
    {
        return shape is Line;
    }

    public void Render(DrawingContext context, Shape shape)
    {
        var line = shape as Line;
        if (line == null) return;

        var pen = new Pen(Brushes.Black, 2);
        var startPoint = new Point(line.X, line.Y);
        var endPoint = new Point(line.X2, line.Y2);
        
        context.DrawLine(pen, startPoint, endPoint);
    }
}