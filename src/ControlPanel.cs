using System;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp
{
    public class ControlPanel : Panel
    {
        // Constructor
        public ControlPanel()
        {
            // Set default properties
            this.Size = new Size(300, 150);
            this.BackColor = Color.LightBlue;

            // Add example controls
            InitializeControls();
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

            // Add a Button
            Button button = new Button
            {
                Text = "Click Me",
                Location = new Point(10, 40)
            };
            button.Click += (sender, e) => MessageBox.Show("Button clicked!");
            this.Controls.Add(button);
        }
    }
}
