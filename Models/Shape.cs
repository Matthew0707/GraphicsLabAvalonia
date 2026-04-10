// GraphicsLabAvalonia/Models/Shape.cs
using Avalonia;

namespace GraphicsLabAvalonia.Models;

// Abstract base class for all shapes
// Does NOT contain drawing method - rendering is handled externally
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

    // Default textual description
    public virtual string GetDescription()
    {
        return $"{GetType().Name}({X}, {Y})";
    }
}