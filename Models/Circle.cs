
using System;
using System.Text.Json;
using GraphicsLabAvalonia.Serialization;

namespace GraphicsLabAvalonia.Models;
public class Circle : Shape
{
    public int Diameter { get; set; }

    public Circle(int x, int y, int diameter) : base(x, y)
    {
        Diameter = diameter;
    }
    
    public Circle() : base() { }

    public override void WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteString("Type", "Circle");
        writer.WriteString("Id", Id.ToString());
        writer.WriteNumber("X", X);
        writer.WriteNumber("Y", Y);
        writer.WriteNumber("Diameter", Diameter);
        writer.WriteString("FillColor", FillColor);
        writer.WriteString("StrokeColor", StrokeColor);
        writer.WriteNumber("StrokeThickness", StrokeThickness);
    }

    public override void ReadJson(JsonElement element)
    {
        Id = Guid.Parse(element.GetProperty("Id").GetString());
        X = element.GetProperty("X").GetInt32();
        Y = element.GetProperty("Y").GetInt32();
        Diameter = element.GetProperty("Diameter").GetInt32();
        FillColor = element.GetProperty("FillColor").GetString();
        StrokeColor = element.GetProperty("StrokeColor").GetString();
        StrokeThickness = element.GetProperty("StrokeThickness").GetDouble();
    }

    public override void Resize(int dx, int dy)
    {
        int delta = (Math.Abs(dx) + Math.Abs(dy)) / 2;
        int direction = (dx >= 0 && dy >= 0) ? 1 : -1;
        Diameter = Math.Max(10, Diameter + direction * delta);
    }

    public override string GetDescription() => $"Circle (X:{X}, Y:{Y}, D:{Diameter})";
    
    static Circle()
    {
        JsonShapeSerializer.RegisterType("Circle", () => new Circle());
    }
    public override bool Contains(int x, int y)
    {
        double cx = X + Diameter / 2.0;
        double cy = Y + Diameter / 2.0;
        double r = Diameter / 2.0;
        return Math.Pow(x - cx, 2) + Math.Pow(y - cy, 2) <= Math.Pow(r, 2);
    }
}