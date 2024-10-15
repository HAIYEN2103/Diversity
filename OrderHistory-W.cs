using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diversity
{
    public partial class OrderHistory_W : Form
    {
        AppEntitiesConnection Prd = new AppEntitiesConnection();
        private string placeholderText = "Search";
        private bool isDetailFormOpen = false;

        public OrderHistory_W()
        {
            InitializeComponent();
            LoadOrderHistory();
            SetPlaceholder();
        }

        private string FormatPriceToYen(decimal price)
        {
            return price.ToString("C0", new CultureInfo("ja-JP"));
        }

        private void LoadOrderHistory(string searchTerm = "")
        {
            var queryOrderHistory = from order in Prd.Orders
                                    join history in Prd.OrderHistories
                                    on order.OrderID equals history.OrderID
                                    where history.StatusDate == (
                                        from h in Prd.OrderHistories
                                        where h.OrderID == order.OrderID
                                        select h.StatusDate
                                    ).Max()
                                    && (order.Customer.Name.Contains(searchTerm) || order.OrderID.ToString().Contains(searchTerm))
                                    select new
                                    {
                                        order.OrderID,
                                        CustomerName = order.Customer.Name,
                                        order.OrderDate,
                                        order.TotalAmount,
                                        OrderStatus = history.Status
                                    };

            var formattedOrderHistory = queryOrderHistory.ToList().Select(order => new
            {
                order.OrderID,
                order.CustomerName,
                order.OrderDate,
                TotalAmount = FormatPriceToYen(order.TotalAmount),
                order.OrderStatus
            }).ToList();

            dgvHistory.DataSource = formattedOrderHistory;
           

            dgvHistory.Columns["OrderID"].HeaderText = "注文ID"; // Order ID
            dgvHistory.Columns["CustomerName"].HeaderText = "顧客名"; // Customer Name
            dgvHistory.Columns["OrderDate"].HeaderText = "注文日"; // Order Date
            dgvHistory.Columns["TotalAmount"].HeaderText = "合計金額"; // Total Amount
            dgvHistory.Columns["OrderStatus"].HeaderText = "注文状況"; // Order Stat

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadOrderHistory(txtSearch.Text.Trim() == placeholderText ? "" : txtSearch.Text.Trim());
        }

        private void SetPlaceholder()
        {
            txtSearch.Text = placeholderText;
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Enter += RemovePlaceholder;
            txtSearch.Leave += ShowPlaceholder;
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (txtSearch.Text == placeholderText)
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void ShowPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = placeholderText;
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void dgvHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvHistory.Columns["Delete"].Index)
            {
                int orderId = (int)dgvHistory.Rows[e.RowIndex].Cells["OrderID"].Value;
                var orderHistory = Prd.OrderHistories.Where(oh => oh.OrderID == orderId).ToList();
                var orderDetails = Prd.OrderDetails.Where(od => od.OrderID == orderId).ToList();
                var order = Prd.Orders.FirstOrDefault(o => o.OrderID == orderId);

                if (orderHistory.Any() || orderDetails.Any() || order != null)
                {
                    Prd.OrderHistories.RemoveRange(orderHistory);
                    Prd.OrderDetails.RemoveRange(orderDetails);
                    Prd.Orders.Remove(order);
                    Prd.SaveChanges();
                    LoadOrderHistory();
                }
            }
        }

        private void dgvHistory_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.ColumnIndex == dgvHistory.Columns["Delete"].Index) && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                System.Drawing.Image buttonImage = null;
                if (e.ColumnIndex == dgvHistory.Columns["Delete"].Index)
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

        private void dgvHistory_DoubleClick(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvHistory.CurrentRow;
            int orderId = (int)selectedRow.Cells["OrderID"].Value;

            OrderDetail_W detailsForm = new OrderDetail_W();
            detailsForm.LoadOrder(orderId);
            detailsForm.ShowDialog();
        }
    }
}
