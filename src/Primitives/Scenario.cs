using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Scenario : IScenario
{
    INaboj[] charges = new INaboj[1];
    int freeIndex = 0;
    int chargesCount = 0;
    private float scale = 1;

    public float worldWidthHalf = 2;
    public float worldHeightHalf = 2;
    public PointF worldCenterPosition = new PointF(0, 0);

    /* xMax, yMax, xMin, yMin*/
    public float[] corners = new float[4];
	private List<IProbe> probes = new List<IProbe>();

    private readonly double[] boundaries = { 0.0, 0.25, 0.5, 0.75, 1.0 };

    private readonly int[,] colors = {
            { 80, 50, 30 },      // fifth
            { 70, 40, 80 },      // fourth
            { 100, 60, 140 },    // third 
            { 140, 120, 190 },   // second
            { 210, 190, 220 }   // first  
    };

    private readonly double[] boundaryDiffs;
    private readonly int[,] colorDiffs;

    public Scenario()
    {
        boundaryDiffs = new double[boundaries.Length - 1];
        for (int i = 0; i < boundaries.Length - 1; i++)
        {
            boundaryDiffs[i] = boundaries[i + 1] - boundaries[i];
        }

        colorDiffs = new int[colors.GetLength(0) - 1, 3];
        for (int i = 0; i < colors.GetLength(0) - 1; i++)
        {
            colorDiffs[i, 0] = colors[i + 1, 0] - colors[i, 0];
            colorDiffs[i, 1] = colors[i + 1, 1] - colors[i, 1];
            colorDiffs[i, 2] = colors[i + 1, 2] - colors[i, 2];
        }
    }

    public void EmptyCharges()
    { 
        chargesCount = 0;
        freeIndex = 0;
        charges = new INaboj[1];
        SettingsObject.probes.Clear();
        this.CreateProbe(new PointF(0, 0), 1, (float)Math.PI / 6);
    }

    public IProbe CreateProbe(PointF center, float radius, float anglePerSecond)
    {
        IProbe p = new Probe(center, radius, anglePerSecond);
        SettingsObject.probes.Add(p);
		if (SettingsObject.graphForm != null) SettingsObject.graphForm.Reset();
        return p;
    }

    public INaboj[] GetCharges()
    {
        if (charges != null)
        {
            if (charges.Length != 0)
            {
                return charges;
            }
            else throw new Exception("scenario neobsahuje naboje");
        }
        else
        {
            throw new Exception("scenario neobsahuje naboje");
        }
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

        if (charges != null)
        {
            for (int i = 0; i < charges.Length; i++)
            {
                newCharges[i] = charges[i];
            }
            freeIndex = charges.Length;
        } 
        charges = newCharges;
    }

    public INaboj RemoveCharge(INaboj naboj)
    {
        if (charges != null)
        {
            return RemoveCharge(naboj.GetID());
        }
        else
        {
            throw new Exception("scenario neobsahuje naboje");
        }
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
    public INaboj GetCharge(int id)
    {
        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] != null)
            {
                if (charges[i].GetID() == id)
                {
                    return charges[i];
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

    private void DrawColorMap(Graphics g, float width, float height, Func<double, Color> ColorMap)
    {
        var img = new Bitmap((int)width, (int)height, PixelFormat.Format24bppRgb);

        var bmp = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format24bppRgb);

        byte[] pixels = new byte[bmp.Stride * bmp.Height];
        const int squareSize = 3; // Size of the squares
        int widthInSquares = bmp.Width / squareSize;
        int heightInSquares = bmp.Height / squareSize;

        //Parallel.For(0, heightInSquares, squareY =>
        for (int squareY = 0; squareY < heightInSquares; squareY++)
        {
            for (int squareX = 0; squareX < widthInSquares; squareX++)
            {
                // Center of the square
                int centerX = squareX * squareSize + squareSize / 2;
                int centerY = squareY * squareSize + squareSize / 2;

                // Calculate intensity and map color for the center pixel
                double intensity = CalcIntensity((centerX - width / 2) / scale, (centerY - height / 2) / scale, charges);
                Color color = ColorMap(intensity);
                byte red = color.B;
                byte green = color.G;
                byte blue = color.R;

                // Fill the square block with the same color
                for (int y = squareY * squareSize; y < (squareY + 1) * squareSize && y < bmp.Height; y++)
                {
                    int offset = y * bmp.Stride + squareX * squareSize * 3;
                    for (int x = 0; x < squareSize && (x + squareX * squareSize) < bmp.Width; x++)
                    {
                        pixels[offset++] = blue; // Blue
                        pixels[offset++] = green; // Green
                        pixels[offset++] = red; // Red
                    }
                }
            }
        }

        Marshal.Copy(pixels, 0, bmp.Scan0, pixels.Length);
        img.UnlockBits(bmp);
        g.DrawImage(img, new RectangleF((-width / 2) / scale, (-height / 2) / scale, width / scale, height / scale));
    }



    private Color GetColorFromIntensity(double intensity)
    {
        // Cap the intensity value to a maximum of 1.0 for a smoother transition.
        double intst = Math.Min(8, Math.Max(0, intensity)) / 8f;

        // Use binary search to find the correct segment
        int index = Array.BinarySearch(boundaries, intst);
        if (index < 0)
        {
            // Adjust to the nearest lower boundary index for BinarySearch results
            index = ~index - 1;
        }

        // Handle edge case where intst == 1.0
        if (index >= boundaries.Length - 1)
        {
            index = boundaries.Length - 2; // Assign to the last segment
        }

        // Calculate factor for interpolation
        double factor = (intst - boundaries[index]) / boundaryDiffs[index];

        // Ensure factor is within [0,1]
        factor = Math.Max(0.0, Math.Min(1.0, factor));

        // Interpolate colors
        int r = (int)(colors[index, 0] + factor * colorDiffs[index, 0]);
        int g = (int)(colors[index, 1] + factor * colorDiffs[index, 1]);
        int b = (int)(colors[index, 2] + factor * colorDiffs[index, 2]);

        // Clamp RGB values to [0,255]
        r = Math.Max(0, Math.Min(255, r));
        g = Math.Max(0, Math.Min(255, g));
        b = Math.Max(0, Math.Min(255, b));

        return Color.FromArgb(r, g, b);
    }

    private double CalcIntensity(PointF point)
    {
        Vector2 start = new Vector2(point.X, point.Y);
        Vector2 sum = Vector2.Zero;

        foreach (var charge in charges)
        {
            if (charge == null) continue;

            PointF p = charge.GetPosition();

            // Compute vector from charge to the start point
            float dx = start.X - p.X;
            float dy = start.Y - p.Y;

            // Compute squared length
            float lenSq = dx * dx + dy * dy;

            // Skip if length is zero
            if (lenSq == 0) continue;

            // Compute 1 / lenCubed directly
            float invLen = 1.0f / MathF.Sqrt(lenSq);
            float invLenCubed = invLen / lenSq;

            // Accumulate force vector
            float chargeValue = charge.GetCharge();
            float scale = chargeValue * invLenCubed;
            sum.X += dx * scale;
            sum.Y += dy * scale;
        }

        // Compute final magnitude
        return Math.Sqrt(sum.X * sum.X + sum.Y * sum.Y);
    }

    public void ZoomIn(float x, float y)
    { 
        worldWidthHalf /= x; worldHeightHalf /= y;
    }

    public void ZoomOut(float x, float y)
    { 
        worldWidthHalf *= x; worldHeightHalf *= y;
    }

    public float Draw(Graphics g, float width, float height, int startTime, int chargeHit)
    {
        float sum_ch = 0;
        int count_ch = 0;

        // spocitame sumu hodnoty naboju a pocet jednotlivych naboju
        if (charges == null) return 0;
        
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
                if (charge != null)
                {
                    if (count_ch >= 2)
                    {
                        float currentCharge = (float)Math.Sqrt(Math.Abs(charge.GetCharge()));

                        float newRadius = (float)Math.Sqrt(charge.GetRadius() * 1.5f * currentCharge / sum_ch);
                        charge.SetRadius(newRadius);
                    }

                    else if (count_ch == 1)
                    {
                        charge.SetRadius(0.7f);
                    }
                }
            }
        }


        // ziskani krajnich pozic naboju
        /*Tuple<float[], float[]> positions = GetPositions();
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
            xMin -= viewportWidth / 9f;
            xMax += viewportWidth / 9f;
            yMin -= viewportHeight / 9f;
            yMax += viewportHeight / 9f;
        }*/

        float xMin = worldCenterPosition.X - worldWidthHalf;
        float xMax = worldCenterPosition.X + worldWidthHalf;
        float yMin = worldCenterPosition.Y - worldHeightHalf;
        float yMax = worldCenterPosition.Y + worldHeightHalf;

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

        this.scale = scale;
        this.corners[0] = xMax;
        this.corners[1] = yMax;
        this.corners[2] = xMin;
        this.corners[3] = yMin;
        
        g.ScaleTransform(scale, scale);

        
        PointF center = new PointF((xMax + xMin) / 2f, (yMax + yMin) / 2f);

        if (SettingsObject.colorMap)
        {
            this.DrawColorMap(g, width, height, GetColorFromIntensity);
        }
        else
        {
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
        }


        // kresleni mrizky
        
        IGrid grid = new Grid(xMin, xMax, yMin, yMax, startTime, this.charges, scale, SettingsObject.gridX, SettingsObject.gridY, width, height);
        float tipLength = 10f; // nastaveni velikosti sipky
        grid.Draw(g, new PointF(xMin, yMin), new PointF(xMax, yMax),tipLength / scale, scale);
        


        // kresleni naboju
        for (int i = 0; i < charges.Length; i++)
        {
            if (charges[i] != null) charges[i].Draw(g, center, scale);
        }
        

        // kresleni sondy s vektorem intenzity
        //Probe probe = new Probe(new PointF(0, 0));
        //probe.Draw(g, StartTime, this.charges, scale, 0, 0);

        foreach (IProbe probe in SettingsObject.probes) 
        {
            probe.Calc(startTime, this.charges);
            probe.Draw(g, startTime, this.charges, scale, 0, false);
        }
        
        return scale;
    }
}
