using System;
using System.Windows.Forms;

public class ImageSizePopup : Form
{
    private NumericUpDown widthNumeric;
    private NumericUpDown heightNumeric;
    private TextBox pathTextBox;
    private Button browseButton;
    private Button okButton;
    private Button cancelButton;

    public int ImageWidth { get; private set; }
    public int ImageHeight { get; private set; }
    public string SavePath { get; private set; }

    public ImageSizePopup()
    {
        this.Text = "Select Image Settings";
        this.Size = new System.Drawing.Size(400, 250);

        // TableLayoutPanel for grid layout
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 4,
            Padding = new Padding(10),
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // Labels
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); // Input fields
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20)); // Buttons

        this.Controls.Add(layout);

        // Width NumericUpDown
        widthNumeric = new NumericUpDown
        {
            Minimum = 100,
            Maximum = 5000,
            Value = 1024,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(new Label { Text = "Width:", Anchor = AnchorStyles.Right }, 0, 0);
        layout.Controls.Add(widthNumeric, 1, 0);

        // Height NumericUpDown
        heightNumeric = new NumericUpDown
        {
            Minimum = 100,
            Maximum = 5000,
            Value = 1024,
            Dock = DockStyle.Fill
        };
        layout.Controls.Add(new Label { Text = "Height:", Anchor = AnchorStyles.Right }, 0, 1);
        layout.Controls.Add(heightNumeric, 1, 1);

        // Path TextBox and Browse Button
        pathTextBox = new TextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true
        };
        browseButton = new Button
        {
            Text = "Browse...",
            Dock = DockStyle.Fill
        };
        browseButton.Click += (sender, e) =>
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Image Files|*.png;*.jpg;*.bmp;*.gif";
                saveDialog.Title = "Select Save Location";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = saveDialog.FileName;
                    SavePath = saveDialog.FileName;
                }
            }
        };
        layout.Controls.Add(new Label { Text = "Save Path:", Anchor = AnchorStyles.Right }, 0, 2);
        layout.Controls.Add(pathTextBox, 1, 2);
        layout.Controls.Add(browseButton, 2, 2);

        // OK and Cancel Buttons
        okButton = new Button
        {
            Text = "OK",
            Dock = DockStyle.Fill
        };
        okButton.Click += (sender, e) =>
        {
            if (string.IsNullOrWhiteSpace(pathTextBox.Text))
            {
                MessageBox.Show("Please select a save path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ImageWidth = (int)widthNumeric.Value;
            ImageHeight = (int)heightNumeric.Value;
            SavePath = pathTextBox.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        };

        cancelButton = new Button
        {
            Text = "Cancel",
            Dock = DockStyle.Fill
        };
        cancelButton.Click += (sender, e) =>
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        };

        // Button Panel
        var buttonPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        buttonPanel.Controls.Add(okButton, 0, 0);
        buttonPanel.Controls.Add(cancelButton, 1, 0);

        layout.Controls.Add(buttonPanel, 0, 3);
        layout.SetColumnSpan(buttonPanel, 3); // Span across all columns

    }
}
