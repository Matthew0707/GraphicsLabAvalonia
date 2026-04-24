
using System;
using System.Text.Json;
using GraphicsLabAvalonia.Serialization;

namespace GraphicsLabAvalonia.Models;

public class Square : Shape
{
    public int Side { get; set; }

    public Square(int x, int y, int side) : base(x, y)
    {
        Side = side;
    }
    
    public Square() : base() { }

    public override void WriteJson(Utf8JsonWriter writer)
    {
        writer.WriteString("Type", "Square");
        writer.WriteString("Id", Id.ToString());
        writer.WriteNumber("X", X);
        writer.WriteNumber("Y", Y);
        writer.WriteNumber("Side", Side);
        writer.WriteString("FillColor", FillColor);
        writer.WriteString("StrokeColor", StrokeColor);
        writer.WriteNumber("StrokeThickness", StrokeThickness);
    }

    public override void ReadJson(JsonElement element)
    {
        Id = Guid.Parse(element.GetProperty("Id").GetString());
        X = element.GetProperty("X").GetInt32();
        Y = element.GetProperty("Y").GetInt32();
        Side = element.GetProperty("Side").GetInt32();
        FillColor = element.GetProperty("FillColor").GetString();
        StrokeColor = element.GetProperty("StrokeColor").GetString();
        StrokeThickness = element.GetProperty("StrokeThickness").GetDouble();
    }

    public override void Resize(int dx, int dy)
    {
        int delta = Math.Max(Math.Abs(dx), Math.Abs(dy));
        int direction = (dx >= 0 && dy >= 0) ? 1 : -1;
        Side = Math.Max(10, Side + direction * delta);
    }

    public override string GetDescription() => $"Square (X:{X}, Y:{Y}, S:{Side})";
    
    static Square()
    {
        JsonShapeSerializer.RegisterType("Square", () => new Square());
    }
    
    public override bool Contains(int x, int y)
    {
        return x >= X && x <= X + Side && y >= Y && y <= Y + Side;
    }
}