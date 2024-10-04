using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024;

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
        throw new NotImplementedException();
    }

    public void SetCharge(int charge)
    {
        throw new NotImplementedException();
    }

    public Tuple<int, int> GetPosition()
    {
        throw new NotImplementedException();
    }

    public void SetPosition(int x, int y)
    {
        throw new NotImplementedException();
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

    public int GetID()
    {
        throw new NotImplementedException();
    }
}