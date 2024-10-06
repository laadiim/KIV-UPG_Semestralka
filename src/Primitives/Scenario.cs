using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024;

public class Scenario : IScenario
{
    INaboj[] charges = new INaboj[1];

    public INaboj[] GetCharges()
    {
        throw new NotImplementedException();
    }

    public void AddCharge(INaboj naboj)
    {
        throw new NotImplementedException();
    }

    public INaboj RemoveCharge(INaboj naboj)
    {
        throw new NotImplementedException();
    }

    public INaboj RemoveCharge(int id)
    {
        throw new NotImplementedException();
    }

    public void Draw(Graphics g)
    {  

        float x_min;
        float x_max;
        float y_min;
        float y_max;
        ;
    }
}