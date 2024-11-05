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
    /// vrati naboj podle ID
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>instance naboje</returns>
    INaboj GetCharge(int id);

    /// <summary>
    /// vykresli scenar
    /// </summary>
    /// <param name="g">graficky kontext</param>
    /// <param name="width">sirka scenare</param>
    /// <param name="height">vyska scenare</param>
    /// <param name="startTime">zacatek casovace</param>
    /// <returns> scale </returns>
    float Draw(Graphics g, float width, float height, int startTime);
}