
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

/// <summary>
/// Manages rendering of shapes using automatically discovered renderers
/// </summary>
public class RenderManager
{
    private readonly List<IShapeRenderer> _renderers;

    public RenderManager()
    {
        _renderers = new List<IShapeRenderer>();
        
        // Automatically discover and register all renderers
        AutoDiscoverRenderers();
    }
    
    /// <summary>
    /// Automatically finds all IShapeRenderer implementations in the assembly
    /// and registers them. No manual registration needed when adding new shapes!
    /// </summary>
    private void AutoDiscoverRenderers()
    {
        // Get the current assembly
        var assembly = Assembly.GetExecutingAssembly();
        
        // Find all types that implement IShapeRenderer and are not abstract
        var rendererTypes = assembly.GetTypes()
            .Where(t => typeof(IShapeRenderer).IsAssignableFrom(t) 
                        && !t.IsInterface 
                        && !t.IsAbstract);
        
        // Create instance of each renderer and register it
        foreach (var type in rendererTypes)
        {
            try
            {
                // Create instance using parameterless constructor
                var renderer = (IShapeRenderer)Activator.CreateInstance(type);
                RegisterRenderer(renderer);
            }
            catch (Exception ex)
            {
                // Log error but continue loading other renderers
                System.Diagnostics.Debug.WriteLine(
                    $"Failed to load renderer {type.Name}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Register a renderer manually 
    /// </summary>
    public void RegisterRenderer(IShapeRenderer renderer)
    {
        _renderers.Add(renderer);
    }

    /// <summary>
    /// Render a single shape using appropriate renderer
    /// </summary>
    public void RenderShape(DrawingContext context, Shape shape)
    {
        foreach (var renderer in _renderers)
        {
            if (renderer.CanRender(shape))
            {
                renderer.Render(context, shape);
                return;
            }
        }
        
        // If no renderer found, log warning
        System.Diagnostics.Debug.WriteLine(
            $"Warning: No renderer found for shape type {shape.GetType().Name}");
    }

    /// <summary>
    /// Render multiple shapes
    /// </summary>
    public void RenderShapes(DrawingContext context, IEnumerable<Shape> shapes)
    {
        foreach (var shape in shapes)
        {
            RenderShape(context, shape);
        }
    }
}