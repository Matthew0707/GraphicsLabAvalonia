

using System;
using Avalonia;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

public class EllipseFactory : IShapeFactory
{
    public Shape CreateShape(params int[] parameters)
    {
        if (parameters.Length < RequiredParametersCount)
            throw new ArgumentException($"Ellipse requires {RequiredParametersCount} parameters: x, y, width, height");
        
        return new Ellipse(parameters[0], parameters[1], parameters[2], parameters[3]);
    }

    public int[] CalculateParameters(Point start, Point end)
    {
        int x = (int)Math.Min(start.X, end.X);
        int y = (int)Math.Min(start.Y, end.Y);
        int width = (int)Math.Abs(end.X - start.X);
        int height = (int)Math.Abs(end.Y - start.Y);
        
        return new[] { x, y, width, height };
    }

    public int RequiredParametersCount => 4;
    public string ShapeTypeName => "Ellipse";
}