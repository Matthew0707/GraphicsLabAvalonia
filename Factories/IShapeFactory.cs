// GraphicsLabAvalonia/Factories/IShapeFactory.cs
using Avalonia;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

/// <summary>
/// Interface for shape factories with drag-and-drop parameter calculation
/// </summary>
public interface IShapeFactory
{
    /// <summary>
    /// Creates a shape with the specified parameters
    /// </summary>
    Shape CreateShape(params int[] parameters);
    
    /// <summary>
    /// Calculates parameters from drag start and end points
    /// </summary>
    int[] CalculateParameters(Point start, Point end);
    
    /// <summary>
    /// Gets the number of parameters required to create this shape
    /// </summary>
    int RequiredParametersCount { get; }
    
    /// <summary>
    /// Gets the display name of the shape type
    /// </summary>
    string ShapeTypeName { get; }
}