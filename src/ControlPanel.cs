﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UPG_SP_2024
{
    /// <summary>
    /// trida ovladaciho panelu
    /// </summary>
    public class ControlPanel : Panel
    {
        private CheckBox colormapCheckBox;
        private CheckBox gridCheckBox;
        private ComboBox scenarioDropdown;
        private NumericUpDown gridXSpinner;
        private NumericUpDown gridYSpinner;
        private Button openButton;
        private Button saveButton;
        private Button saveAsButton;
        private Panel legendPanel;

        private int legendEntries = 1;

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

        /// <summary>
        /// konstruktor ovladaciho panelu
        /// </summary>
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
        
        /// <summary>
        /// zjisti jestli je aktivni designMode - pro designer
        /// </summary>
        /// <returns>bool: je aktivni design mode? </returns>
        private bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        /// <summary>
        /// zajisti nastaveni potrebnych parametru, pokud uz nejsou
        /// </summary>
        private void EnsureSettingsDefaults()
        {
            SettingsObject.gridX = Math.Max(1, SettingsObject.gridX);
            SettingsObject.gridY = Math.Max(1, SettingsObject.gridY);
        }

        /// <summary>
        /// inicializace ovladacich prvku panelu
        /// </summary>
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

            scenarioDropdown.KeyDown += (sender, e) =>
            {
                if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.Escape)
                {
                    e.SuppressKeyPress = true; 
                }
            };

            for (int i = 0; i <= 4; i++)
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

            openButton = new Button
            {
                Location = new Point(10, 280),
                Text = "Open"
            };
            this.Controls.Add(openButton);
            openButton.Click += OpenButton_Click;

            saveButton = new Button
            {
                Location = new Point(60, 280),
                Text = "Save"
            };
            this.Controls.Add(saveButton);
            saveButton.Click += SaveButton_Click;

            saveAsButton = new Button
            {
                Location = new Point(110, 280),
                Text = "Save As"
            };
            this.Controls.Add(saveAsButton);
            saveAsButton.Click += SaveAsButton_Click;

            Button chargeButton = new Button
            {
                Location = new Point(10, 300),
                Text = "Show charges"
            };
            this.Controls.Add(chargeButton);
            chargeButton.Click += ChargeButton_Click;

            Button probeButton = new Button
            {
                Location = new Point(50, 300),
                Text = "Show probes"
            };
            this.Controls.Add(probeButton);
            probeButton.Click += ProbeButton_Click;

            NumericUpDown rSpinner = new NumericUpDown
            {
                Location = new Point(10, 330),
                Width = 40,
                Minimum = 0,
                Maximum = 2,
                Value = 0
            }; 
            rSpinner.ValueChanged += (o, e) =>
            {
                SettingsObject.channels[0] = (int)rSpinner.Value;
                ReloadLegend();

            };
            this.Controls.Add(rSpinner);

            NumericUpDown gSpinner = new NumericUpDown
            {
                Location = new Point(60, 330),
                Width = 40,
                Minimum = 0,
                Maximum = 2,
                Value = 1
            };
            gSpinner.ValueChanged += (o, e) =>
            {
                SettingsObject.channels[1] = (int)gSpinner.Value;
                ReloadLegend();

            };
            this.Controls.Add(gSpinner);

            NumericUpDown bSpinner = new NumericUpDown
            {
                Location = new Point(110, 330),
                Width = 40,
                Minimum = 0,
                Maximum = 2,
                Value = 2
            };
            bSpinner.ValueChanged += (o, e) =>
            {
                SettingsObject.channels[2] = (int)bSpinner.Value;
                ReloadLegend();
            };
            this.Controls.Add(bSpinner);

            // Add colormap legend
            legendPanel = new Panel
            {
                Location = new Point(10, 370),
                Size = new Size(180, 160),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
            };

            Label legendTitle = new Label
            {
                Text = "Colormap Legend",
                Location = new Point(10, 5),
                AutoSize = true
            };
            legendPanel.Controls.Add(legendTitle);

            // Example color entries
            AddLegendEntry(legendPanel, "0 TN/C", this.GetColor(0));
            AddLegendEntry(legendPanel, "25E-2 TN/C", this.GetColor(2.5));
            AddLegendEntry(legendPanel, "50E-2 TN/C", this.GetColor(5));
            AddLegendEntry(legendPanel, "75E-2 TN/C", this.GetColor(7.5));
            AddLegendEntry(legendPanel, ">100E-2 TN/C", this.GetColor(10));

            this.Controls.Add(legendPanel);


        }

        /// <summary>
        /// metoda zajistujici funkcionalitu "Ulozit jako"
        /// </summary>
        /// <param name="sender">odesilajici objekt</param>
        /// <param name="e">parametry udalosti</param>
        private void SaveAsButton_Click(object? sender, EventArgs e)
        {
            // Create a SaveFileDialog to allow the user to save a file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "UPG files (*.upg)|*.upg|All files (*.*)|*.*"; // Filter for .upg files
            saveFileDialog.DefaultExt = "upg"; // Default file extension

            // Show the SaveFileDialog and check if the user selected a file
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the file path selected by the user
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Save some content to the file
                    SettingsObject.drawingPanel.SaveScenario(filePath);

                    // Write content to the selected file
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SettingsObject.openFile = filePath;
                }
                catch (Exception ex)
                {
                    // Handle errors (e.g., permission issues)
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// metoda zajistujici funkcionalitu "Ulozit"
        /// v pripade ze je otevreny nektery z defalutnich souboru, zavola se "Ulozit jako"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object? sender, EventArgs e)
        {
            string[] files =
            {
                "scen0.upg",
                "scen1.upg",
                "scen2.upg",
                "scen3.upg",
                "scen4.upg",
            };
            if (files.Contains(SettingsObject.openFile))
            {
                SaveAsButton_Click(sender, e);
            }
            else
            {
                SettingsObject.drawingPanel.SaveScenario(SettingsObject.openFile);
            }
        }

        /// <summary>
        /// prida radek legendy do panelu
        /// </summary>
        /// <param name="panel">panel pro umisteni</param>
        /// <param name="text">popisek legendy</param>
        /// <param name="color">barva v legende</param>
        private void AddLegendEntry(Panel panel, string text, Color color)
        {
            Panel colorBox = new Panel
            {
                Location = new Point(10, this.legendEntries * 25),
                Size = new Size(20, 20),
                BackColor = color
            };
            panel.Controls.Add(colorBox);

            Label legendLabel = new Label
            {
                Text = text,
                Location = new Point(40, this.legendEntries * 25 + 2),
                AutoSize = true
            };
            panel.Controls.Add(legendLabel);
            this.legendEntries++;
        }

        /// <summary>
        /// obsluha udalosti kliknuti na GraphButton
        /// otevre okno s grafy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphButton_Click(object? sender, EventArgs e)
        {
            if (SettingsObject.graphForm != null) return;
            GraphForm g = new GraphForm();
            g.Show();
            SettingsObject.graphForm = g;
        }

        /// <summary>
        /// obsluha udalosti kliknuti na ChargeButton
        /// otevre okno s tabulkou naboju
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeButton_Click(object? sender, EventArgs e)
        {
            if (SettingsObject.chargeForm != null) return;
            ChargeTable g = new ChargeTable();
            g.Show();
            SettingsObject.chargeForm = g;

        }

        /// <summary>
        /// obsluha udalosti kliknuti na ProbeButton
        /// otevre okno s tabulkou sond
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProbeButton_Click(object? sender, EventArgs e)
        {
            if (SettingsObject.probeForm != null) return;
            ProbeTable g = new ProbeTable();
            g.Show();
            SettingsObject.probeForm = g;

        }

        /// <summary>
        /// obsluha udalosti kliknuti na OpenButton
        /// zajistuje funkcionalitu otevreni souboru *.upg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButton_Click(object sender, EventArgs e)
        {
            // Create and configure the OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "UPG Files (*.upg)|*.upg|All Files (*.*)|*.*";  // Filter for .upg files
            openFileDialog.Title = "Open UPG File";

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // If the user selects a file, display the path in the TextBox
                SettingsObject.drawingPanel.LoadScenario(openFileDialog.FileName);
                // You can also process the file here as needed
            }
        }

        /// <summary>
        /// vypocita barvu ze zadane intenzity
        /// </summary>
        /// <param name="intensity">intenzita</param>
        /// <returns>barva</returns>
        public Color GetColor(double intensity)
        {
            // Cap the intensity value to a maximum of 1.0 for a smoother transition.
            double intst = Math.Pow(Math.Min(10, Math.Max(0, intensity)) / 10f, 0.6);

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

            int[] channels = new int[] {b, g, r };

            // Clamp RGB values to [0,255]
            r = Math.Max(0, Math.Min(255, r));
            g = Math.Max(0, Math.Min(255, g));
            b = Math.Max(0, Math.Min(255, b));

            return Color.FromArgb(channels[SettingsObject.channels[0]], channels[SettingsObject.channels[1]], channels[SettingsObject.channels[2]]);
        }

        private void ReloadLegend()
        {
            legendPanel.Controls.Clear();
            Label legendTitle = new Label
            {
                Text = "Colormap Legend",
                Location = new Point(10, 5),
                AutoSize = true
            };
            legendPanel.Controls.Add(legendTitle);
            legendEntries = 1;
            AddLegendEntry(legendPanel, "0 TN/C", SettingsObject.drawingPanel.scenario.GetColorFromIntensity(0));
            AddLegendEntry(legendPanel, "25E-2 TN/C", SettingsObject.drawingPanel.scenario.GetColorFromIntensity(2.5));
            AddLegendEntry(legendPanel, "50E-2 TN/C", SettingsObject.drawingPanel.scenario.GetColorFromIntensity(5));
            AddLegendEntry(legendPanel, "75E-2 TN/C", SettingsObject.drawingPanel.scenario.GetColorFromIntensity(7.5));
            AddLegendEntry(legendPanel, ">100E-2 TN/C", SettingsObject.drawingPanel.scenario.GetColorFromIntensity(10));
            legendPanel.Invalidate();
        }
    }
}
