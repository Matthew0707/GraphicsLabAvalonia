

using System;
using Avalonia;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

public class TriangleFactory : IShapeFactory
{
    public Shape CreateShape(params int[] parameters)
    {
        if (parameters.Length < RequiredParametersCount)
            throw new ArgumentException($"Triangle requires {RequiredParametersCount} parameters: x1, y1, x2, y2, x3, y3");
        
        return new Triangle(
            parameters[0], parameters[1],
            parameters[2], parameters[3],
            parameters[4], parameters[5]
        );
    }

    public int[] CalculateParameters(Point start, Point end)
    {
        int x1 = (int)Math.Min(start.X, end.X);
        int y1 = (int)Math.Min(start.Y, end.Y);
        int x2 = (int)Math.Max(start.X, end.X);
        int y2 = (int)Math.Max(start.Y, end.Y);
        int width = x2 - x1;
        
        return new[]
        {
            x1, y2,                 // Bottom-left
            x2, y2,                 // Bottom-right
            x1 + width / 2, y1      // Top-center
        };
    }

    public int RequiredParametersCount => 6;
    public string ShapeTypeName => "Triangle";
}