using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

public class Ellipse : Circle
{
    
    private int _height;

    
    public int Height
    {
        get { return _height; }
        set
        {
            if (value > 0)
                _height = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value), "_height must be greater than zero");
        }
    }

    public Ellipse(int x, int y, int a, int b) : base(x, y, a)
    {
      
        Height = b;
    }

    public override void Draw(DrawingContext context)
    {
        Console.WriteLine($"Рисую: X={X}, Y={Y}, W={Radius}, H={Height} эллипс");
        
        var pen = new Pen(Brushes.Red, 2);
        var rect = new Rect(X, Y, Radius, Height);
        context.DrawEllipse(brush:Brushes.Black, pen, rect);
    }
}