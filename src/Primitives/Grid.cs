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
        float tip = 10 / scale;
        
        Console.WriteLine(topLeft);
        Console.WriteLine(bottomRight);
        PointF topCenter = new PointF((topLeft.X + bottomRight.X) / 2, topLeft.Y);
        PointF bottomCenter = new PointF((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
        PointF leftCenter = new PointF(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
        PointF rightCenter = new PointF(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

        PointF[] points1 = new PointF[3];
        points1[0] = topCenter;
        points1[1] = new PointF(topCenter.X + tip, topCenter.Y + tip);
        points1[2] = new PointF(topCenter.X - tip, topCenter.Y + tip);
        Console.WriteLine(points1[1].ToString());
        Console.WriteLine(points1[2].ToString());
        PointF[] points2 = new PointF[3];
        points2[0] = rightCenter;
        points2[1] = new PointF(rightCenter.X - tip, rightCenter.Y + tip);
        points2[2] = new PointF(rightCenter.X - tip, rightCenter.Y - tip);

        Console.WriteLine(points2[1].ToString());
        Console.WriteLine(points2[2].ToString());
        
        g.DrawLine(new Pen(Brushes.Black, 2/scale), rightCenter, leftCenter);
        g.DrawLine(new Pen(Brushes.Black, 2/scale), topCenter, bottomCenter);
        
        g.FillPolygon(new SolidBrush(Color.Black), points1);
        g.FillPolygon(new SolidBrush(Color.Black), points2);
    }
}