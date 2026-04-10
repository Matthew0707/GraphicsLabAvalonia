// GraphicsLabAvalonia/Models/Line.cs
namespace GraphicsLabAvalonia.Models;

// Line shape - only data, no drawing logic
public class Line : Shape
{
    public int X2 { get; set; }
    public int Y2 { get; set; }

    public Line(int x1, int y1, int x2, int y2) : base(x1, y1)
    {
        X2 = x2;
        Y2 = y2;
    }

    public override string GetDescription()
    {
        return $"Line ({X},{Y}) -> ({X2},{Y2})";
    }
}