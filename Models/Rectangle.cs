using System;

using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

public class Rectangle : Shape
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

    
    public Rectangle(int x, int y, int width, int height) : base(x, y)
    {
        Width = width;
        Height = height;
    }

    public override void Draw(DrawingContext context)
    {
        Console.WriteLine($"Рисую: X={X}, Y={Y}, W={Width}, H={Height}");
        
        context.FillRectangle(Brushes.Red, new Rect(X, Y, Width, Height));
        
        var pen = new Pen(Brushes.Black, 2);
        context.DrawRectangle(pen, new Rect(X, Y, Width, Height));
    }

    public override string GetDescription()
    {
        return $"Прямоугольник(X:{X}, Y:{Y}; Ширина:{Width}, Высота{Height})";
    }
    
}

