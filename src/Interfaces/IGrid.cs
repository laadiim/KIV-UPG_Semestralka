namespace UPG_SP_2024.Interfaces;

/// <summary>
/// Rozhrani pro vizualizaci mrizky
/// </summary>
public interface IGrid
{
    /// <summary>
    /// nakresli mrizku
    /// </summary>
    /// <param name="g">graficky kontext</param>
    /// <param name="tipLength">delka sipek</param>
    /// <param name="scale">skalovani vizualizace</param>
    public void Draw(Graphics g, float tipLength, float scale);
}