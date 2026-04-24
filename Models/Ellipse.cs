// GraphicsLabAvalonia/Models/Ellipse.cs
using System.Text.Json.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Ellipse : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Ellipse(int x, int y, int width, int height) : base(x, y)
    {
        Width = width;
        Height = height;
    }
    
    [JsonConstructor]
    public Ellipse() : base() { }

    public override string GetDescription()
    {
        return $"Ellipse (X:{X}, Y:{Y}, W:{Width}, H:{Height})";
    }
}