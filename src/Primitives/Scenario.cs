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

    /// <summary>
    /// ulozi vsechny krajni pozice na mape
    /// </summary>
    /// <returns>dvojice seznamu s pozicemi x a y</returns>
    private Tuple<float[], float[]> GetPositions()
    {
        int j = 0;
        float[] positionsX = new float[chargesCount * 2];
        float[] positionsY = new float[chargesCount * 2];
        for (int i = 0; i < charges.Length;i++)
        {
            if (charges[i] != null)
            {
                positionsX[j] = Math.Abs(charges[i].GetPosition().X - charges[i].GetRadius());
                positionsY[j] = Math.Abs(charges[i].GetPosition().Y - charges[i].GetRadius());
                j++;
                positionsX[j] = Math.Abs(charges[i].GetPosition().X + charges[i].GetRadius());
                positionsY[j] = Math.Abs(charges[i].GetPosition().Y + charges[i].GetRadius());
                j++;
            }
        }
        return Tuple.Create(positionsX, positionsY);
    }

    public void Draw(Graphics g, float width, float height, int startTime)
    {
        float sum_ch = 0;
        int count_ch = 0;

        // spocitame sumu hodnoty naboju a pocet jednotlivych naboju
        foreach (var charge in charges)
        {
            if (charge != null)
            {
                count_ch += 1;
                sum_ch += Math.Abs(charge.GetCharge());
            }
        }

        if (sum_ch > 0) // zajistime, abychom nedelili nulou
        {
            // upravime polomer na zaklade velikosti naboje
            foreach (var charge in charges)
            {
                if (charge != null & count_ch >= 2)
                {
                    float currentCharge = (float)Math.Sqrt(Math.Abs(charge.GetCharge()));
                    
                    float newRadius = (float)Math.Sqrt(charge.GetRadius() * 1.5f * currentCharge / sum_ch);
                    charge.SetRadius(newRadius); 
                }
                else if(charge != null & count_ch == 1)
                {
                    charge.SetRadius(0.7f);
                }
            }
        }


        // ziskani krajnich pozic naboju
        Tuple<float[], float[]> positions = GetPositions();
        float xMax = 1, yMax = 1;
        float xMin = -1, yMin = -1;
        if (positions.Item1.Length != 0 && positions.Item2.Length != 0)
        { 
            xMax = positions.Item1.Max();
            xMin = -xMax;
            yMax = positions.Item2.Max();
            yMin = -yMax;
        }

        
        float viewportWidth = xMax - xMin;
        float viewportHeight = yMax - yMin;

        // v pripade 1 naboje upravime velikost panelu tak, aby nebyl moc velky
        if (chargesCount == 1)
        {
            xMin -= viewportWidth * 1.3f;
            xMax += viewportWidth * 1.3f;
            yMin -= viewportHeight * 1.3f;
            yMax += viewportHeight * 1.3f;

        }
        
        else
        {
            xMin -= viewportWidth / 4f;
            xMax += viewportWidth / 4f;
            yMin -= viewportHeight / 4f;
            yMax += viewportHeight / 4f;
        }
        
        float scaleX = width / (xMax - xMin);
        float scaleY = height / (yMax - yMin);
        float scale;
        
        // upravime xMax a xMin tak, aby stred scenaria byl pokazde ve stredu panelu
        if (scaleX > scaleY)
        {
            scale = scaleY;
            float difX = width - scale * (xMax - xMin);
            xMax = xMax + difX / (2 * scale);
            xMin = xMin - difX / (2 * scale);

        }
        // upravime yMax a yMin tak, aby stred scenaria byl pokazde ve stredu panelu
        else
        {
            scale = scaleX;
            float difY = height - scale * (yMax - yMin);
            yMax = yMax + difY / (2 * scale);
            yMin = yMin - difY / (2 * scale);
        }
        
        g.ScaleTransform(scale, scale);
        
        PointF center = new PointF((xMax + xMin) / 2f, (yMax + yMin) / 2f);


        // kresleni pozadi pro scenar
        LinearGradientBrush brush_scen = new LinearGradientBrush(new PointF(xMin, yMin), new PointF(xMax, yMax),
                                                                 Color.DarkBlue, Color.DarkCyan);
        brush_scen.InterpolationColors = new ColorBlend()
        {
            Colors = new Color[] {
                    Color.FromArgb(200, Color.FromArgb(255, 10, 30, 70)),
                    Color.FromArgb(200, Color.FromArgb(255, 30, 70, 100)),
                    Color.FromArgb(200, Color.FromArgb(255, 30, 130, 140))
                },
            Positions = new float[] { 0f, 0.4f, 1f }
        };
        g.FillRectangle(brush_scen, xMin, yMin, xMax - xMin, yMax - yMin);


        // kresleni mrizky
        IGrid grid = new Grid();
        float tipLength = 10f; // nastaveni velikosti sipky
        grid.Draw(g, new PointF(xMin, yMin), new PointF(xMax, yMax), tipLength / scale, scale);


        // kresleni naboju
        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] != null) charges[i].Draw(g, center, scale);
        }
        

        // kresleni sondy s vektorem intenzity
        Probe probe = new Probe(new PointF(0, 0));
        probe.Draw(g, startTime, this.charges, scale);
    }
}