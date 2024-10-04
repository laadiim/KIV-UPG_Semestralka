namespace UPG_SP_2024.Interfaces;


/// <summary>
/// Rozhrani pro scenar vizualizace
/// </summary>
public interface IScenario
{
    INaboj[] GetCharges();
    void AddCharge(INaboj naboj);
    INaboj RemoveCharge(INaboj naboj);
    INaboj RemoveCharge(int id);
    void Draw(Graphics g);
}