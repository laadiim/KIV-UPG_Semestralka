using System;
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
        private TableLayoutPanel layout;


        private int legendEntries = 0;
        private Button graphButton;
        private Button chargesButton;
        private Button probesButton;
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
            layout = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount = 10,
                Dock = DockStyle.Fill,
                AutoSize = true,
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));

            // Row 0: Title
            layout.Controls.Add(new Label
            {
                Text = "Control Panel",
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            }, 0, 0);
            layout.SetColumnSpan(layout.Controls[layout.Controls.Count - 1], 3);

            // Row 1: Colormap Checkbox
            colormapCheckBox = new CheckBox
            {
                Text = "Enable Colormap",
                AutoSize = true
            };
            colormapCheckBox.CheckedChanged += (o, e) =>
            {
                SettingsObject.colorMap = colormapCheckBox.Checked;
            };
            layout.Controls.Add(colormapCheckBox, 1, 1);
            layout.SetColumnSpan(colormapCheckBox, 2);


            // Row 2: Grid Checkbox
            gridCheckBox = new CheckBox
            {
                Text = "Enable Grid",
                AutoSize = true,
            };
            gridCheckBox.CheckedChanged += (o, e) =>
            {
                SettingsObject.gridShown = gridCheckBox.Checked;
            };
            layout.Controls.Add(gridCheckBox, 1, 2);
            layout.SetColumnSpan(gridCheckBox, 2);

            // Row 3: Scenario Dropdown
            scenarioDropdown = new ComboBox
            {
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 0; i <= 4; i++)
            {
                scenarioDropdown.Items.Add($"Scenario {i}");
            }
            scenarioDropdown.SelectedIndex = SettingsObject.scenario;
            scenarioDropdown.SelectedIndexChanged += (o, e) =>
            {
                SettingsObject.scenario = scenarioDropdown.SelectedIndex;
                SettingsObject.drawingPanel.SetScenario(scenarioDropdown.SelectedIndex);
            };
            layout.Controls.Add(new Label { Text = "Scenario:", TextAlign = ContentAlignment.MiddleRight }, 0, 3);
            layout.Controls.Add(scenarioDropdown, 1, 3);
            layout.SetColumnSpan(scenarioDropdown, 2);

            // Row 4: Grid X Spinner
            gridXSpinner = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 100,
                Width = 120,
                Value = SettingsObject.gridX
            };
            gridXSpinner.ValueChanged += (o, e) =>
            {
                SettingsObject.gridX = (int)gridXSpinner.Value;
            };
            layout.Controls.Add(new Label { Text = "Grid X:", TextAlign = ContentAlignment.MiddleRight }, 0, 4);
            layout.Controls.Add(gridXSpinner, 1, 4);
            layout.SetColumnSpan(gridXSpinner, 2);

            // Row 5: Grid Y Spinner
            gridYSpinner = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 100,
                Width = 120,
                Value = SettingsObject.gridY
            };
            gridYSpinner.ValueChanged += (o, e) =>
            {
                SettingsObject.gridY = (int)gridYSpinner.Value;
            };
            layout.Controls.Add(new Label { Text = "Grid Y:", TextAlign = ContentAlignment.MiddleRight }, 0, 5);
            layout.Controls.Add(gridYSpinner, 1, 5);
            layout.SetColumnSpan(gridYSpinner, 2);


            // Row 6: Graph, Charge and Probe Buttons
            graphButton = new Button { Text = "Graphs" };
            graphButton.Click += GraphButton_Click;
            layout.Controls.Add(graphButton, 0, 7);

            chargesButton = new Button { Text = "Charges" };
            chargesButton.Click += ChargeButton_Click;
            layout.Controls.Add(chargesButton, 1, 7);
            
            probesButton = new Button { Text = "Probes" };
            probesButton.Click += ProbeButton_Click;
            layout.Controls.Add(probesButton, 2, 7);

            // Row 7: Save and Load Buttons
            openButton = new Button { Text = "Open" };
            openButton.Click += OpenButton_Click;
            layout.Controls.Add(openButton, 0, 6);

            saveButton = new Button { Text = "Save" };
            saveButton.Click += SaveButton_Click;
            layout.Controls.Add(saveButton, 1, 6);

            saveAsButton = new Button { Text = "Save As" };
            saveAsButton.Click += SaveAsButton_Click;
            layout.Controls.Add(saveAsButton, 2, 6);

            // Row 8: Legend Panel
            legendPanel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Height = 150,
                Width = 150
            };
            AddLegendEntry(legendPanel, "0 TN/C", GetColor(0));
            AddLegendEntry(legendPanel, "25 E-2TN/C", GetColor(2.5));
            AddLegendEntry(legendPanel, "50E-2 TN/C", GetColor(5));
            AddLegendEntry(legendPanel, "75E-2 TN/C", GetColor(7.5));
            AddLegendEntry(legendPanel, "> 100E-2 TN/C", GetColor(10));
            layout.Controls.Add(new Label { Text = "Legend:", TextAlign = ContentAlignment.MiddleRight }, 0, 8);
            layout.Controls.Add(legendPanel, 1, 8);
            layout.SetColumnSpan(legendPanel, 2);

            this.Controls.Add(layout);
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
                Location = new Point(10, this.legendEntries * 25 + 10),
                Size = new Size(20, 20),
                BackColor = color
            };
            panel.Controls.Add(colorBox);

            Label legendLabel = new Label
            {
                Text = text,
                Location = new Point(40, this.legendEntries * 25 + 12),
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

            // Clamp RGB values to [0,255]
            r = Math.Max(0, Math.Min(255, r));
            g = Math.Max(0, Math.Min(255, g));
            b = Math.Max(0, Math.Min(255, b));

            return Color.FromArgb(b, g, r);
        }
    }
}
