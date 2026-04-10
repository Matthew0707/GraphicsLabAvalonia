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

namespace GraphicsLabAvalonia.Views;

/// <summary>
/// Main window of the graphics editor application
/// Implements drag-and-drop shape creation with preview
/// </summary>
public partial class MainWindow : Window
{
    // Container for all shapes
    private ShapeList _shapes;
    
    // Factory manager for creating shapes dynamically
    private ShapeFactoryManager _factoryManager;
    
    // Render manager for drawing shapes
    private RenderManager _renderManager;
    
    // Current drawing state
    private string _currentShapeType;
    private bool _isDrawing;
    private Point _startPoint;
    private Point _currentPoint;
    
    public MainWindow()
    {
        InitializeComponent();
        
        // Initialize collections and managers
        _shapes = new ShapeList();
        _renderManager = new RenderManager();
        
        // Initialize and configure factory manager
        InitializeFactoryManager();
        
        // Set up UI event handlers
        ShapeTypeListBox.SelectionChanged += OnShapeTypeChanged;
        
        // Register window events
        SizeChanged += (s, e) => DrawShapes();
        
        // Initial draw
        DrawShapes();
    }
    
    /// <summary>
    /// Initialize the factory manager and register all available shape factories
    /// </summary>
    private void InitializeFactoryManager()
    {
        _factoryManager = new ShapeFactoryManager();
        
        // Register all available shape factories
        _factoryManager.RegisterFactory(new CircleFactory());
        _factoryManager.RegisterFactory(new RectangleFactory());
        _factoryManager.RegisterFactory(new SquareFactory());
        _factoryManager.RegisterFactory(new LineFactory());
        _factoryManager.RegisterFactory(new EllipseFactory());
        _factoryManager.RegisterFactory(new TriangleFactory());
        
        // Populate list box with available shape types
        var shapeTypes = _factoryManager.GetAvailableShapeTypes().ToList();
        ShapeTypeListBox.ItemsSource = shapeTypes;
        
        if (shapeTypes.Any())
            ShapeTypeListBox.SelectedIndex = 0;
    }
    
