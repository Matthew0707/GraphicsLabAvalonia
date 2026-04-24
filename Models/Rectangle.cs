// GraphicsLabAvalonia/Models/Rectangle.cs
using System.Text.Json.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Rectangle : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle(int x, int y, int width, int height) : base(x, y)
    {
        Width = width;
        Height = height;
    }
    
    [JsonConstructor]
    public Rectangle() : base() { }

    public override string GetDescription()
    {
        return $"Rectangle (X:{X}, Y:{Y}, W:{Width}, H:{Height})";
    }
}