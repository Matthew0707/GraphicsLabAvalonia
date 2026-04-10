// GraphicsLabAvalonia/Factories/CircleFactory.cs

using System;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

/// <summary>
/// Factory for creating Circle shapes
/// </summary>
public class CircleFactory : IShapeFactory
{
    public Shape CreateShape(params int[] parameters)
    {
        if (parameters.Length < RequiredParametersCount)
            throw new ArgumentException($"Circle requires {RequiredParametersCount} parameters: x, y, diameter");
        
        return new Circle(parameters[0], parameters[1], parameters[2]);
    }

    public int RequiredParametersCount => 3;
    public string ShapeTypeName => "Circle";
}