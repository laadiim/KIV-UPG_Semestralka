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

    private void DrawArrow(Graphics g, Brush brush, PointF topLeft, PointF bottomRight)
    {
        throw new NotImplementedException();
    }

    private void DrawGrid(Graphics g, Pen pen, Brush brush, PointF topLeft, PointF bottomRight)
    {
        PointF topCenter = new PointF((topLeft.X + bottomRight.X) / 2, topLeft.Y);
        PointF bottomCenter = new PointF((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
        PointF leftCenter = new PointF(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
        PointF rightCenter = new PointF(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

        g.DrawLine(pen, rightCenter, leftCenter);
        g.DrawLine(pen, topCenter, bottomCenter);
        DrawArrow(g, brush, topLeft, bottomRight);
    }

    public void Draw(Graphics g, PointF topLeft, PointF bottomRight, Pen pen, Brush brush)
    {
        DrawGrid(g, pen, brush, topLeft, bottomRight);
    }
}