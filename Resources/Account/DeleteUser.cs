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
    public partial class DeleteUser : Form
    {
        database dtb;
        private Form _frm;
        public string _strdatabase = string.Empty;

        public DeleteUser(Form1 frm, string strdatabase)
        {
            _frm = frm;
            _strdatabase = strdatabase;
            InitializeComponent();
        }

        private void btn_confirmDel_Click(object sender, EventArgs e)
        {
            if (cbb_delete_user.Text == "")
            {
                MessageBox.Show("Chưa chọn tài khoản muốn xóa!", "Xóa tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string str = "delete from login where u_ser = '" + cbb_delete_user.Text + "' and part='" + cbb_delPrt.Text + "'";
                dtb.delete(str);
                MessageBox.Show("Xóa tài khoản thành công!", "Xóa tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_cancelDel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            _frm.Enabled = true;
        }

        private void DeleteUser_Load(object sender, EventArgs e)
        {
            dtb = new database(_strdatabase);
            this.Location = new Point(0, 0);

            cbb_delPrt.Items.Add("KTZ");
            cbb_delPrt.Items.Add("PD");
            cbb_delPrt.Items.Add("CPE");
            cbb_delPrt.Items.Add("QA");
        }

        private void cbb_delPrt_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbb_delete_user.Items.Clear();
            try
            {
                string str = "select u_ser from login Where part='" + cbb_delPrt.Text + "'";
                DataTable dt = new DataTable();
                dt = dtb.getData(str);

                // Add users vào comboBox
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.ItemArray[0].ToString() != Form1._user)
                    {
                        cbb_delete_user.Items.Add(dr.ItemArray[0].ToString());
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn bộ phận trước nhé!", "Xóa tài khoản");
            }       
        }
    }
}
