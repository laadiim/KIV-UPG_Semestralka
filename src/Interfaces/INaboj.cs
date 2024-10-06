namespace UPG_SP_2024.Interfaces;

public interface INaboj
{
    /// <summary>
    /// vraci velikost a polaritu naboje
    /// </summary>
    /// <returns>naboj</returns>
    int GetCharge();
    
    /// <summary>
    /// nastavi novou hodnotu naboje
    /// </summary>
    /// <param name="charge">novy naboj</param>
    void SetCharge(int charge);
    
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
    void Draw(Graphics g);
    
    /// <summary>
    /// vrati ID naboje
    /// </summary>
    /// <returns>ID</returns>
    int GetID();
    
    
}