using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

// Square shape with equal sides
public class Square : Shape
{
    private int _width;

    // Side length
    public int Width
    {
        get { return _width; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Сторона должна быть положительной");
                
            _width = value;
        }
    }

    // Constructor initializes side length
    public Square(int x, int y, int side) : base(x, y)
    {
        Width = side;
    }

    // Draw filled square with border
    public override void Draw(DrawingContext context)
    {
        context.FillRectangle(Brushes.Red, new Rect(X, Y, Width, Width));
        
        var pen = new Pen(Brushes.Black, 2);

        context.DrawRectangle(pen, new Rect(X, Y, Width, Width));
    }

    // Returns description
    public override string GetDescription()
    {
        return $"Прямоугольник(X:{X}, Y:{Y}; сторона:{Width})";
    }
}