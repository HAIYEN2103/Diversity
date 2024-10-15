using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Diversity
{
    public partial class OverView_W : Form
    {
        private AppEntitiesConnection Char;
        CultureInfo japaneseCulture = new CultureInfo("ja-JP");

        public OverView_W()
        {
            InitializeComponent();
        }

        private void OverView_W_Load(object sender, EventArgs e)
        {
            LoadRevenueChart();
            LoadDate();
            LoadPrice();
            LoadProduct();
            LoadCustomer();
        }
        private void LoadDate()
        {
            DateTime currentDate = DateTime.Today;
            lbDate.Text = currentDate.ToString("dd/MM/yyyy");
        }

        private void LoadRevenueChart()
        {
            using (var context = new AppEntitiesConnection())
            {
                var query = from order in context.Orders
                            where order.OrderDate.Year == DateTime.Today.Year
                            group order by order.OrderDate.Month into g
                            select new
                            {
                                Month = g.Key,
                                MinRevenue = g.Min(o => o.TotalAmount),
                                MaxRevenue = g.Max(o => o.TotalAmount)
                            };

                var data = query.ToList().Select(x => new { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month), MinRevenue = (double)x.MinRevenue, MaxRevenue = (double)x.MaxRevenue }).ToList();

                SalesChart.Series.Clear();

                var series = SalesChart.Series.Add("月次収益 ");

                series.ChartType = SeriesChartType.RangeColumn;

                series.Color = Color.FromArgb(80, 140, 200);
                series.BackGradientStyle = GradientStyle.VerticalCenter;

                foreach (var item in data)
                {
                    var point = series.Points.AddXY(item.Month, item.MinRevenue, item.MaxRevenue);
                }

                SalesChart.ChartAreas[0].AxisX.Interval = 1;
                SalesChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.DarkSlateGray;

                SalesChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                SalesChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                SalesChart.BackColor = Color.WhiteSmoke;

                SalesChart.Invalidate();
            }
        }

        private void LoadPrice()
        {
            using (var context = new AppEntitiesConnection())
            {
                decimal totalPrice = context.Orders
                    .Where(order => order.OrderDate.Year == DateTime.Today.Year)
                    .Sum(order => order.TotalAmount);
                txtTotalSales.Text = totalPrice.ToString("N2");

                int totalItems = context.OrderDetails
            .Where(detail => detail.Order.OrderDate.Year == DateTime.Today.Year)
            .Sum(detail => detail.Quantity);
                txtTotalitems.Text = totalItems.ToString();

                int numMonths = context.Orders
            .Where(order => order.OrderDate.Year == DateTime.Today.Year)
            .Select(order => order.OrderDate.Month)
            .Distinct()
            .Count();
                decimal averageSales = numMonths > 0 ? totalPrice / numMonths : 0;
                txtAverageSales.Text = averageSales.ToString("N2");

                int countSales = context.Orders
            .Where(order => order.OrderDate.Year == DateTime.Today.Year)
            .Count();
                txtCountSales.Text = countSales.ToString();
            }
        }

        private void LoadProduct()
        {
            dgvProducts.Rows.Clear();

            using (var context = new AppEntitiesConnection())
            {
                var bestSellingProduct = context.OrderDetails
                    .Where(detail => detail.Order.OrderDate.Year == DateTime.Today.Year)
                    .GroupBy(detail => detail.ProductID)
                    .Select(group => new
                    {
                        ProductId = group.Key,
                        TotalQuantity = group.Sum(detail => detail.Quantity)
                    })
                    .OrderByDescending(group => group.TotalQuantity)
                    .FirstOrDefault();

                if (bestSellingProduct != null)
                {
                    var product = context.Products.Find(bestSellingProduct.ProductId);
                    dgvProducts.Rows.Add("ベルセール製品", product.ProductName, bestSellingProduct.TotalQuantity);
                }
            }
        }

        private void LoadCustomer()
        {
            dgvCustomer.Rows.Clear();
            dgvCustomer.Columns.Clear(); 

            if (dgvCustomer.Columns.Count == 0)
            {
                dgvCustomer.Columns.Add("カテゴリー", "カテゴリー");
                dgvCustomer.Columns.Add("氏名", "氏名");
                dgvCustomer.Columns.Add("総注文数", "総注文数");
            }

            using (var context = new AppEntitiesConnection())
            {
                var topCustomer = context.Orders
                    .Where(order => order.OrderDate.Year == DateTime.Today.Year)
                    .GroupBy(order => order.CustomerID)
                    .Select(group => new
                    {
                        CustomerId = group.Key,
                        TotalOrders = group.Count()
                    })
                    .OrderByDescending(group => group.TotalOrders)
                    .FirstOrDefault();

                if (topCustomer != null)
                {
                    var customer = context.Customers.Find(topCustomer.CustomerId);
                    if (customer != null)
                    {
                        dgvCustomer.Rows.Add("最優良顧客", customer.Name, topCustomer.TotalOrders);
                    }
                }
            }
        }

        private void btnDay_Click(object sender, EventArgs e)
        {
            using (var context = new AppEntitiesConnection())
            {
                var query = from order in context.Orders
                            where order.OrderDate.Year == DateTime.Today.Year && order.OrderDate.Month == DateTime.Today.Month
                            group order by order.OrderDate.Day into g
                            select new
                            {
                                Day = g.Key,
                                MinRevenue = g.Min(o => o.TotalAmount),
                                MaxRevenue = g.Max(o => o.TotalAmount)
                            };

                var data = query.ToList().Select(x => new { Day = x.Day, MinRevenue = (double)x.MinRevenue, MaxRevenue = (double)x.MaxRevenue }).ToList();

                SalesChart.Series.Clear();

                var series = SalesChart.Series.Add("日次収益");

                series.ChartType = SeriesChartType.RangeColumn;

                series.Color = Color.FromArgb(80, 140, 200);
                series.BackGradientStyle = GradientStyle.VerticalCenter;

                foreach (var item in data)
                {
                    var point = series.Points.AddXY(item.Day, item.MinRevenue, item.MaxRevenue);
                }

                SalesChart.ChartAreas[0].AxisX.Interval = 1;
                SalesChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.DarkSlateGray;

                SalesChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                SalesChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                SalesChart.BackColor = Color.WhiteSmoke;

                SalesChart.Invalidate();
            }
        }

        private void btnMonth_Click(object sender, EventArgs e)
        {
            LoadRevenueChart(); // Keep the existing behavior to load monthly revenue
        }

        private void btnYear_Click(object sender, EventArgs e)
        {
            using (var context = new AppEntitiesConnection())
            {
                var query = from order in context.Orders
                            where order.OrderDate.Year == DateTime.Today.Year
                            group order by order.OrderDate.Month into g
                            select new
                            {
                                Month = g.Key,
                                MinRevenue = g.Min(o => o.TotalAmount),
                                MaxRevenue = g.Max(o => o.TotalAmount)
                            };

                var data = query.ToList().Select(x => new { Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month), MinRevenue = (double)x.MinRevenue, MaxRevenue = (double)x.MaxRevenue }).ToList();

                SalesChart.Series.Clear();

                var series = SalesChart.Series.Add("年次収益 ");

                series.ChartType = SeriesChartType.RangeColumn;

                series.Color = Color.FromArgb(80, 140, 200);
                series.BackGradientStyle = GradientStyle.VerticalCenter;

                foreach (var item in data)
                {
                    var point = series.Points.AddXY(item.Month, item.MinRevenue, item.MaxRevenue);
                }

                SalesChart.ChartAreas[0].AxisX.Interval = 1;
                SalesChart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.DarkSlateGray;

                SalesChart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                SalesChart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                SalesChart.BackColor = Color.WhiteSmoke;

                SalesChart.Invalidate();
            }
        }

       
    }
}
