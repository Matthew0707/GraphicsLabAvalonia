using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

public class Circle : Shape
{
    
    private int _radius;
    public int Radius
    {
        get { return _radius; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Radius must be positive");
                
            _radius = value;
          
        }
    }
    public Circle(int x, int y, int r) : base(x, y)
    {
        Radius = r;
    }
    
    public override void Draw(DrawingContext context)
    {
        Console.WriteLine($"Рисую: X={X}, Y={Y}, W={Radius} круг");
        
        var pen = new Pen(Brushes.Red, 2);
        var rect = new Rect(X, Y, Radius, Radius);
        context.DrawEllipse(brush:Brushes.Black, pen, rect);
    }
}