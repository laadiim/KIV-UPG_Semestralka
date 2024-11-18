using System;
using System.Drawing;
using System.Windows.Forms;

namespace UPG_SP_2024
{
    public class ControlPanel : Panel
    {
        private SettingsObject settings;
        private CheckBox colormapCheckBox;

        public ControlPanel()
        {
            InitializeControls();
        }

        public void SetSettings(SettingsObject settings)
        {
            this.settings = settings;

            // Sync UI with current settings
            if (settings != null)
            {
                colormapCheckBox.Checked = settings.colorMap;

                // Optionally listen for changes to settings
                settings.SettingsChanged += (s, e) =>
                {
                    // Update UI if needed
                    Console.WriteLine("Settings changed in ControlPanel.");
                };
            }
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
                if (settings != null)
                {
                    settings.colorMap = colormapCheckBox.Checked; // Fires SettingsChanged
                }
            };
            this.Controls.Add(colormapCheckBox);
        }
    }
}