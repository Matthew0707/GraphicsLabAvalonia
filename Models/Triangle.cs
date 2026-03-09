
using Avalonia.Media;
using Avalonia; 

namespace GraphicsLabAvalonia.Models;

public class Triangle : Shape
{
    public int A1 { get;  set; }
    public int A2 { get; set; }
    public int B1 { get; set; }
    public int B2 { get; set; }
    public int C1 { get; set; }
    public int C2 { get; set; }
    public Triangle(int x, int y, int a1, int a2, int b1, int b2, int c1, int c2) : base(x, y)
    {
        A1 = a1;
        A2 = a2;
            
        B1 = b1;
        B2 = b2;
        
        C1 = c1;
        C2 = c2;
    }

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
}