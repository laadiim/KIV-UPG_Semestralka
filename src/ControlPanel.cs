using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPG_SP_2024
{

    public partial class ControlPanel : UserControl
    {
        public event EventHandler ActionButtonClick;

        public ControlPanel()
        {
            InitializeComponent();
        }

        // Property to expose the title label's text
        private void actionButton_Click(object sender, EventArgs e)
        {
            ActionButtonClick?.Invoke(this, e);
        }
    }
}
