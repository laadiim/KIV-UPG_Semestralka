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
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(drawingPanel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1085, 683);
            splitContainer1.SplitterDistance = 836;
            splitContainer1.TabIndex = 0;
            // 
            // drawingPanel
            // 
            drawingPanel.Dock = DockStyle.Fill;
            drawingPanel.Location = new Point(0, 0);
            drawingPanel.Name = "drawingPanel";
            drawingPanel.Size = new Size(836, 683);
            drawingPanel.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(controlPanel);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(graphPanel1);
            splitContainer2.Size = new Size(245, 683);
            splitContainer2.SplitterDistance = 445;
            splitContainer2.TabIndex = 0;
            // 
            // controlPanel
            // 
            controlPanel.Dock = DockStyle.Fill;
            controlPanel.Location = new Point(0, 0);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new Size(245, 445);
            controlPanel.TabIndex = 0;
            // 
            // graphPanel1
            // 
            graphPanel1.Dock = DockStyle.Fill;
            graphPanel1.Location = new Point(0, 0);
            graphPanel1.Name = "graphPanel1";
            graphPanel1.Size = new Size(245, 234);
            graphPanel1.TabIndex = 0;
            graphPanel1.Paint += graphPanel1_Paint;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1085, 683);
            Controls.Add(splitContainer1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "s";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
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
