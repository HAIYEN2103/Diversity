using System;
using System.Windows.Forms;

namespace Diversity
{
    public partial class Notification_W : Form
    {
        private Timer fadeInTimer;
        private double opacityIncrement = 0.02;
        private int animationDuration = 300;
        private bool isClosing = false; // Flag to prevent multiple close attempts

        public Notification_W()
        {
            InitializeComponent();
            this.Opacity = 0;
        }

        public void ShowMessage(Form parentForm, string message)
        {
            txtMessage.Text = message;
            this.StartPosition = FormStartPosition.CenterParent;
            AnimateForm(parentForm);
            this.ShowDialog(parentForm);
        }

        public DialogResult ShowMessage(Form parentForm, string message, string caption, MessageBoxButtons buttons)
        {
            txtMessage.Text = message;
            this.Text = caption;
            this.StartPosition = FormStartPosition.CenterParent;
            AnimateForm(parentForm);
            return this.ShowDialog(parentForm);
        }

        private void AnimateForm(Form parentForm)
        {
            fadeInTimer = new Timer();
            fadeInTimer.Interval = animationDuration / 20;
            fadeInTimer.Tick += (s, e) =>
            {
                if (this.Opacity >= 1)
                {
                    fadeInTimer.Stop();
                }
                else
                {
                    this.Opacity += opacityIncrement;
                }
            };
            fadeInTimer.Start();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!isClosing)
            {
                isClosing = true;
                btnOK.Enabled = false; // Disable button to prevent multiple clicks
                DialogResult = DialogResult.OK;
                FadeOutAndClose();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!isClosing)
            {
                isClosing = true;
                btnClose.Enabled = false; // Disable button to prevent multiple clicks
                DialogResult = DialogResult.Cancel;
                FadeOutAndClose();
            }
        }

        private void FadeOutAndClose()
        {
            Timer fadeOutTimer = new Timer();
            fadeOutTimer.Interval = animationDuration / 20;
            fadeOutTimer.Tick += (s, e) =>
            {
                if (!this.IsDisposed && this.Opacity <= 0)
                {
                    fadeOutTimer.Stop();
                    this.Close();
                }
                else if (!this.IsDisposed)
                {
                    this.Opacity -= opacityIncrement;
                }
            };
            fadeOutTimer.Start();
        }
    }
}
