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

    private void DrawArrows(Graphics g, Brush brush, PointF rightCenter, PointF topCenter, float tipLength)
    {
        var points1 = new PointF[3];
        points1[0] = new PointF(rightCenter.X , rightCenter.Y);
        points1[1] = new PointF(rightCenter.X - tipLength, rightCenter.Y + tipLength / 2);
        points1[2] = new PointF(rightCenter.X - tipLength, rightCenter.Y - tipLength / 2);

        g.FillPolygon(brush, points1);

    }

    private void DrawGrid(Graphics g, Pen pen, Brush brush, PointF topLeft, PointF bottomRight, float tipLength)
    {
        PointF topCenter = new PointF((topLeft.X + bottomRight.X) / 2, topLeft.Y);
        PointF bottomCenter = new PointF((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
        PointF leftCenter = new PointF(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
        PointF rightCenter = new PointF(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

        g.DrawLine(pen, rightCenter, leftCenter);
        g.DrawLine(pen, topCenter, bottomCenter);
        DrawArrows(g, brush, rightCenter, topCenter, tipLength);
    }

    public void Draw(Graphics g, PointF topLeft, PointF bottomRight, Pen pen, Brush brush, float tipLength)
    {
        DrawGrid(g, pen, brush, topLeft, bottomRight, tipLength);
    }
}