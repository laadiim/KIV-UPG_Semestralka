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

    public void Draw(Graphics g, float width, float height)
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
            float difX = width - (xMax - xMin);
            xMax = xMax + difX / 2;
            xMin = xMin - difX / 2;

        }
        else
        {
            scale = scaleX;
            float difY = height - (yMax - yMin);
            yMax = yMax + difY / 2;
            yMin = yMin - difY / 2;
        }
        
        g.ScaleTransform(scale, scale);
        
        PointF center = new PointF((xMax + xMin) / 2, (yMax + yMin) / 2);
        Console.WriteLine(scale);
        Console.WriteLine(center.ToString());
        Console.WriteLine(xMin + ", " + yMin + ", " + xMax + ", " + yMax);
        
        //g.DrawLine(new Pen(Color.Black, 1), new PointF(1, 0), new PointF(-1, 0));
        //g.FillEllipse(new SolidBrush(Color.Black), center.X - 1, center.Y - 1, 2, 2);

        IGrid grid = new Grid();
        grid.Draw(g, new PointF(xMin,yMin), new PointF(xMax, yMax));

        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] != null) charges[i].Draw(g, center, scale);
        }
    }
}