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
    private float _xOffset;
    private float _yOffset;

    public PeriodicNaboj(Func<float, float> charge, Func<float, float> X, Func<float, float> Y, int id, float startTime)
    {
        this.charge = charge;
        this.X = X;
        this.Y = Y;
        this.id = id;
        this.radius = 1f;
        this.startTime = startTime;
    }

    public float GetX(float t)
    {
        return X(t) + _xOffset;
    }

    public float GetY(float t)
    {
        return Y(t) + _yOffset;
    }
    
    public bool IsHit(PointF point)
    {
        float t = (Environment.TickCount - startTime) / 1000;
        float distance = Vector2.Distance(new Vector2(point.X, point.Y), new Vector2(GetX(t), GetY(t)));
        return distance <= radius;
    }

    public void Drag(PointF point, float worldWidth, float worldHeight, PointF worldPosition)
    {
        float t = (Environment.TickCount - startTime) / 1000;
        // Calculate current and new positions
        float currentX = GetX(t);
        float currentY = GetY(t);
        
        float newX = MathF.Max(MathF.Min(currentX + point.X, worldWidth + worldPosition.X), worldPosition.X - worldWidth);
        float newY = MathF.Max(MathF.Min(currentY + point.Y, worldHeight + worldPosition.Y), worldPosition.Y - worldHeight);


        // Update offsets
        _xOffset += newX - currentX;
        _yOffset += newY - currentY;
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
        return new PointF(GetX((Environment.TickCount - startTime) / 1000), GetY((Environment.TickCount - startTime) / 1000));
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
        g.TranslateTransform(GetX(t) - radius, GetY(t) - radius);

        
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
                    brushEll.SurroundColors = new[] { Color.FromArgb(255, 100, 50, 90) };
                }
                else
                {
                    brushEll.CenterColor = Color.FromArgb(255, 240, 220, 220);
                    brushEll.SurroundColors = new[] { Color.FromArgb(255, 100, 20, 100) };
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
                    brushEll.SurroundColors = new[] { Color.FromArgb(220, 160, 150, 190) };
                }
                else
                {
                    brushEll.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    brushEll.SurroundColors = new[] { Color.FromArgb(210, 140, 190, 200) };
                }
                brushEll.FocusScales = new PointF(0.7f, 0.7f);

                // vybarvi pres naboj gradient pro zjemneni okraju
                g.FillEllipse(brushEll, 0, 0, radius * 2, radius * 2);

                if (this.charge(t) < 0)
                {
                    brushEll.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    brushEll.SurroundColors = new[] { Color.FromArgb(150, 240, 170, 190) };
                }
                else
                {
                    brushEll.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    brushEll.SurroundColors = new[] { Color.FromArgb(150, 240, 140, 190) };
                }
                brushEll.FocusScales = new PointF(0.9f, 0.9f);

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

        g.TranslateTransform(radius - GetX(t), radius - GetY(t));
    }
    
}