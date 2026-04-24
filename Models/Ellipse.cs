
using System;
using System.Text.Json;
using GraphicsLabAvalonia.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Ellipse : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Ellipse(int x, int y, int width, int height) : base(x, y)
    {
        Width = width;
        Height = height;
    }
    
    public Ellipse() : base() { }

    public override void WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteString("Type", "Ellipse");
        writer.WriteString("Id", Id.ToString());
        writer.WriteNumber("X", X);
        writer.WriteNumber("Y", Y);
        writer.WriteNumber("Width", Width);
        writer.WriteNumber("Height", Height);
        writer.WriteString("FillColor", FillColor);
        writer.WriteString("StrokeColor", StrokeColor);
        writer.WriteNumber("StrokeThickness", StrokeThickness);
    }

    public override void ReadJson(JsonElement element)
    {
        Id = Guid.Parse(element.GetProperty("Id").GetString());
        X = element.GetProperty("X").GetInt32();
        Y = element.GetProperty("Y").GetInt32();
        Width = element.GetProperty("Width").GetInt32();
        Height = element.GetProperty("Height").GetInt32();
        FillColor = element.GetProperty("FillColor").GetString();
        StrokeColor = element.GetProperty("StrokeColor").GetString();
        StrokeThickness = element.GetProperty("StrokeThickness").GetDouble();
    }

    public override void Resize(int dx, int dy)
    {
        Width = Math.Max(10, Width + dx);
        Height = Math.Max(10, Height + dy);
    }

    public override string GetDescription() => $"Ellipse (X:{X}, Y:{Y}, W:{Width}, H:{Height})";
    
    static Ellipse()
    {
        JsonShapeSerializer.RegisterType("Ellipse", () => new Ellipse());
    }
    
    public override bool Contains(int x, int y)
    {
        double cx = X + Width / 2.0;
        double cy = Y + Height / 2.0;
        double a = Width / 2.0;
        double b = Height / 2.0;
        if (a == 0 || b == 0) return false;
        return Math.Pow(x - cx, 2) / (a * a) + Math.Pow(y - cy, 2) / (b * b) <= 1;
    }
}