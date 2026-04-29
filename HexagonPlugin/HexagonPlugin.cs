using System;
using System.Text.Json;
using Avalonia;
using Avalonia.Media;
using GraphicsLabAvalonia.Factories;
using GraphicsLabAvalonia.Models;
using GraphicsLabAvalonia.Plugins;
using GraphicsLabAvalonia.Rendering;
using GraphicsLabAvalonia.Serialization;

namespace HexagonPlugin;

/// <summary>
/// Plugin that adds a regular hexagon shape.
/// Self-registers via IShapePlugin — base app code never changes.
/// </summary>
public class HexagonPlugin : IShapePlugin
{
    public string PluginName => "Hexagon Plugin";
    public string Version => "1.0.0";
    public IShapeFactory GetFactory() => new HexagonFactory();
    public IShapeRenderer GetRenderer() => new HexagonRenderer();
    public Func<Shape> GetShapeFactory() => () => new HexagonShape();
    public string GetShapeTypeName() => "Hexagon";
}


public class HexagonShape : Shape
{
    public int Radius { get; set; }

    public HexagonShape() : base() { }
    
    public HexagonShape(int x, int y, int radius) : base(x, y)
    {
        Radius = radius;
    }

    // Self-registration for JSON deserialization
    static HexagonShape()
    {
        JsonShapeSerializer.RegisterType("Hexagon", () => new HexagonShape());
    }

    public override void WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteString("Type", "Hexagon");
        writer.WriteString("Id", Id.ToString());
        writer.WriteNumber("X", X);
        writer.WriteNumber("Y", Y);
        writer.WriteNumber("Radius", Radius);
        writer.WriteString("FillColor", FillColor);
        writer.WriteString("StrokeColor", StrokeColor);
        writer.WriteNumber("StrokeThickness", StrokeThickness);
    }

    public override void ReadJson(JsonElement element)
    {
        Id = Guid.Parse(element.GetProperty("Id").GetString());
        X = element.GetProperty("X").GetInt32();
        Y = element.GetProperty("Y").GetInt32();
        Radius = element.GetProperty("Radius").GetInt32();
        FillColor = element.GetProperty("FillColor").GetString() ?? "LightBlue";
        StrokeColor = element.GetProperty("StrokeColor").GetString() ?? "Black";
        StrokeThickness = element.GetProperty("StrokeThickness").GetDouble();
    }

    public override bool Contains(int px, int py)
{
    // Center of hexagon
    double cx = X + Radius;
    double cy = Y + Radius;
    double dist = Math.Sqrt(Math.Pow(px - cx, 2) + Math.Pow(py - cy, 2));
    return dist <= Radius;
}

    public override void Resize(int dx, int dy)
    {
        int delta = Math.Max(Math.Abs(dx), Math.Abs(dy));
        int direction = (dx >= 0 && dy >= 0) ? 1 : -1;
        Radius = Math.Max(10, Radius + direction * delta);
    }

    public override string GetDescription() => $"Hexagon (X:{X}, Y:{Y}, R:{Radius})";
}


public class HexagonFactory : IShapeFactory
{
    public string ShapeTypeName => "Hexagon";
    public int RequiredParametersCount => 3;

    public Shape CreateShape(params int[] p)
    {
        return new HexagonShape(p[0], p[1], p[2]);
    }

    public int[] CalculateParameters(Point start, Point end)
{
    // Hexagon is drawn from center, but stored with top-left corner
    int x = (int)Math.Min(start.X, end.X);
    int y = (int)Math.Min(start.Y, end.Y);
    int width = (int)Math.Abs(end.X - start.X);
    int height = (int)Math.Abs(end.Y - start.Y);
    
    // Use the smaller dimension for radius, so hexagon fits in drag rectangle
    int radius = Math.Min(width, height) / 2;
    radius = Math.Max(10, radius);
    
    // Store top-left corner
    return new[] { x, y, radius };
}
}


public class HexagonRenderer : IShapeRenderer
{
    public bool CanRender(Shape s) => s is HexagonShape;

    public void Render(DrawingContext ctx, Shape s)
    {
        var h = s as HexagonShape;
        if (h == null) return;

        // Calculate 6 vertices of regular hexagon
        double cx = h.X + h.Radius;
        double cy = h.Y + h.Radius;
        var points = new Point[6];
        
        for (int i = 0; i < 6; i++)
        {
            double angle = Math.PI / 3 * i - Math.PI / 6; // -30 degrees offset
            points[i] = new Point(
                cx + h.Radius * Math.Cos(angle),
                cy + h.Radius * Math.Sin(angle));
        }

        var geometry = new PolylineGeometry(points, true);
        var fill = new SolidColorBrush(Colors.Orange);
        var pen = new Pen(Brushes.Black, 2);
        ctx.DrawGeometry(fill, pen, geometry);
    }
}