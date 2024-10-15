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
    public partial class EditCustomer : Form
    {
        AppEntitiesConnection EditCust = new AppEntitiesConnection();
        Notification_W nofi = new Notification_W();
        private string name;
        public EditCustomer(string name)
        {
            InitializeComponent();
            this.name = name;
            LoadCustomer();
        }

        private void LoadCustomer()
        {
            var edCust = EditCust.Customers.FirstOrDefault(p => p.Name == name);
            if (edCust != null)
            {
                txtName.Text = edCust.Name;
                txtAddress.Text = edCust.Address;
                txtPhoneNumber.Text = edCust.PhoneNumber;
                txtEmail.Text = edCust.Email;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            var edCust = EditCust.Customers.FirstOrDefault(p => p.Name == name);
            if (edCust != null)
            {
                edCust.Name = txtName.Text;
                edCust.Address = txtAddress.Text;
                edCust.PhoneNumber = txtPhoneNumber.Text;
                edCust.Email = txtEmail.Text;

                EditCust.SaveChanges();
                ((Customer_W)this.Owner).UpdateCust();
                nofi.ShowMessage(this, "更新を編集しました! 😘");
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
    }
}
