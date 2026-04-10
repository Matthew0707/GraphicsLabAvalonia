// GraphicsLabAvalonia/Factories/RectangleFactory.cs

using System;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

/// <summary>
/// Factory for creating Rectangle shapes
/// </summary>
public class RectangleFactory : IShapeFactory
{
    public Shape CreateShape(params int[] parameters)
    {
        if (parameters.Length < RequiredParametersCount)
            
            throw new ArgumentException($"Rectangle requires {RequiredParametersCount} parameters: x, y, width, height");
        
        return new Rectangle(parameters[0], parameters[1], parameters[2], parameters[3]);
    }

    public int RequiredParametersCount => 4;
    public string ShapeTypeName => "Rectangle";
}