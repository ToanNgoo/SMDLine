using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManageMaterialPBA
{
    public partial class Change_pw : Form
    {
        database dtb;
        string user = string.Empty;
        public string _strdatabase = string.Empty;
        private Form _frm;
        Form1 _frm1;
        private string _txtUser = "";

        public Change_pw(Form1 frm1, string txtUser, string strdatabase)
        {
            _frm1 = frm1;
            _frm = frm1;
            _txtUser = txtUser;
            _strdatabase = strdatabase;
            InitializeComponent();
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if(dtb.login(user, txt_oldpw.Text) == true)
            {
                if(txt_newpw.Text == txt_oldpw.Text) // mk mới giống mk cũ
                {
                    MessageBox.Show("Mật khẩu cũ và mật khẩu mới không được trùng nhau!", "Thay đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_newpw.Clear();
                    txt_newpw_again.Clear();
                    txt_oldpw.Clear();
                }
                else if(txt_newpw.Text == "" && txt_newpw_again.Text == "")
                {
                    MessageBox.Show("Mật khẩu mới và mật khẩu xác nhận bị trống!", "Thay đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(txt_newpw.Text == txt_newpw_again.Text) // mk mới giống mk xác nhận
                {
                    dtb.change_pw(user, txt_newpw_again.Text, dtb.get_TimeChangepass().ToString());
                    DialogResult rel = MessageBox.Show("Bạn đã đổi mật khẩu thành công!", "Thay đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if(rel == DialogResult.OK)
                    {
                        _frm1.doiRoi = true;
                        _frm1.sMDToolStripMenuItem1.Enabled = true;
                        dtb.UpChangePass(_txtUser, txt_oldpw.Text, txt_newpw_again.Text);
                        this.Close();
                    }
                }
                else // Mật khẩu mới và mật khẩu xác nhận lại không khớp
                {
                    MessageBox.Show("Mật khẩu mới và mật khẩu xác nhận lại không trùng khớp!", "Thay đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu. Hãy thử lại!", "Thay đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Gán giá trị user từ form1 vào form mới
        private void Change_pw_Load(object sender, EventArgs e)
        {
            dtb = new database(_strdatabase);
            user = Form1._user;
            this.Location = new Point(0, 0);
        }

        // Sau khi form thay đổi pass đóng, mở lại form 1.
        private void Change_pw_FormClosed(object sender, FormClosedEventArgs e)
        {
            _frm.Enabled = true;
        }
    }
}
