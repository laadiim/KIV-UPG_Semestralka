using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UPG_SP_2024
{
    public class ControlPanel : Panel
    {
        private CheckBox colormapCheckBox;
        private CheckBox gridCheckBox;
        private ComboBox scenarioDropdown;
        private NumericUpDown gridXSpinner;
        private NumericUpDown gridYSpinner;

        private readonly double[] boundaries = { 0.0, 0.25, 0.5, 0.75, 1.0 };

        private readonly int[,] colors = {
            { 80, 50, 30 },      // fifth
            { 70, 40, 80 },      // fourth
            { 100, 60, 140 },    // third 
            { 140, 120, 190 },   // second
            { 210, 190, 220 }   // first  
        };

        private readonly double[] boundaryDiffs;
        private readonly int[,] colorDiffs;

        public ControlPanel()
        {
            boundaryDiffs = new double[boundaries.Length - 1];
            for (int i = 0; i < boundaries.Length - 1; i++)
            {
                boundaryDiffs[i] = boundaries[i + 1] - boundaries[i];
            }

            colorDiffs = new int[colors.GetLength(0) - 1, 3];
            for (int i = 0; i < colors.GetLength(0) - 1; i++)
            {
                colorDiffs[i, 0] = colors[i + 1, 0] - colors[i, 0];
                colorDiffs[i, 1] = colors[i + 1, 1] - colors[i, 1];
                colorDiffs[i, 2] = colors[i + 1, 2] - colors[i, 2];
            }
            if (!IsInDesignMode())
            {
                EnsureSettingsDefaults(); // Ensure SettingsObject values are valid
            }
            InitializeControls();
        }
        
        private bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        private void EnsureSettingsDefaults()
        {
            SettingsObject.gridX = Math.Max(1, SettingsObject.gridX);
            SettingsObject.gridY = Math.Max(1, SettingsObject.gridY);
        }

        private void InitializeControls()
        {
            Label label = new Label
            {
                Text = "Control Panel",
                Location = new Point(10, 10),
                AutoSize = true
            };
            this.Controls.Add(label);

            colormapCheckBox = new CheckBox
            {
                Text = "Enable Colormap",
                Location = new Point(10, 40),
                AutoSize = true
            };
            colormapCheckBox.CheckedChanged += (o, e) =>
            {
                SettingsObject.colorMap = colormapCheckBox.Checked;
            };
            this.Controls.Add(colormapCheckBox);

            gridCheckBox = new CheckBox
            {
                Text = "Enable Grid",
                Location = new Point(10, 70),
                AutoSize = true
            };
            gridCheckBox.CheckedChanged += (o, e) =>
            {
                SettingsObject.gridShown = gridCheckBox.Checked;
                Console.WriteLine(SettingsObject.gridShown);
            };
            this.Controls.Add(gridCheckBox);

            Label dropdownLabel = new Label
            {
                Text = "Select Scenario:",
                Location = new Point(10, 100),
                AutoSize = true
            };
            this.Controls.Add(dropdownLabel);

            scenarioDropdown = new ComboBox
            {
                Location = new Point(10, 130),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            for (int i = 0; i <= 6; i++)
            {
                scenarioDropdown.Items.Add($"Scenario {i}");
            }
            scenarioDropdown.SelectedIndex = SettingsObject.scenario;
            scenarioDropdown.SelectedIndexChanged += (o, e) =>
            {
                int selectedScenario = scenarioDropdown.SelectedIndex;
                Console.WriteLine($"Scenario changed to: {selectedScenario}");
                SettingsObject.scenario = selectedScenario;
                SettingsObject.drawingPanel.SetScenario(selectedScenario);
            };
            this.Controls.Add(scenarioDropdown);

            Label gridLabel = new Label
            {
                Text = "Grid:",
                Location = new Point(10, 160),
                AutoSize = true
            };
            this.Controls.Add(gridLabel);

            Label XLabel = new Label
            {
                Text = "X:",
                Location = new Point(10, 192),
                AutoSize = true
            };
            this.Controls.Add(XLabel);

            gridXSpinner = new NumericUpDown
            {
                Location = new Point(27, 190),
                Width = 100,
                Minimum = 1,
                Maximum = 100,
                Value = Math.Max(1, SettingsObject.gridX)
            };
            gridXSpinner.ValueChanged += (o, e) =>
            {
                SettingsObject.gridX = (int)gridXSpinner.Value;
            };
            this.Controls.Add(gridXSpinner);

            Label YLabel = new Label
            {
                Text = "Y:",
                Location = new Point(10, 220),
                AutoSize = true
            };
            this.Controls.Add(YLabel);

            gridYSpinner = new NumericUpDown
            {
                Location = new Point(27, 218),
                Width = 100,
                Minimum = 1,
                Maximum = 100,
                Value = Math.Max(1, SettingsObject.gridY)
            };
            gridYSpinner.ValueChanged += (o, e) =>
            {
                SettingsObject.gridY = (int)gridYSpinner.Value;
            };
            this.Controls.Add(gridYSpinner);

            Button graphButton = new Button
            {
                Location = new Point(10, 250),
                Text = "Show graph"
            };
            this.Controls.Add(graphButton);
            graphButton.Click += GraphButton_Click;

            // Add colormap legend
            Panel legendPanel = new Panel
            {
                Location = new Point(10, 300),
                Size = new Size(180, 160),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            Label legendTitle = new Label
            {
                Text = "Colormap Legend",
                Location = new Point(10, 5),
                AutoSize = true
            };
            legendPanel.Controls.Add(legendTitle);

            // Example color entries
            AddLegendEntry(legendPanel, "0 TN/C", this.GetColorFromIntensity(0), 25);
            AddLegendEntry(legendPanel, "25E-2 TN/C", this.GetColorFromIntensity(2.5), 50);
            AddLegendEntry(legendPanel, "50E-2 TN/C", this.GetColorFromIntensity(5), 75);
            AddLegendEntry(legendPanel, "75E-2 TN/C", this.GetColorFromIntensity(7.5), 100);
            AddLegendEntry(legendPanel, ">100E-2 TN/C", this.GetColorFromIntensity(10), 125);

            this.Controls.Add(legendPanel);
        }

        private void AddLegendEntry(Panel panel, string text, Color color, int yOffset)
        {
            Panel colorBox = new Panel
            {
                Location = new Point(10, yOffset),
                Size = new Size(20, 20),
                BackColor = color
            };
            panel.Controls.Add(colorBox);

            Label legendLabel = new Label
            {
                Text = text,
                Location = new Point(40, yOffset + 2),
                AutoSize = true
            };
            panel.Controls.Add(legendLabel);
        }


        private void GraphButton_Click(object? sender, EventArgs e)
        {
            if (SettingsObject.graphForm != null) return;
            GraphForm g = new GraphForm();
            g.Show();
            SettingsObject.graphForm = g;

        }

        public Color GetColorFromIntensity(double intensity)
        {
            // Cap the intensity value to a maximum of 1.0 for a smoother transition.
            double intst = Math.Min(10, Math.Max(0, intensity)) / 10f;

            // Use binary search to find the correct segment
            int index = Array.BinarySearch(boundaries, intst);
            if (index < 0)
            {
                // Adjust to the nearest lower boundary index for BinarySearch results
                index = ~index - 1;
            }

            // Handle edge case where intst == 1.0
            if (index >= boundaries.Length - 1)
            {
                index = boundaries.Length - 2; // Assign to the last segment
            }

            // Calculate factor for interpolation
            double factor = (intst - boundaries[index]) / boundaryDiffs[index];

            // Ensure factor is within [0,1]
            factor = Math.Max(0.0, Math.Min(1.0, factor));

            // Interpolate colors
            int r = (int)(colors[index, 0] + factor * colorDiffs[index, 0]);
            int g = (int)(colors[index, 1] + factor * colorDiffs[index, 1]);
            int b = (int)(colors[index, 2] + factor * colorDiffs[index, 2]);

            // Clamp RGB values to [0,255]
            r = Math.Max(0, Math.Min(255, r));
            g = Math.Max(0, Math.Min(255, g));
            b = Math.Max(0, Math.Min(255, b));

            return Color.FromArgb(b, g, r);
        }
    }
}
