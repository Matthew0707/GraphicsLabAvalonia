using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphicsLabAvalonia.Models;

// Container for shapes with editing support
public class ShapeList
{
    private List<Shape> _shapes = new List<Shape>();

    public void Add(Shape shape)
    {
        _shapes.Add(shape);
    }

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < _shapes.Count)
            _shapes.RemoveAt(index);
    }
    
    public void Remove(Shape shape)
    {
        _shapes.Remove(shape);
    }

    public void Clear()
    {
        _shapes.Clear();
    }
    
    public Shape GetAt(int index)
    {
        return (index >= 0 && index < _shapes.Count) ? _shapes[index] : null;
    }

    public IEnumerable<Shape> GetAllShapes()
    {
        return _shapes;
    }

    public IEnumerable<string> GetAllDescriptions()
    {
        return _shapes.Select(s => s.GetDescription());
    }
    
    public int Count => _shapes.Count;
    
    // Find shape by ID
    public Shape FindById(Guid id)
    {
        return _shapes.FirstOrDefault(s => s.Id == id);
    }
    
    
    
    public Shape FindAtPoint(int x, int y)
    {
        for (int i = _shapes.Count - 1; i >= 0; i--)
        {
            if (_shapes[i].Contains(x, y)) 
                return _shapes[i];
        }
        return null;
    }
    
    
}