// GraphicsLabAvalonia/Models/Line.cs
using System;
using System.Text.Json;
using GraphicsLabAvalonia.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Line : Shape
{
    public int X2 { get; set; }
    public int Y2 { get; set; }

    public Line() : base() { }
    
    public Line(int x, int y, int x2, int y2) : base(x, y)
    {
        X2 = x2;
        Y2 = y2;
    }
    
    static Line()
    {
        JsonShapeSerializer.RegisterType("Line", () => new Line());
    }

    public override void WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteString("Type", "Line");
        writer.WriteString("Id", Id.ToString());
        writer.WriteNumber("X", X);
        writer.WriteNumber("Y", Y);
        writer.WriteNumber("X2", X2);
        writer.WriteNumber("Y2", Y2);
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
        FillColor = element.GetProperty("FillColor").GetString() ?? "LightBlue";
        StrokeColor = element.GetProperty("StrokeColor").GetString() ?? "Black";
        StrokeThickness = element.GetProperty("StrokeThickness").GetDouble();
    }

    public override bool Contains(int px, int py)
    {
        double dx = X2 - X;
        double dy = Y2 - Y;
        double lenSq = dx * dx + dy * dy;
        if (lenSq == 0) return Math.Sqrt(Math.Pow(px - X, 2) + Math.Pow(py - Y, 2)) <= 5;
        double t = Math.Max(0, Math.Min(1, ((px - X) * dx + (py - Y) * dy) / lenSq));
        double projX = X + t * dx;
        double projY = Y + t * dy;
        return Math.Sqrt(Math.Pow(px - projX, 2) + Math.Pow(py - projY, 2)) <= 5;
    }

    public override void Resize(int dx, int dy)
    {
        X2 += dx;
        Y2 += dy;
    }

    public override string GetDescription() => $"Line ({X},{Y}) -> ({X2},{Y2})";
}