using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Silocara
{
    private LinkedList<PointF> points = new LinkedList<PointF>();
    private int amount;
    private PointF start;
    private const double e = 8.854E-12;
    private const double k = 1 / (4 * Math.PI * e);
    
    public Silocara(int amount, PointF startPoint)
    {
        this.amount = amount;
        this.start = startPoint;
        this.points.AddLast(startPoint);
        
    }

    private void Eval(INaboj[] charges)
    {
        
        Vector3 electricField = Vector3.Zero; // Initialize the electric field to zero

        for (int i = 0; i < charges.Length; i++)
        {
            Vector3 r = x - chargePositions[i]; // Vector from charge to observation point
            double rMagnitude = r.Length();     // Magnitude of vector r

            if (rMagnitude == 0)
            {
                continue; // Avoid division by zero (if the charge is exactly at the point)
            }

            // Contribution of charge i to the electric field
            Vector3 contribution = (float)(charges[i] / Math.Pow(rMagnitude, 3)) * r;
            electricField += contribution;
        }
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