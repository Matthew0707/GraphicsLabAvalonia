// GraphicsLabAvalonia/Models/ShapeList.cs

using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphicsLabAvalonia.Models;

// Container for shapes with editing support
public class ShapeList
{
    private List<Shape> _shapes = new List<Shape>();

    public void Add(Shape shape)
    {
        _shapes.Add(shape);
    }

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < _shapes.Count)
            _shapes.RemoveAt(index);
    }
    
    public void Remove(Shape shape)
    {
        _shapes.Remove(shape);
    }

    public void Clear()
    {
        _shapes.Clear();
    }
    
    public Shape GetAt(int index)
    {
        return (index >= 0 && index < _shapes.Count) ? _shapes[index] : null;
    }

    public IEnumerable<Shape> GetAllShapes()
    {
        return _shapes;
    }

    public IEnumerable<string> GetAllDescriptions()
    {
        return _shapes.Select(s => s.GetDescription());
    }
    
    public int Count => _shapes.Count;
    
    // Find shape by ID
    public Shape FindById(Guid id)
    {
        return _shapes.FirstOrDefault(s => s.Id == id);
    }
    
    // Find shape at point (for selection)
    public Shape FindAtPoint(int x, int y)
    {
        // Search in reverse order (top shapes first)
        for (int i = _shapes.Count - 1; i >= 0; i--)
        {
            if (IsPointInShape(_shapes[i], x, y))
                return _shapes[i];
        }
        return null;
    }
    
    private bool IsPointInShape(Shape shape, int x, int y)
    {
        // Simple bounding box check for all shapes
        return shape switch
        {
            Circle c => IsInCircle(c, x, y),
            Rectangle r => IsInRect(r, x, y),
            Square s => IsInRect(s.X, s.Y, s.Side, s.Side, x, y),
            Ellipse e => IsInEllipse(e, x, y),
            Line l => IsNearLine(l, x, y),
            Triangle t => IsInTriangle(t, x, y),
            _ => false
        };
    }
    
    private bool IsInCircle(Circle c, int x, int y)
    {
        double cx = c.X + c.Diameter / 2.0;
        double cy = c.Y + c.Diameter / 2.0;
        double r = c.Diameter / 2.0;
        return Math.Pow(x - cx, 2) + Math.Pow(y - cy, 2) <= Math.Pow(r, 2);
    }
    
    private bool IsInRect(Rectangle r, int x, int y)
        => IsInRect(r.X, r.Y, r.Width, r.Height, x, y);
    
    private bool IsInRect(int rx, int ry, int rw, int rh, int x, int y)
        => x >= rx && x <= rx + rw && y >= ry && y <= ry + rh;
    
    private bool IsInEllipse(Ellipse e, int x, int y)
    {
        double cx = e.X + e.Width / 2.0;
        double cy = e.Y + e.Height / 2.0;
        double a = e.Width / 2.0;
        double b = e.Height / 2.0;
        return Math.Pow(x - cx, 2) / (a * a) + Math.Pow(y - cy, 2) / (b * b) <= 1;
    }
    
    private bool IsNearLine(Line l, int x, int y, double threshold = 5.0)
    {
        double dist = DistanceToLine(x, y, l.X, l.Y, l.X2, l.Y2);
        return dist <= threshold;
    }
    
    private double DistanceToLine(int px, int py, int x1, int y1, int x2, int y2)
    {
        double dx = x2 - x1;
        double dy = y2 - y1;
        double lenSq = dx * dx + dy * dy;
        
        if (lenSq == 0) return Math.Sqrt(Math.Pow(px - x1, 2) + Math.Pow(py - y1, 2));
        
        double t = Math.Max(0, Math.Min(1, ((px - x1) * dx + (py - y1) * dy) / lenSq));
        double projX = x1 + t * dx;
        double projY = y1 + t * dy;
        
        return Math.Sqrt(Math.Pow(px - projX, 2) + Math.Pow(py - projY, 2));
    }
    
    private bool IsInTriangle(Triangle t, int x, int y)
    {
        var p1 = t.Point1;
        var p2 = t.Point2;
        var p3 = t.Point3;
        
        double d1 = Sign(x, y, p1.X, p1.Y, p2.X, p2.Y);
        double d2 = Sign(x, y, p2.X, p2.Y, p3.X, p3.Y);
        double d3 = Sign(x, y, p3.X, p3.Y, p1.X, p1.Y);
        
        bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);
        
        return !(hasNeg && hasPos);
    }
    
    private double Sign(double x1, double y1, double x2, double y2, double x3, double y3)
    {
        return (x1 - x3) * (y2 - y3) - (x2 - x3) * (y1 - y3);
    }
}