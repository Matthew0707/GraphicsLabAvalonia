// GraphicsLabAvalonia/Views/MainWindow.xaml.cs
using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using GraphicsLabAvalonia.Factories;
using GraphicsLabAvalonia.Models;
using GraphicsLabAvalonia.Rendering;
using GraphicsLabAvalonia.Serialization;

namespace GraphicsLabAvalonia.Views;

// Main editor window - Controller in MVC
public partial class MainWindow : Window
{
    private ShapeList _shapes;
    private ShapeFactoryManager _factoryManager;
    private RenderManager _renderManager;
    private JsonShapeSerializer _serializer;
    
    // Drawing state
    private string _currentShapeType;
    private bool _isDrawing;
    private Point _startPoint;
    private Point _currentPoint;
    
    // Selection state
    private Shape _selectedShape;
    private bool _isDragging;
    
    public MainWindow()
    {
        InitializeComponent();
        
        _shapes = new ShapeList();
        _factoryManager = new ShapeFactoryManager();
        _renderManager = new RenderManager();
        _serializer = new JsonShapeSerializer();
        
        var shapeTypes = _factoryManager.GetAvailableShapeTypes().ToList();
        ShapeTypeListBox.ItemsSource = shapeTypes;
        if (shapeTypes.Any()) ShapeTypeListBox.SelectedIndex = 0;
        
        ShapeTypeListBox.SelectionChanged += OnShapeTypeChanged;
        SizeChanged += (s, e) => DrawShapes();
        DrawShapes();
    }
    
