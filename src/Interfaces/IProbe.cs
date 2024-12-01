using System.Numerics;
using UPG_SP_2024.Primitives;

namespace UPG_SP_2024.Interfaces;

/// <summary>
/// Rozhrani pro vizualizaci sondy
/// </summary>
public interface IProbe
{
    /// <summary>
    /// vykresli sondu s vektorem intenzity
    /// </summary>
    /// <param name="g">graficky kontext</param>
    /// <param name="startTime">zacatek casovace</param>
    /// <param name="charges">seznam naboju</param>
    /// <param name="scale">scale</param>
    /// <param name="spacing">minimum z romeru obdelniku mrizky</param>
    void Draw(Graphics g, int startTime, INaboj[] charges, float scale, float spacing, bool grid);

    public void Calc(Vector2 start, INaboj[] charges);
    public void Calc(int startTime, INaboj[] charges);

    public void Tick();

    string Save();

    public static Probe Load(float[] args, int id)
    {
        return new Probe(new PointF(args[0], args[1]), args[2], args[3], id);
    }
}

