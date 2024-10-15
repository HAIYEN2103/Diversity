using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace Diversity
{
    public partial class Customer_W : Form
    {
        AppEntitiesConnection Cust = new AppEntitiesConnection();
        Notification_W nofi = new Notification_W();
        private string placeholderText = "Search";

        public Customer_W()
        {
            InitializeComponent();
            SetPlaceholder();
            LoadCustomer();
        }

        public void UpdateCust()
        {
            LoadCustomer();
        }

        private void LoadCustomer(string searchTerm = "")
        {
            var queryCust = from item in Cust.Customers
                            select new
                            {
                                Name = item.Name,
                                Address = item.Address,
                                PhoneNumber = item.PhoneNumber,
                                Email = item.Email,
                            };

            if (!string.IsNullOrEmpty(searchTerm) && searchTerm != placeholderText)
            {
                queryCust = queryCust.Where(c => c.Name.Contains(searchTerm));
            }


            var formattedOrderHistory = queryCust.ToList().Select(item => new
            {
                Name = item.Name,
                Address = item.Address,
                PhoneNumber = item.PhoneNumber,
                Email = item.Email,
            }).ToList();

            dgvCustomer.DataSource = formattedOrderHistory;

            dgvCustomer.Columns["Name"].HeaderText = "氏名";
            dgvCustomer.Columns["Address"].HeaderText = "住所"; 
            dgvCustomer.Columns["PhoneNumber"].HeaderText = "電話番号"; 
            dgvCustomer.Columns["Email"].HeaderText = "メール";
            
        }

        private void dgvCustomer_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.ColumnIndex == dgvCustomer.Columns["Edit"].Index || e.ColumnIndex == dgvCustomer.Columns["Delete"].Index) && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                System.Drawing.Image buttonImage = null;
                if (e.ColumnIndex == dgvCustomer.Columns["Edit"].Index)
                {
                    buttonImage = Properties.Resources.Edit;
                }
                else if (e.ColumnIndex == dgvCustomer.Columns["Delete"].Index)
                {
                    buttonImage = Properties.Resources.Delete;
                }

                if (buttonImage != null)
                {
                    int imageWidth = 16;
                    int imageHeight = 16;
                    var resizedImage = new Bitmap(buttonImage, new System.Drawing.Size(imageWidth, imageHeight));

                    var w = resizedImage.Width;
                    var h = resizedImage.Height;
                    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                    e.Graphics.DrawImage(resizedImage, new Rectangle(x, y, w, h));
                }
                e.Handled = true;
            }
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dgvCustomer.Columns["Edit"].Index)
                {
                    string name = dgvCustomer.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                    var customer = Cust.Customers.FirstOrDefault(c => c.Name == name);

                    if (customer != null)
                    {
                        EditCustomer edit = new EditCustomer(name);
                        edit.Owner = this;
                        edit.ShowDialog();                       
                    }
                }
                else if (e.ColumnIndex == dgvCustomer.Columns["Delete"].Index)
                {
                    string name = dgvCustomer.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                    var customer = Cust.Customers.FirstOrDefault(c => c.Name == name);
                    if (customer != null)
                    {
                        using (var transaction = Cust.Database.BeginTransaction())
                        {
                            var orders = Cust.Orders.Where(o => o.CustomerID == customer.CustomerID).ToList();
                            foreach (var order in orders)
                            {
                                var orderDetails = Cust.OrderDetails.Where(od => od.OrderID == order.OrderID).ToList();
                                Cust.OrderDetails.RemoveRange(orderDetails);

                                var orderHistories = Cust.OrderHistories.Where(oh => oh.OrderID == order.OrderID).ToList();
                                Cust.OrderHistories.RemoveRange(orderHistories);
                            }

                            Cust.Orders.RemoveRange(orders);

                            Cust.Customers.Remove(customer);

                            Cust.SaveChanges();

                            transaction.Commit();

                            LoadCustomer();
                            nofi.ShowMessage(this, "顧客の削除が成功しました!😘");
                        }                        
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddCustomer_W add = new AddCustomer_W();
            add.Owner = this;
            add.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer(txtSearch.Text);
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

        private void btnExportFile_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Customers");

                        for (int i = 0; i < dgvCustomer.Columns.Count; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = dgvCustomer.Columns[i].HeaderText;
                        }

                        for (int i = 0; i < dgvCustomer.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvCustomer.Columns.Count; j++)
                            {
                                worksheet.Cell(i + 2, j + 1).Value = dgvCustomer.Rows[i].Cells[j].Value?.ToString();
                            }
                        }
                        workbook.SaveAs(sfd.FileName);
                    }
                    nofi.ShowMessage(this, "Excelファイルの出力が成功しました !😘");
                }
            }
        }
    }
}
