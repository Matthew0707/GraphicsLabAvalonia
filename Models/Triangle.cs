using Avalonia.Media;
using Avalonia; 

namespace GraphicsLabAvalonia.Models;

// Triangle defined by three points
public class Triangle : Shape
{
    public Point[] Points;
    // Constructor initializes vertex coordinates
    public Triangle(int x, int y, int a1, int a2, int b1, int b2, int c1, int c2) : base(x, y)
    {
        Points = new[]
        {
            new Point(a1 + X, a2 + Y),
            new Point(b1 + X, b2 + Y),
            new Point(c1 + X, c2 + Y)
        };
    }

    // Draw triangle using polygon geometry
    public override void Draw(DrawingContext context)
    {
        var polygon = new PolylineGeometry(Points, true);

        var fillBrush = new SolidColorBrush(Colors.Black);
        var outPen = new Pen(Brushes.BlanchedAlmond, 2);

        context.DrawGeometry(fillBrush, outPen, polygon);
    }

    // Returns triangle description
    public override string GetDescription()
    {
        return $"Треугольник(X:{X}, Y:{Y}; A1:{Points[0]}, A2:{Points[1]}, B1:{Points[2]}, B2:{Points[3]}), C1:{Points[4]}, C2:{Points[5]} )";
    }
}