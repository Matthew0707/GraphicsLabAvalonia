using System;

namespace GraphicsLabAvalonia.Models;

public class Square : Rectangle
{
    public int Size
    {
        get { return Width; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Side must be positive");
                
            Width = value;
            Height = value;  
        }
    }

    public Square(int x, int y, int side) : base(x, y,  side, side)
    {
        
    }
}