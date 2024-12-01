using System;
using System.Drawing;
using System.Windows.Forms;
using UPG_SP_2024.Interfaces;

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

                if (e.KeyCode == Keys.Up)
                {
                    drawingPanel?.scenario?.Move(0, 0.5f);
                }
                if (e.KeyCode == Keys.Down)
                {
                    drawingPanel?.scenario?.Move(0, -0.5f);
                }
                if (e.KeyCode == Keys.Left)
                {
                    drawingPanel?.scenario?.Move(-0.5f, 0);
                }
                if (e.KeyCode == Keys.Right)
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

            // Configure the drawing panel and scenario
            p.SetScenario(scenario_num);
            p.scenario.CreateProbe(new PointF(0, 0), 1, (float)Math.PI / 6);

            // Reset probes in GraphPanel
            //g.ResetProbes();

            // Set form properties
            this.Text = "A23B0149P + A23B0152P - Semestralni prace KIV/UPG 2024/2025";

            // Set up timer for refreshing the drawing panel
            var timer = new System.Windows.Forms.Timer
            {
                Interval = 50 // Milliseconds
            };
            timer.Tick += TimerTick;

            startTime = Environment.TickCount;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            ticks++;
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
