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
    void Draw(Graphics g, INaboj[] charges, float scale, float spacing, bool grid);


    /// <summary>
    /// vypocte a ulozi si vektor intenizty v bode start
    /// </summary>
    /// <param name="start">pocatek sondy</param>
    /// <param name="charges">pole naboju</param>
    public void Calc(Vector2 start, INaboj[] charges);

    /// <summary>
    /// vypocte a ulozi si vektor intnzity na aktualni poloze
    /// </summary>
    /// <param name="startTime">zacatek simulace</param>
    /// <param name="charges">pole naboju</param>
    public void Calc(int startTime, INaboj[] charges);

    /// <summary>
    /// ticks += 1
    /// </summary>
    public void Tick();

    /// <summary>
    /// zjisti zda je bod uvnitr reprezentace sondy
    /// </summary>
    /// <param name="point">bod</param>
    /// <returns></returns>
    public bool IsHit(PointF point);

    /// <summary>
    /// posune obeh sondy o vektor
    /// </summary>
    /// <param name="vector">vektor</param>
    public void Drag(Vector2 vector);


    /// <summary>
    /// vrati id sondy
    /// </summary>
    /// <returns></returns>
    public int GetID();

    /// <summary>
    /// vrati retezec k ulozeni
    /// sonda:{stred X};{stred Y};{polomer};{uhlova rychlost}
    /// </summary>
    /// <returns></returns>
    string Save();

    /// <summary>
    /// pricte cas po ktery byla sonda drzena
    /// </summary>
    /// <param name="t">cas</param>
    public void AddTimeHeld(float t);

    /// <summary>
    /// nastavi stred obihani
    /// </summary>
    /// <param name="newCenter"></param>
    public void SetCenter(PointF newCenter);

    /// <summary>
    /// vrati stred obihani
    /// </summary>
    /// <returns></returns>
    public PointF GetCenter();

    /// <summary>
    /// nastavi polomer obihani
    /// </summary>
    /// <param name="newRadius"></param>
    public void SetRadius(float newRadius);

    /// <summary>
    /// vrati polomer obihani
    /// </summary>
    /// <returns></returns>
    public float GetRadius();

    /// <summary>
    /// nastavi uhlovou rychlost obihani
    /// </summary>
    /// <param name="newAngle">hodnota</param>
    public void SetAnglePerSecond(float newAngle);

    /// <summary>
    /// nastavi uhlovou rychlost obihani
    /// </summary>
    /// <param name="newAngle">retezec pro vypocet</param>
    public void SetAnglePerSecond(string newAngle);

    /// <summary>
    /// vrati uhlovou rychlost obihani
    /// </summary>
    /// <returns></returns>
    public float GetAnglePerSecond();


    /// <summary>
    /// nacte sondu
    /// </summary>
    /// <param name="args">parametry</param>
    /// <param name="id">id</param>
    /// <returns></returns>
    public static Probe Load(float[] args, int id)
    {
        return new Probe(new PointF(args[0], args[1]), args[2], args[3], id);
    }
}

