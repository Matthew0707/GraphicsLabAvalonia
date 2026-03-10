
using System;
using Avalonia;
using Avalonia.Controls;

using Avalonia.Media.Imaging;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Views;

public partial class MainWindow : Window
{
    private ShapeList _shapes;
    
    public MainWindow()
    {
        InitializeComponent();
        _shapes = new ShapeList();
        _shapes.Add(new Rectangle(50, 50, 50, 100));
        _shapes.Add(new Square(4, 200, 50));
        _shapes.Add(new Line(200, 200, 300, 120));
        _shapes.Add(new Ellipse(50, 50, 50, 100));
        _shapes.Add(new Circle(120, 50, 50));
        _shapes.Add(new Triangle(30,30, 10, 60, 50,0,10, 10));
            //Loaded += (s, e) => DrawShapes();
            SizeChanged += (s, e) => DrawShapes();
    }
    
    private void DrawShapes()
    {
        
            // Получаем Canvas
            var canvas = this.FindControl<Canvas>("DrawingCanvas");
            if (canvas == null)
            {
                throw new ArgumentNullException("Канвас не найден, перепроверьте имя");
            }
            
        // Создаем временное изображение для рисования
        var width = (int)canvas.Bounds.Width;
        var height = (int)canvas.Bounds.Height;
        
        // Создаем RenderTargetBitmap для рисования
        var bitmap = new RenderTargetBitmap(new PixelSize(width, height));
        
        using (var ctx = bitmap.CreateDrawingContext())
        {
                _shapes.DrawAll(ctx);
        }
        
        // Создаем Image контрол и кладем на Canvas
        var image = new Image
        {
            Source = bitmap,
            Width = width,
            Height = height
        };
        
        canvas.Children.Clear();
        canvas.Children.Add(image);
    }
}