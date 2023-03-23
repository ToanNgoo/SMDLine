namespace ManageMaterialPBA
{
    partial class DeleteUser
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
            this.cbb_delete_user = new System.Windows.Forms.ComboBox();
            this.btn_confirmDel = new System.Windows.Forms.Button();
            this.btn_cancelDel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbb_delPrt = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 143);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên đăng nhập";
            // 
            // cbb_delete_user
            // 
            this.cbb_delete_user.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_delete_user.FormattingEnabled = true;
            this.cbb_delete_user.Location = new System.Drawing.Point(246, 139);
            this.cbb_delete_user.Margin = new System.Windows.Forms.Padding(4);
            this.cbb_delete_user.Name = "cbb_delete_user";
            this.cbb_delete_user.Size = new System.Drawing.Size(165, 32);
            this.cbb_delete_user.TabIndex = 1;
            // 
            // btn_confirmDel
            // 
            this.btn_confirmDel.Location = new System.Drawing.Point(100, 201);
            this.btn_confirmDel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_confirmDel.Name = "btn_confirmDel";
            this.btn_confirmDel.Size = new System.Drawing.Size(103, 42);
            this.btn_confirmDel.TabIndex = 2;
            this.btn_confirmDel.Text = "Xác nhận";
            this.btn_confirmDel.UseVisualStyleBackColor = true;
            this.btn_confirmDel.Click += new System.EventHandler(this.btn_confirmDel_Click);
            // 
            // btn_cancelDel
            // 
            this.btn_cancelDel.Location = new System.Drawing.Point(308, 201);
            this.btn_cancelDel.Margin = new System.Windows.Forms.Padding(4);
            this.btn_cancelDel.Name = "btn_cancelDel";
            this.btn_cancelDel.Size = new System.Drawing.Size(103, 42);
            this.btn_cancelDel.TabIndex = 3;
            this.btn_cancelDel.Text = "Hủy";
            this.btn_cancelDel.UseVisualStyleBackColor = true;
            this.btn_cancelDel.Click += new System.EventHandler(this.btn_cancelDel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "XÓA TÀI KHOẢN";
            // 
            // cbb_delPrt
            // 
            this.cbb_delPrt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbb_delPrt.FormattingEnabled = true;
            this.cbb_delPrt.Location = new System.Drawing.Point(246, 70);
            this.cbb_delPrt.Margin = new System.Windows.Forms.Padding(4);
            this.cbb_delPrt.Name = "cbb_delPrt";
            this.cbb_delPrt.Size = new System.Drawing.Size(165, 32);
            this.cbb_delPrt.TabIndex = 6;
            this.cbb_delPrt.SelectedIndexChanged += new System.EventHandler(this.cbb_delPrt_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 74);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Bộ phận";
            // 
            // DeleteUser
            // 
            this.AcceptButton = this.btn_confirmDel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(494, 284);
            this.Controls.Add(this.cbb_delPrt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_cancelDel);
            this.Controls.Add(this.btn_confirmDel);
            this.Controls.Add(this.cbb_delete_user);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "DeleteUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DeleteUser";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeleteUser_FormClosed);
            this.Load += new System.EventHandler(this.DeleteUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbb_delete_user;
        private System.Windows.Forms.Button btn_confirmDel;
        private System.Windows.Forms.Button btn_cancelDel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbb_delPrt;
        private System.Windows.Forms.Label label3;
    }
}