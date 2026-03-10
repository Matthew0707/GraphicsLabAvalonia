using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

// Class representing a circle shape
public class Circle : Shape
{
    // Diameter of the circle
    private int _diameter;

    public int Diameter
    {
        get { return _diameter; }
        set
        {
            // Validate that diameter is positive
            if (value <= 0)
                throw new ArgumentException("Диаметр должен быть больше нуля");
                
            _diameter = value;
        }
    }

    // Constructor initializes position and diameter
    public Circle(int x, int y, int d) : base(x, y)
    {
        Diameter = d;
    }

    // Draw circle using Avalonia DrawingContext
    public override void Draw(DrawingContext context)
    {
        var pen = new Pen(Brushes.Red, 2);

        // Rectangle that bounds the ellipse (circle)
        var rect = new Rect(X, Y, Diameter, Diameter);

        context.DrawEllipse(brush: Brushes.Black, pen, rect);
    }

    // Returns text description of the circle
    public override string GetDescription()
    {
        return $"Круг (X:{X}, Y:{Y}; Диаметр:{Diameter})";
    }
}