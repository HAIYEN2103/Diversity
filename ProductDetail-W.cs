using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using DrawingImage = System.Drawing.Image;
using System.Windows.Forms;
using System.Globalization;

namespace Diversity
{
    public partial class ProductDetail_W : Form
    {
        private Product _product;
        AppEntitiesConnection Prd = new AppEntitiesConnection();
        private int _quantity = 1;
        CultureInfo japaneseCulture = new CultureInfo("ja-JP");

      
        public ProductDetail_W(Product product)
        {
            InitializeComponent();
            _product = product;
            DisplayProductDetails();
        }

        private void DisplayProductDetails()
        {
            var prdItem = Prd.Products.FirstOrDefault(m => m.ProductName == _product.ProductName);
            if (prdItem != null)
            {
                txtProductName.Text = prdItem.ProductName;
                txtPrice.Text = prdItem.Price.ToString("C0", japaneseCulture);
                if (!string.IsNullOrEmpty(prdItem.ProductImage) && System.IO.File.Exists(prdItem.ProductImage))
                {
                    picProducts.Image = DrawingImage.FromFile(prdItem.ProductImage);
                }
                else
                {
                    picProducts.Image = null;
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIncrease_Click(object sender, EventArgs e)
        {
            _quantity++; 
            UpdateQuantityAndPriceDisplay();
        }

        private void btnReduce_Click(object sender, EventArgs e)
        {
            if (_quantity > 1)
            {
                _quantity--;
                UpdateQuantityAndPriceDisplay();
            }
        }

        private void UpdateQuantityAndPriceDisplay()
        {
            txtQuantity.Text = _quantity.ToString();

            var prdItem = Prd.Products.FirstOrDefault(m => m.ProductName == _product.ProductName);
            if (prdItem != null)
            {
                decimal productPrice = prdItem.Price; 
                decimal totalPrice = productPrice * _quantity;
                txtPrice.Text = totalPrice.ToString("C0", japaneseCulture);
            }
        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            plColor.BackColor = Color.Black;
        }

        private void btnWhite_Click(object sender, EventArgs e)
        {
            plColor.BackColor = Color.White;
        }

        private void btnBuyNow_Click(object sender, EventArgs e)
        {
            OrderItem newOrderItem = new OrderItem
            {
                ProductName = _product.ProductName,
                Price = _product.Price,
                Quantity = _quantity,
                ProductImage = _product.ProductImage
            };
            Prd.OrderItems.Add(newOrderItem);
            Prd.SaveChanges();

            Payment_W paymentForm = new Payment_W(_product, _quantity);
            paymentForm.ShowDialog();
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {

        }
    }
}
