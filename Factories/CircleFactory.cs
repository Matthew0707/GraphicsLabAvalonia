
using System;
using Avalonia;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

public class CircleFactory : IShapeFactory
{
    public Shape CreateShape(params int[] parameters)
    {
        if (parameters.Length < RequiredParametersCount)
            throw new ArgumentException($"Circle requires {RequiredParametersCount} parameters: x, y, diameter");
        
        return new Circle(parameters[0], parameters[1], parameters[2]);
    }

    public int[] CalculateParameters(Point start, Point end)
    {
        int x = (int)Math.Min(start.X, end.X);
        int y = (int)Math.Min(start.Y, end.Y);
        int width = (int)Math.Abs(end.X - start.X);
        int height = (int)Math.Abs(end.Y - start.Y);
        int diameter = Math.Min(width, height);
        
        return new[] { x, y, diameter };
    }

    public int RequiredParametersCount => 3;
    public string ShapeTypeName => "Circle";
}