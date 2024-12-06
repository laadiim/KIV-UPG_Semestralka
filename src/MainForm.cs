using System;
using System.Drawing;
using System.Windows.Forms;
using UPG_SP_2024.Interfaces;
using UPG_SP_2024.Primitives;

namespace UPG_SP_2024
{
    public partial class MainForm : Form
    {
        private int startTime;
        private int ticks = 0;

        public MainForm(int scenario_num, int gridX, int gridY)
        {
            // Initialize settings
            Console.WriteLine($"Scenario: {scenario_num}");
            SettingsObject.colorMap = false;
            SettingsObject.gridX = gridX;
            SettingsObject.gridY = gridY;
            SettingsObject.scenario = scenario_num;
            SettingsObject.gridShown = false;
            SettingsObject.corners = new float[4];
            SettingsObject.probes = new List<IProbe>();
			SettingsObject.halfWidth = 2;
			SettingsObject.halfHeight = 2;
			SettingsObject.worldCenter = new PointF(0, 0);
            SettingsObject.tickLen = 30;
            SettingsObject.maxProbes = 7;

            // Configure the form
            this.ClientSize = new Size(800, 600);
            this.KeyPreview = true;

            // Add key event handlers
            this.KeyDown += (o, e) =>
            {
                if (e.KeyCode == Keys.I)
                {
                    drawingPanel?.scenario?.ZoomIn(2, 2);
                }
                if (e.KeyCode == Keys.O)
                {
                    drawingPanel?.scenario?.ZoomOut(2, 2);
                }

                if (e.KeyCode == Keys.S)
                {
                    drawingPanel?.scenario?.Move(0, -0.5f);
                }
                if (e.KeyCode == Keys.W)
                {
                    drawingPanel?.scenario?.Move(0, 0.5f);
                }
                if (e.KeyCode == Keys.D)
                {
                    drawingPanel?.scenario?.Move(-0.5f, 0);
                }
                if (e.KeyCode == Keys.A)
                {
                    drawingPanel?.scenario?.Move(0.5f, 0);
                }
            };

            // Initialize UI components
            InitializeComponent();

            // Create and configure panels
            var p = this.drawingPanel; // Instantiate directly instead of casting
            SettingsObject.drawingPanel = p;

            var c = new ControlPanel();
            SettingsObject.controlPanel = c;

            // Add controls to the form
            this.Controls.Add(c);

            // Reset probes in GraphPanel
            //g.ResetProbes();

            // Set form properties
            this.Text = "A23B0149P + A23B0152P - Semestralni prace KIV/UPG 2024/2025";

            // Set up timer for refreshing the drawing panel
            var timer = new System.Windows.Forms.Timer
            {
                Interval = SettingsObject.tickLen // Milliseconds
            };
            timer.Tick += TimerTick;

            startTime = Environment.TickCount;
            SettingsObject.startTime = startTime;
            timer.Start();
            p.SetScenario(scenario_num);
        }

        /// <summary>
        /// obsluha noveho snimku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            ticks++;
            if (SettingsObject.drawingPanel.probeHit != -1)
            {
                SettingsObject.drawingPanel.scenario.GetProbe(SettingsObject.drawingPanel.probeHit).AddTimeHeld(Environment.TickCount - SettingsObject.drawingPanel.timeProbeCaught);
            }

            SettingsObject.drawingPanel.timeProbeCaught = Environment.TickCount;
            foreach (IProbe probe in SettingsObject.probes)
            {
                probe.Tick();
            }
            drawingPanel?.Invalidate(); // Ensure drawingPanel is not null
            if (SettingsObject.graphForm != null && ticks % 20 == 0)
            {
                SettingsObject.graphForm.UpdateGraph();
                ticks = 0;
            }
        }

        private void controlPanel_Paint(object sender, PaintEventArgs e)
        {
            // Placeholder for future use (if needed)
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            // Placeholder for future use (if needed)
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
