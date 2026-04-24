// GraphicsLabAvalonia/Models/Shape.cs
using System;
using System.Text.Json.Serialization;

namespace GraphicsLabAvalonia.Models;

// Abstract base class for all shapes
public abstract class Shape
{
    // Unique identifier for serialization
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Position
    public int X { get; set; }
    public int Y { get; set; }
    
    // Colors for serialization
    public string FillColor { get; set; } = "LightBlue";
    public string StrokeColor { get; set; } = "Black";
    public double StrokeThickness { get; set; } = 2;

    protected Shape(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    // Parameterless constructor for deserialization
    protected Shape() { }

    public virtual string GetDescription()
    {
        return $"{GetType().Name}({X}, {Y})";
    }
}