    /// <summary>
    /// Handle shape type selection change
    /// </summary>
    private void OnShapeTypeChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ShapeTypeListBox.SelectedItem is string shapeType)
        {
            _currentShapeType = shapeType;
            CurrentShapeInfo.Text = $"Выбрана: {shapeType}";
        }
    }
    
    /// <summary>
    /// Handle canvas pointer press - start drawing
    /// </summary>
    private void OnCanvasPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (string.IsNullOrEmpty(_currentShapeType))
        {
            CurrentShapeInfo.Text = "Сначала выберите фигуру!";
            return;
        }
        
        // Only handle left mouse button
        var properties = e.GetCurrentPoint(this).Properties;
        if (!properties.IsLeftButtonPressed)
            return;
        
        // Start drawing
        _isDrawing = true;
        _startPoint = e.GetPosition(DrawingCanvas);
        _currentPoint = _startPoint;
        
        // Capture mouse to receive events even outside canvas
        e.Pointer.Capture(DrawingCanvas);
        
        CurrentShapeInfo.Text = $"Рисование: {_currentShapeType}";
    }
    
    /// <summary>
    /// Handle canvas pointer moved - update preview
    /// </summary>
    private void OnCanvasPointerMoved(object sender, PointerEventArgs e)
    {
        if (!_isDrawing) return;
        
        _currentPoint = e.GetPosition(DrawingCanvas);
        
        // Redraw everything including preview
        DrawShapesWithPreview();
    }
    
    /// <summary>
    /// Handle canvas pointer released - create final shape
    /// </summary>
    private void OnCanvasPointerReleased(object sender, PointerReleasedEventArgs e)
    {
        if (!_isDrawing) return;
        
        _isDrawing = false;
        e.Pointer.Capture(null);
        
        // Create the final shape
        try
        {
            var shape = CreateShapeFromPoints(_startPoint, _currentPoint);
            if (shape != null)
            {
                _shapes.Add(shape);
                CurrentShapeInfo.Text = $"Создана: {_currentShapeType}";
            }
        }
        catch (Exception ex)
        {
            CurrentShapeInfo.Text = $"Ошибка: {ex.Message}";
        }
        
        // Draw final result
        DrawShapes();
    }
    
    /// <summary>
    /// Create shape from start and current points based on shape type
    /// </summary>
    private Shape CreateShapeFromPoints(Point start, Point end)
    {
        var parameters = CalculateShapeParameters(start, end);
        return _factoryManager.CreateShape(_currentShapeType, parameters);
    }
    
    /// <summary>
    /// Calculate parameters for different shape types
    /// </summary>
    private int[] CalculateShapeParameters(Point start, Point end)
    {
        int x1 = (int)Math.Min(start.X, end.X);
        int y1 = (int)Math.Min(start.Y, end.Y);
        int x2 = (int)Math.Max(start.X, end.X);
        int y2 = (int)Math.Max(start.Y, end.Y);
        
        int width = x2 - x1;
        int height = y2 - y1;
        
        switch (_currentShapeType)
        {
            case "Line":
                return new[] { (int)start.X, (int)start.Y, (int)end.X, (int)end.Y };
                
            case "Rectangle":
            case "Ellipse":
                return new[] { x1, y1, width, height };
                
            case "Circle":
                int diameter = Math.Min(width, height);
                return new[] { x1, y1, diameter };
                
            case "Square":
                int side = Math.Min(width, height);
                return new[] { x1, y1, side };
                
            case "Triangle":
                return new[]
                {
                    x1, y2,                    // Bottom-left
                    x2, y2,                    // Bottom-right
                    x1 + width / 2, y1         // Top-center
                };
                
            default:
                throw new ArgumentException($"Unknown shape type: {_currentShapeType}");
        }
    }
    
    /// <summary>
    /// Create preview shape from current drag points
    /// </summary>
    private Shape CreatePreviewShape()
    {
        return CreateShapeFromPoints(_startPoint, _currentPoint);
    }
    
    /// <summary>
    /// Draw all shapes including preview
    /// </summary>
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
            // Draw existing shapes using RenderManager
            _renderManager.RenderShapes(ctx, _shapes.GetAllShapes());
        
            // Draw preview shape
            if (_isDrawing)
            {
                try
                {
                    var previewShape = CreatePreviewShape();
                
                    // Save context state for transparency
                    using (ctx.PushOpacity(0.5))
                    {
                        _renderManager.RenderShape(ctx, previewShape);
                    }
                
                    DrawBoundingBox(ctx);
                }
                catch
                {
                    // Ignore preview drawing errors
                }
            }
        }
    
        var image = new Image
        {
            Source = bitmap,
            Width = width,
            Height = height
        };
    
        canvas.Children.Clear();
        canvas.Children.Add(image);
    }
    
    /// <summary>
    /// Draw bounding box during drag operation
    /// </summary>
    private void DrawBoundingBox(DrawingContext context)
    {
        // Create dashed pen for bounding box
        var dashStyle = new DashStyle(new[] { 5.0, 5.0 }, 0);
        var pen = new Pen(Brushes.Blue, 1, dashStyle);
    
        int x = (int)Math.Min(_startPoint.X, _currentPoint.X);
        int y = (int)Math.Min(_startPoint.Y, _currentPoint.Y);
        int width = (int)Math.Abs(_currentPoint.X - _startPoint.X);
        int height = (int)Math.Abs(_currentPoint.Y - _startPoint.Y);
    
        context.DrawRectangle(pen, new Rect(x, y, width, height));
    }
    
    /// <summary>
    /// Draw all shapes (without preview)
    /// </summary>
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
            // Use RenderManager instead of shape.Draw()
            _renderManager.RenderShapes(ctx, _shapes.GetAllShapes());
        }
        
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