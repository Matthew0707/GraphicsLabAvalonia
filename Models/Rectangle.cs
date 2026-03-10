using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

// Rectangle inherits from Square but allows different height
public class Rectangle : Square
{
    private int _height;

    // Height property with validation
    public int Height
    {
        get { return _height; }
        set
        {
            if (value > 0)
                _height = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value), "Высота должна быть больше нуля");
        }
    }

    // Constructor sets width through base class and height locally
    public Rectangle(int x, int y, int width, int height) : base(x, y, width)
    {
        Width = width;
        Height = height;
    }

    // Draw filled rectangle with border
    public override void Draw(DrawingContext context)
    {
        context.FillRectangle(Brushes.Red, new Rect(X, Y, Width, Height));

        var pen = new Pen(Brushes.Black, 2);

        context.DrawRectangle(pen, new Rect(X, Y, Width, Height));
    }

    // Returns description
    public override string GetDescription()
    {
        return $"Прямоугольник(X:{X}, Y:{Y}; Ширина:{Width}, Высота{Height})";
    }
}