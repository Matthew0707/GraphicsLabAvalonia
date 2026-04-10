// GraphicsLabAvalonia/Models/Triangle.cs
using Avalonia;

namespace GraphicsLabAvalonia.Models;

// Triangle shape - only data, no drawing logic
public class Triangle : Shape
{
    public Point Point1 { get; set; }
    public Point Point2 { get; set; }
    public Point Point3 { get; set; }

    public Triangle(int x1, int y1, int x2, int y2, int x3, int y3) : base(0, 0)
    {
        Point1 = new Point(x1, y1);
        Point2 = new Point(x2, y2);
        Point3 = new Point(x3, y3);
    }

    public override string GetDescription()
    {
        return $"Triangle: ({Point1.X:F0},{Point1.Y:F0}) - ({Point2.X:F0},{Point2.Y:F0}) - ({Point3.X:F0},{Point3.Y:F0})";
    }
}