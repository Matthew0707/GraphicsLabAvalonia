// GraphicsLabAvalonia/Rendering/RenderManager.cs
using System.Collections.Generic;
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

// Manages rendering of shapes using registered renderers
public class RenderManager
{
    private readonly List<IShapeRenderer> _renderers;

    public RenderManager()
    {
        _renderers = new List<IShapeRenderer>();
        
        // Register all renderers
        RegisterRenderer(new CircleRenderer());
        RegisterRenderer(new RectangleRenderer());
        RegisterRenderer(new LineRenderer());
        RegisterRenderer(new SquareRenderer());
        RegisterRenderer(new EllipseRenderer());
        RegisterRenderer(new TriangleRenderer());
    }

    public void RegisterRenderer(IShapeRenderer renderer)
    {
        _renderers.Add(renderer);
    }

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
    }

    public void RenderShapes(DrawingContext context, IEnumerable<Shape> shapes)
    {
        foreach (var shape in shapes)
        {
            RenderShape(context, shape);
        }
    }
}