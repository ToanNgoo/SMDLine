namespace ManageMaterialPBA
{
    partial class ConfirmNVL
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
            this.txt_xn1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_dy = new System.Windows.Forms.Button();
            this.txt_mk = new System.Windows.Forms.TextBox();
            this.txt_dn = new System.Windows.Forms.TextBox();
            this.lbl_mk = new System.Windows.Forms.Label();
            this.lbl_dn = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_ndxn2 = new System.Windows.Forms.Label();
            this.txt_xn2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_ndxn1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_xn1
            // 
            this.txt_xn1.Location = new System.Drawing.Point(66, 101);
            this.txt_xn1.Multiline = true;
            this.txt_xn1.Name = "txt_xn1";
            this.txt_xn1.Size = new System.Drawing.Size(267, 58);
            this.txt_xn1.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 43;
            this.label1.Text = "Ý nghĩa :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(20, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(357, 32);
            this.label3.TabIndex = 42;
            this.label3.Text = "Để thực hiện công việc này, bạn cần có sự đồng ý của CPE.\r\nHãy liên hệ bộ phận CP" +
    "E.";
            // 
            // btn_dy
            // 
            this.btn_dy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_dy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_dy.ForeColor = System.Drawing.Color.Black;
            this.btn_dy.Location = new System.Drawing.Point(151, 425);
            this.btn_dy.Name = "btn_dy";
            this.btn_dy.Size = new System.Drawing.Size(94, 32);
            this.btn_dy.TabIndex = 24;
            this.btn_dy.Text = "Đồng ý";
            this.btn_dy.UseVisualStyleBackColor = false;
            this.btn_dy.Click += new System.EventHandler(this.btn_dy_Click);
            // 
            // txt_mk
            // 
            this.txt_mk.Location = new System.Drawing.Point(129, 104);
            this.txt_mk.Name = "txt_mk";
            this.txt_mk.Size = new System.Drawing.Size(200, 20);
            this.txt_mk.TabIndex = 20;
            this.txt_mk.UseSystemPasswordChar = true;
            // 
            // txt_dn
            // 
            this.txt_dn.Location = new System.Drawing.Point(129, 66);
            this.txt_dn.Name = "txt_dn";
            this.txt_dn.Size = new System.Drawing.Size(200, 20);
            this.txt_dn.TabIndex = 19;
            // 
            // lbl_mk
            // 
            this.lbl_mk.AutoSize = true;
            this.lbl_mk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_mk.Location = new System.Drawing.Point(20, 107);
            this.lbl_mk.Name = "lbl_mk";
            this.lbl_mk.Size = new System.Drawing.Size(58, 15);
            this.lbl_mk.TabIndex = 18;
            this.lbl_mk.Text = "Mật khẩu";
            // 
            // lbl_dn
            // 
            this.lbl_dn.AutoSize = true;
            this.lbl_dn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dn.Location = new System.Drawing.Point(20, 69);
            this.lbl_dn.Name = "lbl_dn";
            this.lbl_dn.Size = new System.Drawing.Size(90, 15);
            this.lbl_dn.TabIndex = 17;
            this.lbl_dn.Text = "Tên đăng nhập";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lbl_ndxn2);
            this.groupBox1.Controls.Add(this.txt_xn2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lbl_ndxn1);
            this.groupBox1.Controls.Add(this.txt_xn1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(23, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 283);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Xác nhận ký tự Maker Part thừa so với BOM";
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(130, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 21);
            this.label7.TabIndex = 31;
            this.label7.Text = "Maker Part";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(245, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 21);
            this.label6.TabIndex = 30;
            this.label6.Text = "(2)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(93, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 21);
            this.label5.TabIndex = 46;
            this.label5.Text = "(1)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_ndxn2
            // 
            this.lbl_ndxn2.AutoSize = true;
            this.lbl_ndxn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ndxn2.Location = new System.Drawing.Point(6, 173);
            this.lbl_ndxn2.Name = "lbl_ndxn2";
            this.lbl_ndxn2.Size = new System.Drawing.Size(109, 15);
            this.lbl_ndxn2.TabIndex = 29;
            this.lbl_ndxn2.Text = "(2) : ..........................";
            // 
            // txt_xn2
            // 
            this.txt_xn2.Location = new System.Drawing.Point(66, 211);
            this.txt_xn2.Multiline = true;
            this.txt_xn2.Name = "txt_xn2";
            this.txt_xn2.Size = new System.Drawing.Size(267, 58);
            this.txt_xn2.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 15);
            this.label4.TabIndex = 28;
            this.label4.Text = "Ý nghĩa :";
            // 
            // lbl_ndxn1
            // 
            this.lbl_ndxn1.AutoSize = true;
            this.lbl_ndxn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ndxn1.Location = new System.Drawing.Point(6, 63);
            this.lbl_ndxn1.Name = "lbl_ndxn1";
            this.lbl_ndxn1.Size = new System.Drawing.Size(109, 15);
            this.lbl_ndxn1.TabIndex = 26;
            this.lbl_ndxn1.Text = "(1) : ..........................";
            // 
            // ConfirmNVL
            // 
            this.AcceptButton = this.btn_dy;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 468);
            this.Controls.Add(this.btn_dy);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_mk);
            this.Controls.Add(this.txt_dn);
            this.Controls.Add(this.lbl_mk);
            this.Controls.Add(this.lbl_dn);
            this.Name = "ConfirmNVL";
            this.Text = "ConfirmNVL";
            this.Load += new System.EventHandler(this.ConfirmNVL_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_xn1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_dy;
        private System.Windows.Forms.TextBox txt_mk;
        private System.Windows.Forms.TextBox txt_dn;
        private System.Windows.Forms.Label lbl_mk;
        private System.Windows.Forms.Label lbl_dn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_ndxn1;
        private System.Windows.Forms.Label lbl_ndxn2;
        private System.Windows.Forms.TextBox txt_xn2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}