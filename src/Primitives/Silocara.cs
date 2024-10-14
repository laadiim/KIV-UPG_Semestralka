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
    
    public Silocara(float epsilon, PointF startPoint)
    {
        this.epsilon = epsilon;
        this.start = startPoint;
        this.points.AddLast(startPoint);
        
    }

    private void Eval(INaboj[] charges)
    {
        
        Vector2 electricField = Vector2.Zero; // Initialize the electric field to zero
        Vector2 x = new Vector2(start.X, start.Y);
        Vector2 newPoint = Vector2.Zero;

        do
        {
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
                electricField = contribution;
            }

            Vector2 force = k * electricField;
            newPoint = x + force;
            this.points.AddLast(new PointF(newPoint.X, newPoint.Y));
        }
        while ((newPoint - x).Length() > epsilon);
    }

    private double Sum(PointF point, INaboj[] charges)
    {
        Vector<double> sum = new Vector<double>();
        for (int i = 0; i < charges.Length; i++)
        {
            Vector<double> p = new Vector<double>();
            p[1] = point.X;
            sum += charges[i].GetCharge() * Vector.Subtract(new)
        }
    }
}