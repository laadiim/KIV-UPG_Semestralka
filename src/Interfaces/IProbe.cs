namespace UPG_SP_2024.Interfaces;

/// <summary>
/// Rozhrani pro vizualizaci sondy
/// </summary>
public interface IProbe
{
    /// <summary>
    /// vykresli sondu s vektorem intenzity
    /// </summary>
    /// <param name="g">graficky kontext</param>
    /// <param name="startTime">zacatek casovace</param>
    /// <param name="charges">seznam naboju</param>
    /// <param name="scale">scale</param>
    void Draw(Graphics g, int startTime, INaboj[] charges, float scale);
}

