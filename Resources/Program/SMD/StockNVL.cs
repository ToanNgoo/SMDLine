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
using System.Data.OleDb;

namespace ManageMaterialPBA
{
    public partial class StockNVL : Form
    {
        database_1 dtb1;
        database dtb;
        ClsExcel ex = new ClsExcel();
        public string _strdatabase = string.Empty;
        public DataTable dtKtz, dtLine, dtPDxn;

        public StockNVL(string strdatabase)
        {
            InitializeComponent();
            _strdatabase = strdatabase;
        }        

        private void StockNVL_Load(object sender, EventArgs e)
        {
            dtb1 = new database_1(_strdatabase);
            dtb = new database(_strdatabase);
            this.Location = new Point(0, 0);

            txt_qtKtz.Enabled = false;
            txt_qtLine.Enabled = false;
            txt_qtyPDxn.Enabled = false;

            //Stock Ktz
            dtKtz = dtb1.get_StockKTZ("Stock_KTZ");
            dtb1.show_StockKTZZ2(dgv_stkKtz, dtKtz, "Lot", "So_luong");

            dtb.get_inf(cbx_tgKtz, "Stock_KTZ", "Thoi_gian");
            dtb.get_inf(cbx_mlKtz, "Stock_KTZ", "Ma_NVL");
            dtb.get_inf(cbx_mkrKtz, "Stock_KTZ", "Maker");
            dtb.get_inf(cbx_mkpKtz, "Stock_KTZ", "Maker_Part");
            dtb.get_inf(cbx_ltKtz, "Stock_KTZ", "Lot");

            //Stock Line
            dtLine = dtb1.search_stock("KtzGiaoPd1", true);
            dtb1.show_StockLinee(dgv_stkLine, dtLine);
            dgv_stkLine.Columns["KTZ"].Visible = false;
            dgv_stkLine.Columns["PD"].Visible = false;

            dtb.get_inf(cbx_tgLine, "KtzGiaoPd1", "Ngay_thang");
            dtb.get_inf(cbx_mlLine, "KtzGiaoPd1", "Ma_NVL");
            dtb.get_inf(cbx_mkrLine, "KtzGiaoPd1", "Maker");
            dtb.get_inf(cbx_mkpLine, "KtzGiaoPd1", "Maker_Part");
            dtb.get_inf(cbx_ltLine, "KtzGiaoPd1", "Lot");

            //PD xac nhan
            dtPDxn = dtb1.search_stock("PDxacnhanStock_1", true);
            dtb1.show_StockLinee(dgv_PDxnStk, dtPDxn);
            dgv_PDxnStk.Columns["KTZ"].Visible = false;
            dgv_PDxnStk.Columns["PD"].Visible = false;

            dtb.get_inf(cbx_tgPDxn, "PDxacnhanStock_1", "Ngay_thang");
            dtb.get_inf(cbx_mlPDxn, "PDxacnhanStock_1", "Ma_NVL");
            dtb.get_inf(cbx_mkPDxn, "PDxacnhanStock_1", "Maker");
            dtb.get_inf(cbx_mkpPDxn, "PDxacnhanStock_1", "Maker_Part");
            dtb.get_inf(cbx_lotPDxn, "PDxacnhanStock_1", "Lot");
        }

