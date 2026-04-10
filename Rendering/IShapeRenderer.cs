// GraphicsLabAvalonia/Rendering/IShapeRenderer.cs
using Avalonia.Media;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Rendering;

// Interface for shape renderers
public interface IShapeRenderer
{
    void Render(DrawingContext context, Shape shape);
    bool CanRender(Shape shape);
}