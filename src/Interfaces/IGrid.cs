namespace UPG_SP_2024.Interfaces;

public interface IGrid
{
    int GetDensity();
    void SetDensity(int row, int column);
    void Draw(Graphics g);
}