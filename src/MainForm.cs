using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace UPG_SP_2024
{
    public partial class MainForm : Form
    {

        private int startTime;
        public MainForm(int scenario_num, int gridX, int gridY)
        {
            Console.WriteLine(scenario_num);
            SettingsObject.colorMap = false;
            SettingsObject.gridX = gridX;
            SettingsObject.gridY = gridY;
            SettingsObject.scenario = scenario_num;
            SettingsObject.gridShown = false;
            SettingsObject.corners = new float[4];

            this.ClientSize = new System.Drawing.Size(800, 600);
            this.KeyPreview = true;
            this.KeyDown += (o, e) =>
            {
                if (e.KeyCode == Keys.I)
                {
                    drawingPanel.scenario.ZoomIn(2, 2);
                }
                if (e.KeyCode == Keys.O) drawingPanel.scenario.ZoomOut(2, 2);
            };
            InitializeComponent();
            GraphPanel g = new GraphPanel();
            SettingsObject.graphPanel = g;
            DrawingPanel p = (DrawingPanel)drawingPanel;
            // Create and add the custom ControlPanel
            ControlPanel c = new ControlPanel();
            this.Controls.Add(c);
            this.Controls.Add(g);
            SettingsObject.drawingPanel = p;
            SettingsObject.controlPanel = c;
            p.SetScenario(scenario_num);
            p.scenario.CreateProbe(new PointF(0, 0), 1, (float)Math.PI / 6);
            this.Text = "A23B0149P + A23B0152P - Semestralni prace KIV/UPG 2024/2025";
            var timer = new System.Windows.Forms.Timer();
            timer.Tick += TimerTick;
            timer.Interval = 50;

            startTime = Environment.TickCount;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            this.drawingPanel.Invalidate();
        }

        private void controlPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
