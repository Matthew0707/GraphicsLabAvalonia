// GraphicsLabAvalonia/Factories/TriangleFactory.cs

using System;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

/// <summary>
/// Factory for creating Triangle shapes
/// </summary>
public class TriangleFactory : IShapeFactory
{
    public Shape CreateShape(params int[] parameters)
    {
        if (parameters.Length < RequiredParametersCount)
            throw new ArgumentException($"Triangle requires {RequiredParametersCount} parameters: x1, y1, x2, y2, x3, y3");
        
        return new Triangle(
            parameters[0], parameters[1],  // Point 1
            parameters[2], parameters[3],  // Point 2
            parameters[4], parameters[5]   // Point 3
        );
    }

    public int RequiredParametersCount => 6;
    public string ShapeTypeName => "Triangle";
}