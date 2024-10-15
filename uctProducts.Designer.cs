namespace Diversity
{
    partial class uctProducts
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uctProducts));
            this.plUctProducts = new Guna.UI2.WinForms.Guna2Panel();
            this.txtProductName = new Guna.UI2.WinForms.Guna2TextBox();
            this.plEditDelete = new System.Windows.Forms.Panel();
            this.btnDelete = new Guna.UI2.WinForms.Guna2Button();
            this.btnDetail = new Guna.UI2.WinForms.Guna2Button();
            this.btnView = new Guna.UI2.WinForms.Guna2Button();
            this.picProducts = new Guna.UI2.WinForms.Guna2PictureBox();
            this.ElipseUserControl = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.plUctProducts.SuspendLayout();
            this.plEditDelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // plUctProducts
            // 
            this.plUctProducts.BackColor = System.Drawing.Color.Transparent;
            this.plUctProducts.BorderColor = System.Drawing.Color.Silver;
            this.plUctProducts.BorderRadius = 2;
            this.plUctProducts.Controls.Add(this.txtProductName);
            this.plUctProducts.Controls.Add(this.plEditDelete);
            this.plUctProducts.Controls.Add(this.btnView);
            this.plUctProducts.Controls.Add(this.picProducts);
            this.plUctProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plUctProducts.FillColor = System.Drawing.Color.White;
            this.plUctProducts.Location = new System.Drawing.Point(0, 0);
            this.plUctProducts.Name = "plUctProducts";
            this.plUctProducts.Size = new System.Drawing.Size(216, 245);
            this.plUctProducts.TabIndex = 3;
            // 
            // txtProductName
            // 
            this.txtProductName.BorderThickness = 0;
            this.txtProductName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtProductName.DefaultText = "";
            this.txtProductName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtProductName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtProductName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtProductName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtProductName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtProductName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtProductName.ForeColor = System.Drawing.Color.Red;
            this.txtProductName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtProductName.Location = new System.Drawing.Point(3, 212);
            this.txtProductName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.PasswordChar = '\0';
            this.txtProductName.PlaceholderText = "";
            this.txtProductName.SelectedText = "";
            this.txtProductName.Size = new System.Drawing.Size(210, 23);
            this.txtProductName.TabIndex = 4;
            this.txtProductName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // plEditDelete
            // 
            this.plEditDelete.Controls.Add(this.btnDelete);
            this.plEditDelete.Controls.Add(this.btnDetail);
            this.plEditDelete.Location = new System.Drawing.Point(168, 54);
            this.plEditDelete.Name = "plEditDelete";
            this.plEditDelete.Size = new System.Drawing.Size(46, 91);
            this.plEditDelete.TabIndex = 3;
            this.plEditDelete.Visible = false;
            // 
            // btnDelete
            // 
            this.btnDelete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDelete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDelete.FillColor = System.Drawing.Color.Gainsboro;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageSize = new System.Drawing.Size(30, 30);
            this.btnDelete.Location = new System.Drawing.Point(4, 47);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(38, 41);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDetail
            // 
            this.btnDetail.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDetail.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDetail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDetail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDetail.FillColor = System.Drawing.Color.Gainsboro;
            this.btnDetail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDetail.ForeColor = System.Drawing.Color.White;
            this.btnDetail.Image = ((System.Drawing.Image)(resources.GetObject("btnDetail.Image")));
            this.btnDetail.ImageSize = new System.Drawing.Size(25, 25);
            this.btnDetail.Location = new System.Drawing.Point(4, 4);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new System.Drawing.Size(38, 41);
            this.btnDetail.TabIndex = 4;
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // btnView
            // 
            this.btnView.BorderRadius = 2;
            this.btnView.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnView.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnView.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnView.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnView.FillColor = System.Drawing.Color.Black;
            this.btnView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnView.ForeColor = System.Drawing.Color.White;
            this.btnView.Image = ((System.Drawing.Image)(resources.GetObject("btnView.Image")));
            this.btnView.Location = new System.Drawing.Point(201, 16);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(12, 37);
            this.btnView.TabIndex = 2;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // picProducts
            // 
            this.picProducts.ImageRotate = 0F;
            this.picProducts.Location = new System.Drawing.Point(17, 16);
            this.picProducts.Name = "picProducts";
            this.picProducts.Size = new System.Drawing.Size(180, 189);
            this.picProducts.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picProducts.TabIndex = 0;
            this.picProducts.TabStop = false;
            // 
            // ElipseUserControl
            // 
            this.ElipseUserControl.BorderRadius = 5;
            this.ElipseUserControl.TargetControl = this;
            // 
            // uctProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.plUctProducts);
            this.Name = "uctProducts";
            this.Size = new System.Drawing.Size(216, 245);
            this.plUctProducts.ResumeLayout(false);
            this.plEditDelete.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picProducts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Button btnView;
        private Guna.UI2.WinForms.Guna2Panel plUctProducts;
        private System.Windows.Forms.Panel plEditDelete;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
        private Guna.UI2.WinForms.Guna2Button btnDetail;
        private Guna.UI2.WinForms.Guna2TextBox txtProductName;
        private Guna.UI2.WinForms.Guna2PictureBox picProducts;
        private Guna.UI2.WinForms.Guna2Elipse ElipseUserControl;
    }
}
