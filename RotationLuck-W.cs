using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diversity
{
    public partial class RotationLuck : Form
    {
        bool wheelIsMoved;
        float wheelTimes;
        LuckyCircle koloFortuny;

        public RotationLuck()
        {
            InitializeComponent();
            wheelTimer = new Timer();
            wheelTimer.Interval = 30;
            wheelTimer.Tick += wheelTimer_Tick;
            koloFortuny = new LuckyCircle();
        }

        public class LuckyCircle
        {
            public Bitmap obrazek;
            public Bitmap tempObrazek;
            public float kat;
            public string[] wartosciStanu;
            public int stan;

            public LuckyCircle()
            {
                tempObrazek = new Bitmap(Properties.Resources.RotationLuck);
                obrazek = new Bitmap(Properties.Resources.RotationLuck);
                wartosciStanu = new string[] { "Áo thun Unisex", "Sáp vuốt tóc BluMaan", "Gấu bông con kiu dài", "Bóp Nam Nữ mini", "Tai Bluetooth Pro 4", "Dây xạc nhanh 20W 5A", "Đồng hồ T800 Pro Max" };
                kat = 0.0f;
            }
        }

        public static Bitmap RotateImage(System.Drawing.Image image, float angle)
        {
            return RotateImage(image, new PointF((float)image.Width / 2, (float)image.Height / 2), angle);
        }

        public static Bitmap RotateImage(System.Drawing.Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                g.TranslateTransform(offset.X, offset.Y);
                g.RotateTransform(angle);
                g.TranslateTransform(-offset.X, -offset.Y);
                g.DrawImage(image, new PointF(0, 0));
            }

            return rotatedBmp;
        }

        private void RotateImage(PictureBox pb, System.Drawing.Image img, float angle)
        {
            if (img == null || pb.Image == null)
                return;

            System.Drawing.Image oldImage = pb.Image;
            pb.Image = RotateImage(img, angle);
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        private void wheelTimer_Tick(object sender, EventArgs e)
        {
            if (wheelIsMoved && wheelTimes > 0)
            {
                koloFortuny.kat += wheelTimes / 10;
                koloFortuny.kat = koloFortuny.kat % 360;
                RotateImage(pictureBoxWheel, koloFortuny.obrazek, koloFortuny.kat);
                wheelTimes--;
            }
            else
            {
                wheelIsMoved = false;
                wheelTimer.Stop();
            }

            int numSegments = koloFortuny.wartosciStanu.Length;
            koloFortuny.stan = (int)((koloFortuny.kat / 360) * numSegments);
            koloFortuny.stan = koloFortuny.stan % numSegments;

            txtResult.Text = koloFortuny.wartosciStanu[koloFortuny.stan];
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            wheelIsMoved = true;
            Random rand = new Random();
            wheelTimes = rand.Next(150, 200);
            wheelTimer.Start();
        }

        private void lbReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
