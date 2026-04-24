// GraphicsLabAvalonia/Models/Line.cs
using System.Text.Json.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Line : Shape
{
    public int X2 { get; set; }
    public int Y2 { get; set; }

    public Line(int x1, int y1, int x2, int y2) : base(x1, y1)
    {
        X2 = x2;
        Y2 = y2;
    }
    
    [JsonConstructor]
    public Line() : base() { }

    public override string GetDescription()
    {
        return $"Line ({X},{Y}) -> ({X2},{Y2})";
    }
}