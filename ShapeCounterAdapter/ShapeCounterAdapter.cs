
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GraphicsLabAvalonia.Models;
using GraphicsLabAvalonia.Plugins;

namespace ShapeCounterAdapter;

public class ShapeCounterAdapter : IDataProcessor
{
    private readonly object _calculator;
    private readonly MethodInfo _getStatsMethod;
    
    public string ProcessorName => "Счётчик фигур (адаптирован)";
    public string Description => "Считает количество фигур каждого типа.";
    
    // Store last result for display
    public string LastResult { get; private set; }

    public ShapeCounterAdapter()
    {
        var pluginsPath = Path.Combine(
            System.AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        var assembly = Assembly.LoadFrom(
            Path.Combine(pluginsPath, "ShapeCounterPlugin.dll"));
        
        var calcType = assembly.GetType("ShapeCounterPlugin.FigureCalculator");
        _calculator = System.Activator.CreateInstance(calcType);
        _getStatsMethod = calcType.GetMethod("GetStatisticsText");
    }

    public List<Shape> ProcessBeforeSave(List<Shape> shapes)
    {
        LastResult = _getStatsMethod.Invoke(_calculator, new object[] { shapes }) as string;
        return shapes;
    }

    public List<Shape> ProcessAfterLoad(List<Shape> shapes)
    {
        LastResult = _getStatsMethod.Invoke(_calculator, new object[] { shapes }) as string;
        return shapes;
    }
}