using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

// Ellipse inherits from Circle and adds a second axis (Height)
public class Ellipse : Circle
{
    private int _height;

    // Vertical size of ellipse
    public int Height
    {
        get { return _height; }
        set
        {
            // Ensure height is positive
            if (value > 0)
                _height = value;
            else
                throw new ArgumentOutOfRangeException(nameof(value), "Высота должна быть больше нуля");
        }
    }

    // Constructor sets horizontal size via base class and vertical size locally
    public Ellipse(int x, int y, int a, int b) : base(x, y, a)
    {
        Height = b;
    }

    // Draw ellipse using bounding rectangle
    public override void Draw(DrawingContext context)
    {
        var pen = new Pen(Brushes.Red, 2);

        // Width from Diameter, height from Height property
        var rect = new Rect(X, Y, Diameter, Height);

        context.DrawEllipse(brush: Brushes.Black, pen, rect);
    }

    // Returns text description
    public override string GetDescription()
    {
        return $"Эллипс (X:{X}, Y:{Y}; Ширина:{Diameter}, Высота{Height})";
    }
}