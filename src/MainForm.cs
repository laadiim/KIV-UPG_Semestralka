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
            this.ClientSize = new System.Drawing.Size(800, 600);
            InitializeComponent(scenario_num);
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
