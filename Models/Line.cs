
using System;
using System.Text.Json;
using GraphicsLabAvalonia.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Line : Shape
{
    public int X2 { get; set; }
    public int Y2 { get; set; }

    public Line(int x1, int y1, int x2, int y2) : base(x1, y1)
    {
        X2 = x2;
        Y2 = y2;
    }
    
    public Line() : base() { }

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
        Id = Guid.Parse(element.GetProperty("Id").GetString());
        X = element.GetProperty("X").GetInt32();
        Y = element.GetProperty("Y").GetInt32();
        X2 = element.GetProperty("X2").GetInt32();
        Y2 = element.GetProperty("Y2").GetInt32();
        FillColor = element.GetProperty("FillColor").GetString();
        StrokeColor = element.GetProperty("StrokeColor").GetString();
        StrokeThickness = element.GetProperty("StrokeThickness").GetDouble();
    }

    public override void Resize(int dx, int dy)
    {
        X2 += dx;
        Y2 += dy;
    }

    public override string GetDescription() => $"Line ({X},{Y}) -> ({X2},{Y2})";
    
    static Line()
    {
        JsonShapeSerializer.RegisterType("Line", () => new Line());
    }
    
    public override bool Contains(int x, int y)
    {
        double dist = DistanceToLine(x, y, X, Y, X2, Y2);
        return dist <= 5.0;
    }

    private double DistanceToLine(int px, int py, int x1, int y1, int x2, int y2)
    {
        double dx = x2 - x1;
        double dy = y2 - y1;
        double lenSq = dx * dx + dy * dy;
        if (lenSq == 0) return Math.Sqrt(Math.Pow(px - x1, 2) + Math.Pow(py - y1, 2));
        double t = Math.Max(0, Math.Min(1, ((px - x1) * dx + (py - y1) * dy) / lenSq));
        double projX = x1 + t * dx;
        double projY = y1 + t * dy;
        return Math.Sqrt(Math.Pow(px - projX, 2) + Math.Pow(py - projY, 2));
    }
}