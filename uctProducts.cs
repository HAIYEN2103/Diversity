using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diversity
{
    public partial class uctProducts : UserControl
    {
        AppEntitiesConnection UserCtrl = new AppEntitiesConnection();
        Notification_W nofi = new Notification_W();
        public uctProducts()
        {
            InitializeComponent();
        }

        private string productName;
        private System.Drawing.Image productImage;

        public new string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                txtProductName.Text = value;
            }
        }

        public System.Drawing.Image ProductImage
        {
            get { return productImage; }
            set
            {
                productImage = value;
                picProducts.Image = value;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            plEditDelete.Visible = !plEditDelete.Visible;
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text.Trim();
            var product = UserCtrl.Products.FirstOrDefault(p => p.ProductName == productName);

            if (product != null)
            {
                ProductDetail_W detailForm = new ProductDetail_W(product);
                detailForm.Show();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text.Trim();
            var productToDelete = UserCtrl.Products.FirstOrDefault(p => p.ProductName == productName);

            if (productToDelete != null)
            {
                UserCtrl.Products.Remove(productToDelete);
                UserCtrl.SaveChanges();
                nofi.ShowMessage(this.ParentForm, "製品を削除されました! 😘");
            }
        }
    }
}