    private void OnShapeTypeChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ShapeTypeListBox.SelectedItem is string type)
        {
            _currentShapeType = type;
            CurrentShapeInfo.Text = $"Selected: {type}";
        }
    }
    
    // Handle pointer press - draw new shape or select existing
    private void OnCanvasPointerPressed(object sender, PointerPressedEventArgs e)
    {
        var pos = e.GetPosition(DrawingCanvas);
        var props = e.GetCurrentPoint(this).Properties;
        
        if (!props.IsLeftButtonPressed) return;
        
        // Check if clicking on existing shape for selection/edit
        var clickedShape = _shapes.FindAtPoint((int)pos.X, (int)pos.Y);
        
        if (clickedShape != null)
        {
            // Select and prepare for dragging
            _selectedShape = clickedShape;
            _isDragging = true;
            _startPoint = pos;
            CurrentShapeInfo.Text = $"Selected: {_selectedShape.GetDescription()}";
        }
        else
        {
            // Start drawing new shape
            if (string.IsNullOrEmpty(_currentShapeType)) return;
            _selectedShape = null;
            _isDrawing = true;
            _startPoint = pos;
            _currentPoint = pos;
            e.Pointer.Capture(DrawingCanvas);
        }
    }
    
    // Handle pointer move - preview drawing or resize
    private void OnCanvasPointerMoved(object sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(DrawingCanvas);
        
        if (_isDrawing)
        {
            _currentPoint = pos;
            DrawShapesWithPreview();
        }
        else if (_isDragging && _selectedShape != null)
        {
            // Resize/move selected shape
            var dx = (int)(pos.X - _startPoint.X);
            var dy = (int)(pos.Y - _startPoint.Y);
            
            ResizeShape(_selectedShape, dx, dy);
            _startPoint = pos;
            DrawShapes();
        }
    }
    
    // Resize selected shape
    private void ResizeShape(Shape shape, int dx, int dy)
    {
        switch (shape)
        {
            case Circle c:
                c.Diameter = Math.Max(10, c.Diameter + Math.Max(dx, dy));
                break;
            case Rectangle r:
                r.Width = Math.Max(10, r.Width + dx);
                r.Height = Math.Max(10, r.Height + dy);
                break;
            case Square s:
                int delta = Math.Max(dx, dy);
                s.Side = Math.Max(10, s.Side + delta);
                break;
            case Line l:
                l.X2 += dx;
                l.Y2 += dy;
                break;
            case Ellipse e:
                e.Width = Math.Max(10, e.Width + dx);
                e.Height = Math.Max(10, e.Height + dy);
                break;
        }
    }
    
    // Handle pointer release - finalize drawing or dragging
    private void OnCanvasPointerReleased(object sender, PointerReleasedEventArgs e)
    {
        if (_isDrawing)
        {
            _isDrawing = false;
            e.Pointer.Capture(null);
            
            try
            {
                var factory = _factoryManager.GetFactory(_currentShapeType);
                var parameters = factory.CalculateParameters(_startPoint, _currentPoint);
                var shape = factory.CreateShape(parameters);
                _shapes.Add(shape);
                CurrentShapeInfo.Text = $"Created: {_currentShapeType}";
            }
            catch (Exception ex)
            {
                CurrentShapeInfo.Text = $"Error: {ex.Message}";
            }
            
            DrawShapes();
        }
        
        _isDragging = false;
    }
    
    // Save to JSON file
    private async void OnSaveButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Save Shapes",
            DefaultExtension = ".json",
            Filters = { new FileDialogFilter { Name = "JSON", Extensions = { "json" } } }
        };
        
        var path = await dialog.ShowAsync(this);
        if (!string.IsNullOrEmpty(path))
        {
            _serializer.Serialize(path, _shapes.GetAllShapes());
            CurrentShapeInfo.Text = $"Saved: {_shapes.Count} shapes";
        }
    }
    
    // Load from JSON file
    private async void OnLoadButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Load Shapes",
            Filters = { new FileDialogFilter { Name = "JSON", Extensions = { "json" } } },
            AllowMultiple = false
        };
        
        var files = await dialog.ShowAsync(this);
        if (files != null && files.Length > 0)
        {
            _shapes.Clear();
            var loadedShapes = _serializer.Deserialize(files[0]);
            foreach (var shape in loadedShapes)
                _shapes.Add(shape);
            
            CurrentShapeInfo.Text = $"Loaded: {_shapes.Count} shapes";
            DrawShapes();
        }
    }
    
    // Delete selected shape
    private void OnDeleteButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_selectedShape != null)
        {
            _shapes.Remove(_selectedShape);
            _selectedShape = null;
            CurrentShapeInfo.Text = "Deleted";
            DrawShapes();
        }
    }
    
    // Clear canvas
    private void OnClearButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _shapes.Clear();
        _selectedShape = null;
        CurrentShapeInfo.Text = "Cleared";
        DrawShapes();
    }
    
    // Draw all shapes
    private void DrawShapes()
    {
        var canvas = this.FindControl<Canvas>("DrawingCanvas");
        if (canvas == null) return;
        
        var width = (int)canvas.Bounds.Width;
        var height = (int)canvas.Bounds.Height;
        if (width <= 0 || height <= 0) return;
        
        var bitmap = new RenderTargetBitmap(new PixelSize(width, height));
        
        using (var ctx = bitmap.CreateDrawingContext())
        {
            _renderManager.RenderShapes(ctx, _shapes.GetAllShapes());
            
            // Highlight selected shape
            if (_selectedShape != null)
                DrawSelectionHighlight(ctx, _selectedShape);
        }
        
        var image = new Image { Source = bitmap, Width = width, Height = height };
        canvas.Children.Clear();
        canvas.Children.Add(image);
    }
    
    // Draw with preview
    private void DrawShapesWithPreview()
    {
        var canvas = this.FindControl<Canvas>("DrawingCanvas");
        if (canvas == null) return;
        
        var width = (int)canvas.Bounds.Width;
        var height = (int)canvas.Bounds.Height;
        if (width <= 0 || height <= 0) return;
        
        var bitmap = new RenderTargetBitmap(new PixelSize(width, height));
        
        using (var ctx = bitmap.CreateDrawingContext())
        {
            _renderManager.RenderShapes(ctx, _shapes.GetAllShapes());
            
            if (_isDrawing)
            {
                try
                {
                    var factory = _factoryManager.GetFactory(_currentShapeType);
                    var parameters = factory.CalculateParameters(_startPoint, _currentPoint);
                    var previewShape = factory.CreateShape(parameters);
                    
                    using (ctx.PushOpacity(0.5))
                        _renderManager.RenderShape(ctx, previewShape);
                    
                    DrawBoundingBox(ctx);
                }
                catch { }
            }
        }
        
        var image = new Image { Source = bitmap, Width = width, Height = height };
        canvas.Children.Clear();
        canvas.Children.Add(image);
    }
    
    private void DrawSelectionHighlight(DrawingContext ctx, Shape shape)
    {
        var pen = new Pen(Brushes.Red, 2, new DashStyle(new[] { 5.0, 5.0 }, 0));
        // Draw highlight based on shape type
        ctx.DrawRectangle(pen, new Rect(shape.X - 5, shape.Y - 5, 10, 10));
    }
    
    private void DrawBoundingBox(DrawingContext ctx)
    {
        var dashStyle = new DashStyle(new[] { 5.0, 5.0 }, 0);
        var pen = new Pen(Brushes.Blue, 1, dashStyle);
        
        int x = (int)Math.Min(_startPoint.X, _currentPoint.X);
        int y = (int)Math.Min(_startPoint.Y, _currentPoint.Y);
        int w = (int)Math.Abs(_currentPoint.X - _startPoint.X);
        int h = (int)Math.Abs(_currentPoint.Y - _startPoint.Y);
        
        ctx.DrawRectangle(pen, new Rect(x, y, w, h));
    }
}