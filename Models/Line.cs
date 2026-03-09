using System;
using Avalonia;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

public class Line : Shape
{
     
     public int X2 {get; set;}
     
     public int Y2 { get; set; }
     
     
     public Line(int x, int y, int x2, int y2) : base(x, y)
     {
          X2 = x2;
          Y2 = y2;
     }

     public override void Draw(DrawingContext context)
     {
          Console.WriteLine($"Рисую: X={X}, Y={Y}, X2={X2}, Y2={Y2}");
         
          var pen = new Pen(Brushes.Black, 4);
          var startPoin = new Point(X, Y);
          var endPoin = new Point(X2, Y2);
          context.DrawLine(pen, startPoin,  endPoin );
     }
     
     public override string GetDescription()
     {
          return $"Line({X},{Y} >> {X2},{Y2})";
     }
}