using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Transactions;
using NCalc;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Probe : IProbe
{
    private PointF center;
    private float radius;
    private float anglePerSecond;
    private Vector2 v;
    private long ticks = 0;
    public List<Tuple<float, float>> values;
    public int id;
    private float r;
    private float timeHeld = 0;

    public Probe(PointF center, float radius, float anglePerSecond, int id)
    {
        this.center = center;
        this.radius = radius;
        this.anglePerSecond = anglePerSecond;
        this.v = Vector2.Zero;
		this.values = new List<Tuple<float, float>>();
        this.id = id;
        this.r = 14;
    }

    public float GetRadius()
    {
        return this.radius;
    }

    public void SetRadius(float newRadius)
    {
        this.radius = newRadius;
    }

    public PointF GetCenter()
    {
        return this.center;
    }

    public void SetCenter(PointF newCenter)
    {
        this.center = newCenter;
    }

    public float GetAnglePerSecond()
    {
        return this.anglePerSecond;
    }

    public void SetAnglePerSecond(float newAngle)
    {
        this.anglePerSecond = newAngle;
    }

    public void SetAnglePerSecond(string newAngle)
    {
        Expression e = new Expression(newAngle);
        this.anglePerSecond = Convert.ToSingle(e.Evaluate());
    }

    public void AddTimeHeld(float t)
    {
        timeHeld += t;
    }

    public int GetID()
    {
        return this.id;
    }

    public bool IsHit(PointF point)
    {
        if (radius == 0)
        {
            float distance = Vector2.Distance(new Vector2(point.X, point.Y), new Vector2(center.X, center.Y));
            return distance <= r / SettingsObject.scale;
        }
        else
        {
            float angle = anglePerSecond * (Environment.TickCount - SettingsObject.startTime - timeHeld) / 1000;
            Vector2 start = new Vector2(center.X - radius * MathF.Sin(angle), center.Y - radius * MathF.Cos(angle));
            float distance = Vector2.Distance(new Vector2(point.X, point.Y), start);
            return distance <= r / SettingsObject.scale;
        }
    }

    public void Drag(PointF point)
    {
        this.center.X += point.X;
        this.center.Y += point.Y;
        SettingsObject.probeForm.Refresh(this.id);
    }
    public string Save()
    {
        return $"sonda:{this.center.X};{this.center.Y};{this.radius};{this.anglePerSecond}";
    }

    public void Tick()
    {
        this.ticks++;
    }

    public void Draw(Graphics g, INaboj[] charges, float scale, float spacing, bool grid)
    {
        float angle = anglePerSecond * (Environment.TickCount - SettingsObject.startTime - timeHeld) / 1000;
        Vector2 start = new Vector2(center.X - radius * MathF.Sin(angle), center.Y - radius * MathF.Cos(angle));
        if (!grid) start += new Vector2(SettingsObject.worldCenter.X, SettingsObject.worldCenter.Y);

        if (this.v.Length() == 0)
        {
            if (charges == null)
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

        if (!grid)
        {
            float len = this.v.Length() * 100;
            string label = $"{len.ToString("n2")}E-2 TN/C";

            Font font = new Font("Arial", 12 * l / scale, FontStyle.Bold);
            Font font_id = new Font("Arial", 18 * l / scale, FontStyle.Bold);

            g.DrawString(label, font, brush, this.r/SettingsObject.scale * 1.1f, -2 * this.r / SettingsObject.scale);

            g.FillEllipse(brush, -this.r / SettingsObject.scale, -this.r / SettingsObject.scale, 2 * this.r / SettingsObject.scale, 2 * this.r / SettingsObject.scale);

            string id = this.id.ToString();
            g.DrawString(id, font_id, new SolidBrush(Color.MidnightBlue), -g.MeasureString(id, font_id).Width / 2f, -g.MeasureString(id, font_id).Height / 2f);
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
        float angle = anglePerSecond * (Environment.TickCount - SettingsObject.startTime - timeHeld) / 1000;
        Vector2 start = new Vector2(center.X - radius * MathF.Sin(angle) + SettingsObject.worldCenter.X, center.Y - radius * MathF.Cos(angle) + SettingsObject.worldCenter.Y);

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
