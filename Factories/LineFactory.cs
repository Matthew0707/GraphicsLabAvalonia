// GraphicsLabAvalonia/Factories/LineFactory.cs

using System;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

/// <summary>
/// Factory for creating Line shapes
/// </summary>
public class LineFactory : IShapeFactory
{
    public Shape CreateShape(params int[] parameters)
    {
        if (parameters.Length < RequiredParametersCount)
            throw new ArgumentException($"Line requires {RequiredParametersCount} parameters: x1, y1, x2, y2");
        
        return new Line(parameters[0], parameters[1], parameters[2], parameters[3]);
    }

    public int RequiredParametersCount => 4;
    public string ShapeTypeName => "Line";
}