using System.Drawing;
using System.Runtime.InteropServices;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives
{ 

//TODO: doplnit cely Grid
public class Grid : IGrid
{
    readonly float panelWidth;
    readonly float panelHeight;

    readonly float xMin, xMax, yMin, yMax;

    readonly int startTime;
    readonly INaboj[] charges;
    readonly float scale;

    float spacingX;
    float spacingY;

    readonly int spacingXpixels;
    readonly int spacingYpixels;

    public Grid(float xMin, float xMax, float yMin, float yMax, int startTime, INaboj[] charges, float scale, int spacingXpixels, int spacingYpixels, float viewportWidth, float viewportHeight)
    {
        this.xMin = xMin;
        this.xMax = xMax;
        this.yMin = yMin;
        this.yMax = yMax;

        this.panelWidth = xMax - xMin;
        this.panelHeight = yMax - yMin;

        this.startTime = startTime;
        this.charges = charges;
        this.scale = scale;

        this.spacingXpixels = spacingXpixels;
        this.spacingYpixels = spacingYpixels;

        float countX = viewportWidth / spacingXpixels;
        float countY = viewportHeight / spacingYpixels;


        this.spacingX = panelWidth / countX;
        this.spacingY = panelHeight / countY;
    }

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
    /// <param name="penAxes">predane pero pro pro osy</param>
    /// <param name="penGrid">predane pero pro mrizku</param>
    /// <param name="brush">predany stetec</param>
    /// <param name="brushStr">predany stetec pro string</param>
    /// <param name="topLeft">horni levy roh</param>
    /// <param name="bottomRight">dolni pravy roh</param>
    /// <param name="tipLength">delka sipky</param>
    /// <param name="scale">predany scale</param>
    private void DrawAxes(Graphics g, Pen penAxes, Pen penGrid, Brush brush, Brush brushStr, PointF topLeft, PointF bottomRight, float tipLength, float scale)
    {
        PointF topCenter = new PointF((topLeft.X + bottomRight.X) / 2, topLeft.Y);
        PointF bottomCenter = new PointF((topLeft.X + bottomRight.X) / 2, bottomRight.Y);
        PointF leftCenter = new PointF(topLeft.X, (topLeft.Y + bottomRight.Y) / 2);
        PointF rightCenter = new PointF(bottomRight.X, (topLeft.Y + bottomRight.Y) / 2);

        Font font = new Font("Arial", 1f / (float)Math.Sqrt(scale), FontStyle.Bold);
        SizeF size = new SizeF(tipLength, 0);

        g.DrawLine(penAxes, rightCenter - size, leftCenter);
        g.DrawString("x", font, brushStr, rightCenter.X - 2 * tipLength, rightCenter.Y + tipLength);

        g.DrawLine(penAxes, topCenter + new SizeF(0, tipLength), bottomCenter);
        g.DrawString("y", font, brushStr, topCenter.X - 2 * tipLength, topCenter.Y + tipLength);

        DrawArrows(g, brush, rightCenter, topCenter, tipLength);

        if (SettingsObject.gridShown)
        {
            DrawGrid(g, penGrid);
        }
    }
        private void DrawGrid(Graphics g, Pen pen)
        {
            float width_half = this.panelWidth / 2;
            float height_half = this.panelHeight / 2;
            int nX, nY, n2x, n2y;

            nX = (int)(width_half / this.spacingX);
            nY = (int)(height_half / this.spacingY);

            n2x = 2 * nX + 1;
            n2y = 2 * nY + 1;

            float[] intersectionsX = new float[n2x + 1];
            float[] intersectionsY = new float[n2y + 1];

            float x_minus = this.xMin + width_half - this.spacingX;
            float x_plus = this.xMax - width_half + this.spacingX;
            float y_minus = this.yMin + height_half - this.spacingY;
            float y_plus = this.yMax - height_half + this.spacingY;

            int i = 0;
            int j = 0;

            while (x_minus >= this.xMin)
            {
                g.DrawLine(pen, x_minus, this.yMin, x_minus, this.yMax);
                intersectionsX[i] = x_minus;

                x_minus -= this.spacingX;

                i++;
            }

            intersectionsX[i] = xMin + width_half;

            while (x_plus <= this.xMax)
            {
                g.DrawLine(pen, x_plus, this.yMin, x_plus, this.yMax);
                intersectionsX[i] = x_plus;

                x_plus += this.spacingX;
                i++;
            }

            if (intersectionsX.Length == (i + 2))
            {
                intersectionsX[i + 1] = x_plus;
            }

            while (y_minus >= this.yMin)
            {
                g.DrawLine(pen, this.xMin, y_minus, this.xMax, y_minus);
                intersectionsY[j] = y_minus;

                y_minus -= this.spacingY;
                j++;
            }

            intersectionsY[j] = yMin + height_half;

            while (y_plus <= this.yMax)
            {
                g.DrawLine(pen, this.xMin, y_plus, this.xMax, y_plus);
                intersectionsY[j] = y_plus;

                y_plus += this.spacingY;
                j++;
            }

            if (intersectionsY.Length == (j + 2))
            {
                intersectionsY[j + 1] = y_plus;
            }

            IProbe[,] probes = new IProbe[intersectionsX.Length, intersectionsY.Length];
            PointF point = new PointF(0, 0);
            for (int x = 0; x < intersectionsX.Length; x++)
            {
                for (int y = 0; y < intersectionsY.Length; y++)
                {
                    point.X = intersectionsX[x] - spacingX / 2f;
                    point.Y = intersectionsY[y] - spacingY / 2f;

                    IProbe probe = new Probe(point, 0, 0);
                    probes[x, y] = probe;
                    probe.Calc(this.startTime, this.charges);
                }
            }

            float spacing = Math.Min(spacingX, spacingY);
            for (int x = 0; x < intersectionsX.Length; x++)
            {
                for (int y = 0; y < intersectionsY.Length; y++)
                {
                    probes[x, y].Draw(g, startTime, charges, scale, spacing);
                }
            }
        }

        public void Draw(Graphics g, PointF topLeft, PointF bottomRight, float tipLength, float scale)
        {
            Color colorAxes = Color.FromArgb(80, Color.White);
            Color colorGrid = Color.FromArgb(40, Color.White);

            Pen penAxes = new Pen(colorAxes, 2f / scale);
            Pen penGrid = new Pen(colorGrid, 1f / scale);


            Brush brush = new SolidBrush(colorAxes);
            Brush brushStr = new SolidBrush(Color.White);

            tipLength = 1f / (float)Math.Sqrt(scale);
            DrawAxes(g, penAxes, penGrid, brush, brushStr, topLeft, bottomRight, tipLength, scale);
        }
    }
}