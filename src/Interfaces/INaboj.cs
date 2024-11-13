namespace UPG_SP_2024.Interfaces;

/// <summary>
/// Rozhrani pro vizualizaci naboje
/// </summary>
public interface INaboj
{
    /// <summary>
    /// zkontroluje jestli je bod uvnitr naboje
    /// </summary>
    /// <param name="point"></param>
    /// <returns> je bod uvnitr naboje </returns>
    bool IsHit(PointF point);
    /// <summary>
    /// vraci velikost a polaritu naboje
    /// </summary>
    /// <returns>naboj</returns>
    float GetCharge();
    
    /// <summary>
    /// nastavi novou hodnotu naboje
    /// </summary>
    /// <param name="charge">novy naboj</param>
    void SetCharge(float charge);
    
    /// <summary>
    /// vrati pozici stredu naboje
    /// </summary>
    /// <returns>stred naboje</returns>
    PointF GetPosition();
    
    /// <summary>
    /// nastavi novy stred
    /// </summary>
    /// <param name="point">novy stred</param>
    void SetPosition(PointF point);
    
    /// <summary>
    /// vykresli naboj
    /// </summary>
    /// <param name="g">graficky kontext</param>
    void Draw(Graphics g, PointF panelCenter, float scale);
    
    /// <summary>
    /// vrati ID naboje
    /// </summary>
    /// <returns>ID</returns>
    int GetID();

    /// <summary>
    /// vrati polomer naboje pro vykresleni
    /// </summary>
    /// <returns>polomer</returns>
    float GetRadius();
    
    /// <summary>
    /// nastavi novy polomer
    /// </summary>
    /// <param name="radius">novy polomer</param>
    void SetRadius(float radius);
    
    /// <summary>
    /// posune naboj na novou pozici
    /// </summary>
    /// <param name="point">pozice</param>
    void Drag(PointF point, float worldWidth, float worldHeight, PointF worldPosition);
}