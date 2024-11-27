namespace UPG_SP_2024
{
    class ColorMaps
		{
			public static readonly double[] boundaries = { 0.0, 0.25, 0.5, 0.75, 1.0 };

			public static readonly int[,] colors = {
							{ 80, 50, 30 },      // fifth
							{ 70, 40, 80 },      // fourth
							{ 100, 60, 140 },    // third 
							{ 140, 120, 190 },   // second
							{ 210, 190, 220 }   // first  
			};

			public static double[] boundaryDiffs;
			public static int[,] colorDiffs;
            public static Color[] basicMap;
		

			public static void Setup()
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


    public static Color[] BasicMap(int len)
    {
        Setup();
        Color[] colorTable = new Color[len + 1];
			for (double i = 0; i <= len; i++)
			{
                // Cap the intensity value to a maximum of 1.0 for a smoother transition.
                double intst = i / len;

                // Use binary search to find the correct segment
                int index = Array.BinarySearch(boundaries, intst);
                if (index < 0)
                {
                    index = ~index - 1; // Convert to the nearest lower boundary index
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

                colorTable[(int)i] = Color.FromArgb(r, g, b);
			}
			return colorTable;
    }
	}
}
