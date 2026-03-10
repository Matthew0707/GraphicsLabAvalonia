using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

// Class representing a line between two points
public class Line : Shape
{
    // End point coordinates
    public int X2 {get; set;}
    public int Y2 { get; set; }
     
    // Constructor sets start point (base) and end point
    public Line(int x, int y, int x2, int y2) : base(x, y)
    {
        X2 = x2;
        Y2 = y2;
    }

    // Draw line using DrawingContext
    public override void Draw(DrawingContext context)
    {
        var pen = new Pen(Brushes.Black, 4);

        var startPoin = new Point(X, Y);
        var endPoin = new Point(X2, Y2);

        context.DrawLine(pen, startPoin,  endPoin );
    }

    // Returns text description
    public override string GetDescription()
    {
        return $"Линия({X},{Y} >> {X2},{Y2})";
    }
}