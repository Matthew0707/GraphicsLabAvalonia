using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

public interface IShapeFactory
{
    Shape CreateShape(params int[] parameters);
    int RequiredParametersCount { get; }
    string ShapeTypeName { get; }
}