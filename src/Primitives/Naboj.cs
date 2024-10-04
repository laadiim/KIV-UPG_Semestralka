namespace UPG_SP_2024;

public class Naboj
{
    private int charge;
    private int radius;
    private PointF center;
    
    public Naboj(int charge, int radius, PointF center)
    {
        this.charge = charge;
        this.radius = radius;
        this.center = center;
    }
    
    public int Charge
    {
        get => charge;
        set => charge = value;
    }

    public int Radius
    {
        get => radius;
        set => radius = value;
    }

    public PointF Center
    {
        get => center;
        set => center = value;
    }

    public void Draw(Graphics g)
    {
        Brush brush = new SolidBrush(Color.Gray);
        g.FillEllipse(brush, center.X - radius, center.Y - radius, radius * 2, radius * 2);
        
        Pen pen = new Pen(Color.Black);
        g.DrawEllipse(pen, center.X - radius, center.Y - radius, radius * 2, radius * 2);

        string label = $"{charge} C";
        Font font = new Font("Arial", 12, FontStyle.Bold);
        float width = g.MeasureString(label, font).Width;
        float height = g.MeasureString(label, font).Height;
        
        brush = new SolidBrush(Color.Black);
        g.DrawString(label, font, brush, center.X - width / 2, center.Y - height / 2);
    }
    
}