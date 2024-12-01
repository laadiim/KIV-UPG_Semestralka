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

        public Grid(float[] corners, int startTime, INaboj[] charges, float scale, int spacingXpixels, int spacingYpixels, float viewportWidth, float viewportHeight)
        {
            this.xMin = corners[2];
            this.xMax = corners[0];
            this.yMin = corners[3];
            this.yMax = corners[1];

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
        private static void DrawArrows(Graphics g, Brush brush, PointF rightCenter, PointF topCenter, float tipLength)
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
        private void DrawAxes(Graphics g, Pen penAxes, Pen penGrid, Brush brush, Brush brushStr, float scale)
        {
            PointF bottomCenter = new PointF(SettingsObject.worldCenter.X, this.yMin - SettingsObject.worldCenter.Y);
            PointF topCenter = new PointF(SettingsObject.worldCenter.X, this.yMax - SettingsObject.worldCenter.Y);
            PointF leftCenter = new PointF(this.xMin - SettingsObject.worldCenter.X, SettingsObject.worldCenter.Y);
            PointF rightCenter = new PointF(this.xMax - SettingsObject.worldCenter.X, SettingsObject.worldCenter.Y);

            Font font = new Font("Arial", 15f / scale, FontStyle.Bold);

            float len = 20f / scale;
            g.DrawLine(penAxes, rightCenter - new SizeF(len, 0), leftCenter);
            g.DrawString("x", font, brushStr, rightCenter.X - len * 1.2f, (rightCenter.Y + len / 1.8f));

            g.DrawLine(penAxes, bottomCenter + new SizeF(0, len), topCenter);
            g.DrawString("y", font, brushStr, bottomCenter.X - len, bottomCenter.Y + len);

            DrawArrows(g, brush, rightCenter, bottomCenter, len);

            if (SettingsObject.gridShown)
            {
                DrawGrid(g, penGrid);
            }
        }
        private void DrawGrid(Graphics g, Pen pen)
        {
            float top = this.yMax - SettingsObject.worldCenter.Y;
            float bottom = this.yMin - SettingsObject.worldCenter.Y;
            float left = this.xMin - SettingsObject.worldCenter.X;
            float right = this.xMax - SettingsObject.worldCenter.X;

            float dx1 = left - SettingsObject.worldCenter.X;
            float dx2 = right - SettingsObject.worldCenter.X;
            float dy1 = bottom - SettingsObject.worldCenter.Y;
            float dy2 = top - SettingsObject.worldCenter.Y;

            float nx1 = dx1 / spacingX;
            float nx2 = dx2 / spacingX;
            float ny1 = dy1 / spacingY;
            float ny2 = dy2 / spacingY;

            List<float> xFloats = new List<float>();
            List<float> yFloats = new List<float>();

            for (int i = 0; i > nx1; i--)
            {
                g.DrawLine(pen, SettingsObject.worldCenter.X + i * this.spacingX, bottom, SettingsObject.worldCenter.X + i * this.spacingX, top);
                xFloats.Add(SettingsObject.worldCenter.X + i * this.spacingX - this.spacingX/2);
            }
            for (int i = 0; i <= nx2; i++)
            {
                g.DrawLine(pen, SettingsObject.worldCenter.X + i * this.spacingX, bottom, SettingsObject.worldCenter.X + i * this.spacingX, top);
                xFloats.Add(SettingsObject.worldCenter.X + i * this.spacingX + this.spacingX/2);
            }
            for (int i = 0; i > ny1; i--)
            {
                g.DrawLine(pen, left, SettingsObject.worldCenter.Y + i * this.spacingY, right, SettingsObject.worldCenter.Y + i * this.spacingY);
                yFloats.Add(SettingsObject.worldCenter.Y + i * this.spacingY - this.spacingY/2);
            }
            for (int i = 0; i <= ny2; i++)
            { 
                g.DrawLine(pen, left, SettingsObject.worldCenter.Y + i * this.spacingY, right, SettingsObject.worldCenter.Y + i * this.spacingY);
                yFloats.Add(SettingsObject.worldCenter.Y + i * this.spacingY + this.spacingY / 2);
            }

            List<IProbe> probes = new List<IProbe>();
            for (int i = 0; i < xFloats.Count; i++)
            {
                for (int j = 0; j < yFloats.Count; j++)
                {
                    IProbe p = new Probe(new PointF(xFloats[i], yFloats[j]), 0, 0, -1);
                    p.Calc(this.startTime, this.charges);
                    p.Draw(g, startTime, charges, scale, Math.Min(spacingX, spacingY), true);
                }
            }

        }

        public void Draw(Graphics g, float tipLength, float scale)
        {
            Color colorAxes = Color.FromArgb(80, Color.White);
            Color colorGrid = Color.FromArgb(40, Color.White);

            Pen penAxes = new Pen(colorAxes, 2f / scale);
            Pen penGrid = new Pen(colorGrid, 1f / scale);


            Brush brush = new SolidBrush(colorAxes);
            Brush brushStr = new SolidBrush(Color.White);

            tipLength = 1f / (float)Math.Sqrt(scale);
            DrawAxes(g, penAxes, penGrid, brush, brushStr, scale);
        }
    }
}

/*
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
           probes[x, y].Draw(g, startTime, charges, scale, spacing, true);
       }
   }
*/