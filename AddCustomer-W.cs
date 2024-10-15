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
    public partial class AddCustomer_W : Form
    {
        AppEntitiesConnection AddCust = new AppEntitiesConnection();
        Notification_W nofi = new Notification_W();
        public AddCustomer_W()
        {
            InitializeComponent();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            Customer newCust = new Customer
            {
                Name = txtName.Text,
                Address = txtAddress.Text,
                PhoneNumber = txtPhoneNumber.Text,
                Email = txtEmail.Text,
            };
            AddCust.Customers.Add(newCust);
            AddCust.SaveChanges();
            ((Customer_W)this.Owner).UpdateCust();
            nofi.ShowMessage(this, "顧客を追加しました。 😘");
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
