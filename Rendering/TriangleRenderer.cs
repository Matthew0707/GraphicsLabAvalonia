
using Avalonia;
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

// Renderer for Triangle shapes
public class TriangleRenderer : IShapeRenderer
{
    public bool CanRender(Shape shape)
    {
        return shape is Triangle;
    }

    public void Render(DrawingContext context, Shape shape)
    {
        var triangle = shape as Triangle;
        if (triangle == null) return;

        var points = new[]
        {
            triangle.Point1,
            triangle.Point2,
            triangle.Point3
        };
        
        var polygon = new PolylineGeometry(points, true);
        
        var fillBrush = new SolidColorBrush(Colors.LightCoral);
        var outlinePen = new Pen(Brushes.Black, 2);
        
        context.DrawGeometry(fillBrush, outlinePen, polygon);
    }
}