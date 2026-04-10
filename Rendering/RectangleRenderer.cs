// GraphicsLabAvalonia/Rendering/RectangleRenderer.cs
using Avalonia;
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

// Renderer for Rectangle shapes
public class RectangleRenderer : IShapeRenderer
{
    public bool CanRender(Shape shape)
    {
        return shape is Rectangle;
    }

    public void Render(DrawingContext context, Shape shape)
    {
        var rect = shape as Rectangle;
        if (rect == null) return;

        var rectangle = new Rect(rect.X, rect.Y, rect.Width, rect.Height);
        
        context.FillRectangle(Brushes.LightGreen, rectangle);
        
        var pen = new Pen(Brushes.Black, 2);
        context.DrawRectangle(pen, rectangle);
    }
}