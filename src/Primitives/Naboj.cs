using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Naboj : INaboj
{
    private int charge;
    private float radius;
    private PointF center;
    private int id;
    
    public Naboj(int charge, int radius, PointF center, int id)
    {
        this.charge = charge;
        this.radius = 0.5f;
        this.center = center;
        this.id = id;
    }

    public int GetCharge()
    {
        return this.charge;
    }

    public void SetCharge(int charge)
    {
        this.charge = charge;
    }

    public PointF GetPosition()
    {
        return this.center;
    }

    public void SetPosition(PointF point)
    {
        this.center = point;
    }
    public float GetRadius()
    {
        return this.radius;
    }

    public void SetRadius(float radius)
    {
        this.radius = radius;
    }

    public int GetID()
    {
        return this.id;
    }

    public void Draw(Graphics g, PointF panelCenter, float scale)
    {
        Console.WriteLine(scale);
        Brush brush = new SolidBrush(Color.Gold);
        g.FillEllipse(brush, panelCenter.X + center.X * scale - radius * scale, panelCenter.Y + center.Y * scale - radius * scale, radius * scale * 2, radius * scale * 2);
        
        Pen pen = new Pen(Color.Black);
        g.DrawEllipse(pen, panelCenter.X + center.X * scale - radius * scale, panelCenter.Y + center.Y * scale - radius * scale, radius * scale * 2, radius * scale * 2);

        string label = $"{this.charge} C";
        Font font = new Font("Arial", 0.2f * scale, FontStyle.Bold);
        float width = g.MeasureString(label, font).Width;
        float height = g.MeasureString(label, font).Height;
        
        brush = new SolidBrush(Color.Black);
        g.DrawString(label, font, brush, center.X * scale - width / 2 + panelCenter.X, center.Y * scale - height / 2 + panelCenter.Y);
    }
    
}