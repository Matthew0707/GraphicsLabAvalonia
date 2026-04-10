// GraphicsLabAvalonia/Factories/ShapeFactoryManager.cs

using System;
using System.Collections.Generic;
using System.Linq;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

/// <summary>
/// Manages all shape factories. This class allows dynamic registration
/// of new shape factories without modifying existing code.
/// Follows the Open/Closed Principle - open for extension, closed for modification.
/// </summary>
public class ShapeFactoryManager
{
    private readonly Dictionary<string, IShapeFactory> _factories;
    
    public ShapeFactoryManager()
    {
        _factories = new Dictionary<string, IShapeFactory>();
    }
    
    /// <summary>
    /// Registers a new shape factory
    /// </summary>
    /// <param name="factory">The factory to register</param>
    public void RegisterFactory(IShapeFactory factory)
    {
        _factories[factory.ShapeTypeName] = factory;
    }
    
    /// <summary>
    /// Gets all available shape type names
    /// </summary>
    public IEnumerable<string> GetAvailableShapeTypes()
    {
        return _factories.Keys;
    }
    
    /// <summary>
    /// Gets a specific factory by shape type name
    /// </summary>
    public IShapeFactory GetFactory(string shapeType)
    {
        if (_factories.TryGetValue(shapeType, out var factory))
            return factory;
        
        throw new ArgumentException($"No factory registered for shape type: {shapeType}");
    }
    
    /// <summary>
    /// Creates a shape using the appropriate factory
    /// </summary>
    public Shape CreateShape(string shapeType, params int[] parameters)
    {
        var factory = GetFactory(shapeType);
        return factory.CreateShape(parameters);
    }
}