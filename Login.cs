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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            using (var db = new AppEntitiesConnection())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

                if (user != null)
                {
                    string roleName = user.Role.RoleName;
                    MainDiversity mainForm = new MainDiversity(roleName);

                    if (roleName == "Manager" || roleName == "Employee")
                    {
                        mainForm.ZoomForm();
                    }

                    mainForm.Show();
                    this.Hide();

                    await Task.Delay(500);
                    LoginNotification notificationForm = new LoginNotification();
                    notificationForm.Show();

                    await Task.Delay(2000);

                    notificationForm.Hide();
                    notificationForm.Close();
                }
                else
                {
                    MessageBox.Show("ログインID又はパスワードが存在しません。");
                }
            }
        }

        private void btnShowOf_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '●';
            btnShowOf.Visible = false; 
            btnShowOnn.Visible = true; 
        }

        private void btnShowOnn_Click(object sender, EventArgs e)
        {
            if (txtPassword.PasswordChar == '●')
            {
                txtPassword.PasswordChar = '\0';
            }
            btnShowOnn.Visible = false; 
            btnShowOf.Visible = true; 
        }

      
    }
}
