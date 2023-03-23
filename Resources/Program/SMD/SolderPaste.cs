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
    public partial class SolderPaste : Form
    {
        database_1 dtb1;
        database dtb;
        ClsExcel ex = new ClsExcel();
        public string _strdatabase = string.Empty;
        public string dtim = string.Empty;
        public DataTable dt_common;

        public SolderPaste(string strdatabase)
        {
            InitializeComponent();
            _strdatabase = strdatabase;
        }

        private void SolderPaste_Load(object sender, EventArgs e)
        {
            dtb1 = new database_1(_strdatabase);
            dtb = new database(_strdatabase);
            this.Location = new Point(0, 0);

            dt_common = dtb1.get_Stock("PD_NVLSpecial_Stock", "STT");
            dtb1.show_SP(dgv_stk, dt_common);

            dtb.get_inf(cbx_mol, "PD_NVLSpecial_Stock", "Model");
            dtb.get_inf(cbx_tenNVL, "PD_NVLSpecial_Stock", "NVL");
            dtb.get_inf(cbx_maker, "PD_NVLSpecial_Stock", "Maker");
            dtb.get_inf(cbx_codeWH, "PD_NVLSpecial_Stock", "Code_WH");

            dtim = getYearMonthDay();
        }

        public string getYearMonthDay()
        {
            string str = string.Empty;
            if (DateTime.Now.Month < 10)
                str = DateTime.Now.Year.ToString() + "-0" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            else
                str = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            return str;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cbx_mol.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cbx_tenNVL.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cbx_maker.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cbx_codeWH.Text = "";
        }

        private void btn_SaveAss_Click(object sender, EventArgs e)
        {
            SaveFileDialog savDia = new SaveFileDialog();
            savDia.Title = "Excel Save Dialog";
            savDia.InitialDirectory = @"C:\";
            savDia.Filter = "Excel File |*.csv";
            savDia.FilterIndex = 1;
            string fil_name = "";
            if (savDia.ShowDialog() == DialogResult.OK)
            {
                fil_name = savDia.FileName;
            }

            if (fil_name != "")
            {
                bool chek = ex.checkExitLog(fil_name);
                ex.exportStockKTZZ(dgv_stk, fil_name, chek);                
                MessageBox.Show("Lưu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }     
        }

        private void btn_excute_Click(object sender, EventArgs e)
        {
            dgv_stk.Columns.Clear();

            string str = string.Empty;

            try
            {
                //all
                if (cbx_mol.Text == "" && cbx_tenNVL.Text == "" && cbx_maker.Text == "" && cbx_codeWH.Text == "")
                {
                    str = "Select * From PD_NVLSpecial_Stock";
                    goto jump;
                }

                //
                if (cbx_mol.Text != "" && cbx_tenNVL.Text != "" && cbx_maker.Text != "" && cbx_codeWH.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where Model='" + cbx_mol.Text + "' And NVL='" + cbx_tenNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Code_WH='" + cbx_codeWH.Text + "'";
                    goto jump;
                }

                if (cbx_mol.Text != "" && cbx_tenNVL.Text != "" && cbx_maker.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where Model='" + cbx_mol.Text + "' And NVL='" + cbx_tenNVL.Text + "' And Maker='" + cbx_maker.Text + "'";
                    goto jump;
                }

                if (cbx_mol.Text != "" && cbx_tenNVL.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where Model='" + cbx_mol.Text + "' And NVL='" + cbx_tenNVL.Text + "'";
                    goto jump;
                }

                if (cbx_mol.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where Model='" + cbx_mol.Text + "'";
                    goto jump;
                }               

                //
                if (cbx_tenNVL.Text != "" && cbx_maker.Text != "" && cbx_codeWH.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where NVL='" + cbx_tenNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Code_WH='" + cbx_codeWH.Text + "'";
                    goto jump;
                }

                if (cbx_tenNVL.Text != "" && cbx_maker.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where NVL='" + cbx_tenNVL.Text + "' And Maker='" + cbx_maker.Text + "'";
                    goto jump;
                }

                if (cbx_tenNVL.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where NVL='" + cbx_tenNVL.Text + "'";
                    goto jump;
                }                

                //
                if (cbx_maker.Text != "" && cbx_codeWH.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where Maker='" + cbx_maker.Text + "'And Code_WH='" + cbx_codeWH.Text + "'";
                    goto jump;
                }

                if (cbx_maker.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where Maker='" + cbx_maker.Text + "'";
                    goto jump;
                }

                if (cbx_codeWH.Text != "")
                {
                    str = "Select * From PD_NVLSpecial_Stock where Maker='" + cbx_codeWH.Text + "'";
                    goto jump;
                }   

            jump:
                DataTable dt = dtb.getData(str);
                dtb1.show_SP(dgv_stk, dt);
                
                int sum = 0;
                for (int i = 0; i < dgv_stk.RowCount - 1; i++)
                {
                    if (dgv_stk.Rows[i].Cells["So_luong"].Value.ToString() != "" && dgv_stk.Rows[i].Cells["So_luong"].Value.ToString() != null)
                    {
                        sum = sum + int.Parse(dgv_stk.Rows[i].Cells["So_luong"].Value.ToString());
                    }
                    else
                    {
                        break;
                    }
                }

                txt_qty.Text = sum.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi không thể lọc dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
