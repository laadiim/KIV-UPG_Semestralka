using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace UPG_SP_2024
{
    public partial class MainForm : Form
    {
        
        private int startTime;
        
        public MainForm(int scenario_num)
        {
            Console.WriteLine(scenario_num);
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
            DrawingPanel p = (DrawingPanel)drawingPanel;
            p.SetScenario(scenario_num);
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
    }
}
