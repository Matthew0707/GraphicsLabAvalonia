using Avalonia.Media;
using Avalonia;

namespace GraphicsLabAvalonia.Models;

public abstract class Shape
{
    public int X { get; set; }
    public int Y { get; set; }
    
    protected Shape(int x, int y)
    {
        X = x;
        Y = y;
    }

    public abstract void Draw(DrawingContext context);
   
    public virtual string GetDescription()
    {
        return $"{GetType().Name}({X}, {Y})";
    }
    
    
}