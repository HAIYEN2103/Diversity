using DocumentFormat.OpenXml.Spreadsheet;
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
    public partial class MainDiversity : Form
    {
        private Size originalSize;
        private Timer zoomTimer;
        private int zoomStep = 50;
        private int minSize = 100; 
        private float aspectRatio;

        private string userRoleName;

        public MainDiversity(string roleName)
        {
            InitializeComponent();
            originalSize = this.Size;
            aspectRatio = (float)originalSize.Width / originalSize.Height; 
            zoomTimer = new Timer();
            zoomTimer.Interval = 1; 
            zoomTimer.Tick += ZoomTimer_Tick;

            userRoleName = roleName; 
            InitializeButtonsBasedOnRole();
        }
        private void InitializeButtonsBasedOnRole()
        {
            if (userRoleName == "Employee")
            {
                btnEmployees.Enabled = false;
                btnAdmin.Enabled = false;
            }
        }

        public void ZoomForm() 
        {
            this.Size = new Size(minSize, (int)(minSize / aspectRatio));
            CenterToScreen(); 
            zoomTimer.Start();
        }

        private void HideAllbtn()
        {
            btnOne.Visible = false;
            btnTwo.Visible = false;
            btnThree.Visible = false;
            btnFour.Visible = false;
            btnFive.Visible = false;
            btnSix.Visible = false;
            btnSeven.Visible = false;
            btnEight.Visible = false;
        }

        private void ShowFormInPanel(Form form)
        {
            plOverview.Controls.Clear();

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;

            plOverview.Controls.Add(form);
            plOverview.BringToFront();
            plOverview.Show();
            form.Show();
        }

        private void btnOverview_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new OverView_W());
            HideAllbtn();
            btnOne.Visible = true;
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Customer_W());
            HideAllbtn();
            btnTwo.Visible = true;   
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            HideAllbtn();
            btnThree.Visible = true;
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Products_W());
            HideAllbtn();
            btnFour.Visible = true;
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            HideAllbtn();
            btnFive.Visible = true;
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new OrderHistory_W());
            HideAllbtn();
            btnSix.Visible = true;
        }

        private void btnMessenger_Click(object sender, EventArgs e)
        {
            HideAllbtn();
            btnSeven.Visible = true;
        }

        private void btnHelpCenter_Click(object sender, EventArgs e)
        {
            HideAllbtn();
            btnEight.Visible = true;
        }

        private void ZoomTimer_Tick(object sender, EventArgs e)
        {
            Size newSize = this.Size;
            newSize.Width += zoomStep;
            newSize.Height = (int)(newSize.Width / aspectRatio); 

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            if (newSize.Width > screenWidth || newSize.Height > screenHeight)
            {
                zoomTimer.Stop();
                this.Size = originalSize;
                CenterToScreen();
                return;
            }

            int newLocationX = (screenWidth - newSize.Width) / 2;
            int newLocationY = (screenHeight - newSize.Height) / 2;

            this.Location = new Point(newLocationX, newLocationY);
            this.Size = newSize;

            if (newSize.Width >= originalSize.Width && newSize.Height >= originalSize.Height)
            {
                zoomTimer.Stop();
                this.Size = originalSize; 
                CenterToScreen(); 
            }
        }

        private void btnRotationLuck_Click(object sender, EventArgs e)
        {
            RotationLuck luck = new RotationLuck();
            luck.ShowDialog();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            this.Close();
            Login lg = new Login();
            lg.Show();
        }
    }
}
