using System;
using System.Collections.Generic;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

public class ShapeList
{
    private List<Shape> _shapes;
    
    public ShapeList()
    {
        _shapes = new List<Shape>();
    }
    
    public void Add(Shape shape)
    {
        _shapes.Add(shape);
    }

    public void DrawAll(DrawingContext context)
    {
        foreach (var shape in _shapes)
        {
            shape.Draw(context);
        }
    }
}