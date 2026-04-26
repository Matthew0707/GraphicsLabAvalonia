
using System.Collections.Generic;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Plugins;

/// <summary>
/// Interface for data processing plugins.
/// Applied before saving and after loading shapes.
/// </summary>
public interface IDataProcessor
{
    /// Display name shown in settings menu
    string ProcessorName { get; }
    
    /// Short description of transformation
    string Description { get; }
    
    /// Transform shapes before writing to file
    List<Shape> ProcessBeforeSave(List<Shape> shapes);
    
    /// Transform shapes after reading from file
    List<Shape> ProcessAfterLoad(List<Shape> shapes);
}