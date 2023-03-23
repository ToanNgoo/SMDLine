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
    public partial class AddUser : Form
    {
        database dtb;
        private Form _frm;
        public string _strdatabase = string.Empty;

        public AddUser(Form1 frm, string strdatabase)
        {
            _frm = frm;
            _strdatabase = strdatabase;
            InitializeComponent();            
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            dtb = new database(_strdatabase);
            this.Location = new Point(0, 0);

            // Thêm items vào comboBox quản lý quyền
            cbb_kind.Items.Add("admin");
            cbb_kind.Items.Add("manager");
            cbb_kind.Items.Add("user");

            cbb_part.Items.Add("KTZ");
            cbb_part.Items.Add("PD");
            cbb_part.Items.Add("CPE");
            cbb_part.Items.Add("QA");
        }

        private void btn_confirmAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_addUser.Text == "" || txt_addPass.Text == "" || cbb_kind.Text == ""
                    || txt_hoten.Text == "" || cbb_part.Text == "" || txt_maNV.Text == "") // user/pass để trống
                {
                    MessageBox.Show("Các thông tin không được để trống!", "Thêm tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dtb.insert_account(txt_addUser.Text, txt_addPass.Text, cbb_kind.Text, txt_hoten.Text, cbb_part.Text, txt_maNV.Text);
                    MessageBox.Show("Thêm tài khoản thành công!", "Thêm tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi thêm tài khoản!", "Thêm tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cancelAddUser_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            _frm.Enabled = true;
        }
    }
}
