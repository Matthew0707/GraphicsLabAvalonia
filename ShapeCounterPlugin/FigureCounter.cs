
using System.Collections.Generic;
using System.Linq;
using GraphicsLabAvalonia.Models;

namespace ShapeCounterPlugin;

/// <summary>
/// Counts shapes by type.
/// Uses different naming: FigureInfo, CalculateAmount, GetFigureCount
/// </summary>
public class FigureInfo
{
    public string FigureType { get; set; }
    public int Amount { get; set; }
}

public interface IFigureCalculator
{
    List<FigureInfo> CalculateAmount(List<Shape> figures);
    string GetStatisticsText(List<Shape> figures);
}

public class FigureCalculator : IFigureCalculator
{
    public List<FigureInfo> CalculateAmount(List<Shape> figures)
    {
        return figures
            .GroupBy(f => f.GetType().Name)
            .Select(g => new FigureInfo 
            { 
                FigureType = g.Key, 
                Amount = g.Count() 
            })
            .ToList();
    }

    public string GetStatisticsText(List<Shape> figures)
    {
        var stats = CalculateAmount(figures);
        int total = figures.Count;
        return $"Total figures: {total}\n" + 
               string.Join("\n", stats.Select(s => $"  {s.FigureType}: {s.Amount}"));
    }
}