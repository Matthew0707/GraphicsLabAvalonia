// GraphicsLabAvalonia/Factories/SquareFactory.cs

using System;
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

    public int RequiredParametersCount => 3;
    public string ShapeTypeName => "Square";
}