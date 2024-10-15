using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Silocara
{
    private LinkedList<PointF> points = new LinkedList<PointF>();
    private PointF start;
    private const double e = 8.854E-12;
    private const float k = 1 / (float)(4 * Math.PI * e);
    private float epsilon;
   
    
    public Silocara(float epsilon, PointF startPoint, INaboj[] charges, int chargesCount)
    {
        this.epsilon = epsilon;
        this.start = startPoint;
        this.points.AddLast(startPoint);
        INaboj[] tmp = new INaboj[chargesCount];
        int j = 0;
        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] == null) continue;
            tmp[j] = charges[i];
            j++;
        }

        Console.WriteLine(tmp.Length);
        this.Eval(tmp);
    }

    private void Eval(INaboj[] charges)
    {
         if (charges == null || charges.Length == 0)
         {
             Console.WriteLine("No charges");
         }   
        Vector2 electricField = Vector2.Zero; // Initialize the electric field to zero
        Vector2 x = new Vector2(start.X, start.Y);
        Vector2 newPoint = Vector2.Zero;
        Vector2 force = Vector2.Zero;

        do
        {
            Console.WriteLine(points.Count);
            electricField = Vector2.Zero;
            for (int i = 0; i < charges.Length; i++)
            {
                PointF point = charges[i].GetPosition();
                Vector2 r = x - new Vector2(point.X, point.Y); // Vector from charge to observation point
                double rMagnitude = r.Length(); // Magnitude of vector r

                if (rMagnitude == 0)
                {
                    continue; // Avoid division by zero (if the charge is exactly at the point)
                }

                // Contribution of charge i to the electric field
                Vector2 contribution = (float)(charges[i].GetCharge() / Math.Pow(rMagnitude, 3)) * r;
                electricField += contribution;
            }

            force = k * electricField;
            newPoint = x + 10 * force / force.Length();
            x = newPoint;
            this.points.AddLast(new PointF(newPoint.X, newPoint.Y));
            Console.WriteLine(newPoint.ToString());
        }
        while (force.Length() > epsilon && points.Count < 10);
    }

    public void Draw(Graphics g, PointF center, float scale)
    {
        PointF[] points = new PointF[this.points.Count];
        for (int i = 0; i < points.Length; i++)
        {
            points[i].X = scale * (points[i].X + center.X);
            points[i].Y = scale * (points[i].Y + center.Y);
            Console.WriteLine(points[i].ToString());
        }

        g.DrawLines(new Pen(Brushes.Black, 1), points);
    }
}