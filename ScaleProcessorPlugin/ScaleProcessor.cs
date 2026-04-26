
using System;
using System.Collections.Generic;
using GraphicsLabAvalonia.Models;
using GraphicsLabAvalonia.Plugins;

namespace ScaleProcessorPlugin;

public class ScaleProcessor : IDataProcessor
{
    private const int CanvasWidth = 600;
    private const int CanvasHeight = 600;
    
    public string ProcessorName => "Случайные позиции";
    public string Description => "Разбрасывает фигуры случайно при сохранении и загрузке.";

    public List<Shape> ProcessBeforeSave(List<Shape> shapes)
    {
        return RandomizePositions(shapes);
    }

    public List<Shape> ProcessAfterLoad(List<Shape> shapes)
    {
        return RandomizePositions(shapes);
    }

    private List<Shape> RandomizePositions(List<Shape> shapes)
    {
        var random = new Random();
        foreach (var shape in shapes)
        {
            shape.X = random.Next(0, CanvasWidth - 100);
            shape.Y = random.Next(0, CanvasHeight - 100);
        }
        return shapes;
    }
}