﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using UPG_SP_2024.Interfaces;
using UPG_SP_2024.Primitives;


namespace UPG_SP_2024
{

    /// <summary>
    /// The main panel with the custom visualization
    /// </summary>
    public class DrawingPanel : Panel
    {
        Scenario scenario;
        private int startTime { get; set; }
        private int chargeHit = -1;
        private float scale = 1;
        private PointF prevMouse = new PointF(0, 0);
        /// <summary>
        /// konstruktor DrawingPanel
        /// </summary>
        /// <param name="scenarioNum">ocislovani scenaria od 0 do 3</param>
        public DrawingPanel()
        {
            this.DoubleBuffered = true;
            this.ClientSize = new System.Drawing.Size(800, 600);
            scenario = new Scenario();
            this.MouseDown += (o, e) =>
            {
                PointF point = new PointF(e.X , e.Y);
                prevMouse = new PointF(point.X, point.Y);
                point.X = (point.X - this.Width / 2) / scale;
                point.Y = (point.Y - this.Height / 2) / scale;
                INaboj[] charges = scenario.GetCharges();
                if (charges.Length == 0) return;
                for (int i = 0; i < charges.Length; i++)
                {
                    if (charges[i] == null) continue;
                    chargeHit = charges[i].IsHit(point) ? charges[i].GetID() : chargeHit;
                }
            };
            this.MouseMove += (o, e) =>
            {
                if (chargeHit == -1) return;
                if (e.X < this.Width / 9 || e.X >= this.Width - this.Width / 9 || e.Y < this.Height / 9 || e.Y >= this.Height - this.Height / 9)
                {
                    chargeHit = -1;
                    return;
                }
                INaboj charge = scenario.GetCharge(chargeHit);
                charge.Drag(new PointF((e.X - prevMouse.X) / scale, (e.Y - prevMouse.Y) / scale));
                prevMouse.X = e.X;
                prevMouse.Y = e.Y;
            };
            this.MouseUp += (o, e) =>
            {
                chargeHit = -1;
            };
        }


        public void SetScenario(int scenarioNum)
        {
            switch(scenarioNum)
            {
                case 0:
                    INaboj naboj = new Naboj(1, new PointF(0, 0), 0);
                    scenario.AddCharge(naboj);
                    break;
                case 1:
                    INaboj naboj1 = new Naboj(1, new PointF(-1, 0), 0);
                    INaboj naboj2 = new Naboj(1, new PointF(1, 0), 1);
                    scenario.AddCharge(naboj1);
                    scenario.AddCharge(naboj2);
                    break;
                case 2:
                    INaboj naboj3 = new Naboj(-1, new PointF(-1, 0), 0);
                    INaboj naboj4 = new Naboj(2, new PointF(1, 0), 1);
                    scenario.AddCharge(naboj3);
                    scenario.AddCharge(naboj4);
                    break;
                case 3:
                    INaboj naboj5 = new Naboj(1, new PointF(-1, -1), 0);
                    INaboj naboj6 = new Naboj(2, new PointF(1, -1), 1);
                    INaboj naboj7 = new Naboj(-3, new PointF(1, 1), 2);
                    INaboj naboj8 = new Naboj(-4, new PointF(-1, 1), 3);
                    scenario.AddCharge(naboj5);
                    scenario.AddCharge(naboj6);
                    scenario.AddCharge(naboj7);
                    scenario.AddCharge(naboj8);
                    break;
            }
        }

        /// <summary>TODO: Custom visualization code comes into this method</summary>
        /// <remarks>Raises the <see cref="E:System.Windows.Forms.Control.Paint">Paint</see> event.</remarks>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs">PaintEventArgs</see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;



            //TODO: Add custom paint code here
            float panelHeight = this.Height;
            float panelWidth = this.Width;
            g.TranslateTransform(panelWidth / 2, panelHeight / 2);
            
            this.scale = scenario.Draw(g, panelWidth, panelHeight, this.startTime, true, this.chargeHit);

            // Calling the base class OnPaint
            base.OnPaint(e);
        }

        /// <summary>
        /// Fires the event indicating that the panel has been resized. Inheriting controls should use this in favor of actually listening to the event, but should still call <span class="keyword">base.onResize</span> to ensure that the event is fired for external listeners.
        /// </summary>
        /// <param name="eventargs">An <see cref="T:System.EventArgs">EventArgs</see> that contains the event data.</param>
        protected override void OnResize(EventArgs eventargs)
        {
            this.Invalidate();  //ensure repaint

            base.OnResize(eventargs);
        }
    }

}
