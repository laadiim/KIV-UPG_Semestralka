using System.Drawing;
using System.Drawing.Drawing2D;
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
        
        //Brush brush = new SolidBrush(Color.DarkTurquoise);
        
        g.TranslateTransform(center.X - radius, center.Y - radius);

        //TODO: grafika naboju


        using (var ellipsePath = new GraphicsPath())
        {
            ellipsePath.AddEllipse(0, 0, radius * 2, radius * 2);
            using (var brushEll = new PathGradientBrush(ellipsePath))
            {
                brushEll.CenterPoint = new PointF(radius / 1.7f, radius / 1.7f);

                if (this.charge < 0)
                {
                    brushEll.CenterColor = Color.FromArgb(250, 70, 240, 240);
                    brushEll.SurroundColors = new[] { Color.FromArgb(250, 40, 50, 70) };
                }
                else
                {
                    brushEll.CenterColor = Color.FromArgb(250, 250, 220, 160);
                    brushEll.SurroundColors = new[] { Color.FromArgb(250, 80, 20, 30) };
                }
                    brushEll.FocusScales = new PointF(0f, 0f);
                
                g.FillEllipse(brushEll, 0, 0, radius * 2, radius * 2);

                brushEll.CenterPoint = new PointF(radius / 2.2f, radius / 2.2f);

                if (this.charge < 0)
                {
                    brushEll.CenterColor = Color.FromArgb(0, 70, 240, 240);
                    brushEll.SurroundColors = new[] { Color.FromArgb(120, 140, 140, 170) };
                }
                else
                {
                    brushEll.CenterColor = Color.FromArgb(0, 250, 220, 160);
                    brushEll.SurroundColors = new[] { Color.FromArgb(130, 90, 190, 230) };
                }
                brushEll.FocusScales = new PointF(0.7f, 0.7f);

                g.FillEllipse(brushEll, 0, 0, radius * 2, radius * 2);
            }

        }

        string label = $"{this.charge} C";
        Font font = new Font("Arial", 1f / (float)Math.Sqrt(scale), FontStyle.Bold);
        float width = g.MeasureString(label, font).Width;
        float height = g.MeasureString(label, font).Height;
        
        Brush brush = new SolidBrush(Color.White);
        g.DrawString(label, font, brush, radius - width / 2, radius - height / 2);
        g.TranslateTransform(radius - center.X, radius - center.Y);
    }
    
}