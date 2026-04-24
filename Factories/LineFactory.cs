// GraphicsLabAvalonia/Factories/LineFactory.cs

using System;
using Avalonia;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

public class LineFactory : IShapeFactory
{
    public Shape CreateShape(params int[] parameters)
    {
        if (parameters.Length < RequiredParametersCount)
            throw new ArgumentException($"Line requires {RequiredParametersCount} parameters: x1, y1, x2, y2");
        
        return new Line(parameters[0], parameters[1], parameters[2], parameters[3]);
    }

    public int[] CalculateParameters(Point start, Point end)
    {
        return new[] { (int)start.X, (int)start.Y, (int)end.X, (int)end.Y };
    }

    public int RequiredParametersCount => 4;
    public string ShapeTypeName => "Line";
}