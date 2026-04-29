
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Commands;

public class ClearCommand : IEditorCommand
{
    private readonly ShapeList _shapes;
    public string Name => "Clear";
    
    public ClearCommand(ShapeList shapes) { _shapes = shapes; }
    
    public void Execute() { _shapes.Clear(); }
}