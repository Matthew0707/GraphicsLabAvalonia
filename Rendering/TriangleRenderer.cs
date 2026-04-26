
using Avalonia;
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

public class TriangleRenderer : IShapeRenderer
{
    public bool CanRender(Shape s) => s is Triangle;

    public void Render(DrawingContext ctx, Shape s)
    {
        var t = s as Triangle;
        if (t == null) return;

        var points = new[]
        {
            new Point(t.X, t.Y),
            new Point(t.X2, t.Y2),
            new Point(t.X3, t.Y3)
        };

        var geometry = new PolylineGeometry(points, true);
        var fill = new SolidColorBrush(Colors.LightCoral);
        var pen = new Pen(Brushes.Black, 2);
        ctx.DrawGeometry(fill, pen, geometry);
    }
}