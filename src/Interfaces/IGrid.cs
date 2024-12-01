namespace UPG_SP_2024.Interfaces;

/// <summary>
/// Rozhrani pro vizualizaci mrizky
/// </summary>
public interface IGrid
{
    int GetSpacingXinPixels();

    int GetSpacingYinPixels();

    void Draw(Graphics g, float tipLength, float scale);
}
