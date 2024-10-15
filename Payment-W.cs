using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Diversity
{
    public partial class Payment_W : Form
    {
        AppEntitiesConnection Prd = new AppEntitiesConnection();
        Notification_W nofi = new Notification_W();
        CultureInfo japaneseCulture = new CultureInfo("ja-JP");

        private Product _product;
        private int _quantity;

        public Payment_W(Product product, int quantity)
        {
            InitializeComponent();
            _product = product;
            _quantity = quantity;
            LoadDate();
            LoadProducts();
            dgvProducts.CellFormatting += dgvProducts_CellFormatting;
        }

        private void LoadDate()
        {
            DateTime currentDate = DateTime.Today;
            lbDate.Text = currentDate.ToString("dd/MM/yyyy");
        }


        private void LoadProducts()
        {
            var queryPrd = from item in Prd.OrderItems
                           select new
                           {
                               ProductImage = item.ProductImage,
                               ProductName = item.ProductName,
                               Price = item.Price,
                               Quantity = item.Quantity,
                           };
            var productsWithImages = queryPrd.ToList().Select(prd => new
            {
                ProductImage = System.IO.File.Exists(prd.ProductImage) ? System.Drawing.Image.FromFile(prd.ProductImage) : null,
                prd.ProductName,
                prd.Price,
                prd.Quantity
            }).ToList();

            dgvProducts.DataSource = productsWithImages;

            if (dgvProducts.Columns.Contains("ProductImage"))
            {
                dgvProducts.Columns["ProductImage"].HeaderText = "商品画像";
                dgvProducts.Columns["ProductImage"].Width = 100;
                dgvProducts.Columns["ProductImage"].DefaultCellStyle.NullValue = null;
                ((DataGridViewImageColumn)dgvProducts.Columns["ProductImage"]).ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
            if (dgvProducts.Columns.Contains("ProductName"))
            {
                dgvProducts.Columns["ProductName"].HeaderText = "商品名"; // Product Name
            }
            if (dgvProducts.Columns.Contains("Price"))
            {
                dgvProducts.Columns["Price"].HeaderText = "価格"; // Price 
            }
            if (dgvProducts.Columns.Contains("Quantity"))
            {
                dgvProducts.Columns["Quantity"].HeaderText = "数量"; // Quantity 
            }
            UpdateSubTotal();
        }

        private void UpdateSubTotal()
        {
            decimal subTotal = 0;
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.DataBoundItem != null)
                {
                    var product = (dynamic)row.DataBoundItem;
                    decimal price = product.Price;
                    int quantity = product.Quantity;
                    subTotal += price * quantity;
                }
            }

            txtSubTotal.Text = subTotal.ToString("C0", japaneseCulture);
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            string customerName = txtName.Text.Trim();
            string customerAddress = txtAddress.Text.Trim();
            string customerPhone = txtPhoneNumber.Text.Trim();
            string customerEmail = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(customerAddress) || string.IsNullOrEmpty(customerPhone) || string.IsNullOrEmpty(customerEmail))
            {
                nofi.ShowMessage(this, "支払い情報をすべて入力してください! 😒");
                return;
            }

            using (var transaction = Prd.Database.BeginTransaction())
            {
                try
                {
                    Customer existingCustomer = Prd.Customers.FirstOrDefault(c => c.Name == customerName);
                    if (existingCustomer == null)
                    {
                        existingCustomer = new Customer
                        {
                            Name = customerName,
                            Address = customerAddress,
                            PhoneNumber = customerPhone,
                            Email = customerEmail
                        };

                        Prd.Customers.Add(existingCustomer);
                        Prd.SaveChanges();
                    }

                    Order newOrder = new Order
                    {
                        CustomerID = existingCustomer.CustomerID,
                        OrderDate = DateTime.Today,
                        TotalAmount = _product.Price * _quantity
                    };

                    Prd.Orders.Add(newOrder);
                    Prd.SaveChanges();

                    int orderID = newOrder.OrderID;

                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderID = orderID,
                        ProductID = _product.ProductID,
                        Quantity = _quantity,
                        Price = _product.Price,
                        ProductImage = _product.ProductImage
                    };

                    Prd.OrderDetails.Add(orderDetail);
                    Prd.SaveChanges();

                    OrderHistory orderHistory = new OrderHistory
                    {
                        OrderID = orderID,
                        Status = "完了",
                        StatusDate = DateTime.Now
                    };

                    Prd.OrderHistories.Add(orderHistory);
                    Prd.SaveChanges();

                    var orderItemsToRemove = Prd.OrderItems.ToList();
                    Prd.OrderItems.RemoveRange(orderItemsToRemove);
                    Prd.SaveChanges();

                    transaction.Commit();
                    nofi.ShowMessage(this, "注文が成功しました!😘");
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    nofi.ShowMessage(this, "エラーが発生しました: " + ex.Message);
                }
            }
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            string discountCode = txtDiscount.Text.Trim();

            var discount = Prd.DiscountCodes
                              .FirstOrDefault(d => d.Code == discountCode && d.ValidFrom <= DateTime.Today && d.ValidUntil >= DateTime.Today);

            if (discount != null)
            {
                decimal discountAmount = discount.DiscountAmount;
                decimal totalPrice = CalculateTotalPrice();
                decimal discountedPrice = totalPrice - discountAmount;

                if (discountedPrice >= 0)
                {
                    txtPaidAmount.Text = discountedPrice.ToString("C0", japaneseCulture);
                    txtDiscountCode.Text = $"-{discountAmount.ToString("C0", japaneseCulture)}";

                    nofi.ShowMessage(this, $"割引コード {discountCode} が正常に適用されました! 😍");
                }
                else
                {
                    nofi.ShowMessage(this, "割引コードが無効または期限切れです! 😒");
                }
            }
            else
            {
                nofi.ShowMessage(this, "割引コードが無効または期限切れです! 😒");
            }
        }

        private decimal CalculateTotalPrice()
        {
            decimal totalPrice = Prd.OrderItems.Sum(item => item.Price * item.Quantity);
            return totalPrice;
        }

        private void dgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProducts.Columns[e.ColumnIndex].Name == "Price" && e.Value != null)
            {
                if (e.Value is decimal price)
                {
                    e.Value = price.ToString("C0", new CultureInfo("ja-JP"));
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
