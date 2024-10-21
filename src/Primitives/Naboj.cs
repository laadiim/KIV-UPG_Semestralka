using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Naboj : INaboj
{
    private float charge;
    private float radius;
    private PointF center;
    private int id;
    
    public Naboj(int charge, PointF center, int id)
    {
        this.charge = charge;
        this.center = center;
        this.center.Y = -center.Y;
        this.id = id;
        this.radius = 1f;
    }

    public float GetCharge()
    {
        return this.charge;
    }

    public void SetCharge(float charge)
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
        
        Brush brush = new SolidBrush(Color.DarkTurquoise);
        
        g.TranslateTransform(center.X - radius, center.Y - radius);

        //TODO: grafika naboju

        g.FillEllipse(brush, 0, 0, radius * 2, radius * 2);
        
        /* ohraniceni pro naboj, zatim to nechceme
        Pen pen = new Pen(Color.FromArgb(150, Color.White), 2 / scale);
        g.DrawEllipse(pen, 0, 0, radius * 2, radius * 2);
        */

        string label = $"{this.charge} C";
        Font font = new Font("Arial", 1f / (float)Math.Sqrt(scale), FontStyle.Bold);
        float width = g.MeasureString(label, font).Width;
        float height = g.MeasureString(label, font).Height;
        
        brush = new SolidBrush(Color.White);
        g.DrawString(label, font, brush, radius - width / 2, radius - height / 2);
        g.TranslateTransform(radius - center.X, radius - center.Y);
    }
    
}