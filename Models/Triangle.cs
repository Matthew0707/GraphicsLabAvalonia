// GraphicsLabAvalonia/Models/Triangle.cs
using System.Text.Json.Serialization;
using Avalonia;

namespace GraphicsLabAvalonia.Models;

public class Triangle : Shape
{
    public double P1X { get; set; }
    public double P1Y { get; set; }
    public double P2X { get; set; }
    public double P2Y { get; set; }
    public double P3X { get; set; }
    public double P3Y { get; set; }

    public Point Point1 => new Point(P1X, P1Y);
    public Point Point2 => new Point(P2X, P2Y);
    public Point Point3 => new Point(P3X, P3Y);

    public Triangle(int x1, int y1, int x2, int y2, int x3, int y3) : base(0, 0)
    {
        P1X = x1; P1Y = y1;
        P2X = x2; P2Y = y2;
        P3X = x3; P3Y = y3;
    }
    
    [JsonConstructor]
    public Triangle() : base() { }

    public override string GetDescription()
    {
        return $"Triangle ({P1X:F0},{P1Y:F0}) ({P2X:F0},{P2Y:F0}) ({P3X:F0},{P3Y:F0})";
    }
}