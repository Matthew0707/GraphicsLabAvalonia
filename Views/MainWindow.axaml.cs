using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Views;

// Main window of the application
public partial class MainWindow : Window
{
    // Collection that stores all shapes to be drawn
    private ShapeList _shapes;
    
    public MainWindow()
    {
        InitializeComponent();

        // Create shape container
        _shapes = new ShapeList();

        // Add different shapes to the list
        _shapes.Add(new Rectangle(50, 50, 50, 100));
        _shapes.Add(new Square(4, 200, 50));
        _shapes.Add(new Line(200, 200, 300, 120));
        _shapes.Add(new Ellipse(50, 50, 50, 100));
        _shapes.Add(new Circle(120, 50, 50));
        _shapes.Add(new Triangle(230,230, 0,0, 0, 10, 10, 0));
        _shapes.Add(new GeometryPol(200,300, 30,30, 70, 70, 40, 20));
        // Redraw shapes when window size changes
        SizeChanged += (s, e) => DrawShapes();
    }
    
    // Method responsible for rendering all shapes
    private void DrawShapes()
    {
        // Get Canvas control from XAML
        var canvas = this.FindControl<Canvas>("DrawingCanvas");
        if (canvas == null)
        {
            throw new ArgumentNullException("Канвас не найден, перепроверьте имя");
        }
            
        // Get current canvas size
        var width = (int)canvas.Bounds.Width;
        var height = (int)canvas.Bounds.Height;
        
        // Create off-screen bitmap for drawing
        var bitmap = new RenderTargetBitmap(new PixelSize(width, height));
        
        // Create drawing context and draw all shapes
        using (var ctx = bitmap.CreateDrawingContext())
        {
            _shapes.DrawAll(ctx);
        }
        
        // Create Image control to display rendered bitmap
        var image = new Image
        {
            Source = bitmap,
            Width = width,
            Height = height
        };
        
        // Clear canvas and place the new image
        canvas.Children.Clear();
        canvas.Children.Add(image);
    }
}