// GraphicsLabAvalonia/Models/ShapeList.cs (добавьте методы)
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

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < _shapes.Count)
            _shapes.RemoveAt(index);
    }

    public void Clear()
    {
        _shapes.Clear();
    }

    public IEnumerable<string> GetAllDescriptions()
    {
        foreach (var shape in _shapes)
        {
            yield return shape.GetDescription();
        }
    }

    public void DrawAll(DrawingContext context)
    {
        foreach (var shape in _shapes)
        {
            shape.Draw(context);
        }
    }
}