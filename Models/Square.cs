using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

public class Square : Shape
{
    private int _width;
    
    public int Width
    {
        get { return _width; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Side must be positive");
                
            _width = value;
            
        }
    }

    public Square(int x, int y, int side) : base(x, y)
    {
        Width = side;
    }
    
    public override void Draw(DrawingContext context)
    {
        Console.WriteLine($"Рисую: X={X}, Y={Y}, W={Width}");
        
        context.FillRectangle(Brushes.Red, new Rect(X, Y, Width, Width));
        
        var pen = new Pen(Brushes.Black, 2);
        context.DrawRectangle(pen, new Rect(X, Y, Width, Width));
    }
}