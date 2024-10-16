namespace UPG_SP_2024.Primitives;

public class Probe
{
    private PointF center;
    private float radius;
    private float anglePerSecond;

    public Probe(PointF center, float radius = 1f, float anglePerSecond = MathF.PI / 6)
    {
        this.center = center;
        this.radius = radius;
        this.anglePerSecond = anglePerSecond;
    }

    public void Draw(Graphics g, )
    {
        
    }
}