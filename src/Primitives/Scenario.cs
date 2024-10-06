using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024;

public class Scenario : IScenario
{
    INaboj[] charges = new INaboj[1];
    int free_index = 0;

    public INaboj[] GetCharges()
    {
        return charges;
    }

    public void AddCharge(INaboj naboj)
    {
        charges[free_index] = naboj;
            
        for (; free_index < charges.Length; free_index++)
        {
            if (charges[free_index] == null) return;
        }

        INaboj[] new_charges = new INaboj[charges.Length*2];

        for (int i = 0; i < charges.Length; i++)
        {
            new_charges[i] = charges[i];
        }
        /// free_index = charges.Length - prepsal se jiz v cyklu
        charges = new_charges;
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