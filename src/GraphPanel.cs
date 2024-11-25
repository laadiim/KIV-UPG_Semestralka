using System;
using System.Drawing;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024
{
    public class GraphPanel : Panel
    {
        private ComboBox viewSelector;
        private Panel contentPanel;
        private FlowLayoutPanel probesPanel;
        private CartesianChart graphChart;
        private int probe = 0;

        public GraphPanel()
        {
            InitializeComponents();
        }

        //public void 

        private void InitializeComponents()
        {
            // Dropdown menu to select between "Probes" and "Graph"
            viewSelector = new ComboBox
            {
                Location = new Point(10, 10),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            if (SettingsObject.drawingPanel != null)
            {
                for (int i = 0; i < SettingsObject.drawingPanel.scenario.probes.Count; i++)
                {
                    viewSelector.Items.Add($"Probe {i}");
                }
            }
            viewSelector.SelectedIndexChanged += (o, e) =>
            {
                this.probe = viewSelector.SelectedIndex;
            };

            this.Controls.Add(viewSelector);

            // Panel to hold the content (either probes or graph)
            contentPanel = new Panel
            {
                Location = new Point(10, 50),
                Size = new Size(600, 400),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(contentPanel);

            // Initialize the Graph view
            InitializeGraph();

            contentPanel.Controls.Add(graphChart);
        }

        private void InitializeGraph()
        {
            ISeries[] Series = [
                new LineSeries<double>
                {
                    Values = [5, 0, 5, 0, 5, 0],
                    Fill = null,
                    GeometrySize = 0,
                    // use the line smoothness property to control the curve
                    // it goes from 0 to 1
                    // where 0 is a straight line and 1 the most curved
                    //LineSmoothness = 0 
                },
            ];
            graphChart = new CartesianChart
            {
                Dock = DockStyle.Fill,
                Series = Series,
                //Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
            };

        }

        private void SwitchView(int viewIndex)
        {
            contentPanel.Controls.Clear();

            if (viewIndex == 0) // Probes view
            {
                contentPanel.Controls.Add(probesPanel);
            }
            else if (viewIndex == 1) // Graph view
            {
                contentPanel.Controls.Add(graphChart);
            }
        }
    }
}
