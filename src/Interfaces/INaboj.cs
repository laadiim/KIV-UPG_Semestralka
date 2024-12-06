using System.Globalization;
using System.Numerics;
using UPG_SP_2024.Primitives;

namespace UPG_SP_2024.Interfaces;

/// <summary>
/// Rozhrani pro vizualizaci naboje
/// </summary>
public interface INaboj
{
    /// <summary>
    /// zkontroluje jestli je bod uvnitr naboje
    /// </summary>
    /// <param name="point"></param>
    /// <returns> je bod uvnitr naboje </returns>
    bool IsHit(PointF point);
    /// <summary>
    /// vraci velikost a polaritu naboje
    /// </summary>
    /// <returns>naboj</returns>
    float GetCharge();
    
    /// <summary>
    /// nastavi novou hodnotu naboje
    /// </summary>
    /// <param name="charge">novy naboj</param>
    void SetCharge(string charge);
    
    /// <summary>
    /// vrati pozici stredu naboje
    /// </summary>
    /// <returns>stred naboje</returns>
    PointF GetPosition();
    
    /// <summary>
    /// nastavi novy stred
    /// </summary>
    /// <param name="point">novy stred</param>
    void SetPosition(float X, float Y);
    
    /// <summary>
    /// vykresli naboj
    /// </summary>
    /// <param name="g">graficky kontext</param>
    void Draw(Graphics g, PointF panelCenter, float scale);
    
    /// <summary>
    /// vrati ID naboje
    /// </summary>
    /// <returns>ID</returns>
    int GetID();

    /// <summary>
    /// vrati polomer naboje pro vykresleni
    /// </summary>
    /// <returns>polomer</returns>
    float GetRadius();
    
    /// <summary>
    /// nastavi novy polomer
    /// </summary>
    /// <param name="radius">novy polomer</param>
    void SetRadius(float radius);
    
    /// <summary>
    /// posune naboj o vektor
    /// </summary>
    /// <param name="v">vektor posunu</param>
    void Drag(Vector2 v);

    /// <summary>
    /// vytvori retezec k ulozeni naboje ve tvaru
    /// "naboj:{vzorec pro vypocet naboje};{pozice x};{pozice y}"
    /// </summary>
    /// <returns>retezec k ulozeni</returns>
    string Save();

    /// <summary>
    /// getter pro retezec vypoctu naboje
    /// </summary>
    /// <returns>retezec pro vypocet naboje</returns>
    public string GetChargeStr();

    /// <summary>
    /// setter pro retezec vypoctu naboje
    /// </summary>
    /// <param name="chargeStr">novy retezec</param>
    public void SetChargeStr(string chargeStr);

    /// <summary>
    /// vytvori instanci naboje podle predanych parametru
    /// </summary>
    /// <param name="args">parametry naboje [retezec pro naboj, x, y]</param>
    /// <param name="id">id pro naboj</param>
    /// <param name="startTime">zacatek simulace</param>
    /// <returns></returns>
    public static INaboj Load(string[] args, int id, float startTime)
    {
        return new Naboj(args[0], Single.Parse(args[1], CultureInfo.InvariantCulture),
            Single.Parse(args[2], CultureInfo.InvariantCulture), id, startTime);
    }
}