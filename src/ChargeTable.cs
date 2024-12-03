using System;
using System.Windows.Forms;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024
{
    public partial class ChargeTable : Form
    {
        private DataGridView chargesGridView;
				private Button addChargeButton;


        public ChargeTable()
        {
            InitializeComponent();
            InitializeDataGridView();
						InitializeAddChargeButton();
            LoadData();
            this.FormClosing += (o, e) => SettingsObject.chargeForm = null;
        }

        private void InitializeDataGridView()
        {
            chargesGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };

            chargesGridView.Columns.Add("Id", "ID");

            var chargeColumn = new DataGridViewTextBoxColumn
            {
                Name = "Charge",
                HeaderText = "Charge",
                ValueType = typeof(string)
            };
            chargesGridView.Columns.Add(chargeColumn);

            var xColumn = new DataGridViewTextBoxColumn
            {
                Name = "X",
                HeaderText = "X",
                ValueType = typeof(double)
            };
            chargesGridView.Columns.Add(xColumn);

            var yColumn = new DataGridViewTextBoxColumn
            {
                Name = "Y",
                HeaderText = "Y",
                ValueType = typeof(double)
            };
            chargesGridView.Columns.Add(yColumn);

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                Name = "DeleteButton",
                HeaderText = "Actions",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            };
            chargesGridView.Columns.Add(deleteButtonColumn);

            chargesGridView.CellClick += ChargesGridView_CellClick;
            chargesGridView.CellValueChanged += ChargesGridView_CellEndEdit;

            Controls.Add(chargesGridView);
        }

				private void InitializeAddChargeButton()
        {
            addChargeButton = new Button
            {
                Text = "Add Charge",
                Dock = DockStyle.Bottom,
                Height = 40
            };

            addChargeButton.Click += AddChargeButton_Click;

            Controls.Add(addChargeButton);
        }

				private void AddChargeButton_Click(object sender, EventArgs e)
				{
					INaboj c = SettingsObject.drawingPanel.scenario.AddCharge(new string[]{"1", "0", "0"}, SettingsObject.startTime);
					DataAdd(c.GetID(), c.GetChargeStr(), c.GetPosition().X, c.GetPosition().Y);
				}

        private void ChargesGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = chargesGridView.Rows[e.RowIndex];
                int id = Convert.ToInt32(row.Cells["Id"].Value);
                string charge = Convert.ToString(row.Cells["Charge"].Value);
                float x = Convert.ToSingle(row.Cells["X"].Value);
                float y = Convert.ToSingle(row.Cells["Y"].Value);
                INaboj c;
                // Update data list
                try
                {
                    c = SettingsObject.drawingPanel.scenario.GetCharge(id);
                }
                catch
                {
                    return;
                }
                c.SetChargeStr(charge);
                c.SetPosition(x, y);
            }
        }

        public void DataAdd(int id, string charge, float x, float y)
        {
            chargesGridView.Rows.Add(id, charge, x, y);
        }

        public void DataEmpty()
        {
            chargesGridView.Rows.Clear();
        }

        public void Reload()
        {
            DataEmpty();
            LoadData();
        }

        public void Refresh(int id)
        {
            INaboj c = SettingsObject.drawingPanel.scenario.GetCharge(id);
            foreach (DataGridViewRow row in chargesGridView.Rows)
            {
                if (row.Cells["Id"].Value != null && (int)row.Cells["Id"].Value == id)
                {
                    // Update the values in the row
                    row.Cells["Charge"].Value = c.GetChargeStr();
                    row.Cells["X"].Value = c.GetPosition().X;
                    row.Cells["Y"].Value = c.GetPosition().Y;

                    return; // Exit after finding and updating the row
                }
            }
        }

        public void LoadData()
        {
            INaboj[] charges = SettingsObject.drawingPanel.scenario.GetCharges();
            foreach (INaboj c in charges)
            {
                DataAdd(c.GetID(), c.GetChargeStr(), c.GetPosition().X, c.GetPosition().Y);
            }
        }

        private void ChargesGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle Delete button click
            if (e.RowIndex >= 0 && e.ColumnIndex == chargesGridView.Columns["DeleteButton"].Index)
            {
                SettingsObject.drawingPanel.scenario.RemoveCharge((int)(chargesGridView.Rows[e.RowIndex].Cells["Id"].Value));
                chargesGridView.Rows.RemoveAt(e.RowIndex);
            }
        }
    }
}
