using Accessibility;
using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Scenario : IScenario
{
    INaboj?[] charges = new INaboj?[1];
    int freeIndex = 0;
    int chargesCount = 0;

    public INaboj[] GetCharges()
    {
        return charges;
    }

    public void AddCharge(INaboj naboj)
    {
        charges[freeIndex] = naboj;
        chargesCount++;
            
        for (; freeIndex < charges.Length; freeIndex++)
        {
            if (charges[freeIndex] == null) return;
        }

        INaboj[] newCharges = new INaboj[charges.Length*2];

        for (int i = 0; i < charges.Length; i++)
        {
            newCharges[i] = charges[i];
        }
        freeIndex = charges.Length;
        charges = newCharges;
    }

    public INaboj RemoveCharge(INaboj naboj)
    {
        return RemoveCharge(naboj.GetID());
    }

    public INaboj RemoveCharge(int id)
    {
        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] != null)
            {
                if (charges[i].GetID() == id)
                {
                    INaboj charge = charges[i];
                    charges[i] = null;
                    chargesCount--;

                    if (i < freeIndex)
                    {
                        freeIndex = i;
                    }
                    return charge;
                }

            }
        }
        throw new Exception("naboj nebyl nalezen");
    }

    public Tuple<float[], float[]> GetPositions()
    {
        int j = 0;
        float[] positionsX = new float[chargesCount * 2];
        float[] positionsY = new float[chargesCount * 2];
        for (int i = 0; i < charges.Length;i++)
        {
            if (charges[i] != null)
            {
                positionsX[j] = charges[i].GetPosition().X - charges[i].GetRadius();
                positionsY[j] = charges[i].GetPosition().Y - charges[i].GetRadius();
                j++;
                positionsX[j] = charges[i].GetPosition().X + charges[i].GetRadius();
                positionsY[j] = charges[i].GetPosition().Y + charges[i].GetRadius();
                j++;
            }
        }
        return Tuple.Create(positionsX, positionsY);
    }

    public void Draw(Graphics g, float width, float height, int startTime)
    {
        Tuple<float[], float[]> positions = GetPositions();

        float xMin = positions.Item1.Min();
        float xMax = positions.Item1.Max();
        float yMin = positions.Item2.Min();
        float yMax = positions.Item2.Max();
        
        float viewportWidth = xMax - xMin;
        float viewportHeight = yMax - yMin;
        
        xMin -= viewportWidth / 10f;
        xMax += viewportWidth / 10f;
        yMin -= viewportHeight / 10f;
        yMax += viewportHeight / 10f;
        
        
        float scaleX = width / (xMax - xMin);
        float scaleY = height / (yMax - yMin);
        float scale;
        if (scaleX > scaleY)
        {
            scale = scaleY;
            float difX = width - scale * (xMax - xMin);
            xMax = xMax + difX / (2 * scale);
            xMin = xMin - difX / (2 * scale);

        }
        else
        {
            scale = scaleX;
            float difY = height - scale * (yMax - yMin);
            yMax = yMax + difY / (2 * scale);
            yMin = yMin - difY / (2 * scale);
        }
        
        g.ScaleTransform(scale, scale);
        
        PointF center = new PointF((xMax + xMin) / 2, (yMax + yMin) / 2);
        Console.WriteLine(scale);
        Console.WriteLine(center.ToString());
        Console.WriteLine(xMin + ", " + yMin + ", " + xMax + ", " + yMax);
        
        //g.DrawLine(new Pen(Color.Black, 1), new PointF(1, 0), new PointF(-1, 0));
        //g.FillEllipse(new SolidBrush(Color.Black), center.X - 1, center.Y - 1, 2, 2);

        //TODO: grafika pozadi

        IGrid grid = new Grid();

        Color color = Color.FromArgb(150, Color.LightCoral);

        Pen pen = new Pen(color, 2 / scale);
        Brush brush = new SolidBrush(color);

        float tipLength = 10f;

        grid.Draw(g, new PointF(xMin, yMin), new PointF(xMax, yMax), pen, brush, tipLength / scale);

        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] != null) charges[i].Draw(g, center, scale);
        }
        
        Probe probe = new Probe(new PointF(0, 0));
        probe.Draw(g, startTime);
    }
}