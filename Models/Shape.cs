using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

// Abstract base class for all shapes
public abstract class Shape
{
    // Position of shape
    public int X { get; set; }
    public int Y { get; set; }

    // Base constructor
    protected Shape(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Every shape must implement its own drawing logic
    public abstract void Draw(DrawingContext context);

    // Default textual description
    public virtual string GetDescription()
    {
        return $"{GetType().Name}({X}, {Y})";
    }
}