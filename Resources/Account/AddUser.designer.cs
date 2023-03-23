namespace ManageMaterialPBA
{
    partial class AddUser
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_addUser = new System.Windows.Forms.TextBox();
            this.txt_addPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbb_kind = new System.Windows.Forms.ComboBox();
            this.btn_confirmAddUser = new System.Windows.Forms.Button();
            this.btn_cancelAddUser = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_hoten = new System.Windows.Forms.Label();
            this.txt_hoten = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbb_part = new System.Windows.Forms.ComboBox();
            this.txt_maNV = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên đăng nhập";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Mật khẩu";
            // 
            // txt_addUser
            // 
            this.txt_addUser.Location = new System.Drawing.Point(156, 61);
            this.txt_addUser.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txt_addUser.Name = "txt_addUser";
            this.txt_addUser.Size = new System.Drawing.Size(148, 26);
            this.txt_addUser.TabIndex = 2;
            // 
            // txt_addPass
            // 
            this.txt_addPass.Location = new System.Drawing.Point(156, 103);
            this.txt_addPass.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txt_addPass.Name = "txt_addPass";
            this.txt_addPass.Size = new System.Drawing.Size(148, 26);
            this.txt_addPass.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 234);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Loại tài khoản";
            // 
            // cbb_kind
            // 
            this.cbb_kind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_kind.FormattingEnabled = true;
            this.cbb_kind.Location = new System.Drawing.Point(156, 230);
            this.cbb_kind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbb_kind.Name = "cbb_kind";
            this.cbb_kind.Size = new System.Drawing.Size(148, 28);
            this.cbb_kind.TabIndex = 5;
            // 
            // btn_confirmAddUser
            // 
            this.btn_confirmAddUser.Location = new System.Drawing.Point(16, 334);
            this.btn_confirmAddUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_confirmAddUser.Name = "btn_confirmAddUser";
            this.btn_confirmAddUser.Size = new System.Drawing.Size(100, 45);
            this.btn_confirmAddUser.TabIndex = 6;
            this.btn_confirmAddUser.Text = "Xác nhận";
            this.btn_confirmAddUser.UseVisualStyleBackColor = true;
            this.btn_confirmAddUser.Click += new System.EventHandler(this.btn_confirmAddUser_Click);
            // 
            // btn_cancelAddUser
            // 
            this.btn_cancelAddUser.Location = new System.Drawing.Point(220, 334);
            this.btn_cancelAddUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_cancelAddUser.Name = "btn_cancelAddUser";
            this.btn_cancelAddUser.Size = new System.Drawing.Size(84, 45);
            this.btn_cancelAddUser.TabIndex = 7;
            this.btn_cancelAddUser.Text = "Hủy";
            this.btn_cancelAddUser.UseVisualStyleBackColor = true;
            this.btn_cancelAddUser.Click += new System.EventHandler(this.btn_cancelAddUser_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 29);
            this.label4.TabIndex = 8;
            this.label4.Text = "THÊM TÀI KHOẢN";
            // 
            // lbl_hoten
            // 
            this.lbl_hoten.AutoSize = true;
            this.lbl_hoten.Location = new System.Drawing.Point(12, 148);
            this.lbl_hoten.Name = "lbl_hoten";
            this.lbl_hoten.Size = new System.Drawing.Size(57, 20);
            this.lbl_hoten.TabIndex = 13;
            this.lbl_hoten.Text = "Họ tên";
            // 
            // txt_hoten
            // 
            this.txt_hoten.Location = new System.Drawing.Point(156, 145);
            this.txt_hoten.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_hoten.Name = "txt_hoten";
            this.txt_hoten.Size = new System.Drawing.Size(148, 26);
            this.txt_hoten.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 279);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Bộ phận";
            // 
            // cbb_part
            // 
            this.cbb_part.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_part.FormattingEnabled = true;
            this.cbb_part.Location = new System.Drawing.Point(156, 275);
            this.cbb_part.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbb_part.Name = "cbb_part";
            this.cbb_part.Size = new System.Drawing.Size(148, 28);
            this.cbb_part.TabIndex = 6;
            // 
            // txt_maNV
            // 
            this.txt_maNV.Location = new System.Drawing.Point(156, 187);
            this.txt_maNV.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_maNV.Name = "txt_maNV";
            this.txt_maNV.Size = new System.Drawing.Size(148, 26);
            this.txt_maNV.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Mã nhân viên";
            // 
            // AddUser
            // 
            this.AcceptButton = this.btn_confirmAddUser;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(337, 397);
            this.Controls.Add(this.txt_maNV);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbb_part);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_hoten);
            this.Controls.Add(this.lbl_hoten);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_cancelAddUser);
            this.Controls.Add(this.btn_confirmAddUser);
            this.Controls.Add(this.cbb_kind);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_addPass);
            this.Controls.Add(this.txt_addUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "AddUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddUser";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AddUser_FormClosed);
            this.Load += new System.EventHandler(this.AddUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_addUser;
        private System.Windows.Forms.TextBox txt_addPass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbb_kind;
        private System.Windows.Forms.Button btn_confirmAddUser;
        private System.Windows.Forms.Button btn_cancelAddUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_hoten;
        private System.Windows.Forms.TextBox txt_hoten;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbb_part;
        private System.Windows.Forms.TextBox txt_maNV;
        private System.Windows.Forms.Label label6;
    }
}