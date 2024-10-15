using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Diversity
{
    public partial class EditProducts_W : Form
    {
        AppEntitiesConnection EditPrd = new AppEntitiesConnection();
        Notification_W nofi = new Notification_W();
        private string name;

        public EditProducts_W(string name)
        {
            InitializeComponent();
            this.name = name;
            LoadProduct();
        }

        private void LoadProduct()
        {
            var edPrd = EditPrd.Products.FirstOrDefault(p => p.ProductName == name);
            if (edPrd != null)
            {
                txtName.Text = edPrd.ProductName;
                txtPrice.Text = edPrd?.Price.ToString();

                if (!string.IsNullOrEmpty(edPrd.ProductImage) && System.IO.File.Exists(edPrd.ProductImage))
                {
                    picProducts.Image = System.Drawing.Image.FromFile(edPrd.ProductImage);
                }
                else
                {
                    picProducts.Image = null;
                }
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                picProducts.ImageLocation = openFileDialog.FileName;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var prdItem = EditPrd.Products.FirstOrDefault(m => m.ProductName == name);
            if (prdItem != null)
            {
                prdItem.ProductName = txtName.Text;
                prdItem.Price = decimal.Parse(txtPrice.Text);

                if (!string.IsNullOrEmpty(picProducts.ImageLocation) && System.IO.File.Exists(picProducts.ImageLocation))
                {
                    prdItem.ProductImage = picProducts.ImageLocation;
                }

                EditPrd.SaveChanges();

                nofi.ShowMessage(this, "製品を更新出来ました!😊");
                this.Close();
            }
        }
    }
}
