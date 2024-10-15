using Diversity.Image;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Diversity
{
    public partial class Products_W : Form
    {
        private AppEntitiesConnection Prd = new AppEntitiesConnection();
        Notification_W nofi = new Notification_W();
        private string placeholderText = "Search";

        public Products_W()
        {
            InitializeComponent();
            LoadProduct();
            LoadProductDgv();
            SetPlaceholder();
            SetButtonStates(false);
        }
        private string FormatPriceToYen(decimal price)
        {
            return price.ToString("C0", new CultureInfo("ja-JP"));
        }
        public void LoadProduct(string searchText = "")
        {
            var queryProducts = from item in Prd.Products
                                where item.ProductName.Contains(searchText)
                                select new
                                {
                                    ProductName = item.ProductName,
                                    Price = item.Price,
                                    ProductImage = item.ProductImage,
                                };

            flpProducts.Controls.Clear();
            foreach (var item in queryProducts)
            {
                uctProducts uct = new uctProducts
                {
                    ProductName = item.ProductName,
                    ProductImage = System.IO.File.Exists(item.ProductImage) ? System.Drawing.Image.FromFile(item.ProductImage) : null
                };
                flpProducts.Controls.Add(uct);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text != placeholderText)
            {
                LoadProduct(txtSearch.Text);
                LoadProductDgv(txtSearch.Text);
            }
        }

        private void SetPlaceholder()
        {
            txtSearch.Text = placeholderText;
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Enter += RemovePlaceholder;
            txtSearch.Leave += SetPlaceholder;
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (txtSearch.Text == placeholderText)
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = placeholderText;
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void btnDataGridView_Click(object sender, EventArgs e)
        {
            flpProducts.Visible = false;
            dgvProducts.Visible = true;
            SetButtonStates(true);
        }

        private void btnDisplayed_Click(object sender, EventArgs e)
        {
            dgvProducts.Visible = false;
            flpProducts.Visible = true;
            SetButtonStates(false);
        }

        private void LoadProductDgv(string searchText = "")
        {
            var queryPrdDgv = from item in Prd.Products
                              where item.ProductName.Contains(searchText)
                              select new
                              {
                                  item.ProductImage,
                                  item.ProductName,
                                  item.Price
                              };

            var productsWithImages = queryPrdDgv.ToList().Select(prd => new
            {
                ProductImage = System.IO.File.Exists(prd.ProductImage) ? System.Drawing.Image.FromFile(prd.ProductImage) : null,
                prd.ProductName,
                Price = FormatPriceToYen(prd.Price)
            }).ToList();

            dgvProducts.DataSource = productsWithImages;

            if (dgvProducts.Columns.Contains("ProductImage"))
            {
                dgvProducts.Columns["ProductImage"].HeaderText = "製品画像";
                dgvProducts.Columns["ProductImage"].Width = 80;
                dgvProducts.Columns["ProductImage"].DefaultCellStyle.NullValue = null;
                ((DataGridViewImageColumn)dgvProducts.Columns["ProductImage"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
            if (dgvProducts.Columns.Contains("ProductName"))
            {
                dgvProducts.Columns["ProductName"].HeaderText = "製品名";
            }
            if (dgvProducts.Columns.Contains("Price"))
            {
                dgvProducts.Columns["Price"].HeaderText = "価格";
            }
        }

        private void SetButtonStates(bool enabled)
        {
            btnEdit.Enabled = enabled;
            btnDelete.Enabled = enabled;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProducts_W addProductForm = new AddProducts_W();
            addProductForm.ShowDialog();
            LoadProduct();
            LoadProductDgv();
        }

        private void dgvProducts_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == dgvProducts.Columns["ProductImage"].Index && e.RowIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);
                e.PaintContent(e.ClipBounds);

                if (e.Value != null)
                {
                    var image = (System.Drawing.Image)e.Value;
                    var rect = e.CellBounds;

                    using (GraphicsPath path = new GraphicsPath())
                    {
                        int cornerRadius = 5; 
                        path.AddArc(rect.X, rect.Y, cornerRadius, cornerRadius, 180, 90); 
                        path.AddArc(rect.Right - cornerRadius, rect.Y, cornerRadius, cornerRadius, 270, 90); 
                        path.AddArc(rect.Right - cornerRadius, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90); 
                        path.AddArc(rect.X, rect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90); 
                        path.CloseFigure();

                        e.Graphics.SetClip(path);
                        e.Graphics.DrawImage(image, rect); 
                        e.Graphics.ResetClip();
                    }
                }
                e.Handled = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                string productNameToEdit = dgvProducts.SelectedRows[0].Cells["ProductName"].Value.ToString();

                EditProducts_W edProductForm = new EditProducts_W(productNameToEdit);
                edProductForm.ShowDialog();
                LoadProduct();
                LoadProductDgv();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                string productNameToDelete = dgvProducts.SelectedRows[0].Cells["ProductName"].Value.ToString();
                var productToDelete = Prd.Products.FirstOrDefault(p => p.ProductName == productNameToDelete);
                if (productToDelete != null)
                {
                    Prd.Products.Remove(productToDelete);
                    Prd.SaveChanges();
                    nofi.ShowMessage(this, "製品を削除された!😥");
                    LoadProduct();
                    LoadProductDgv();
                }
            }
        }
    }
}
