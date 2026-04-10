// GraphicsLabAvalonia/Rendering/SquareRenderer.cs
using Avalonia;
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

// Renderer for Square shapes
public class SquareRenderer : IShapeRenderer
{
    public bool CanRender(Shape shape)
    {
        return shape is Square;
    }

    public void Render(DrawingContext context, Shape shape)
    {
        var square = shape as Square;
        if (square == null) return;

        var rect = new Rect(square.X, square.Y, square.Side, square.Side);
        
        context.FillRectangle(Brushes.LightYellow, rect);
        
        var pen = new Pen(Brushes.Black, 2);
        context.DrawRectangle(pen, rect);
    }
}