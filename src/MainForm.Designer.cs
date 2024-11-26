using System.Drawing;
using System.Windows.Forms;

namespace UPG_SP_2024
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        
       private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            drawingPanel = new DrawingPanel();
            splitContainer2 = new SplitContainer();
            controlPanel = new ControlPanel();
            graphPanel1 = new GraphPanel();

            // Configure splitContainer1
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Panel1.Controls.Add(drawingPanel);
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1085, 683);
            splitContainer1.Panel1MinSize = 100;
            splitContainer1.Panel2MinSize = 200;
            splitContainer1.SplitterDistance = 700; // Valid default value

            // Configure splitContainer2
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Orientation = Orientation.Horizontal;
            splitContainer2.Panel1.Controls.Add(controlPanel);
            splitContainer2.Panel2.Controls.Add(graphPanel1);
            splitContainer2.Panel1MinSize = 100;
            splitContainer2.Panel2MinSize = 100;
            splitContainer2.SplitterDistance = 300; // Valid default value

            // Configure panels
            drawingPanel.Dock = DockStyle.Fill;
            controlPanel.Dock = DockStyle.Fill;
            graphPanel1.Dock = DockStyle.Fill;

            // Add to form
            this.Controls.Add(splitContainer1);
            this.ClientSize = new Size(1085, 683);
            this.StartPosition = FormStartPosition.CenterScreen;

            Console.WriteLine($"graphPanel1 size: {graphPanel1.Width}x{graphPanel1.Height}");
            Console.WriteLine($"graphPanel1 Visible: {graphPanel1.Visible}");
            this.Text = "Main Form Debugging";
        }

        private void graphPanel1_Paint(object sender, PaintEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion

        private SplitContainer splitContainer1;
        private DrawingPanel drawingPanel;
        private SplitContainer splitContainer2;
        private ControlPanel controlPanel;
        private GraphPanel graphPanel1;
    }
}
