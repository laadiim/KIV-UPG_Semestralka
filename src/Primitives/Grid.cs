using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

//TODO: doplnit cely Grid
public class Grid : IGrid
{
    public int GetNumberOfGridLines()
    {
        throw new NotImplementedException();
    }

    public void SetNumberOfGridLines(int row, int column)
    {
        throw new NotImplementedException();
    }

    public void Draw(Graphics g, PointF topLeft, PointF bottomRight, float scale)
    {
        Console.WriteLine(topLeft);
        Console.WriteLine(bottomRight);
        PointF topCenter = new PointF((topLeft.X + bottomRight.X) / 2, topLeft.Y);
        PointF bottomCenter = new PointF((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
        PointF leftCenter = new PointF(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
        PointF rightCenter = new PointF(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);
        
        g.DrawLine(new Pen(Brushes.Black, 2/scale), rightCenter, leftCenter);
        g.DrawLine(new Pen(Brushes.Black, 2/scale), topCenter, bottomCenter);
    }
}