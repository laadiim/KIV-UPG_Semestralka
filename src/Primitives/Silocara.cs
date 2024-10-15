using System.Numerics;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Silocara
{
    private PointF start;
    private LinkedList<PointF> points;
    private int amount;
    private float epsilon;
    private const float k = 8.9875517923E9f;

    public Silocara(PointF start, float epsilon)
    {
        this.start = start;
        this.epsilon = epsilon;
        this.points.AddLast(start);
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
    
}