using System;
using System.Windows.Forms;
using UPG_SP_2024.Interfaces;

namespace UPG_SP_2024
{
    /// <summary>
    /// trida okna s tabulkou sond
    /// </summary>
    public partial class ProbeTable : Form
    {
        private DataGridView probesGridView;
        private Button addProbeButton;

        /// <summary>
        /// konstruktor
        /// </summary>
        public ProbeTable()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeAddProbeButton();
            LoadData();
            this.FormClosing += (o, e) => SettingsObject.probeForm = null;
        }

        /// <summary>
        /// inicializace tabulky
        /// </summary>
        private void InitializeDataGridView()
        {
            probesGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };

            probesGridView.Columns.Add("Id", "ID");

            var centerXColumn = new DataGridViewTextBoxColumn
            {
                Name = "X",
                HeaderText = "Center X",
                ValueType = typeof(double)
            };
            probesGridView.Columns.Add(centerXColumn);

            var centerYColumn = new DataGridViewTextBoxColumn
            {
                Name = "Y",
                HeaderText = "Center Y",
                ValueType = typeof(double)
            };
            probesGridView.Columns.Add(centerYColumn);

            var radiusColumn = new DataGridViewTextBoxColumn
            {
                Name = "Radius",
                HeaderText = "Radius",
                ValueType = typeof(double)
            };
            probesGridView.Columns.Add(radiusColumn);

            var angleColumn = new DataGridViewTextBoxColumn
            {
                Name = "AnglePerSecond",
                HeaderText = "Angle per Second",
                ValueType = typeof(double)
            };
            probesGridView.Columns.Add(angleColumn);

            var deleteButtonColumn = new DataGridViewButtonColumn
            {
                Name = "DeleteButton",
                HeaderText = "Actions",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            };
            probesGridView.Columns.Add(deleteButtonColumn);

            probesGridView.CellClick += ProbesGridViewCellClick;
            probesGridView.CellValueChanged += ProbesGridViewCellEndEdit;

            Controls.Add(probesGridView);
        }

        /// <summary>
        /// inicializace tlacitka pro pridani sondy
        /// </summary>
        private void InitializeAddProbeButton()
        {
            addProbeButton = new Button
            {
                Text = "Add Probe",
                Dock = DockStyle.Bottom,
                Height = 40
            };

            addProbeButton.Click += AddProbeButtonClick;

            Controls.Add(addProbeButton);
        }

        /// <summary>
        /// obsluha udalosti kliknuti na AddProbeButton
        /// prida do scenare a tabulky sondu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProbeButtonClick(object sender, EventArgs e)
        {
            if (SettingsObject.probes.Count == SettingsObject.maxProbes) return;
            IProbe p = SettingsObject.drawingPanel.scenario.CreateProbe(new PointF(0, 0), 0, 0);
            DataAdd(p.GetID(), p.GetCenter().X, p.GetCenter().Y, p.GetRadius(), p.GetAnglePerSecond());
        }

        /// <summary>
        /// obsluha udalosti ukonceni upravy pole v tabulce
        /// upravi parametry sondy ve scenari
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProbesGridViewCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = probesGridView.Rows[e.RowIndex];
                int id = Convert.ToInt32(row.Cells["Id"].Value);
                string x = (Convert.ToString(row.Cells["X"].Value)).Replace(",", ".");
                string y = (Convert.ToString(row.Cells["Y"].Value)).Replace(",", ".");
                string radius = (Convert.ToString(row.Cells["Radius"].Value)).Replace(",", ".");
                string angle = (Convert.ToString(row.Cells["AnglePerSecond"].Value)).Replace(",", ".");
                IProbe p;
                // Update data list
                try
                {
                    p = SettingsObject.drawingPanel.scenario.GetProbe(id);
                }
                catch
                {
                    return;
                }
                p.SetCenter(new PointF(Convert.ToSingle(x), Convert.ToSingle(y)));
                p.SetRadius(Convert.ToSingle(radius));
                p.SetAnglePerSecond(Convert.ToSingle(angle));
            }
        }

        /// <summary>
        /// prida radek dat o sonde do tabulky
        /// </summary>
        /// <param name="id">id sondy</param>
        /// <param name="x">souradnice x stredu obihani sondy</param>
        /// <param name="y">souradnice y stredu obihani sondy</param>
        /// <param name="radius">polomer obehu sondy</param>
        /// <param name="angle">uhlova rychlost obehu sondy</param>
        public void DataAdd(int id, float x, float y, float radius, float angle)
        {
            probesGridView.Rows.Add(id, x, y, radius, angle);
        }

        /// <summary>
        /// vyprazdni tabulku
        /// </summary>
        public void DataEmpty()
        {
            probesGridView.Rows.Clear();
        }

        /// <summary>
        /// aktualizuje data o sonde se zadanym id
        /// </summary>
        /// <param name="id">id sondy</param>
        public void Refresh(int id)
        {
            IProbe p = SettingsObject.drawingPanel.scenario.GetProbe(id);
            foreach (DataGridViewRow row in probesGridView.Rows)
            {
                if (row.Cells["Id"].Value != null && (int)row.Cells["Id"].Value == id)
                {
                    // Update the values in the row
                    row.Cells["X"].Value = p.GetCenter().X;
                    row.Cells["Y"].Value = p.GetCenter().Y;
                    row.Cells["Radius"].Value = p.GetRadius();
                    row.Cells["AnglePerSecond"].Value = p.GetAnglePerSecond();

                    return; // Exit after finding and updating the row
                }
            }
        }

        /// <summary>
        /// nacte data ze scenare do tabulky
        /// </summary>
        public void LoadData()
        {
            List<IProbe> probes = SettingsObject.probes;
            foreach (IProbe p in probes)
            {
                if (p == null) continue;
                DataAdd(p.GetID(), p.GetCenter().X, p.GetCenter().Y, p.GetRadius(), p.GetAnglePerSecond());
            }
        }

        /// <summary>
        /// obsluha udalosti kliknuti na pole v tabulce
        /// kliknuti na sloupec delete - smaze sondu ze scenare
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProbesGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle Delete button click
            if (e.RowIndex >= 0 && e.ColumnIndex == probesGridView.Columns["DeleteButton"].Index)
            {
                SettingsObject.drawingPanel.scenario.RemoveProbe((int)(probesGridView.Rows[e.RowIndex].Cells["Id"].Value));
                probesGridView.Rows.RemoveAt(e.RowIndex);
            }
        }

        /// <summary>
        /// vyprazdni tabulku a nacte nova data
        /// </summary>
        public void Reload()
        {
            DataEmpty();
            LoadData();
        }
    }
}
