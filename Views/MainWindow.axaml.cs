
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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


using GraphicsLabAvalonia.Plugins;

// Main editor window - Controller in MVC
public partial class MainWindow : Window
{
    private ShapeList _shapes;
    private ShapeFactoryManager _factoryManager;
    private RenderManager _renderManager;
    private JsonShapeSerializer _serializer;
    private PluginLoader _pluginLoader; 
    
    // Drawing state
    private string _currentShapeType;
    private bool _isDrawing;
    private Point _startPoint;
    private Point _currentPoint;
    private IDataProcessor _activeProcessor;
    private bool _processingEnabled;
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
        
        
        _pluginLoader = new PluginLoader();
        _pluginLoader.LoadAll(_factoryManager, _renderManager, _serializer);
        _pluginLoader.LoadProcessors();
        var assembly = Assembly.GetExecutingAssembly();
        var shapeTypes = assembly.GetTypes()
            .Where(t => typeof(Shape).IsAssignableFrom(t) && !t.IsAbstract && t != typeof(Shape));

        foreach (var type in shapeTypes)
        {
            try
            {
                Activator.CreateInstance(type);
            }
            catch { }
        }
        
        UpdateProcessorStatus();
        
        var shapeTypes1 = _factoryManager.GetAvailableShapeTypes().ToList();
        ShapeTypeListBox.ItemsSource = shapeTypes1;
        
        if (shapeTypes1.Any())
        {
            ShapeTypeListBox.SelectedIndex = 0;
            _currentShapeType = shapeTypes1[0];
            CurrentShapeInfo.Text = $"Selected: {_currentShapeType}";
        }
        
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
    
        // Clear previous selection visual
        if (_selectedShape != null)
            DrawShapes();
    
        // Check if clicking on existing shape
        var clickedShape = _shapes.FindAtPoint((int)pos.X, (int)pos.Y);
    
        // Deselect if clicking empty space
        if (clickedShape == null)
        {
            _selectedShape = null;
            _isDragging = false;
            CurrentShapeInfo.Text = "Deselected";
            DrawShapes();
        
            // Start drawing new shape
            if (!string.IsNullOrEmpty(_currentShapeType))
            {
                _isDrawing = true;
                _startPoint = pos;
                _currentPoint = pos;
                e.Pointer.Capture(DrawingCanvas);
            }
            return;
        }
    
        // Select shape
        _selectedShape = clickedShape;
        _isDragging = true;
        _startPoint = pos;
        CurrentShapeInfo.Text = $"Selected: {_selectedShape.GetDescription()}";
        DrawShapes(); // Will show selection highlight
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
            // Calculate how much the mouse moved
            int dx = (int)(pos.X - _startPoint.X);
            int dy = (int)(pos.Y - _startPoint.Y);
        
            if (dx != 0 || dy != 0)
            {
                // Resize - shape stays in place, only size changes
                _selectedShape.Resize(dx, dy);
            
                // Update reference point for continuous resizing
                _startPoint = pos;
                DrawShapes();
            }
        }
    }
    
    // Resize selected shape
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
            var shapes = _shapes.GetAllShapes().ToList();
        
            // DEBUG
            System.Diagnostics.Debug.WriteLine($"Processor enabled: {_processingEnabled}");
            System.Diagnostics.Debug.WriteLine($"Active processor: {_activeProcessor?.ProcessorName ?? "NULL"}");
        
            // Apply processor before save
            if (_processingEnabled && _activeProcessor != null)
            {
                Console.WriteLine($"MAIN: Calling processor {_activeProcessor.ProcessorName}");
                shapes = _activeProcessor.ProcessBeforeSave(shapes);
            }
            else
            {
                Console.WriteLine($"MAIN: Processor skipped. Enabled={_processingEnabled}, Processor={_activeProcessor?.ProcessorName ?? "null"}");
            }
        
            _serializer.Serialize(path, shapes);
            CurrentShapeInfo.Text = $"Saved: {shapes.Count} shapes";
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
            var shapes = _serializer.Deserialize(files[0]);
        
            // Apply processor after load
            if (_processingEnabled && _activeProcessor != null)
                shapes = _activeProcessor.ProcessAfterLoad(shapes);
        
            _shapes.Clear();
            foreach (var shape in shapes) _shapes.Add(shape);
        
            CurrentShapeInfo.Text = $"Loaded: {shapes.Count} shapes";
            DrawShapes();
        }
    }
    private void OnSettingsButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var settings = new SettingsWindow(
            _pluginLoader.Processors.ToList(),
            _activeProcessor);
    
        settings.ShowDialog(this).ContinueWith(_ =>
        {
            _activeProcessor = settings.SelectedProcessor;
            _processingEnabled = settings.SelectedProcessor != null;
            UpdateProcessorStatus();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void UpdateProcessorStatus()
    {
        if (_processingEnabled && _activeProcessor != null)
            ProcessorStatus.Text = $"Processor: {_activeProcessor.ProcessorName}";
        else
            ProcessorStatus.Text = "Processor: None";
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
        ctx.DrawEllipse(Brushes.Yellow, pen, new Rect(shape.X, shape.Y - 5, 2, 2));
    }
    
    private void DrawBoundingBox(DrawingContext ctx)
    {
        var dashStyle = new DashStyle(new[] { 5.0, 5.0 }, 0);
        var pen = new Pen(Brushes.Blue, 1, dashStyle);
    
        // Always draw from start to current - works for ALL shapes
        double x = Math.Min(_startPoint.X, _currentPoint.X);
        double y = Math.Min(_startPoint.Y, _currentPoint.Y);
        double w = Math.Abs(_currentPoint.X - _startPoint.X);
        double h = Math.Abs(_currentPoint.Y - _startPoint.Y);
    
        // Minimum size so box is always visible
        if (w < 5 && h < 5)
        {
            w = 20;
            h = 20;
        }
    
        ctx.DrawRectangle(null, pen, new Rect(x, y, w, h));
    }
}