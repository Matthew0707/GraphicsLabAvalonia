using System;
using System.Text.Json;
using Avalonia;
using Avalonia.Media;
using GraphicsLabAvalonia.Factories;
using GraphicsLabAvalonia.Models;
using GraphicsLabAvalonia.Plugins;
using GraphicsLabAvalonia.Rendering;
using GraphicsLabAvalonia.Serialization;

namespace StarPlugin;

/// <summary>
/// Plugin that adds a 5-pointed star shape.
/// </summary>
public class StarPlugin : IShapePlugin
{
    public string PluginName => "Star Plugin";
    public string Version => "1.0.0";
    public IShapeFactory GetFactory() => new StarFactory();
    public IShapeRenderer GetRenderer() => new StarRenderer();
    public Func<Shape> GetShapeFactory() => () => new StarShape();
    public string GetShapeTypeName() => "Star";
}


public class StarShape : Shape
{
    public int OuterRadius { get; set; }
    public int InnerRadius { get; set; }

    public StarShape() : base() { }
    
    public StarShape(int x, int y, int outer, int inner) : base(x, y)
    {
        OuterRadius = outer;
        InnerRadius = inner;
    }

    static StarShape()
    {
        JsonShapeSerializer.RegisterType("Star", () => new StarShape());
    }

    public override void WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteString("Type", "Star");
        writer.WriteString("Id", Id.ToString());
        writer.WriteNumber("X", X);
        writer.WriteNumber("Y", Y);
        writer.WriteNumber("OuterRadius", OuterRadius);
        writer.WriteNumber("InnerRadius", InnerRadius);
        writer.WriteString("FillColor", FillColor);
        writer.WriteString("StrokeColor", StrokeColor);
        writer.WriteNumber("StrokeThickness", StrokeThickness);
    }

    public override void ReadJson(JsonElement element)
    {
        Id = Guid.Parse(element.GetProperty("Id").GetString()!);
        X = element.GetProperty("X").GetInt32();
        Y = element.GetProperty("Y").GetInt32();
        OuterRadius = element.GetProperty("OuterRadius").GetInt32();
        InnerRadius = element.GetProperty("InnerRadius").GetInt32();
        FillColor = element.GetProperty("FillColor").GetString() ?? "LightBlue";
        StrokeColor = element.GetProperty("StrokeColor").GetString() ?? "Black";
        StrokeThickness = element.GetProperty("StrokeThickness").GetDouble();
    }

    public override bool Contains(int px, int py)
{
    double cx = X + OuterRadius;
    double cy = Y + OuterRadius;
    double dist = Math.Sqrt(Math.Pow(px - cx, 2) + Math.Pow(py - cy, 2));
    return dist <= OuterRadius;
}

    public override void Resize(int dx, int dy)
    {
        int delta = Math.Max(Math.Abs(dx), Math.Abs(dy));
        int direction = (dx >= 0 && dy >= 0) ? 1 : -1;
        OuterRadius = Math.Max(15, OuterRadius + direction * delta);
        InnerRadius = Math.Max(5, OuterRadius / 2);
    }

    public override string GetDescription() => $"Star (X:{X}, Y:{Y}, R:{OuterRadius})";
}


public class StarFactory : IShapeFactory
{
    public string ShapeTypeName => "Star";
    public int RequiredParametersCount => 4;

    public Shape CreateShape(params int[] p)
    {
        return new StarShape(p[0], p[1], p[2], p[3]);
    }

    public int[] CalculateParameters(Point start, Point end)
{
    int x = (int)Math.Min(start.X, end.X);
    int y = (int)Math.Min(start.Y, end.Y);
    int width = (int)Math.Abs(end.X - start.X);
    int height = (int)Math.Abs(end.Y - start.Y);
    
    // Outer radius from drag size
    int outer = Math.Min(width, height) / 2;
    outer = Math.Max(15, outer);
    int inner = Math.Max(5, outer / 2);
    
    return new[] { x, y, outer, inner };
}
}


public class StarRenderer : IShapeRenderer
{
    public bool CanRender(Shape s) => s is StarShape;

    public void Render(DrawingContext ctx, Shape s)
    {
        var star = s as StarShape;
        if (star == null) return;

        double cx = star.X + star.OuterRadius;
        double cy = star.Y + star.OuterRadius;
        int points = 5;
        var vertices = new Point[points * 2];

        for (int i = 0; i < points * 2; i++)
        {
            double radius = (i % 2 == 0) ? star.OuterRadius : star.InnerRadius;
            double angle = Math.PI / points * i - Math.PI / 2;
            vertices[i] = new Point(
                cx + radius * Math.Cos(angle),
                cy + radius * Math.Sin(angle));
        }

        var geometry = new PolylineGeometry(vertices, true);
        var fill = new SolidColorBrush(Colors.Gold);
        var pen = new Pen(Brushes.Black, 2);
        ctx.DrawGeometry(fill, pen, geometry);
    }
}