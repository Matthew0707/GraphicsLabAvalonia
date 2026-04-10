// GraphicsLabAvalonia/Models/ShapeList.cs
using System.Collections.Generic;

namespace GraphicsLabAvalonia.Models;

// Container class storing multiple shapes
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

    // New method to get all shapes for rendering
    public IEnumerable<Shape> GetAllShapes()
    {
        return _shapes;
    }

    public IEnumerable<string> GetAllDescriptions()
    {
        foreach (var shape in _shapes)
        {
            yield return shape.GetDescription();
        }
    }
}