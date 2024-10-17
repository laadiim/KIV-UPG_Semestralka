using System.Numerics;
using UPG_SP_2024.Interfaces;

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

    public void Draw(Graphics g, int startTime, INaboj[] charges)
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
            sum += charges[i].GetCharge() * vect / (vect.Length() * vect.Length() * vect.Length());
        }
        sum *= k;
        end = start + sum;
        end *= 0.000001f;
        PointF[] points = new PointF[2];
        points[0] = new PointF(end.X, end.Y);
        points[1] = new PointF(start.X, start.Y);
        Console.WriteLine("probe");
        Console.WriteLine(points[1].ToString());
        Console.WriteLine(points[0].ToString());
        g.DrawLine(new Pen(Color.Black, 0.1f), points[0], points[1]);
    }
}