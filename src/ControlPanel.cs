using System;
using System.Windows.Forms;
using System.Drawing;

namespace UPG_SP_2024
{
    public class ControlPanel : Panel
    {
        private SettingsObject settings;

        private CheckBox colormapCheckBox;

        // Constructor
        public ControlPanel()
        {
            // Set default properties
            this.Size = new Size(300, 150);
            this.BackColor = Color.LightBlue;

            // Add example controls
            InitializeControls();
        }

        public void SetSettings(SettingsObject settings)
        { 
            this.settings = settings;
            Console.WriteLine(this.settings);
        }

        // Initialize custom controls
        private void InitializeControls()
        {
            // Add a Label
            Label label = new Label
            {
                Text = "Welcome to the Control Panel!",
                Location = new Point(10, 10),
                AutoSize = true
            };
            this.Controls.Add(label);
            
            CheckBox colormapCheckBox = new CheckBox
            {
                Text = "Enable Colormap",
                Location = new Point(10, 40),
                AutoSize = true
            };
            colormapCheckBox.CheckedChanged += ColormapCheckBox_CheckedChanged;
            this.Controls.Add(colormapCheckBox);

            // Add a Button
            Button button = new Button
            {
                Text = "Click Me",
                Location = new Point(10, 40)
            };
            button.Click += (sender, e) => MessageBox.Show("Button clicked!");
            this.Controls.Add(button);

            

        }

        private void ColormapCheckBox_CheckedChanged(object sender, EventArgs e)
            {
                if (settings != null)
                {
                    settings.colorMap = colormapCheckBox.Checked;
                    MessageBox.Show($"Colormap set to: {settings.colorMap}");
                }
            }
    }
}
