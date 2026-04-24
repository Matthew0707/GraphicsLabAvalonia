using System;
using System.Text.Json;
using Avalonia;
using GraphicsLabAvalonia.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Triangle : Shape
{
    public double P1X { get; set; }
    public double P1Y { get; set; }
    public double P2X { get; set; }
    public double P2Y { get; set; }
    public double P3X { get; set; }
    public double P3Y { get; set; }

    public Point Point1 => new Point(P1X, P1Y);
    public Point Point2 => new Point(P2X, P2Y);
    public Point Point3 => new Point(P3X, P3Y);

    public Triangle(int x1, int y1, int x2, int y2, int x3, int y3) : base(0, 0)
    {
        P1X = x1; P1Y = y1;
        P2X = x2; P2Y = y2;
        P3X = x3; P3Y = y3;
    }
    
    public Triangle() : base() { }

    public override void WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteString("Type", "Triangle");
        writer.WriteString("Id", Id.ToString());
        writer.WriteNumber("P1X", P1X);
        writer.WriteNumber("P1Y", P1Y);
        writer.WriteNumber("P2X", P2X);
        writer.WriteNumber("P2Y", P2Y);
        writer.WriteNumber("P3X", P3X);
        writer.WriteNumber("P3Y", P3Y);
        writer.WriteString("FillColor", FillColor);
        writer.WriteString("StrokeColor", StrokeColor);
        writer.WriteNumber("StrokeThickness", StrokeThickness);
    }

    public override void ReadJson(JsonElement element)
    {
        Id = Guid.Parse(element.GetProperty("Id").GetString());
        P1X = element.GetProperty("P1X").GetDouble();
        P1Y = element.GetProperty("P1Y").GetDouble();
        P2X = element.GetProperty("P2X").GetDouble();
        P2Y = element.GetProperty("P2Y").GetDouble();
        P3X = element.GetProperty("P3X").GetDouble();
        P3Y = element.GetProperty("P3Y").GetDouble();
        FillColor = element.GetProperty("FillColor").GetString();
        StrokeColor = element.GetProperty("StrokeColor").GetString();
        StrokeThickness = element.GetProperty("StrokeThickness").GetDouble();
    }

    public override void Resize(int dx, int dy)
    {
        double cx = (P1X + P2X + P3X) / 3.0;
        double cy = (P1Y + P2Y + P3Y) / 3.0;
        
        double scaleX = 1.0 + dx / 100.0;
        double scaleY = 1.0 + dy / 100.0;
        
        scaleX = Math.Max(0.1, Math.Min(scaleX, 3.0));
        scaleY = Math.Max(0.1, Math.Min(scaleY, 3.0));
        
        P1X = cx + (P1X - cx) * scaleX;
        P1Y = cy + (P1Y - cy) * scaleY;
        P2X = cx + (P2X - cx) * scaleX;
        P2Y = cy + (P2Y - cy) * scaleY;
        P3X = cx + (P3X - cx) * scaleX;
        P3Y = cy + (P3Y - cy) * scaleY;
    }

    public override string GetDescription() => $"Triangle ({P1X:F0},{P1Y:F0}) ({P2X:F0},{P2Y:F0}) ({P3X:F0},{P3Y:F0})";
    
    static Triangle()
    {
        JsonShapeSerializer.RegisterType("Triangle", () => new Triangle());
    }
    
    public override bool Contains(int x, int y)
    {
        double d1 = Sign(x, y, P1X, P1Y, P2X, P2Y);
        double d2 = Sign(x, y, P2X, P2Y, P3X, P3Y);
        double d3 = Sign(x, y, P3X, P3Y, P1X, P1Y);
        bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);
        return !(hasNeg && hasPos);
    }

    private double Sign(double x1, double y1, double x2, double y2, double x3, double y3)
    {
        return (x1 - x3) * (y2 - y3) - (x2 - x3) * (y1 - y3);
    }
}