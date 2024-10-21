using System.Drawing;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using UPG_SP_2024.Interfaces;
using static System.Windows.Forms.AxHost;

namespace UPG_SP_2024.Primitives;

public class Probe
{
    private PointF center;
    private float radius;
    private float anglePerSecond;

    public Probe(PointF center, float radius = 1f, float anglePerSecond = MathF.PI / 6)
    {
        this.center = center;
        this.radius = radius;
        this.anglePerSecond = anglePerSecond;
    }

    public void Draw(Graphics g, int startTime, INaboj[] charges, float scale)
    {
        float angle = anglePerSecond * (Environment.TickCount - startTime) / 1000;
        Vector2 start = new Vector2(center.X - radius * MathF.Sin(angle), center.Y - radius * MathF.Cos(angle));
        Vector2 end = new Vector2(0, 0);
        float k = 8.9875517923E9f;
        Vector2 sum = Vector2.Zero;
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
        end = start + sum; // vektor konce sipky

        PointF[] points = new PointF[2];
        points[0] = new PointF(start.X, start.Y); // bod zacatku sipky
        points[1] = new PointF(end.X, end.Y); // bod konce sipky

        Color color = Color.FromArgb(120, Color.White);
        Brush brush = new SolidBrush(Color.White);
        float r = 0.3f / (float)Math.Sqrt(scale);

        g.TranslateTransform(points[0].X, points[0].Y);

        float len = sum.Length() * 100;
        string label = $"{len.ToString("n2")}E-2 TN/C";
        Font font = new Font("Arial", 1f / (float)Math.Sqrt(scale), FontStyle.Bold);
        float width = g.MeasureString(label, font).Width;
        float height = g.MeasureString(label, font).Height;

        g.DrawString(label, font, brush, 3/2 * r, - 6 * r);

        g.FillEllipse(brush, -r, -r, 2 * r, 2 * r);

        if (sum.X > 2E9 || sum.Y > 2E9)
        {
            sum /= 10E6f;
        }

        DrawArrow(g, sum, scale, color);

        g.TranslateTransform(- points[0].X, - points[0].Y);
    }

    private void DrawArrow(Graphics g, Vector2 sum, float scale, Color color)
    {
        float x = sum.X / 2;
        float y = sum.Y / 2;

        float tipLen = 30f / scale;

        float norma = 1 / (float)sum.Length();

        // vektor u bude jednotkovy
        float u_x = x * norma;
        float u_y = y * norma;

        float Clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        float endX = Clamp(x, -10000f, 10000f);
        float endY = Clamp(y, -10000f, 10000f);

        g.DrawLine(new Pen(color, 5 / scale), 0, 0, endX, endY);

        var points_arrow = new PointF[3];
        points_arrow[0] = new PointF(x - u_y * tipLen / 2, y + u_x * tipLen / 2);
        points_arrow[1] = new PointF(x + u_x * tipLen, y + u_y * tipLen);
        points_arrow[2] = new PointF(x + u_y * tipLen / 2, y - u_x * tipLen / 2);

        Brush brush = new SolidBrush(color);

        g.FillPolygon(brush, points_arrow);
    }
}