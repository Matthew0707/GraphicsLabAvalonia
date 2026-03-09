using System;

namespace GraphicsLabAvalonia.Models;

public class Circle : Ellipse
{
    public Circle(int x, int y, int r) : base (x, y, r, r)
    {
    }
    public int Radius
    {
        get { return Width; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Radius must be positive");
                
            Width = value;
            Height = value;  
        }
    }
}