using System;
using GraphicsLabAvalonia.Factories;
using GraphicsLabAvalonia.Models;
using GraphicsLabAvalonia.Rendering;

namespace GraphicsLabAvalonia.Plugins;

/// <summary>
/// Contract that every plugin must implement.
/// Plugin provides: factory (creation), renderer (drawing), 
/// and shape factory function (deserialization).
/// </summary>
public interface IShapePlugin
{
    /// Display name of the plugin
    string PluginName { get; }
    
    /// Plugin version
    string Version { get; }
    
    /// Factory for creating shape from mouse input
    IShapeFactory GetFactory();
    
    /// Renderer for drawing the shape
    IShapeRenderer GetRenderer();
    
    /// Factory function for deserialization (creates empty shape)
    Func<Shape> GetShapeFactory();
    
    /// Type name used for JSON serialization 
    string GetShapeTypeName();
}