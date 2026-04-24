// GraphicsLabAvalonia/Factories/ShapeFactoryManager.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

/// <summary>
/// Manages all shape factories with automatic discovery
/// </summary>
public class ShapeFactoryManager
{
    private readonly Dictionary<string, IShapeFactory> _factories;
    
    public ShapeFactoryManager()
    {
        _factories = new Dictionary<string, IShapeFactory>();
        
        // Automatically discover and register all factories
        AutoDiscoverFactories();
    }
    
    /// <summary>
    /// Automatically finds all IShapeFactory implementations in the assembly
    /// No manual registration needed when adding new shapes!
    /// </summary>
    private void AutoDiscoverFactories()
    {
        // Get the current assembly
        var assembly = Assembly.GetExecutingAssembly();
        
        // Find all types that implement IShapeFactory and are not abstract
        var factoryTypes = assembly.GetTypes()
            .Where(t => typeof(IShapeFactory).IsAssignableFrom(t) 
                        && !t.IsInterface 
                        && !t.IsAbstract);
        
        // Create instance of each factory and register it
        foreach (var type in factoryTypes)
        {
            try
            {
                var factory = (IShapeFactory)Activator.CreateInstance(type);
                RegisterFactory(factory);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Failed to load factory {type.Name}: {ex.Message}");
            }
        }
    }
    
    /// <summary>
    /// Register a factory (kept for manual registration if needed)
    /// </summary>
    public void RegisterFactory(IShapeFactory factory)
    {
        _factories[factory.ShapeTypeName] = factory;
    }
    
    public IEnumerable<string> GetAvailableShapeTypes()
    {
        return _factories.Keys;
    }
    
    public IShapeFactory GetFactory(string shapeType)
    {
        if (_factories.TryGetValue(shapeType, out var factory))
            return factory;
        
        throw new ArgumentException($"No factory registered for shape type: {shapeType}");
    }
    
    public Shape CreateShape(string shapeType, params int[] parameters)
    {
        var factory = GetFactory(shapeType);
        return factory.CreateShape(parameters);
    }
}