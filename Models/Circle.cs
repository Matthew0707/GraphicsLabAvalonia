// GraphicsLabAvalonia/Models/Circle.cs
namespace GraphicsLabAvalonia.Models;

// Circle shape - only data, no drawing logic
public class Circle : Shape
{
    public int Diameter { get; set; }

    public Circle(int x, int y, int diameter) : base(x, y)
    {
        Diameter = diameter;
    }

    public override string GetDescription()
    {
        return $"Circle (X:{X}, Y:{Y}, Diameter:{Diameter})";
    }
}