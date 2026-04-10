// GraphicsLabAvalonia/Factories/EllipseFactory.cs

using System;
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

    public int RequiredParametersCount => 4;
    public string ShapeTypeName => "Ellipse";
}