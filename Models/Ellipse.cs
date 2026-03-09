using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

public class Ellipse : Shape
{
    private int _width;
    private int _height;

    public int Width
    {
        get { return _width; }
        set
        {
            if (value > 0)
                _width = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be greater than zero");
        }
    }
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

    public Ellipse(int x, int y, int a, int b) : base(x, y)
    {
        Width = a; 
        Height = b;
    }

    public override void Draw(DrawingContext context)
    {
        Console.WriteLine($"Рисую: X={X}, Y={Y}, W={Width}, H={Height} эллипс");
        
        var pen = new Pen(Brushes.Red, 2);
        var rect = new Rect(X, Y, Width, Height);
        context.DrawEllipse(brush:Brushes.Black, pen, rect);
    }
}