
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Commands;

public class DeleteCommand : IEditorCommand
{
    private readonly ShapeList _shapes;
    private Shape _selectedShape;
    public string Name => "Delete";
    
    public DeleteCommand(ShapeList shapes) { _shapes = shapes; }
    
    public void SetSelectedShape(Shape shape) { _selectedShape = shape; }
    
    public void Execute()
    {
        if (_selectedShape != null)
            _shapes.Remove(_selectedShape);
    }
}