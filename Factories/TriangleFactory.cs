

using System;
using Avalonia;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Factories;

public class TriangleFactory : IShapeFactory
{
    public string ShapeTypeName => "Triangle";
    public int RequiredParametersCount => 6;

    public Shape CreateShape(params int[] p)
    {
        return new Triangle(p[0], p[1], p[2], p[3], p[4], p[5]);
    }

    public int[] CalculateParameters(Point start, Point end)
    {
        int x1 = (int)Math.Min(start.X, end.X);
        int y1 = (int)Math.Max(start.Y, end.Y);
        int x2 = (int)Math.Max(start.X, end.X);
        int y2 = (int)Math.Max(start.Y, end.Y);
        int x3 = (int)((start.X + end.X) / 2);
        int y3 = (int)Math.Min(start.Y, end.Y);
        return new[] { x1, y1, x2, y2, x3, y3 };
    }
}