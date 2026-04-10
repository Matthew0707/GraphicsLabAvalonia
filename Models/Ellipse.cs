// GraphicsLabAvalonia/Models/Ellipse.cs
namespace GraphicsLabAvalonia.Models;

// Ellipse shape - only data, no drawing logic
public class Ellipse : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Ellipse(int x, int y, int width, int height) : base(x, y)
    {
        Width = width;
        Height = height;
    }

    public override string GetDescription()
    {
        return $"Ellipse (X:{X}, Y:{Y}, Width:{Width}, Height:{Height})";
    }
}