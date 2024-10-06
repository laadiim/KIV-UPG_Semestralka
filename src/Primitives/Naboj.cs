using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Naboj : INaboj
{
    private int charge;
    private int radius;
    private PointF center;
    private int id;
    
    public Naboj(int charge, int radius, PointF center, int id)
    {
        this.charge = charge;
        this.radius = radius;
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
    public int GetRadius()
    {
        return this.radius;
    }

    public void SetRadius(int radius)
    {
        this.radius = radius;
    }

    public int GetID()
    {
        return this.id;
    }

    public void Draw(Graphics g)
    {
        Brush brush = new SolidBrush(Color.Gold);
        g.FillEllipse(brush, center.X - radius, center.Y - radius, radius * 2, radius * 2);
        
        Pen pen = new Pen(Color.Black);
        g.DrawEllipse(pen, center.X - radius, center.Y - radius, radius * 2, radius * 2);

        string label = $"{this.charge} C";
        Font font = new Font("Arial", 12, FontStyle.Bold);
        float width = g.MeasureString(label, font).Width;
        float height = g.MeasureString(label, font).Height;
        
        brush = new SolidBrush(Color.Black);
        g.DrawString(label, font, brush, center.X - width / 2, center.Y - height / 2);
    }
    
}