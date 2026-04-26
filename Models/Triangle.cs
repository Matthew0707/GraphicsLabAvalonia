
using System;
using System.Text.Json;
using Avalonia;
using GraphicsLabAvalonia.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Triangle : Shape
{
    public int X2 { get; set; }
    public int Y2 { get; set; }
    public int X3 { get; set; }
    public int Y3 { get; set; }

    public Triangle() : base() { }
    
    public Triangle(int x, int y, int x2, int y2, int x3, int y3) : base(x, y)
    {
        X2 = x2;
        Y2 = y2;
        X3 = x3;
        Y3 = y3;
    }
    
    static Triangle()
    {
        JsonShapeSerializer.RegisterType("Triangle", () => new Triangle());
    }

    public override void WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteString("Type", "Triangle");
        writer.WriteString("Id", Id.ToString());
        writer.WriteNumber("X", X);
        writer.WriteNumber("Y", Y);
        writer.WriteNumber("X2", X2);
        writer.WriteNumber("Y2", Y2);
        writer.WriteNumber("X3", X3);
        writer.WriteNumber("Y3", Y3);
        writer.WriteString("FillColor", FillColor);
        writer.WriteString("StrokeColor", StrokeColor);
        writer.WriteNumber("StrokeThickness", StrokeThickness);
    }

    public override void ReadJson(JsonElement element)
    {
        Id = Guid.Parse(element.GetProperty("Id").GetString()!);
        X = element.GetProperty("X").GetInt32();
        Y = element.GetProperty("Y").GetInt32();
        X2 = element.GetProperty("X2").GetInt32();
        Y2 = element.GetProperty("Y2").GetInt32();
        X3 = element.GetProperty("X3").GetInt32();
        Y3 = element.GetProperty("Y3").GetInt32();
        FillColor = element.GetProperty("FillColor").GetString() ?? "LightBlue";
        StrokeColor = element.GetProperty("StrokeColor").GetString() ?? "Black";
        StrokeThickness = element.GetProperty("StrokeThickness").GetDouble();
    }

    public override bool Contains(int px, int py)
    {
        double d1 = Sign(px, py, X, Y, X2, Y2);
        double d2 = Sign(px, py, X2, Y2, X3, Y3);
        double d3 = Sign(px, py, X3, Y3, X, Y);
        bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);
        return !(hasNeg && hasPos);
    }

    private double Sign(double x1, double y1, double x2, double y2, double x3, double y3)
    {
        return (x1 - x3) * (y2 - y3) - (x2 - x3) * (y1 - y3);
    }

    public override void Resize(int dx, int dy)
    {
        X2 += dx;
        Y2 += dy;
        X3 += dx;
        Y3 += dy;
    }

    public override string GetDescription() => $"Triangle ({X},{Y})-({X2},{Y2})-({X3},{Y3})";
}