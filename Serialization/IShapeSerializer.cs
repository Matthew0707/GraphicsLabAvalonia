using System.Collections.Generic;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Serialization;

// Interface for shape serialization
public interface IShapeSerializer
{
    void Serialize(string filePath, IEnumerable<Shape> shapes);
    List<Shape> Deserialize(string filePath);
    string FileExtension { get; }
}