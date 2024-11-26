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
        private CartesianChart graphChart;
        private int probe = 0;

        public GraphPanel()
        {
            InitializeComponents();
        }

        
        public void ResetProbes()
        {
            viewSelector.Items.Clear();
            viewSelector.Items.Add("TEST");
            System.Console.WriteLine($"Probes: {viewSelector.Items.Count}");

            if (SettingsObject.drawingPanel != null)
            {
                for (int i = 0; i < SettingsObject.drawingPanel.scenario.probes.Count; i++)
                {
                    viewSelector.Items.Add($"Probe {i}");
                }
                System.Console.WriteLine($"Probes: {viewSelector.Items.Count}");
            }
            else
            {
                System.Console.WriteLine("Line 40: No drawing panel");
            }

            // Set default selected index if items exist
            if (viewSelector.Items.Count > 0)
            {
                viewSelector.SelectedIndex = 0;
                foreach (var VARIABLE in viewSelector.Items)
                {
                    Console.WriteLine(VARIABLE);
                }
            }
            else
            {
                System.Console.WriteLine("Line 50: No items in viewSelector");
            }
            this.Invalidate();
        }


        private void InitializeComponents()
        {
            // Dropdown menu to select between probes or graph
            viewSelector = new ComboBox
            {
                Location = new Point(10, 10),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Initialize ComboBox with available probes if available
            if (SettingsObject.drawingPanel != null)
            {
                for (int i = 0; i < SettingsObject.drawingPanel.scenario.probes.Count; i++)
                {
                    viewSelector.Items.Add($"Probe {i}");
                }
                System.Console.WriteLine($"Probes: {viewSelector.Items.Count}");
            }
            else
            {
                System.Console.WriteLine("Line 76: No drawing panel");
            }

            // Set up event to handle ComboBox selection change
            viewSelector.SelectedIndexChanged += (o, e) =>
            {
                this.probe = viewSelector.SelectedIndex;
            };

            this.Controls.Add(viewSelector);

            // Panel to hold the graph
            contentPanel = new Panel
            {
                Location = new Point(10, 50),
                Size = new Size(600, 400),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(contentPanel);

            // Initialize the Graph view
            InitializeGraph();

            // Add graph to contentPanel
            contentPanel.Controls.Add(graphChart);
        }

        private void InitializeGraph()
        {
            ISeries[] Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = new double[] { 5, 0, 5, 0, 5, 0 },
                    Fill = null,
                    GeometrySize = 0,
                    // Line smoothness setting (0 for straight line, 1 for smooth curve)
                    LineSmoothness = 0 
                }
            };

            graphChart = new CartesianChart
            {
                Dock = DockStyle.Fill,
                Series = Series
            };
        }
    }
}
