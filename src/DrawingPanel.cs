using System.Drawing;
using System.Numerics;
using UPG_SP_2024.Interfaces;
using UPG_SP_2024.Primitives;


namespace UPG_SP_2024
{

    /// <summary>
    /// The main panel with the custom visualization
    /// </summary>
    public class DrawingPanel : Panel
    {
        /// <summary>
        /// instance scenare
        /// </summary>
        public Scenario scenario;

        private int StartTime { get; set; }
        private int chargeHit = -1;

        /// <summary>
        /// nakliknuta sondy
        /// </summary>
        public int probeHit = -1;

        private float scale = 1;
        private bool rightDown = false;
        private PointF prevMouse = new PointF(0, 0);

        /// <summary>
        /// cas zachyceni sondy
        /// </summary>
        public float timeProbeCaught = 0;
        
        /// <summary>
        /// konstruktor DrawingPanel
        /// </summary>
        public DrawingPanel()
        {
            this.DoubleBuffered = true;
            this.ClientSize = new System.Drawing.Size(800, 600);

            scenario = new Scenario();

            // obsluha scrollu kolecka mysi
            this.MouseWheel += (o, e) =>
            {
                if (e.Delta > 0) scenario.ZoomIn(e.Delta / 90f, e.Delta / 90f);
                else scenario.ZoomOut(-e.Delta / 90f, -e.Delta / 90f);
            };

            // obsluha stisknuti tlacitka mysi
            this.MouseDown += (o, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    INaboj[] charges;
                    PointF point = new PointF(e.X, e.Y);

                    prevMouse = new PointF(point.X, point.Y);

                    point.X = (point.X - this.Width / 2) / scale;
                    point.Y = (point.Y - this.Height / 2) / scale;

                    for (int i = 0; i < SettingsObject.probes.Count; i++)
                    {
                        if (SettingsObject.probes[i] == null) continue;
                        probeHit = SettingsObject.probes[i].IsHit(point) ? SettingsObject.probes[i].GetID() : probeHit;
                        timeProbeCaught = Environment.TickCount;
                    }

                    if (probeHit != -1) return;

                    try
                    {
                        charges = scenario.GetCharges();
                    }
                    catch
                    {
                        throw new Exception("scenario neobsahuje naboje");
                    }

                    if (charges.Length == 0) return;

                    for (int i = 0; i < charges.Length; i++)
                    {
                        if (charges[i] == null) continue;
                        chargeHit = charges[i].IsHit(point) ? charges[i].GetID() : chargeHit;
                    }
                    
                    if (chargeHit == -1 && probeHit == -1)
                    {
                        PointF p = new PointF(point.X - SettingsObject.worldCenter.X,
                            point.Y - SettingsObject.worldCenter.Y);
                        scenario.CreateProbe(p, 0, 0);
                    }
                }

                if (e.Button == MouseButtons.Right)
                {
                    this.rightDown = true;
                }
            };

            // obsluha pohybu mysi
            this.MouseMove += (o, e) =>
            {
                INaboj charge;
                IProbe probe;

                PointF point = new PointF((e.X - this.Width / 2) / scale, (e.Y - this.Height / 2) / scale);
                if (chargeHit != -1)
                {
                    //if (point.X < (scenario.corners[2]) || point.X >= (scenario.corners[0]) || point.Y < (scenario.corners[3]) || point.Y >= (scenario.corners[1]))
                    //{
                    //    chargeHit = -1;
                    //    return;
                    //}

                    try
                    {
                        charge = scenario.GetCharge(chargeHit);
                    }
                    catch
                    {
                        throw new Exception("naboj se nepodarilo ziskat");
                    }

                    charge.Drag(new Vector2((e.X - prevMouse.X) / scale, (e.Y - prevMouse.Y) / scale));
                }
                if (probeHit != -1)
                {
                    try
                    {
                        probe = scenario.GetProbe(probeHit);
                    }
                    catch
                    {
                        throw new Exception("sondu se nepodarilo ziskat");
                    }
                    
                    probe.Drag(new Vector2((e.X - prevMouse.X) / scale, (e.Y - prevMouse.Y) / scale));
                }

                if (rightDown)
                {
                    //if (point.X < (scenario.corners[2]) || point.X >= (scenario.corners[0]) || point.Y < (scenario.corners[3]) || point.Y >= (scenario.corners[1]))
                    //{
                    //    rightDown = false;
                    //    return;
                    //}
                    scenario.Move((e.X - prevMouse.X) / scale, (e.Y - prevMouse.Y) / scale);
                }
                prevMouse.X = e.X;
                prevMouse.Y = e.Y;
            };

            // obsluha pusteni tlacitka mysi
            this.MouseUp += (o, e) =>
            {
                chargeHit = -1;

                probeHit = -1;
                rightDown = false;
            };
        }


        /// <summary>
        /// nacte scenar ze souboru
        /// </summary>
        /// <param name="filename">cesta k souboru</param>
        public void LoadScenario(string filename)
        {
            scenario.EmptyCharges();
            scenario.EmptyProbes();
            float startTime = SettingsObject.startTime = Environment.TickCount;
            StreamReader sr = new StreamReader(filename);
            List<string> lines = new List<string>();
            string line = sr.ReadLine();
            //Continue to read until you reach end of file
            while (line != null)
            {
                //write the line to console window
                line = line.Replace(",", ".");
                lines.Add(line);
                //Read the next line
                line = sr.ReadLine();
            }
            sr.Close();
            scenario.Load(lines.ToArray(), startTime);
            SettingsObject.openFile = filename;
            
            if (SettingsObject.probeForm != null) SettingsObject.probeForm.Reload();
            if (SettingsObject.chargeForm != null) SettingsObject.chargeForm.Reload();
        }


        /// <summary>
        /// ulozi scenar do souboru
        /// </summary>
        /// <param name="filename">cesta k souboru</param>
        public void SaveScenario(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            string s = scenario.Save();
            sw.Write(s);
            sw.Close();
        }

        /// <summary>
        /// nastaveni scenare podle cisla
        /// </summary>
        /// <param name="scenarioNum">cislo scenare</param>
        public void SetScenario(int scenarioNum)
        {
            string[] files =
            {   
                "scen0.upg",
                "scen1.upg",
                "scen2.upg",
                "scen3.upg",
                "scen4.upg",
                "scen5.upg",
            };

            LoadScenario(files[scenarioNum]);
        }

        /// <summary>Custom visualization code comes into this method</summary>
        /// <remarks>Raises the <see cref="E:System.Windows.Forms.Control.Paint">Paint</see> event.</remarks>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs">PaintEventArgs</see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //TODO: Add custom paint code here
            float panelHeight = this.Height;
            float panelWidth = this.Width;
            g.TranslateTransform(panelWidth / 2, panelHeight / 2);
            
            this.scale = scenario.Draw(g, panelWidth, panelHeight, this.StartTime, this.chargeHit);

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
