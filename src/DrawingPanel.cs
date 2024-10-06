using System;
using System.Drawing;
using System.Windows.Forms;
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
        /// <summary>
        /// konstruktor DrawingPanel
        /// </summary>
        /// <param name="scenario_num">ocislovani scenaria od 0 do 3</param>
        public DrawingPanel(int scenario_num)
        {
            this.ClientSize = new System.Drawing.Size(800, 600);
            scenario = new Scenario();
            switch(scenario_num)
            {
                case 0:
                    INaboj naboj = new Naboj(1,5, new PointF(0, 0), 0);
                    scenario.AddCharge(naboj);
                    break;
                case 1:
                    INaboj naboj1 = new Naboj(1, 5, new PointF(-1, 0), 0);
                    INaboj naboj2 = new Naboj(1, 5, new PointF(1, 0), 1);
                    scenario.AddCharge(naboj1);
                    scenario.AddCharge(naboj2);
                    break;
                case 2:
                    INaboj naboj3 = new Naboj(-1, 5, new PointF(-1, 0), 0);
                    INaboj naboj4 = new Naboj(2, 5, new PointF(1, 0), 1);
                    scenario.AddCharge(naboj3);
                    scenario.AddCharge(naboj4);
                    break;
                case 3:
                    INaboj naboj5 = new Naboj(1, 5, new PointF(-1, -1), 0);
                    INaboj naboj6 = new Naboj(2, 5, new PointF(1, -1), 1);
                    INaboj naboj7 = new Naboj(-3, 5, new PointF(1, 1), 0);
                    INaboj naboj8 = new Naboj(-4, 5, new PointF(-1, 1), 1);
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
            INaboj n1 = new Naboj(5, 20, new PointF(100, 100), 1);
            INaboj n2 = new Naboj(16, 40, new PointF(200, 150), 2);

            n1.Draw(g);
            n2.Draw(g);

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
