namespace UPG_SP_2024.Interfaces;


/// <summary>
/// Rozhrani pro scenar vizualizace
/// </summary>
public interface IScenario
{
    /// <summary>
    /// vrati pole naboju
    /// </summary>
    /// <returns>pole naboju</returns>
    INaboj[] GetCharges();
    
    /// <summary>
    /// prida naboj do sceny
    /// </summary>
    /// <param name="naboj">naboj k pridani</param>
    void AddCharge(INaboj naboj);
    
    /// <summary>
    /// odstrani naboj ze sceny
    /// </summary>
    /// <param name="naboj">reference na odstranovany naboj</param>
    /// <returns>odstraneny naboj</returns>
    INaboj RemoveCharge(INaboj naboj);
    
    /// <summary>
    /// odstrani naboj ze sceny
    /// </summary>
    /// <param name="id">id naboje</param>
    /// <returns>odstraneny naboj</returns>
    INaboj RemoveCharge(int id);
    
    /// <summary>
    /// vykresli scenar
    /// </summary>
    /// <param name="g">graficky kontext</param>
    void Draw(Graphics g, Panel panel);
}