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

        public ControlPanel()
        {
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

            // Scenario selection dropdown
            scenarioDropdown = new ComboBox
            {
                Location = new Point(10, 130),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Populate dropdown with scenario numbers 0-6
            for (int i = 0; i <= 6; i++)
            {
                scenarioDropdown.Items.Add($"Scenario {i}");
            }

            // Set default selection
            scenarioDropdown.SelectedIndex = SettingsObject.scenario;

            // Handle scenario selection change
            scenarioDropdown.SelectedIndexChanged += (o, e) =>
            {
                int selectedScenario = scenarioDropdown.SelectedIndex;
                Console.WriteLine($"Scenario changed to: {selectedScenario}");

                // Trigger the event to notify listeners
                SettingsObject.scenario = selectedScenario;
                SettingsObject.drawingPanel.SetScenario( selectedScenario );
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
                    Value = Math.Max(1, SettingsObject.gridX) // Default gridX value
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
                    Value = Math.Max(1, SettingsObject.gridY) // Default gridX value
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
        }

        private void GraphButton_Click(object? sender, EventArgs e)
        {
            if (SettingsObject.graphForm != null) return;
            GraphForm g = new GraphForm();
            g.Show();
            SettingsObject.graphForm = g;

        }
    }
}
