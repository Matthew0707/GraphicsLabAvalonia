
using Avalonia.Media;
using Avalonia; 
namespace GraphicsLabAvalonia.Models;

public class GeometryPol : Shape
{

    public int[] _params;
    
    public GeometryPol(int x, int y, params int[] XYparams) : base(x, y)
    {
        _params = XYparams;  
    }
    
    public override void Draw(DrawingContext context)
    {
        var points = new Point[ _params.Length/2];
        int j = 0;
        for (int i = 0; i < _params.Length / 2; i++)
        {
            points[i] = new Point(X + _params[j], Y + _params[j + 1]);
            j += 2;
        }
        
        var polygon = new PolylineGeometry(points, true);

        var fillBrush = new SolidColorBrush(Colors.Black);
        var outPen = new Pen(Brushes.BlanchedAlmond, 2);

        context.DrawGeometry(fillBrush, outPen, polygon);
    }
}
