using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Transactions;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Probe : IProbe
{
    private PointF center;
    readonly private float radius;
    readonly private float anglePerSecond;
    private Vector2 v;
    private long ticks = 0;
    public List<Tuple<float, float>> values;

    public Probe(PointF center, float radius = 1f, float anglePerSecond = MathF.PI / 6)
    {
        this.center = center;
        this.radius = radius;
        this.anglePerSecond = anglePerSecond;
        this.v = Vector2.Zero;
		this.values = new List<Tuple<float, float>>();
    }

    public void Tick()
    {
        this.ticks++;
    }

    public void Draw(Graphics g, int startTime, INaboj[] charges, float scale, float spacing, bool grid)
    {
        float angle = anglePerSecond * (Environment.TickCount - startTime) / 1000;
        Vector2 start = new Vector2(center.X - radius * MathF.Sin(angle), center.Y - radius * MathF.Cos(angle));

        if (this.v.Length() == 0)
        {
            if (charges == null || charges.Length == 0)
            {
                throw new ArgumentException("Scenario neobsahuje naboje");
            }
            else
            {
                this.Calc(start, charges);
            }  
        }

        Vector2 end = start + this.v;

        PointF[] points =
        [
            new PointF(start.X, start.Y), // bod zacatku sipky
            new PointF(end.X, end.Y)  // bod konce sipky
        ];

        Color color = Color.FromArgb(120, Color.White);
        Brush brush = new SolidBrush(Color.White);
        float r = 0.3f / (float)Math.Sqrt(scale); 

        g.TranslateTransform(points[0].X, points[0].Y);

        float l;

        if (SettingsObject.corners == null || SettingsObject.corners.Length < 4)
        {
            throw new InvalidOperationException("SettingsObject.corners is not properly initialized.");
        }
        else
        {
            l = Math.Min((SettingsObject.corners[0] - SettingsObject.corners[2]) / 2,
                (SettingsObject.corners[1] - SettingsObject.corners[3]) / 2);
            l = l != 0 ? l : 1;
        }  

        if (!grid)
        {
            float len = this.v.Length() * 100;
            string label = $"{len.ToString("n2")}E-2 TN/C";

            Font font = new Font("Arial", 12 * l / scale, FontStyle.Bold);

            g.DrawString(label, font, brush, 3 / 2 * r, -6 * r);

            g.FillEllipse(brush, -r, -r, 2 * r, 2 * r);
        }

        if (this.v.X > 2E9 || this.v.Y > 2E9)
        {
            this.v /= 10E6f;
        }

        if (grid)
        {
            Color color_arr_grid = Color.FromArgb(150, 240, 220, 250);
            DrawArrow(g, this.v, scale, color_arr_grid, spacing, grid);
        } 
        else
        {
            DrawArrow(g, this.v, scale, color, l, grid);
        }

        g.TranslateTransform(-points[0].X, -points[0].Y);
    }

    /// <summary>
    /// vykresli sipku pro znazorneni vektoru intenzity
    /// </summary>
    /// <param name="g">graficky kontext</param>
    /// <param name="sum">vysledny vektor</param>
    /// <param name="scale">predany scale</param>
    /// <param name="color">barva sipky</param>
    private void DrawArrow(Graphics g, Vector2 sum, float scale, Color color, float spacing, bool grid)
    {
        float x = sum.X / 3f;
        float y = sum.Y / 3f;

        Vector2 newSum = new Vector2(x, y);

        float norma = 1 / (float)newSum.Length();

        float u_x, u_y;
        float tipLen;

        PointF point;

        if (!grid)
        {
            // vektor u bude jednotkovy
            u_x = x * norma;
            u_y = y * norma;
            tipLen = 20f / scale;
            point = new PointF(u_x * tipLen * 2f, u_y * tipLen * 2f);
            g.DrawLine(new Pen(color, tipLen / 5f), 0, 0, point.X, point.Y);
        }

        else
        {
            norma /= 6f;
            u_x = x * norma;
            u_y = y * norma;
            tipLen = spacing;
            point = new PointF(u_x * 2 * tipLen, u_y * 2 * tipLen);
            g.DrawLine(new Pen(color, tipLen / 15f), 0, 0, point.X, point.Y);
        }

        var points_arrow = new PointF[3];
        points_arrow[0] = new PointF(point.X - u_y * tipLen / 2f, point.Y + u_x * tipLen / 2f);
        points_arrow[1] = new PointF(point.X + u_x * tipLen, point.Y + u_y * tipLen);
        points_arrow[2] = new PointF(point.X + u_y * tipLen / 2f, point.Y - u_x * tipLen / 2f);

        Brush brush = new SolidBrush(color);

        g.FillPolygon(brush, points_arrow);
    }

    public void Calc(Vector2 start, INaboj[] charges)
    {
        Vector2 sum = Vector2.Zero;
        const float k = 8.9875517923E9f; // konstanta - 1/4PI*e0
        
        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] == null) continue;

            PointF p = charges[i].GetPosition();
            Vector2 vect = start - new Vector2(p.X, p.Y);
            float l = vect.Length() > 0 ? (vect.Length() * vect.Length() * vect.Length()) : 0.01f;
            sum += charges[i].GetCharge() * vect / l;
        }
        sum *= k; // vektor intenzity el. pole (Newton/Coulomb)
        sum *= 10E-12f; // prevod na TN/C
        this.v = sum;    
    }

    public void Calc(int startTime, INaboj[] charges)
    {
        Vector2 sum = Vector2.Zero;
        const float k = 8.9875517923E9f; // konstanta - 1/4PI*e0
        float angle = anglePerSecond * (Environment.TickCount - startTime) / 1000;
        Vector2 start = new Vector2(center.X - radius * MathF.Sin(angle), center.Y - radius * MathF.Cos(angle));

        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] == null) continue;

            PointF p = charges[i].GetPosition();
            Vector2 vect = start - new Vector2(p.X, p.Y);
            float l = vect.Length() > 0 ? (vect.Length() * vect.Length() * vect.Length()) : 0.01f;
            sum += charges[i].GetCharge() * vect / l;
        }
        sum *= k; // vektor intenzity el. pole (Newton/Coulomb)
        sum *= 10E-12f; // prevod na TN/C
        this.v = sum;
        if (ticks % 2 == 0) 
        {
            values.Add(new Tuple<float, float>((float)ticks / 20 , (float)sum.Length()));
            //System.Console.WriteLine($"{values.Last().Item1}: {values.Last().Item2}");
        }
    }
}
