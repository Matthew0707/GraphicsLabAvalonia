using System;
using System.Collections.Generic;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

// Container class storing multiple shapes
public class ShapeList
{
    private List<Shape> _shapes;

    // Initialize empty list
    public ShapeList()
    {
        _shapes = new List<Shape>();
    }

    // Add shape to collection
    public void Add(Shape shape)
    {
        _shapes.Add(shape);
    }

    // Draw all shapes in the list
    public void DrawAll(DrawingContext context)
    {
        foreach (var shape in _shapes)
        {
            shape.Draw(context);
        }
    }
}