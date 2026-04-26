
using System.Collections.Generic;
using System.Linq;
using GraphicsLabAvalonia.Models;
using GraphicsLabAvalonia.Plugins;

namespace SortProcessorPlugin;

public class SortProcessor : IDataProcessor
{
    private const int HalfHeight = 300;
    
    public string ProcessorName => "Только верхняя половина";
    public string Description => "Оставляет только фигуры выше середины холста.";

    public List<Shape> ProcessBeforeSave(List<Shape> shapes)
    {
        return shapes.Where(s => s.Y < HalfHeight).ToList();
    }

    public List<Shape> ProcessAfterLoad(List<Shape> shapes)
    {
        return shapes.Where(s => s.Y < HalfHeight).ToList();
    }
}