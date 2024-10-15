namespace UPG_SP_2024.Interfaces;

public interface IGrid
{
    int GetNumberOfGridLines();
    void SetNumberOfGridLines(int row, int column);
    void Draw(Graphics g, PointF topLeft, PointF bottomRight, Pen pen, Brush brush, float tipLength);
}