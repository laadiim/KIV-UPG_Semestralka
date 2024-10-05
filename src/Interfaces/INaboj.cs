namespace UPG_SP_2024.Interfaces;

public interface INaboj
{
    int GetCharge();
    void SetCharge(int charge);
    PointF GetPosition();
    void SetPosition(PointF point);
    void Draw(Graphics g);
    int GetID();
}