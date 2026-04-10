// GraphicsLabAvalonia/Models/Triangle.cs
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

/// <summary>
/// Triangle defined by three points
/// </summary>
public class Triangle : Shape
{
    public Point[] Points { get; set; }
    
    // Constructor for three points (no base offset)
    public Triangle(int x1, int y1, int x2, int y2, int x3, int y3) : base(0, 0)
    {
        Points = new[]
        {
            new Point(x1, y1),
            new Point(x2, y2),
            new Point(x3, y3)
        };
    }

    /// <summary>
    /// Draw triangle using polygon geometry
    /// </summary>
    public override void Draw(DrawingContext context)
    {
        var polygon = new PolylineGeometry(Points, true);
        
        var fillBrush = new SolidColorBrush(Colors.LightBlue);
        var outlinePen = new Pen(Brushes.Black, 2);
        
        context.DrawGeometry(fillBrush, outlinePen, polygon);
    }

    /// <summary>
    /// Returns triangle description
    /// </summary>
    public override string GetDescription()
    {
        return $"Triangle: ({Points[0].X:F0},{Points[0].Y:F0}) - " +
               $"({Points[1].X:F0},{Points[1].Y:F0}) - " +
               $"({Points[2].X:F0},{Points[2].Y:F0})";
    }
}