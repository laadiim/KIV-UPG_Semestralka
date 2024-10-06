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

    public void Draw(Graphics g)
    {
        Tuple<float[], float[]> positions = GetPositions();

        float x_min = positions.Item1.Min();
        float x_max = positions.Item1.Max();
        float y_min = positions.Item2.Min();
        float y_max = positions.Item2.Max();

        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] != null) charges[i].Draw(g);
        }
    }
}