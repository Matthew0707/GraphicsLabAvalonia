// GraphicsLabAvalonia/Models/Rectangle.cs
namespace GraphicsLabAvalonia.Models;

// Rectangle shape - only data, no drawing logic
public class Rectangle : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle(int x, int y, int width, int height) : base(x, y)
    {
        Width = width;
        Height = height;
    }

    public override string GetDescription()
    {
        return $"Rectangle (X:{X}, Y:{Y}, Width:{Width}, Height:{Height})";
    }
}