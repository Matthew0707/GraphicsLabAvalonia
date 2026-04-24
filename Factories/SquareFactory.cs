// GraphicsLabAvalonia/Factories/SquareFactory.cs

using System;
using Avalonia;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

public class SquareFactory : IShapeFactory
{
    public Shape CreateShape(params int[] parameters)
    {
        if (parameters.Length < RequiredParametersCount)
            throw new ArgumentException($"Square requires {RequiredParametersCount} parameters: x, y, side");
        
        return new Square(parameters[0], parameters[1], parameters[2]);
    }

    public int[] CalculateParameters(Point start, Point end)
    {
        int x = (int)Math.Min(start.X, end.X);
        int y = (int)Math.Min(start.Y, end.Y);
        int width = (int)Math.Abs(end.X - start.X);
        int height = (int)Math.Abs(end.Y - start.Y);
        int side = Math.Min(width, height);
        
        return new[] { x, y, side };
    }

    public int RequiredParametersCount => 3;
    public string ShapeTypeName => "Square";
}