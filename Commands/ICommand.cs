
namespace GraphicsLabAvalonia.Commands;

public interface IEditorCommand
{
    string Name { get; }
    void Execute();
}