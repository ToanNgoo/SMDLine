namespace ManageMaterialPBA
{
    partial class ConfirmWHKTZ
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.btn_dy = new System.Windows.Forms.Button();
            this.txt_mk = new System.Windows.Forms.TextBox();
            this.txt_dn = new System.Windows.Forms.TextBox();
            this.lbl_mk = new System.Windows.Forms.Label();
            this.lbl_dn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(16, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(337, 36);
            this.label3.TabIndex = 11;
            this.label3.Text = "Để thực hiện công việc này, bạn cần có sự đồng ý \r\ncủa CPE. Hãy liên hệ bộ phận C" +
    "PE.";
            // 
            // btn_dy
            // 
            this.btn_dy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_dy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_dy.ForeColor = System.Drawing.Color.Black;
            this.btn_dy.Location = new System.Drawing.Point(139, 170);
            this.btn_dy.Name = "btn_dy";
            this.btn_dy.Size = new System.Drawing.Size(94, 32);
            this.btn_dy.TabIndex = 10;
            this.btn_dy.Text = "Đồng ý";
            this.btn_dy.UseVisualStyleBackColor = false;
            this.btn_dy.Click += new System.EventHandler(this.btn_dy_Click);
            // 
            // txt_mk
            // 
            this.txt_mk.Location = new System.Drawing.Point(145, 123);
            this.txt_mk.Name = "txt_mk";
            this.txt_mk.Size = new System.Drawing.Size(179, 20);
            this.txt_mk.TabIndex = 9;
            this.txt_mk.UseSystemPasswordChar = true;
            // 
            // txt_dn
            // 
            this.txt_dn.Location = new System.Drawing.Point(145, 71);
            this.txt_dn.Name = "txt_dn";
            this.txt_dn.Size = new System.Drawing.Size(179, 20);
            this.txt_dn.TabIndex = 8;
            // 
            // lbl_mk
            // 
            this.lbl_mk.AutoSize = true;
            this.lbl_mk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_mk.Location = new System.Drawing.Point(27, 126);
            this.lbl_mk.Name = "lbl_mk";
            this.lbl_mk.Size = new System.Drawing.Size(62, 16);
            this.lbl_mk.TabIndex = 7;
            this.lbl_mk.Text = "Mật khẩu";
            // 
            // lbl_dn
            // 
            this.lbl_dn.AutoSize = true;
            this.lbl_dn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dn.Location = new System.Drawing.Point(27, 74);
            this.lbl_dn.Name = "lbl_dn";
            this.lbl_dn.Size = new System.Drawing.Size(99, 16);
            this.lbl_dn.TabIndex = 6;
            this.lbl_dn.Text = "Tên đăng nhập";
            // 
            // ConfirmWHKTZ
            // 
            this.AcceptButton = this.btn_dy;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 220);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_dy);
            this.Controls.Add(this.txt_mk);
            this.Controls.Add(this.txt_dn);
            this.Controls.Add(this.lbl_mk);
            this.Controls.Add(this.lbl_dn);
            this.Name = "ConfirmWHKTZ";
            this.Text = "ConfirmWHKTZ";
            this.Load += new System.EventHandler(this.ConfirmWHKTZ_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_dy;
        private System.Windows.Forms.TextBox txt_mk;
        private System.Windows.Forms.TextBox txt_dn;
        private System.Windows.Forms.Label lbl_mk;
        private System.Windows.Forms.Label lbl_dn;
    }
}