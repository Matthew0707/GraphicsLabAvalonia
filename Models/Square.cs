// GraphicsLabAvalonia/Models/Square.cs
namespace GraphicsLabAvalonia.Models;

// Square shape - only data, no drawing logic
public class Square : Shape
{
    public int Side { get; set; }

    public Square(int x, int y, int side) : base(x, y)
    {
        Side = side;
    }

    public override string GetDescription()
    {
        return $"Square (X:{X}, Y:{Y}, Side:{Side})";
    }
}