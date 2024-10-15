using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Diversity.Image
{
    public partial class AddProducts_W : Form
    {
        AppEntitiesConnection Prd = new AppEntitiesConnection();
        Notification_W nofi = new Notification_W();
        private string imagePath = "";

        public AddProducts_W()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Product newprd = new Product()
            {
                ProductName = txtName.Text,
                Price = Convert.ToDecimal(txtPrice.Text),
                ProductImage = imagePath
            };
            Prd.Products.Add(newprd);
            Prd.SaveChanges();
            nofi.ShowMessage(this,"商品をアップデートしました!😘");
            this.Close();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                picProducts.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                imagePath = openFileDialog.FileName;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
