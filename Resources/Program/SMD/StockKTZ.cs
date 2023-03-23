using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ManageMaterialPBA
{
    public partial class StockKTZ : Form
    {
        Barcode frm;
        database_1 dtb1;
        database dtb;
        ClsExcel ex = new ClsExcel();
        public string dtim = string.Empty;
        public string _strdatabase = string.Empty;
        public DataTable dt_common;

        public StockKTZ(Barcode _frm, string strdatabase)
        {
            InitializeComponent();
            frm = _frm;
            _strdatabase = strdatabase;
        }

        private void StockKTZ_Load(object sender, EventArgs e)
        {
            dtb1 = new database_1(_strdatabase);
            dtb = new database(_strdatabase);
            this.Location = new Point(0, 0);

            dt_common = dtb1.get_StockKTZ("Stock_KTZ");
            dtb1.show_StockKTZZ(dgv_stkKTZ, dt_common, "Lot", "So_luong");
            
            dtb.get_inf(cbx_tgian, "Stock_KTZ", "Thoi_gian");
            dtb.get_inf(cbx_maNVL, "Stock_KTZ", "Ma_NVL");
            dtb.get_inf(cbx_maker, "Stock_KTZ", "Maker");
            dtb.get_inf(cbx_mkrprt, "Stock_KTZ", "Maker_Part");
            dtb.get_inf(cbx_lot, "Stock_KTZ", "Lot");

            dtim = getYearMonthDay();
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
                ex.exportStockKTZZ(dgv_stkKTZ, fil_name, chek);

                bool chek2 = ex.checkExitLog(_strdatabase + "\\tem\\" + dtim + "_StockKTZ.csv");
                ex.exportStockKTZZ(dgv_stkKTZ, _strdatabase + "\\tem\\" + dtim + "_StockKTZ.csv", chek2);
                MessageBox.Show("Lưu thành công!", "StockKTZ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }     
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

        private void cbx_tgian_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cbx_maNVL.Items.Clear();
            //cbx_maNVL.Text = "";
            //cbx_maker.Text = "";
            //cbx_mkrprt.Text = "";
            //cbx_lot.Text = "";
            //dtb.get_inf2(cbx_maNVL, "Stock_KTZ", "Ma_NVL", "Thoi_gian", cbx_tgian.Text);
        }

        private void cbx_maNVL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cbx_maker.Items.Clear();
            //cbx_maker.Text = "";
            //cbx_mkrprt.Text = "";
            //cbx_lot.Text = "";
            //dtb.get_inf2(cbx_maker, "Stock_KTZ", "Maker", "Ma_NVL", cbx_maNVL.Text);
        }

        private void cbx_maker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cbx_mkrprt.Items.Clear();
            //cbx_mkrprt.Text = "";
            //dtb.get_inf2(cbx_mkrprt, "Stock_KTZ", "Maker_Part", "Maker", cbx_maker.Text);

            //cbx_lot.Items.Clear();
            //cbx_lot.Text = "";
            //dtb.get_inf3(cbx_lot, "Stock_KTZ", "Lot", "Maker", cbx_maker.Text, "Thoi_gian", cbx_tgian.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dgv_stkKTZ.Columns.Clear();

            string str = string.Empty;

            try
            {
                //all
                if (cbx_tgian.Text == "" && cbx_maNVL.Text == "" && cbx_maker.Text == "" && cbx_mkrprt.Text == "" && cbx_lot.Text == "")
                {
                    str = "Select * From Stock_KTZ";
                    goto jump;
                }

                //
                if (cbx_tgian.Text != "" && cbx_maNVL.Text != "" && cbx_maker.Text != "" && cbx_mkrprt.Text != "" && cbx_lot.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgian.Text + "' And Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "' And Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

                if (cbx_tgian.Text != "" && cbx_maNVL.Text != "" && cbx_maker.Text != "" && cbx_mkrprt.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgian.Text + "' And Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "'";
                    goto jump;
                }

                if (cbx_tgian.Text != "" && cbx_maNVL.Text != "" && cbx_maker.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgian.Text + "' And Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "'";
                    goto jump;
                }

                if (cbx_tgian.Text != "" && cbx_maNVL.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgian.Text + "' And Ma_NVL='" + cbx_maNVL.Text + "'";
                    goto jump;
                }

                if (cbx_tgian.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgian.Text + "'";
                    goto jump;
                }

                //
                if (cbx_maNVL.Text != "" && cbx_maker.Text != "" && cbx_mkrprt.Text != "" && cbx_lot.Text != "")
                {
                    str = "Select * From Stock_KTZ where Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "' And Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

                if (cbx_maNVL.Text != "" && cbx_maker.Text != "" && cbx_mkrprt.Text != "")
                {
                    str = "Select * From Stock_KTZ where Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "'";
                    goto jump;
                }

                if (cbx_maNVL.Text != "" && cbx_maker.Text != "")
                {
                    str = "Select * From Stock_KTZ where Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "'";
                    goto jump;
                }

                if (cbx_maNVL.Text != "")
                {
                    str = "Select * From Stock_KTZ where Ma_NVL='" + cbx_maNVL.Text + "'";
                    goto jump;
                }

                //
                if (cbx_maker.Text != "" && cbx_mkrprt.Text != "" && cbx_lot.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "' And Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

                if (cbx_maker.Text != "" && cbx_mkrprt.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "'";
                    goto jump;
                }

                if (cbx_maker.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker='" + cbx_maker.Text + "'";
                    goto jump;
                }

                //
                if (cbx_mkrprt.Text != "" && cbx_lot.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker_Part='" + cbx_mkrprt.Text + "' And Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

                if (cbx_mkrprt.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker_Part='" + cbx_mkrprt.Text + "'";
                    goto jump;
                }

                //
                if (cbx_lot.Text != "")
                {
                    str = "Select * From Stock_KTZ where Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

                jump:
                DataTable dt = dtb.getData(str);                
                dtb1.show_StockKTZZ(dgv_stkKTZ, dt, "Lot", "So_luong");
                int sum = 0;
                for (int i = 0; i < dgv_stkKTZ.RowCount - 1; i++)
                {
                    if (dgv_stkKTZ.Rows[i].Cells["So_luong"].Value.ToString() != "" && dgv_stkKTZ.Rows[i].Cells["So_luong"].Value.ToString() != null)
                    {
                        sum = sum + int.Parse(dgv_stkKTZ.Rows[i].Cells["So_luong"].Value.ToString());
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
                MessageBox.Show("Xảy ra lỗi không thể lọc dữ liệu!", "StockKTZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cbx_tgian.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cbx_maNVL.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cbx_maker.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cbx_mkrprt.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cbx_lot.Text = "";
        }

        private void dgv_stkKTZ_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string[] str = dgv_stkKTZ.CurrentRow.Cells["Thoi_gian"].Value.ToString().Split('-');
                if(str[2].Length == 1)
                {
                    str[2] = "0" + str[2];
                }
                StreamReader sr = new StreamReader(_strdatabase + "\\Log\\Duplicate\\Old\\" + str[0] + str[1] + str[2] + "_NewCode.log");
                FileStream fs = new FileStream(_strdatabase + "\\Print\\KTZ\\" + str[0] + str[1] + str[2] + "_NewCode.log", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                string code = dgv_stkKTZ.CurrentRow.Cells["Ma_NVL"].Value.ToString();
                string mkp = dgv_stkKTZ.CurrentRow.Cells["Maker_Part"].Value.ToString();
                string lot = dgv_stkKTZ.CurrentRow.Cells["Lot"].Value.ToString();
                while (sr.EndOfStream == false)
                {
                    string srL = sr.ReadLine();
                    if (srL.Contains(code) && srL.Contains(mkp) && srL.Contains(lot))
                    {
                        sw.WriteLine(srL);
                    }
                }
                sw.Close();
                fs.Close();
                sr.Close();
                System.Threading.Thread.Sleep(500);
                frm.cfrm = true;
                frm.dP = str[0] + str[1] + str[2];
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi!", "StockKTZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                              
        }
    }
}
