using System.Drawing;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

//TODO: doplnit cely Grid
public class Grid : IGrid
{
    float panelWidth;
    float panelHeight;
    float spacingX;
    float spacingY;
    int spacingXpixels;
    int spacingYpixels;

    public Grid(float panelWidth, float panelHeight, int spacingXpixels = 0, int spacingYpixels = 0)
    {
        this.panelWidth = panelWidth;
        this.panelHeight = panelHeight;
        this.spacingX = spacingXpixels / panelWidth;
        this.spacingY = spacingYpixels / panelHeight;
    }

    public int GetNumberOfGridLines()
    {
        throw new NotImplementedException();
    }

    public void SetNumberOfGridLines(int row, int column)
    {
        throw new NotImplementedException();
    }

    /*
    Prostor okna bude pravidelnì vzorkován (møížka) a v každém vzorku bude zobrazena šipka
    znázoròující intenzitu elektrického pole (tj. de facto se jedná o statické sondy)
     
    Pøi spouštìní programu z pøíkazové øádky bude možné volitelnì specifikovat
    parametr -g<X>x<Y>, kde za <X> a <Y> jsou celoèíselné hodnoty udávající rozteè vzorkù møížky v ose x
    a y v pixlech
    */

    public int GetSpacingXinPixels()
    {
        return this.spacingXpixels;
    }

    public int GetSpacingYinPixels()
    {
        return this.spacingYpixels;
    }

    /// <summary>
    /// vykresli sipky
    /// </summary>
    /// <param name="g">graficky kontext</param>
    /// <param name="brush">predany stetec</param>
    /// <param name="rightCenter">prostredek prave strany scenare</param>
    /// <param name="topCenter">prostredek horni strany scenare</param>
    /// <param name="tipLength">delka sipky</param>
    private void DrawArrows(Graphics g, Brush brush, PointF rightCenter, PointF topCenter, float tipLength)
    {
        var points = new PointF[3];
        points[0] = new PointF(rightCenter.X, rightCenter.Y);
        points[1] = new PointF(rightCenter.X - tipLength, rightCenter.Y + tipLength / 2);
        points[2] = new PointF(rightCenter.X - tipLength, rightCenter.Y - tipLength / 2);

        g.FillPolygon(brush, points);

        points[0] = new PointF(topCenter.X, topCenter.Y);
        points[1] = new PointF(topCenter.X - tipLength / 2, topCenter.Y + tipLength);
        points[2] = new PointF(topCenter.X + tipLength / 2, topCenter.Y + tipLength);

        g.FillPolygon(brush, points);
    }
    /// <summary>
    /// nakresli mrizku
    /// </summary>
    /// <param name="g">graficky kontext</param>
    /// <param name="pen">predane pero</param>
    /// <param name="brush">predany stetec</param>
    /// <param name="topLeft">horni levy roh</param>
    /// <param name="bottomRight">dolni pravy roh</param>
    /// <param name="tipLength">delka sipky</param>
    private void DrawAxes(Graphics g, Pen pen, Brush brush, PointF topLeft, PointF bottomRight, float tipLength, float scale, Color color)
    {
        PointF topCenter = new PointF((topLeft.X + bottomRight.X) / 2, topLeft.Y);
        PointF bottomCenter = new PointF((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
        PointF leftCenter = new PointF(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
        PointF rightCenter = new PointF(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

        Font font = new Font("Arial", 1f / (float)Math.Sqrt(scale), FontStyle.Bold);
        SizeF size = new SizeF(tipLength, 0);

        g.DrawLine(pen, rightCenter - size, leftCenter);
        g.DrawString("x", font, brush, rightCenter.X - 2 * tipLength, rightCenter.Y + tipLength);

        g.DrawLine(pen, topCenter + new SizeF(0, tipLength), bottomCenter);
        g.DrawString("y", font, brush, topCenter.X - 2 * tipLength, topCenter.Y + tipLength);

        DrawArrows(g, brush, rightCenter, topCenter, tipLength);

        if (this.spacingXpixels != 0 && this.spacingYpixels != 0)
        {
            Pen penGrid = new Pen(color, 3 / scale);
            DrawGrid(g, penGrid);
        }
    }
    private void DrawGrid(Graphics g, Pen pen)
    {
        int nX, nY, aX, aY;
        nX = (int)((this.panelWidth / 2) / this.spacingX);
        nY = (int)((this.panelHeight / 2) / this.spacingY);

        aX = 2 * nX - 1;
        aY = 2 * nY - 1;

        float[] intersectionsX = new float[aX * aY];
        float[] intersectionsY = new float[aX * aY];

    }

    public void Draw(Graphics g, PointF topLeft, PointF bottomRight, float tipLength, float scale)
    {
        Color color = Color.FromArgb(40, Color.White);
        Pen pen = new Pen(color, 3 / scale);
        Brush brush = new SolidBrush(color);

        tipLength = 1f / (float)Math.Sqrt(scale);
        DrawAxes(g, pen, brush, topLeft, bottomRight, tipLength, scale, color);
    }
}