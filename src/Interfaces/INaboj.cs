namespace UPG_SP_2024.Interfaces;

public interface INaboj
{
    int GetCharge();
    void SetCharge(int charge);
    Tuple<int, int> GetPosition();
    void SetPosition(int x, int y);
    void Draw(Graphics g);
    int GetID();
}