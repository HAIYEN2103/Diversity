using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diversity
{
    public partial class LoginNotification : Form
    {
        public LoginNotification()
        {
            InitializeComponent();
        }

        private void LoginNotification_Load(object sender, EventArgs e)
        {
            int margin = 270;
            int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width - margin;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height - margin;
            y -= -155;
            this.Location = new Point(x, y);
        }
    }
}
