using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Diversity
{
    public partial class OrderDetail_W : Form
    {
        Notification_W nofi = new Notification_W();
        CultureInfo japaneseCulture = new CultureInfo("ja-JP");
        private int orderId;

        public OrderDetail_W()
        {
            InitializeComponent();
        }

        public void LoadOrder(int orderId)
        {
            this.orderId = orderId;
            LoadOrderDetails();
            LoadPrice();
            DateTime currentDate = DateTime.Today;
            lbDate.Text = currentDate.ToString("dd/MM/yyyy");
        }

        private void OrderDetail_W_Load(object sender, EventArgs e)
        {
            LoadOrderDetails();
            LoadPrice();
        }

        private void LoadOrderDetails()
        {
            using (var Prd = new AppEntitiesConnection())
            {
                var orderDetails = (from od in Prd.OrderDetails
                                    where od.OrderID == orderId
                                    select new
                                    {
                                        od.Product.ProductImage,
                                        od.Product.ProductName,
                                        od.Quantity,
                                        od.Price
                                    }).ToList();

                foreach (var detail in orderDetails)
                {
                    if (!string.IsNullOrEmpty(detail.ProductImage))
                    {
                        picProducts.Image = System.Drawing.Image.FromFile(detail.ProductImage);
                    }                                             
                    txtProductName.Text = detail.ProductName;
                    txtQuantity.Text = detail.Quantity.ToString();
                    txtProductPrice.Text = detail.Price.ToString("C0", japaneseCulture);
                }
            }
        }

        private void LoadPrice()
        {
            using (var Prd = new AppEntitiesConnection())
            {
                decimal subTotal = (from od in Prd.OrderDetails
                                    where od.OrderID == orderId
                                    select od.Quantity * od.Price).Sum();

                decimal discount = 0;
                var discountCode = Prd.DiscountCodes.FirstOrDefault(dc => dc.ValidFrom <= DateTime.Today && dc.ValidUntil >= DateTime.Today);
                if (discountCode != null)
                {
                    discount = discountCode.DiscountAmount;
                }
                txtDiscountCode.Text = $"-{(-discount).ToString("C0", japaneseCulture)}";
                decimal totalAfterDiscount = subTotal - discount;
                txtTotal.Text = totalAfterDiscount.ToString("C0", japaneseCulture);
            }
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;

            float pageWidth = e.PageSettings.PrintableArea.Width;
            float pageHeight = e.PageSettings.PrintableArea.Height;

            Size formSize = this.Size;

            float x = (pageWidth - formSize.Width) / 2;
            float y = (pageHeight - formSize.Height) / 2;

            Bitmap bmp = new Bitmap(formSize.Width, formSize.Height);

            this.DrawToBitmap(bmp, new Rectangle(0, 0, formSize.Width, formSize.Height));

            graphics.DrawImage(bmp, new PointF(x, y));

            bmp.Dispose();
        }

        private void lbHistory_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
