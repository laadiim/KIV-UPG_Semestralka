using System.Drawing.Drawing2D;
using System.Numerics;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class PeriodicNaboj : INaboj
{
    private Func<float, float> charge;
    private float radius;
    private Func<float, float> X;
    private Func<float, float> Y;
    private int id;
    private float startTime;
    
    public PeriodicNaboj(Func<float, float> charge, Func<float, float> X, Func<float, float> Y, int id, float startTime)
    {
        this.charge = charge;
        this.X = X;
        this.Y = Y;
        this.id = id;
        this.radius = 1f;
        this.startTime = startTime;
    }

    public bool IsHit(PointF point)
    {
        float t = (Environment.TickCount - startTime) / 1000;
        float distance = Vector2.Distance(new Vector2(point.X, point.Y), new Vector2(X(t), Y(t)));
        return distance <= radius;
    }

    public void Drag(PointF point, float worldWidth, float worldHeight, PointF worldPosition)
    {
        return;
    }

    public float GetCharge()
    {
        return this.charge((Environment.TickCount - startTime) / 1000);
    }

    public void SetCharge(Func<float, float> charge)
    {
        this.charge = charge;
    }

    public PointF GetPosition()
    {
        return new PointF(X((Environment.TickCount - startTime) / 1000), Y((Environment.TickCount - startTime) / 1000));
    }

    public void SetPosition(Func<float, float> X, Func<float, float> Y)
    {
        this.X = X;
        this.Y = Y;
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
        float t = Environment.TickCount - startTime;
        t /= 1000;
        Console.WriteLine(t);
        g.TranslateTransform(X(t) - radius, Y(t) - radius);

        using (var shadowPath = new GraphicsPath())
        {
            float r = 2.03f;
            shadowPath.AddEllipse(0, 0, radius * r, radius * r);

            using (var brushEll = new PathGradientBrush(shadowPath))
            {
                // stin
                brushEll.CenterPoint = new PointF(radius * 1.66f, radius * 1.66f);

                // nastaveni jine barvy pro zapornou hodnotu naboje
                if (this.charge(t) < 0)
                {
                    brushEll.CenterColor = Color.FromArgb(255, 120, 230, 210);
                    brushEll.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
                }
                else
                {   
                    brushEll.CenterColor = Color.FromArgb(255, 240, 220, 150);
                    brushEll.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
                }
                brushEll.FocusScales = new PointF(0f, 0f);

                // vybarvi stin
                g.FillEllipse(brushEll, 0, 0, radius * r, radius * r);
            }
        }

        // nastaveni barvy pro naboje
        using (var ellipsePath = new GraphicsPath())
        {

            ellipsePath.AddEllipse(0, 0, radius * 2, radius * 2);
            
            using (var brushEll = new PathGradientBrush(ellipsePath))
            {
                // prvni cast
                brushEll.CenterPoint = new PointF(radius / 1.7f, radius / 1.7f);

                // nastaveni jine barvy pro zapornou hodnotu naboje
                if (this.charge(t) < 0)
                {
                    brushEll.CenterColor = Color.FromArgb(255, 70, 240, 240);
                    brushEll.SurroundColors = new[] { Color.FromArgb(255, 40, 50, 70) };
                }
                else
                {
                    brushEll.CenterColor = Color.FromArgb(255, 250, 220, 160);
                    brushEll.SurroundColors = new[] { Color.FromArgb(255, 80, 20, 30) };
                }
                brushEll.FocusScales = new PointF(0f, 0f);
                
                // vybarvi naboj
                g.FillEllipse(brushEll, 0, 0, radius * 2, radius * 2);


                // druha cast
                brushEll.CenterPoint = new PointF(radius / 2.2f, radius / 2.2f);

                // nastaveni jine barvy pro zapornou hodnotu naboje
                if (this.charge(t) < 0)
                {
                    brushEll.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    brushEll.SurroundColors = new[] { Color.FromArgb(120, 140, 140, 170) };
                }
                else
                {
                    brushEll.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    brushEll.SurroundColors = new[] { Color.FromArgb(130, 90, 190, 230) };
                }
                brushEll.FocusScales = new PointF(0.7f, 0.7f);

                // vybarvi pres naboj gradient pro zjemneni okraju
                g.FillEllipse(brushEll, 0, 0, radius * 2, radius * 2);
            }
        }

        // napis - hodnota naboje
        string label = $"{this.charge(t)} C";
        Font font = new Font("Arial", 1f / (float)Math.Sqrt(scale), FontStyle.Bold);
        Brush brush = new SolidBrush(Color.FromArgb(230, Color.White));
        float width = g.MeasureString(label, font).Width;
        float height = g.MeasureString(label, font).Height;
        
        g.DrawString(label, font, brush, radius - width / 2, radius - height / 2);

        g.TranslateTransform(radius - X(t), radius - Y(t));
    }
    
}