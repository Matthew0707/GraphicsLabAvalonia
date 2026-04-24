// GraphicsLabAvalonia/Models/Square.cs
using System.Text.Json.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Square : Shape
{
    public int Side { get; set; }

    public Square(int x, int y, int side) : base(x, y)
    {
        Side = side;
    }
    
    [JsonConstructor]
    public Square() : base() { }

    public override string GetDescription()
    {
        return $"Square (X:{X}, Y:{Y}, S:{Side})";
    }
}