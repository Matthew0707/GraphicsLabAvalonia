
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GraphicsLabAvalonia.Models;

public abstract class Shape
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int X { get; set; }
    public int Y { get; set; }
    public string FillColor { get; set; } = "LightBlue";
    public string StrokeColor { get; set; } = "Black";
    public double StrokeThickness { get; set; } = 2;

    protected Shape(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    protected Shape() { }

    // Each shape writes its own data to JSON
    public abstract void WriteJson(Utf8JsonWriter writer);
    
    // Each shape reads its own data from JSON
    public abstract void ReadJson(JsonElement element);

    public abstract void Resize(int dx, int dy);
    
    public virtual string GetDescription()
    {
        return $"{GetType().Name}({X}, {Y})";
    }
    public abstract bool Contains(int x, int y);
}