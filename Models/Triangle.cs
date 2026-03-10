using Avalonia.Media;
using Avalonia; 

namespace GraphicsLabAvalonia.Models;

// Triangle defined by three points
public class Triangle : Shape
{
    // Coordinates of triangle vertices
    public int A1 { get;  set; }
    public int A2 { get; set; }
    public int B1 { get; set; }
    public int B2 { get; set; }
    public int C1 { get; set; }
    public int C2 { get; set; }

    // Constructor initializes vertex coordinates
    public Triangle(int x, int y, int a1, int a2, int b1, int b2, int c1, int c2) : base(x, y)
    {
        A1 = a1;
        A2 = a2;
            
        B1 = b1;
        B2 = b2;
        
        C1 = c1;
        C2 = c2;
    }

    // Draw triangle using polygon geometry
    public override void Draw(DrawingContext context)
    {
        var points = new[]
        {
            new Point(A1, A2),
            new Point(B1, B2),
            new Point(C1, C2)
        };

        var polygon = new PolylineGeometry(points, true);

        var fillBrush = new SolidColorBrush(Colors.Black);
        var outPen = new Pen(Brushes.BlanchedAlmond, 2);

        context.DrawGeometry(fillBrush, outPen, polygon);
    }

    // Returns triangle description
    public override string GetDescription()
    {
        return $"Треугольник(X:{X}, Y:{Y}; A1:{A1}, A2:{A2}, B1:{B1}, B2:{B2}), C1:{C1}, C2:{C2} )";
    }
}