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
    public partial class StockLine2 : Form
    {
        database_1 dtb1;
        database dtb;
        ClsExcel excl = new ClsExcel();
        public string dtim = string.Empty;
        public string _strdatabase = string.Empty;

        public StockLine2(string strdatabase)
        {
            InitializeComponent();
            _strdatabase = strdatabase;
        }

        private void StockLine2_Load(object sender, EventArgs e)
        {
            dtb1 = new database_1(_strdatabase);
            dtb = new database(_strdatabase);
            this.Location = new Point(0, 0);

            DataTable dt_sl = dtb1.search_stock("KtzGiaoPd1", true);
            dtb1.show_StockLinee(dgv_stockLine, dt_sl);

            dtb.get_inf(cbx_ngaythang, "KtzGiaoPd1", "Ngay_thang");
            dtb.get_inf(cbx_maNVL, "KtzGiaoPd1", "Ma_NVL");
            dtb.get_inf(cbx_maker, "KtzGiaoPd1", "Maker");
            dtb.get_inf(cbx_mkrprt, "KtzGiaoPd1", "Maker_Part");
            dtb.get_inf(cbx_lot, "KtzGiaoPd1", "Lot");

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

        private void cbx_ngaythang_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cbx_maNVL.Items.Clear();
            //cbx_maNVL.Text = "";
            //cbx_maker.Text = "";
            //cbx_mkrprt.Text = "";
            //cbx_lot.Text = "";
            //dtb.get_inf2(cbx_maNVL, "KtzGiaoPd1", "Ma_NVL", "Ngay_thang", cbx_ngaythang.Text);
        }

        private void cbx_maNVL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cbx_maker.Items.Clear();
            //cbx_maker.Text = "";
            //cbx_mkrprt.Text = "";
            //cbx_lot.Text = "";
            //dtb.get_inf2(cbx_maker, "KtzGiaoPd1", "Maker", "Ma_NVL", cbx_maNVL.Text);
        }

        private void cbx_maker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cbx_mkrprt.Items.Clear();
            //cbx_mkrprt.Text = "";
            //dtb.get_inf2(cbx_mkrprt, "KtzGiaoPd1", "Maker_Part", "Maker", cbx_maker.Text);

            //cbx_lot.Items.Clear();
            //cbx_lot.Text = "";
            //dtb.get_inf3(cbx_lot, "KtzGiaoPd1", "Lot", "Maker", cbx_maker.Text, "Ngay_thang", cbx_ngaythang.Text);
        }

        private void btn_svStkLine_Click(object sender, EventArgs e)
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
                bool chek = excl.checkExitLog(fil_name);
                excl.exportStockKTZZ(dgv_stockLine, fil_name, chek);

                bool chek2 = excl.checkExitLog(_strdatabase + "\\tem\\" + dtim + "_StockLine.csv");
                excl.exportStockKTZZ(dgv_stockLine, _strdatabase + "\\tem\\" + dtim + "_StockLine.csv", chek2);
                MessageBox.Show("Lưu thành công!", "StockLine", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dgv_stockLine.Columns.Clear();

            string str = string.Empty;

            try
            {
                //all
                if (cbx_ngaythang.Text == "" && cbx_maNVL.Text == "" && cbx_maker.Text == "" && cbx_mkrprt.Text == "" && cbx_lot.Text == "")
                {
                    str = "Select * From KtzGiaoPd1";
                    goto jump;
                }

                //
                if (cbx_ngaythang.Text != "" && cbx_maNVL.Text != "" && cbx_maker.Text != "" && cbx_mkrprt.Text != "" && cbx_lot.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_ngaythang.Text + "' And Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "' And Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

                if (cbx_ngaythang.Text != "" && cbx_maNVL.Text != "" && cbx_maker.Text != "" && cbx_mkrprt.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_ngaythang.Text + "' And Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "'";
                    goto jump;
                }

                if (cbx_ngaythang.Text != "" && cbx_maNVL.Text != "" && cbx_maker.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_ngaythang.Text + "' And Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "'";
                    goto jump;
                }

                if (cbx_ngaythang.Text != "" && cbx_maNVL.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_ngaythang.Text + "' And Ma_NVL='" + cbx_maNVL.Text + "'";
                    goto jump;
                }

                if (cbx_ngaythang.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_ngaythang.Text + "'";
                    goto jump;
                }

                //
                if (cbx_maNVL.Text != "" && cbx_maker.Text != "" && cbx_mkrprt.Text != "" && cbx_lot.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "' And Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

                if (cbx_maNVL.Text != "" && cbx_maker.Text != "" && cbx_mkrprt.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "'";
                    goto jump;
                }

                if (cbx_maNVL.Text != "" && cbx_maker.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Ma_NVL='" + cbx_maNVL.Text + "' And Maker='" + cbx_maker.Text + "'";
                    goto jump;
                }

                if (cbx_maNVL.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Ma_NVL='" + cbx_maNVL.Text + "'";
                    goto jump;
                }

                //
                if (cbx_maker.Text != "" && cbx_mkrprt.Text != "" && cbx_lot.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "' And Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

                if (cbx_maker.Text != "" && cbx_mkrprt.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkrprt.Text + "'";
                    goto jump;
                }

                if (cbx_maker.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Maker='" + cbx_maker.Text + "'";
                    goto jump;
                }

                //
                if (cbx_mkrprt.Text != "" && cbx_lot.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Maker_Part='" + cbx_mkrprt.Text + "' And Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

                if (cbx_mkrprt.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Maker_Part='" + cbx_mkrprt.Text + "'";
                    goto jump;
                }

                //
                if (cbx_lot.Text != "")
                {
                    str = "Select * From KtzGiaoPd1 where Lot='" + cbx_lot.Text + "'";
                    goto jump;
                }

            jump:
                DataTable dt = dtb.getData(str);
                dtb1.show_StockLinee(dgv_stockLine, dt);

                int sum = 0;
                for (int i = 0; i < dgv_stockLine.RowCount - 1; i++)
                {
                    if (dgv_stockLine.Rows[i].Cells["So_luong_cap"].Value.ToString() != "" && dgv_stockLine.Rows[i].Cells["So_luong_cap"].Value.ToString() != null)
                    {
                        sum = sum + int.Parse(dgv_stockLine.Rows[i].Cells["So_luong_cap"].Value.ToString());
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
                MessageBox.Show("Xảy ra lỗi không thể lọc dữ liệu!", "StockLine", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cbx_ngaythang.Text = "";
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
    }
}
