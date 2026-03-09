using System;
using System.Collections.Generic;
using Avalonia.Media;

namespace GraphicsLabAvalonia.Models;

public class ShapeList
{
    private List<Shape> _shapes;

    public int Count => _shapes.Count;
    
    public ShapeList()
    {
        _shapes = new List<Shape>();
    }
    
    public void Clear()
    {
        _shapes.Clear();
    }
    
    public void Add(Shape shape)
    {
        _shapes.Add(shape);
    }
    
    public void Remove(Shape shape)
    {
        _shapes.Remove(shape);
    }


    public void DrawAll(DrawingContext context)
    {
        foreach (var shape in _shapes)
        {
            shape.Draw(context);
        }
    }
    public void PrintAllDescriptions()
    {
        Console.WriteLine("\n=== Список фигур ===");
        foreach (var shape in _shapes)
        {
            Console.WriteLine(shape.GetDescription());
        }
        Console.WriteLine($"Всего фигур: {Count}\n");
    }
}