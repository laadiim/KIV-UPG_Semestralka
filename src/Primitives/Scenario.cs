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

        // First loop: Calculate the sum of charges
        foreach (var charge in charges)
        {
            if (charge != null)
            {
                count_ch += 1;
                sum_ch += Math.Abs(charge.GetCharge());
            }
        }

        
        if (sum_ch > 0) // Ensure sum is greater than 0 to avoid division by zero
        {
            // Second loop: Adjust radius based on the sum of charges
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
                    charge.SetRadius(0.5f);
                }
            }
        }

        Tuple<float[], float[]> positions = GetPositions();

        float xMax = positions.Item1.Max();
        float xMin = -xMax;
        float yMax = positions.Item2.Max();
        float yMin = -yMax;
        
        float viewportWidth = xMax - xMin;
        float viewportHeight = yMax - yMin;

        if (chargesCount == 1)
        {
            xMin -= viewportWidth * 2;
            xMax += viewportWidth * 2;
            yMin -= viewportHeight * 2;
            yMax += viewportHeight * 2;

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
        
        PointF center = new PointF((xMax + xMin) / 2f, (yMax + yMin) / 2f);

        /* pro debug
        Console.WriteLine(scale);
        Console.WriteLine(center.ToString());
        Console.WriteLine(xMin + ", " + yMin + ", " + xMax + ", " + yMax);
        */

        LinearGradientBrush brush_scen = new LinearGradientBrush(new PointF(xMin, yMin), new PointF(xMax, yMax),
                                                                 Color.DarkBlue, Color.DarkCyan);

        brush_scen.InterpolationColors = new ColorBlend()
        {
            Colors = new Color[] {
                    Color.FromArgb(200, Color.MidnightBlue),
                    Color.FromArgb(200, Color.FromArgb(200, 30, 70, 100)),
                    Color.FromArgb(200, Color.FromArgb(200, 30, 130, 140))
                },
            Positions = new float[] { 0f, 0.4f, 1f }
        };

        g.FillRectangle(brush_scen, xMin, yMin, xMax - xMin, yMax - yMin);


        IGrid grid = new Grid();

        Color color = Color.FromArgb(80, Color.White);

        Pen pen = new Pen(color, 2 / scale);
        Brush brush = new SolidBrush(color);

        float tipLength = 10f;

        grid.Draw(g, new PointF(xMin, yMin), new PointF(xMax, yMax), pen, brush, tipLength / scale);

        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] != null) charges[i].Draw(g, center, scale);
        }
        
        Probe probe = new Probe(new PointF(0, 0));
        probe.Draw(g, startTime, this.charges, scale);
    }
}