        //=============================Stock KTZ============================================
        private void btn_fterKtz_Click(object sender, EventArgs e)
        {
            dgv_stkKtz.Columns.Clear();

            string str = string.Empty;

            try
            {
                #region
                //all
                if (cbx_tgKtz.Text == "" && cbx_mlKtz.Text == "" && cbx_mkrKtz.Text == "" && cbx_mkpKtz.Text == "" && cbx_ltKtz.Text == "")
                {
                    str = "Select * From Stock_KTZ";
                    goto jump;
                }

                //
                if (cbx_tgKtz.Text != "" && cbx_mlKtz.Text != "" && cbx_mkrKtz.Text != "" && cbx_mkpKtz.Text != "" && cbx_ltKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgKtz.Text + "' And Ma_NVL='" + cbx_mlKtz.Text + "' And Maker='" + cbx_mkrKtz.Text + "' And Maker_Part='" + cbx_mkpKtz.Text + "' And Lot='" + cbx_ltKtz.Text + "'";
                    goto jump;
                }

                if (cbx_tgKtz.Text != "" && cbx_mlKtz.Text != "" && cbx_mkrKtz.Text != "" && cbx_mkpKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgKtz.Text + "' And Ma_NVL='" + cbx_mlKtz.Text + "' And Maker='" + cbx_mkrKtz.Text + "' And Maker_Part='" + cbx_mkpKtz.Text + "'";
                    goto jump;
                }

                if (cbx_tgKtz.Text != "" && cbx_mlKtz.Text != "" && cbx_mkrKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgKtz.Text + "' And Ma_NVL='" + cbx_mlKtz.Text + "' And Maker='" + cbx_mkrKtz.Text + "'";
                    goto jump;
                }

                if (cbx_tgKtz.Text != "" && cbx_mlKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgKtz.Text + "' And Ma_NVL='" + cbx_mlKtz.Text + "'";
                    goto jump;
                }

                if (cbx_tgKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Thoi_gian='" + cbx_tgKtz.Text + "'";
                    goto jump;
                }

                //
                if (cbx_mlKtz.Text != "" && cbx_mkrKtz.Text != "" && cbx_mkpKtz.Text != "" && cbx_ltKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Ma_NVL='" + cbx_mlKtz.Text + "' And Maker='" + cbx_mkrKtz.Text + "' And Maker_Part='" + cbx_mkpKtz.Text + "' And Lot='" + cbx_ltKtz.Text + "'";
                    goto jump;
                }

                if (cbx_mlKtz.Text != "" && cbx_mkrKtz.Text != "" && cbx_mkpKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Ma_NVL='" + cbx_mlKtz.Text + "' And Maker='" + cbx_mkrKtz.Text + "' And Maker_Part='" + cbx_mkpKtz.Text + "'";
                    goto jump;
                }

                if (cbx_mlKtz.Text != "" && cbx_mkrKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Ma_NVL='" + cbx_mlKtz.Text + "' And Maker='" + cbx_mkrKtz.Text + "'";
                    goto jump;
                }

                if (cbx_mlKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Ma_NVL='" + cbx_mlKtz.Text + "'";
                    goto jump;
                }

                //
                if (cbx_mkrKtz.Text != "" && cbx_mkpKtz.Text != "" && cbx_ltKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker='" + cbx_mkrKtz.Text + "' And Maker_Part='" + cbx_mkpKtz.Text + "' And Lot='" + cbx_ltKtz.Text + "'";
                    goto jump;
                }

                if (cbx_mkrKtz.Text != "" && cbx_mkpKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker='" + cbx_mkrKtz.Text + "' And Maker_Part='" + cbx_mkpKtz.Text + "'";
                    goto jump;
                }

                if (cbx_mkrKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker='" + cbx_mkrKtz.Text + "'";
                    goto jump;
                }

                //
                if (cbx_mkpKtz.Text != "" && cbx_ltKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker_Part='" + cbx_mkpKtz.Text + "' And Lot='" + cbx_ltKtz.Text + "'";
                    goto jump;
                }

                if (cbx_mkpKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Maker_Part='" + cbx_mkpKtz.Text + "'";
                    goto jump;
                }

                //
                if (cbx_ltKtz.Text != "")
                {
                    str = "Select * From Stock_KTZ where Lot='" + cbx_ltKtz.Text + "'";
                    goto jump;
                }
                #endregion

            jump:
                DataTable dt = dtb.getData(str);
                dtb1.show_StockKTZZ2(dgv_stkKtz, dt, "Lot", "So_luong");
                int sum = 0;
                for (int i = 0; i < dgv_stkKtz.RowCount - 1; i++)
                {
                    if (dgv_stkKtz.Rows[i].Cells["So_luong"].Value.ToString() != "" && dgv_stkKtz.Rows[i].Cells["So_luong"].Value.ToString() != null)
                    {
                        sum = sum + int.Parse(dgv_stkKtz.Rows[i].Cells["So_luong"].Value.ToString());
                    }
                    else
                    {
                        break;
                    }
                }

                txt_qtKtz.Text = sum.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi không thể lọc dữ liệu!", "StockKTZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_svKtz_Click(object sender, EventArgs e)
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
                ex.exportStockKTZZ(dgv_stkKtz, fil_name, chek);                
                MessageBox.Show("Lưu thành công!", "StockKTZ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }

        private void btn_cltgKtz_Click(object sender, EventArgs e)
        {
            cbx_tgKtz.Text = "";
        }

        private void btn_clmlKtz_Click(object sender, EventArgs e)
        {
            cbx_mlKtz.Text = "";
        }

        private void btn_clmkrKtz_Click(object sender, EventArgs e)
        {
            cbx_mkrKtz.Text = "";
        }

        private void btn_clmkpKtz_Click(object sender, EventArgs e)
        {
            cbx_mkpKtz.Text = "";
        }

        private void btn_clltKtz_Click(object sender, EventArgs e)
        {
            cbx_ltKtz.Text = "";
        }

        //=============================Stock Line============================================
        private void btn_fterLine_Click(object sender, EventArgs e)
        {
            dgv_stkLine.Columns.Clear();

            string str = string.Empty;

            try
            {
                if(chb_tieuhao.Checked == true)
                {
                    #region
                    //all
                    if (cbx_tgLine.Text == "" && cbx_mlLine.Text == "" && cbx_mkrLine.Text == "" && cbx_mkpLine.Text == "" && cbx_ltLine.Text == "")
                    {
                        str = "Select * From StockTieuHao";
                        goto jump;
                    }

                    //
                    if (cbx_tgLine.Text != "" && cbx_mlLine.Text != "" && cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "" && cbx_ltLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Ngay_thang='" + cbx_tgLine.Text + "' And Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "' And Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_tgLine.Text != "" && cbx_mlLine.Text != "" && cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Ngay_thang='" + cbx_tgLine.Text + "' And Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_tgLine.Text != "" && cbx_mlLine.Text != "" && cbx_mkrLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Ngay_thang='" + cbx_tgLine.Text + "' And Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_tgLine.Text != "" && cbx_mlLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Ngay_thang='" + cbx_tgLine.Text + "' And Ma_NVL='" + cbx_mlLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_tgLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Ngay_thang='" + cbx_tgLine.Text + "'";
                        goto jump;
                    }

                    //
                    if (cbx_mlLine.Text != "" && cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "" && cbx_ltLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "' And Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mlLine.Text != "" && cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mlLine.Text != "" && cbx_mkrLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mlLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Ma_NVL='" + cbx_mlLine.Text + "'";
                        goto jump;
                    }

                    //
                    if (cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "" && cbx_ltLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "' And Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mkrLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Maker='" + cbx_mkrLine.Text + "'";
                        goto jump;
                    }

                    //
                    if (cbx_mkpLine.Text != "" && cbx_ltLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Maker_Part='" + cbx_mkpLine.Text + "' And Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mkpLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Maker_Part='" + cbx_mkpLine.Text + "'";
                        goto jump;
                    }

                    //
                    if (cbx_ltLine.Text != "")
                    {
                        str = "Select * From StockTieuHao where Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }
                    #endregion  
                }
                else
                {
                    #region
                    //all
                    if (cbx_tgLine.Text == "" && cbx_mlLine.Text == "" && cbx_mkrLine.Text == "" && cbx_mkpLine.Text == "" && cbx_ltLine.Text == "")
                    {
                        str = "Select * From KtzGiaoPd1";
                        goto jump;
                    }

                    //
                    if (cbx_tgLine.Text != "" && cbx_mlLine.Text != "" && cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "" && cbx_ltLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_tgLine.Text + "' And Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "' And Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_tgLine.Text != "" && cbx_mlLine.Text != "" && cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_tgLine.Text + "' And Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_tgLine.Text != "" && cbx_mlLine.Text != "" && cbx_mkrLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_tgLine.Text + "' And Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_tgLine.Text != "" && cbx_mlLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_tgLine.Text + "' And Ma_NVL='" + cbx_mlLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_tgLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Ngay_thang='" + cbx_tgLine.Text + "'";
                        goto jump;
                    }

                    //
                    if (cbx_mlLine.Text != "" && cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "" && cbx_ltLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "' And Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mlLine.Text != "" && cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mlLine.Text != "" && cbx_mkrLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Ma_NVL='" + cbx_mlLine.Text + "' And Maker='" + cbx_mkrLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mlLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Ma_NVL='" + cbx_mlLine.Text + "'";
                        goto jump;
                    }

                    //
                    if (cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "" && cbx_ltLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "' And Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mkrLine.Text != "" && cbx_mkpLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Maker='" + cbx_mkrLine.Text + "' And Maker_Part='" + cbx_mkpLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mkrLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Maker='" + cbx_mkrLine.Text + "'";
                        goto jump;
                    }

                    //
                    if (cbx_mkpLine.Text != "" && cbx_ltLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Maker_Part='" + cbx_mkpLine.Text + "' And Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }

                    if (cbx_mkpLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Maker_Part='" + cbx_mkpLine.Text + "'";
                        goto jump;
                    }

                    //
                    if (cbx_ltLine.Text != "")
                    {
                        str = "Select * From KtzGiaoPd1 where Lot='" + cbx_ltLine.Text + "'";
                        goto jump;
                    }
                    #endregion
                }                

            jump:
                DataTable dt = dtb.getData(str);
                dtb1.show_StockLinee(dgv_stkLine, dt);

                int sum = 0;
                for (int i = 0; i < dgv_stkLine.RowCount - 1; i++)
                {
                    if (dgv_stkLine.Rows[i].Cells["So_luong_cap"].Value.ToString() != "" && dgv_stkLine.Rows[i].Cells["So_luong_cap"].Value.ToString() != null)
                    {
                        sum = sum + int.Parse(dgv_stkLine.Rows[i].Cells["So_luong_cap"].Value.ToString());
                    }
                    else
                    {
                        break;
                    }
                }

                txt_qtLine.Text = sum.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi không thể lọc dữ liệu!", "StockLine", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_chkLine_Click(object sender, EventArgs e)
        {
            if(txt_poLine.Text != "")
            {
                int rel = 0;
                if(int.TryParse(txt_poLine.Text, out rel) == true)
                {
                    try
                    {
                        dgv_stkLine.Columns.Clear();
                        int qtyPO = int.Parse(txt_poLine.Text);
                        DataTable dtTieuHao = TinhTieuHao(qtyPO);
                        dtb1.show_StockTieuHao(dgv_stkLine, dtTieuHao);
                        dgv_stkLine.Columns["KTZ"].Visible = false;
                        dgv_stkLine.Columns["PD"].Visible = false;

                        if (arr_NVLLack[0] != null)
                        {
                            string toDisply = string.Join("\n", arr_NVLLack);
                            MessageBox.Show("NVL bị thiếu :\n" + toDisply, "StockLine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Xảy ra lỗi khi kiểm tra tiêu hao!", "StockLine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                    
                }
                else
                {
                    MessageBox.Show("Hãy điền sản lượng là số để tính tiêu hao NVL!", "StockLine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa điền sản lượng nên không thể tính tiêu hao NVL!", "StockLine", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_svLine_Click(object sender, EventArgs e)
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
                ex.exportStockKTZZ(dgv_stkLine, fil_name, chek);              
                MessageBox.Show("Lưu thành công!", "StockLine", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_cltgLine_Click(object sender, EventArgs e)
        {
            cbx_tgLine.Text = "";
        }

        private void btn_clmlLine_Click(object sender, EventArgs e)
        {
            cbx_mlLine.Text = "";
        }

        private void btn_clmkrLine_Click(object sender, EventArgs e)
        {
            cbx_mkrLine.Text = "";
        }

        private void btn_clmkpLine_Click(object sender, EventArgs e)
        {
            cbx_mkpLine.Text = "";
        }

        private void btn_clltLine_Click(object sender, EventArgs e)
        {
            cbx_ltLine.Text = "";
        }

        public string[] arr_NVLLack = new string[]{""};
        public DataTable TinhTieuHao(int slg)
        {
            dtb1.delete_Transport("StockTieuHao");
            DataTable dt_code = dtb1.getData("Select distinct Model, Ma_NVL From KtzGiaoPd1");
            arr_NVLLack = new string[dt_code.Rows.Count];
            int count_NVlLack = 0;
            foreach(DataRow dtr_code in dt_code.Rows)
            {       
                DataTable dt_dgan = dtb1.getData("Select distinct Ma_NVL, Diem_gan From All_model1 Where Model='" + dtr_code[0].ToString() + "' and Ma_NVL ='" + dtr_code[1].ToString() + "'");
                foreach(DataRow dtr_dgan in dt_dgan.Rows)
                {
                    int sanLg_code = slg * int.Parse(dtr_dgan["Diem_gan"].ToString());
                    //kay so luong/cuon theo BOM
                    DataTable dtStdQty = dtb1.getData("Select distinct So_luong From All_model1 Where Ma_NVL='" + dtr_dgan["Ma_NVL"].ToString() + "'");
                    //lay all thong tin code trong KtzGiaoPd1
                    DataTable dt_Getcode = dtb1.getData("Select * From KtzGiaoPd1 Where Ma_NVL='" + dtr_dgan["Ma_NVL"].ToString() + "'");
                    //Gan date temcode vao mang 1 chieu
                    string[] arr_date = new string[dt_Getcode.Rows.Count];
                    int i = 0;
                    foreach (DataRow dtr1 in dt_Getcode.Rows)
                    {
                        string[] str_arr = dtr1["Tem_code"].ToString().Split('+');
                        arr_date[i] = str_arr[1].Substring(0, 8);
                        i++;
                    }
                    //sap xep date temcode min -> max
                    for (int n = 0; n < arr_date.Length - 1; n++)
                    {
                        for (int m = n + 1; m < arr_date.Length; m++)
                        {
                            if (DateTime.Compare(Convert.ToDateTime(arr_date[n]), Convert.ToDateTime(arr_date[m])) > 0)
                            {
                                string tg = string.Empty;
                                tg = arr_date[n];
                                arr_date[n] = arr_date[m];
                                arr_date[m] = tg;
                            }
                        }
                    }
                    //Loai bo cuon NVL theo fifo de tinh tieu hao
                    int j = 0;
                jump:
                    if (sanLg_code >= int.Parse(dtStdQty.Rows[0]["So_luong"].ToString()))
                    {
                        if (dt_Getcode.Rows.Count > 0)
                        {
                            //xoa ca dog trong KtzGiaoPd1
                            foreach (DataRow dtr1 in dt_Getcode.Rows)
                            {
                                if (dtr1["Tem_code"].ToString().Contains(arr_date[j]))
                                {
                                    sanLg_code = sanLg_code - int.Parse(dtr1["So_luong_cap"].ToString());
                                    dt_Getcode.Rows.Remove(dtr1);
                                    break;
                                }
                            }
                            j++;
                            goto jump;
                        }
                        else
                        {
                            //NVL bi thieu so vs san luong PO
                            arr_NVLLack[count_NVlLack] = dtr_dgan["Ma_NVL"].ToString() + " : " + sanLg_code.ToString() + "EA";
                            count_NVlLack++;
                        }
                    }
                    else
                    {
                        //update so luong
                        foreach (DataRow dtr1 in dt_Getcode.Rows)
                        {
                            if (dtr1["Tem_code"].ToString().Contains(arr_date[j]))
                            {
                                dtr1["So_luong_cap"] = (int.Parse(dtr1["So_luong_cap"].ToString()) - sanLg_code).ToString();
                                break;
                            }
                        }
                    }
                    //luu database           
                    OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + _strdatabase + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                    cnn.Open();
                    if (dt_Getcode.Rows.Count > 0)
                    {
                        foreach (DataRow dtr1 in dt_Getcode.Rows)
                        {
                            string strIn = "Insert Into StockTieuHao Values ('" + dtr1["Ngay_thang"].ToString() + "','" +
                                                                                  dtr1["Ca_kip"].ToString() + "','" +
                                                                                  dtr1["Line"].ToString() + "','" +
                                                                                  dtr1["Model"].ToString() + "','" +
                                                                                  dtr1["Mo_ta"].ToString() + "','" +
                                                                                  dtr1["Ma_NVL"].ToString() + "','" +
                                                                                  dtr1["Maker"].ToString() + "','" +
                                                                                  dtr1["Maker_Part"].ToString() + "','" +
                                                                                  dtr_dgan["Diem_gan"].ToString() + "','" +
                                                                                  dtr1["Lot"].ToString() + "','" +
                                                                                  dtr1["So_luong_cap"].ToString() + "','" +
                                                                                  dtr1["Tem_code"].ToString() + "','" +
                                                                                  dtr1["KTZ"].ToString() + "','" +
                                                                                  dtr1["PD"].ToString() + "')";
                            OleDbCommand cmd = new OleDbCommand(strIn, cnn);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    cnn.Close();           
                }                
            }
            //select all
            DataTable dt_rel = dtb1.getData("Select * From StockTieuHao");            
            return dt_rel;
        }

        //=============================Stock PDxn============================================
        private void btn_txPDxn_Click(object sender, EventArgs e)
        {
            dgv_PDxnStk.Columns.Clear();

            string str = string.Empty;

            try
            {
                #region
                //all
                if (cbx_tgPDxn.Text == "" && cbx_mlPDxn.Text == "" && cbx_mkPDxn.Text == "" && cbx_mkpPDxn.Text == "" && cbx_lotPDxn.Text == "")
                {
                    str = "Select * From PDxacnhanStock_1";
                    goto jump;
                }

                //
                if (cbx_tgPDxn.Text != "" && cbx_mlPDxn.Text != "" && cbx_mkPDxn.Text != "" && cbx_mkpPDxn.Text != "" && cbx_lotPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Ngay_thang='" + cbx_tgPDxn.Text + "' And Ma_NVL='" + cbx_mlPDxn.Text + "' And Maker='" + cbx_mkPDxn.Text + "' And Maker_Part='" + cbx_mkpPDxn.Text + "' And Lot='" + cbx_lotPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_tgPDxn.Text != "" && cbx_mlPDxn.Text != "" && cbx_mkPDxn.Text != "" && cbx_mkpPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Ngay_thang='" + cbx_tgPDxn.Text + "' And Ma_NVL='" + cbx_mlPDxn.Text + "' And Maker='" + cbx_mkPDxn.Text + "' And Maker_Part='" + cbx_mkpPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_tgPDxn.Text != "" && cbx_mlPDxn.Text != "" && cbx_mkPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Ngay_thang='" + cbx_tgPDxn.Text + "' And Ma_NVL='" + cbx_mlPDxn.Text + "' And Maker='" + cbx_mkPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_tgPDxn.Text != "" && cbx_mlPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Ngay_thang='" + cbx_tgPDxn.Text + "' And Ma_NVL='" + cbx_mlPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_tgPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Ngay_thang='" + cbx_tgPDxn.Text + "'";
                    goto jump;
                }

                //
                if (cbx_mlPDxn.Text != "" && cbx_mkPDxn.Text != "" && cbx_mkpPDxn.Text != "" && cbx_lotPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Ma_NVL='" + cbx_mlPDxn.Text + "' And Maker='" + cbx_mkPDxn.Text + "' And Maker_Part='" + cbx_mkpPDxn.Text + "' And Lot='" + cbx_lotPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_mlPDxn.Text != "" && cbx_mkPDxn.Text != "" && cbx_mkpPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Ma_NVL='" + cbx_mlPDxn.Text + "' And Maker='" + cbx_mkPDxn.Text + "' And Maker_Part='" + cbx_mkpPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_mlPDxn.Text != "" && cbx_mkPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Ma_NVL='" + cbx_mlPDxn.Text + "' And Maker='" + cbx_mkPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_mlPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Ma_NVL='" + cbx_mlPDxn.Text + "'";
                    goto jump;
                }

                //
                if (cbx_mkPDxn.Text != "" && cbx_mkpPDxn.Text != "" && cbx_lotPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Maker='" + cbx_mkPDxn.Text + "' And Maker_Part='" + cbx_mkpPDxn.Text + "' And Lot='" + cbx_lotPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_mkPDxn.Text != "" && cbx_mkpPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Maker='" + cbx_mkPDxn.Text + "' And Maker_Part='" + cbx_mkpPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_mkPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Maker='" + cbx_mkPDxn.Text + "'";
                    goto jump;
                }

                //
                if (cbx_mkpPDxn.Text != "" && cbx_lotPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Maker_Part='" + cbx_mkpPDxn.Text + "' And Lot='" + cbx_lotPDxn.Text + "'";
                    goto jump;
                }

                if (cbx_mkpPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Maker_Part='" + cbx_mkpPDxn.Text + "'";
                    goto jump;
                }

                //
                if (cbx_lotPDxn.Text != "")
                {
                    str = "Select * From PDxacnhanStock_1 where Lot='" + cbx_lotPDxn.Text + "'";
                    goto jump;
                }
                #endregion

            jump:
                DataTable dt = dtb.getData(str);
                dtb1.show_StockLinee(dgv_PDxnStk, dt);
                int sum = 0;
                for (int i = 0; i < dgv_PDxnStk.RowCount - 1; i++)
                {
                    if (dgv_PDxnStk.Rows[i].Cells["So_luong_cap"].Value.ToString() != "" && dgv_PDxnStk.Rows[i].Cells["So_luong_cap"].Value.ToString() != null)
                    {
                        sum = sum + int.Parse(dgv_PDxnStk.Rows[i].Cells["So_luong_cap"].Value.ToString());
                    }
                    else
                    {
                        break;
                    }
                }

                txt_qtyPDxn.Text = sum.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi không thể lọc dữ liệu!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_svPDxn_Click(object sender, EventArgs e)
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
                ex.exportStockKTZZ(dgv_PDxnStk, fil_name, chek);
                MessageBox.Show("Lưu thành công!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cbx_tgPDxn.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cbx_mlPDxn.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cbx_mkPDxn.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cbx_mkpPDxn.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cbx_lotPDxn.Text = "";
        }        
    }
}
