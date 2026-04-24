// GraphicsLabAvalonia/Models/Circle.cs
using System.Text.Json.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Circle : Shape
{
    public int Diameter { get; set; }

    public Circle(int x, int y, int diameter) : base(x, y)
    {
        Diameter = diameter;
    }
    
    // Parameterless constructor for JSON deserialization
    [JsonConstructor]
    public Circle() : base() { }

    public override string GetDescription()
    {
        return $"Circle (X:{X}, Y:{Y}, D:{Diameter})";
    }
}