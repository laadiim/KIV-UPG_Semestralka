using System.Drawing.Drawing2D;
using System.Numerics;
using System.Runtime.CompilerServices;
using NCalc;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024.Primitives;

public class Naboj : INaboj
{
    private Expression charge;
    private string chargeStr;
    private float radius;
    private float X;
    private float Y;
    private int id;
    private float startTime;

    public Naboj(string charge, float X, float Y, int id, float startTime)
    {
        this.charge = new Expression(charge);
        this.chargeStr = charge;
        this.X = X;
        this.Y = -Y;
        this.id = id;
        this.radius = 1f;
        this.startTime = startTime;
    }

    public void SetChargeStr(string chargeStr)
    {
        this.chargeStr = chargeStr != "" ? chargeStr : "0";
        this.charge = new Expression(this.chargeStr);
    }

    public string GetChargeStr()
    {
        return chargeStr;
    }

    public string Save()
    {
        return $"naboj:[t] - [t] + {this.chargeStr};{this.X};{this.Y}";
    }

    public float GetX()
    {
        return X + SettingsObject.worldCenter.X;
    }

    public float GetY()
    {
        return Y + SettingsObject.worldCenter.Y;
    }

    public bool IsHit(PointF point)
    {
        float distance = Vector2.Distance(new Vector2(point.X, point.Y), new Vector2(GetX(), GetY()));
        return distance <= radius;
    }

    public void Drag(PointF point)
    {
			this.X += point.X;
            this.Y += point.Y;
            if (SettingsObject.graphForm != null)SettingsObject.chargeForm.Refresh(this.id);
    }

    public float GetCharge()
    {
        charge.Parameters["t"] = (Environment.TickCount - startTime) / 1000;
        return Convert.ToSingle(charge.Evaluate());
    }

    public void SetCharge(string charge)
    {
        this.charge = new Expression(charge);
    }

    public PointF GetPosition()
    {
        return new PointF(GetX(), GetY());
    }

    public void SetPosition(float X, float Y)
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
        g.TranslateTransform(GetX() - radius, GetY() - radius);
        float charge = GetCharge();
        // nastaveni barvy pro naboje
        if (radius <= 0 || radius > 1000) // Example threshold
        {
            throw new ArgumentException("Radius must be greater than 0 and less than a reasonable upper limit.");
        }
        using (var ellipsePath = new GraphicsPath())
        {
            ellipsePath.AddEllipse(0, 0, radius * 2, radius * 2);

            // First part
            using (var brushEll1 = new PathGradientBrush(ellipsePath))
            {
                if (charge < 0)
                {
                    brushEll1.CenterColor = Color.FromArgb(255, 70, 240, 240);
                    brushEll1.SurroundColors = new[] { Color.FromArgb(255, 100, 50, 90) };
                }
                else
                {
                    brushEll1.CenterColor = Color.FromArgb(255, 240, 220, 220);
                    brushEll1.SurroundColors = new[] { Color.FromArgb(255, 100, 20, 100) };
                }
                brushEll1.FocusScales = new PointF(0f, 0f);
                g.FillEllipse(brushEll1, 0, 0, radius * 2, radius * 2);
            }

            // Second part
            using (var brushEll2 = new PathGradientBrush(ellipsePath))
            {
                if (charge < 0)
                {
                    brushEll2.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    brushEll2.SurroundColors = new[] { Color.FromArgb(220, 160, 150, 190) };
                }
                else
                {
                    brushEll2.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    brushEll2.SurroundColors = new[] { Color.FromArgb(210, 140, 190, 200) };
                }
                brushEll2.FocusScales = new PointF(0.7f, 0.7f);
                g.FillEllipse(brushEll2, 0, 0, radius * 2, radius * 2);
            }

            // Third part
            using (var brushEll3 = new PathGradientBrush(ellipsePath))
            {
                if (charge < 0)
                {
                    brushEll3.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    brushEll3.SurroundColors = new[] { Color.FromArgb(150, 240, 170, 190) };
                }
                else
                {
                    brushEll3.CenterColor = Color.FromArgb(0, 0, 0, 0);
                    brushEll3.SurroundColors = new[] { Color.FromArgb(150, 240, 140, 190) };
                }
                brushEll3.FocusScales = new PointF(0.9f, 0.9f);
                g.FillEllipse(brushEll3, 0, 0, radius * 2, radius * 2);
            }
        }


        // napis - hodnota naboje
        string label = $"{charge:n1} C";
        Font font = new Font("Arial", (float)Math.Sqrt(radius) / 5f, FontStyle.Bold);
        Brush brush = new SolidBrush(Color.FromArgb(230, Color.White));
        float width = g.MeasureString(label, font).Width;
        float height = g.MeasureString(label, font).Height;

        g.DrawString(label, font, brush, radius - width / 2, radius - height / 2);

        g.TranslateTransform(radius - GetX(), radius - GetY());
    }

}
