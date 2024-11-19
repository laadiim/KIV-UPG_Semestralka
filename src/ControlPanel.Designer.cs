namespace UPG_SP_2024
{

    partial class ControlPanel
    {
        private Label titleLabel;
        private Button actionButton;

        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.actionButton = new System.Windows.Forms.Button();

            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.actionButton);

            // Set control properties
            this.titleLabel.Text = "Panel Title";
            this.titleLabel.Location = new System.Drawing.Point(10, 10);

            this.actionButton.Text = "Action";
            this.actionButton.Location = new System.Drawing.Point(10, 50);
            this.actionButton.Click += new System.EventHandler(this.actionButton_Click);

            // Set size of the control itself
            this.Size = new System.Drawing.Size(200, 100);
        }
    }
}