using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024
{
    public partial class GraphForm : Form
    {
        public GraphForm()
        {
            InitializeComponent();
            for (int i = 0; i < SettingsObject.probes.Count; i++)
            {
                GraphTab p = new GraphTab(SettingsObject.probes[i], i); 
                this.tabControl.Controls.Add(p);
                p.Location = new Point(4, 24);
                p.Name = $"Probe {i}";
                p.Padding = new Padding(3);
                p.Dock = DockStyle.Fill;
                p.TabIndex = 0;
                p.Text = $"Probe {i}";
                p.UseVisualStyleBackColor = true;
            }
            this.FormClosing += (o, e) => SettingsObject.graphForm = null;
        }

        public void UpdateGraph()
        {
            foreach (GraphTab tab in tabControl.Controls) tab.UpdateChart();
        }
    }
}
