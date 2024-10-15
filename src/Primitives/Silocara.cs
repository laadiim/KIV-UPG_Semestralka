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
            return; // Exit the method if no charges
        }
    
        Vector2 electricField = Vector2.Zero; // Initialize the electric field to zero
        Vector2 x = new Vector2(start.X, start.Y); // Start point
        Vector2 newPoint = Vector2.Zero;
        Vector2 force = Vector2.Zero;

        const float stepSize = 0.1f; // Use a small, constant step size

        do
        {
            electricField = Vector2.Zero; // Reset the field for each iteration
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

            // Normalize the force and scale the step size based on field strength
            if (force.Length() == 0)
            {
                break; // Stop if the force becomes zero
            }

            newPoint = x + stepSize * force / force.Length(); // Take a small step in the force direction
            x = newPoint;
            this.points.AddLast(new PointF(newPoint.X, newPoint.Y));

        } while (force.Length() > epsilon && points.Count < 100); // Limit the number of points
    }


    public void Draw(Graphics g, PointF center, float scale)
    {
        PointF[] pointsArray = new PointF[this.points.Count];
        this.points.CopyTo(pointsArray, 0);

        for (int i = 0; i < pointsArray.Length; i++)
        {
            // First apply scaling
            pointsArray[i].X = pointsArray[i].X * scale;
            pointsArray[i].Y = pointsArray[i].Y * scale;

            // Then apply the translation for centering
            pointsArray[i].X += center.X;
            pointsArray[i].Y += center.Y;
        }

        // Only draw if there are at least 2 points to form a line
        if (pointsArray.Length > 1)
        {
            g.DrawLines(new Pen(Brushes.Black, 1), pointsArray);
        }
    }

}