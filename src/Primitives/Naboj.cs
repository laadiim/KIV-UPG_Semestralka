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
    private Expression X;
    private string xStr;
    private Expression Y;
    private string yStr;
    private int id;
    private float startTime;
    private float _xOffset;
    private float _yOffset;

    public Naboj(string charge, string X, string Y, int id, float startTime)
    {
        this.charge = new Expression(charge);
        this.chargeStr = charge;
        this.xStr = X;
        this.yStr = Y;
        this.X = new Expression(X);
        this.Y = new Expression(Y);
        this.id = id;
        this.radius = 1f;
        this.startTime = startTime;
    }

    public string Save()
    {
        return $"naboj:[t] - [t] + {this.chargeStr};[t] - [t] + {this._xOffset} + {this.xStr};[t] - [t] + {this._yOffset} + {this.yStr}";
    }

    public float GetX(float t)
    {
        X.Parameters["t"] = t;
        return _xOffset + Convert.ToSingle(X.Evaluate());
    }

    public float GetY(float t)
    {
        Y.Parameters["t"] = t;
        return -_yOffset - Convert.ToSingle(Y.Evaluate());
    }

    public bool IsHit(PointF point)
    {
        float t = (Environment.TickCount - startTime) / 1000;
        float distance = Vector2.Distance(new Vector2(point.X, point.Y), new Vector2(GetX(t), GetY(t)));
        return distance <= radius;
    }

    public void Drag(PointF point, float[] corners)
    {
        float t = (Environment.TickCount - startTime) / 1000;
        // Calculate current and new positions
        float currentX = GetX(t);
        float currentY = GetY(t);

        float newX = MathF.Max(MathF.Min(currentX + point.X, corners[0]), corners[2]);
        float newY = MathF.Max(MathF.Min(currentY + point.Y, corners[1]), corners[3]);


        // Update offsets
        _xOffset += newX - currentX;
        _yOffset -= newY - currentY;
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
        return new PointF(GetX((Environment.TickCount - startTime) / 1000), GetY((Environment.TickCount - startTime) / 1000));
    }

    public void SetPosition(string X, string Y)
    {
        this.X = new Expression(X);
        this.Y = new Expression(Y);
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
        g.TranslateTransform(GetX(t) - radius, GetY(t) - radius);
        float charge = GetCharge();

        // nastaveni barvy pro naboje
        using (var ellipsePath = new GraphicsPath())
        {

            ellipsePath.AddEllipse(0, 0, radius * 2, radius * 2);

            using (var brushEll = new PathGradientBrush(ellipsePath))
            {
                // prvni cast
                brushEll.CenterPoint = new PointF(radius / 1.7f, radius / 1.7f);

                // nastaveni jine barvy pro zapornou hodnotu naboje
                if (charge < 0)
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
                if (charge < 0)
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

                if (charge < 0)
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
        string label = $"{charge:n1} C";
        Font font = new Font("Arial", (float)Math.Sqrt(radius) / 5f, FontStyle.Bold);
        Brush brush = new SolidBrush(Color.FromArgb(230, Color.White));
        float width = g.MeasureString(label, font).Width;
        float height = g.MeasureString(label, font).Height;

        g.DrawString(label, font, brush, radius - width / 2, radius - height / 2);

        g.TranslateTransform(radius - GetX(t), radius - GetY(t));
    }

}