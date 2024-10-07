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

    public void Draw(Graphics g, Panel panel)
    {
        Tuple<float[], float[]> positions = GetPositions();

        float xMin = positions.Item1.Min();
        float xMax = positions.Item1.Max();
        float yMin = positions.Item2.Min();
        float yMax = positions.Item2.Max();
        
        xMin -= panel.Width / 10f;
        xMax += panel.Width / 10f;
        yMin -= panel.Height / 10f;
        yMax += panel.Height / 10f;
        
        int height = panel.Size.Height;
        int width = panel.Size.Width;
        
        float scaleX = width / (xMax - xMin);
        float scaleY = height / (yMax - yMin);
        float scale;
        if (scaleX > scaleY)
        {
            scale = scaleY;
            float difX = width - scale * (xMax - xMin);
            xMax += difX / 2;
            xMin -= difX / 2;
        }
        else
        {
            scale = scaleX;
            float difY = height - scale * (yMax - yMin);
            yMax += difY / 2;
            yMin -= difY / 2;
        }
        
        PointF center = new PointF((xMax - xMin) / 2, (yMax - yMin) / 2);

        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] != null) charges[i].Draw(g);
        }
    }
}