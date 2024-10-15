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
        var points = new PointF[3];
        points[0] = new PointF(rightCenter.X , rightCenter.Y);
        points[1] = new PointF(rightCenter.X - tipLength, rightCenter.Y + tipLength / 2);
        points[2] = new PointF(rightCenter.X - tipLength, rightCenter.Y - tipLength / 2);

        g.FillPolygon(brush, points);

        points[0] = new PointF(topCenter.X, topCenter.Y);
        points[1] = new PointF(topCenter.X - tipLength / 2, topCenter.Y + tipLength);
        points[2] = new PointF(topCenter.X + tipLength / 2, topCenter.Y + tipLength);

        g.FillPolygon(brush, points);
    }

    private void DrawGrid(Graphics g, Pen pen, Brush brush, PointF topLeft, PointF bottomRight, float tipLength)
    {
        PointF topCenter = new PointF((topLeft.X + bottomRight.X) / 2, topLeft.Y);
        PointF bottomCenter = new PointF((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
        PointF leftCenter = new PointF(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
        PointF rightCenter = new PointF(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

        g.DrawLine(pen, rightCenter - new SizeF(tipLength, 0), leftCenter);
        g.DrawLine(pen, topCenter + new SizeF(0, tipLength), bottomCenter);
        DrawArrows(g, brush, rightCenter, topCenter, tipLength);
    }

    public void Draw(Graphics g, PointF topLeft, PointF bottomRight, Pen pen, Brush brush, float tipLength)
    {
        DrawGrid(g, pen, brush, topLeft, bottomRight, tipLength);
    }
}