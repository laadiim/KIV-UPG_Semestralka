﻿using System.Drawing.Drawing2D;
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

    /// <summary>
    /// kostruktor
    /// </summary>
    /// <param name="charge">retezec pro vypocet naboje</param>
    /// <param name="X">soradnice na ose X</param>
    /// <param name="Y">souradnice na ose Y</param>
    /// <param name="id">id</param>
    /// <param name="startTime">zacatek simulace</param>
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
        return $"naboj:{this.chargeStr};{this.X};{this.Y}";
    }

    /// <summary>
    /// vrati pozici na ose X
    /// </summary>
    /// <returns></returns>
    private float GetX()
    {
        return X + SettingsObject.worldCenter.X;
    }

    /// <summary>
    /// vrati pozici na ose Y
    /// </summary>
    /// <returns></returns>
    public float GetY()
    {
        return Y + SettingsObject.worldCenter.Y;
    }

    public bool IsHit(PointF point)
    {
        float distance = Vector2.Distance(new Vector2(point.X, point.Y), new Vector2(GetX(), GetY()));
        return distance <= radius;
    }

    public void Drag(Vector2 v)
    {
		this.X += v.X;
        this.Y += v.Y;
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
        return new PointF(GetX(), -GetY());
    }

    public void SetPosition(float X, float Y)
    {
        this.X = X;
        this.Y = -Y;
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

        g.TranslateTransform(radius - GetX(), radius - GetY());
    }

}
