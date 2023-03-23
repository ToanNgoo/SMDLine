using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace ManageMaterialPBA
{
    public partial class TraceHistory : Form
    {
        database dtb;
        database_1 dtb1;
        ClsExcel ex = new ClsExcel();
        public DataTable dt;
        public string _strdatabase = string.Empty;

        public TraceHistory(string strdatabase)
        {
            InitializeComponent();
            _strdatabase = strdatabase;
        }

        private void TraceHistory_Load(object sender, EventArgs e)
        {
            dtb = new database(_strdatabase);
            dtb1 = new database_1(_strdatabase);
            this.Location = new Point(0, 0);
            //tab BOM
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            cbx_period.Items.Add("Hôm nay");
            cbx_period.Items.Add("Hôm qua");
            cbx_period.Items.Add("Tuần trước");
            cbx_period.Items.Add("Tháng trước");
            cbx_period.Items.Add("Tùy chọn");
            cbx_period.Items.Add("Tất cả");
            cbx_period.Text = "Hôm nay";
            //tab Print
            dateTimePicker3.Enabled = false;
            dateTimePicker4.Enabled = false;
            cbx_perPrint.Items.Add("Hôm nay");
            cbx_perPrint.Items.Add("Hôm qua");
            cbx_perPrint.Items.Add("Tuần trước");
            cbx_perPrint.Items.Add("Tháng trước");
            cbx_perPrint.Items.Add("Tùy chọn");
            cbx_perPrint.Items.Add("Tất cả");
            cbx_perPrint.Text = "Hôm nay";                     
        }

        private void cbx_period_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbx_period.Text)
            {
                case "Hôm nay":
                    dateTimePicker1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    dateTimePicker1.Value = DateTime.Now;
                    dateTimePicker2.Value = DateTime.Now;
                    break;
                case "Hôm qua":
                    dateTimePicker1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    dateTimePicker1.Value = DateTime.Now.AddDays(-1.0);
                    dateTimePicker2.Value = DateTime.Now.AddDays(-1.0);
                    break;
                case "Tuần trước":
                    dateTimePicker1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    dateTimePicker1.Value = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek + 6));
                    dateTimePicker2.Value = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    break;
                case "Tháng trước":
                    dateTimePicker1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
                    dateTimePicker2.Value = dateTimePicker1.Value.AddMonths(1).AddDays(-1);
                    break;
                case "Tùy chọn":
                    dateTimePicker1.Enabled = true;
                    dateTimePicker2.Enabled = true;
                    break;
                default:
                    dateTimePicker1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    break;
            }           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dgv_BOMhistory.Columns.Clear();

            if(DateTime.Compare(dateTimePicker1.Value, dateTimePicker2.Value) > 0)
            {
                MessageBox.Show("Khoảng thời gian bạn chọn không phù hợp\nHãy kiểm tra lại!", "TraceHistory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if(cbx_period.Text == "Tất cả")
                {
                    string str = "select * from HistoryBOM'";
                    DataTable dt = dtb.getData(str);
                    dtb.ShowupBOMHistory(dgv_BOMhistory, dt);
                }
                else
                {
                    string str = "select * from HistoryBOM Where Ngay_Thang Between '" + dateTimePicker1.Value.ToShortDateString() + "' And '" + dateTimePicker2.Value.ToShortDateString() + "'";
                    DataTable dt = dtb.getData(str);
                    dtb.ShowupBOMHistory(dgv_BOMhistory, dt);
                }               
            }
        }

        private void cbx_perPrint_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbx_perPrint.Text)
            {
                case "Hôm nay":
                    dateTimePicker3.Enabled = false;
                    dateTimePicker4.Enabled = false;
                    dateTimePicker3.Value = DateTime.Now;
                    dateTimePicker4.Value = DateTime.Now;
                    break;
                case "Hôm qua":
                    dateTimePicker3.Enabled = false;
                    dateTimePicker4.Enabled = false;
                    dateTimePicker3.Value = DateTime.Now.AddDays(-1.0);
                    dateTimePicker4.Value = DateTime.Now.AddDays(-1.0);
                    break;
                case "Tuần trước":
                    dateTimePicker3.Enabled = false;
                    dateTimePicker4.Enabled = false;
                    dateTimePicker3.Value = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek + 6));
                    dateTimePicker4.Value = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    break;
                case "Tháng trước":
                    dateTimePicker3.Enabled = false;
                    dateTimePicker4.Enabled = false;
                    dateTimePicker3.Value = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
                    dateTimePicker4.Value = dateTimePicker3.Value.AddMonths(1).AddDays(-1);
                    break;
                case "Tùy chọn":
                    dateTimePicker3.Enabled = true;
                    dateTimePicker4.Enabled = true;
                    break;
                default:
                    dateTimePicker3.Enabled = false;
                    dateTimePicker4.Enabled = false;
                    break;
            }           
        }

        private void btn_trxuatPrint_Click(object sender, EventArgs e)
        {
            dgv_historyPrint.Columns.Clear();

            if (DateTime.Compare(dateTimePicker3.Value, dateTimePicker4.Value) > 0)
            {
                MessageBox.Show("Khoảng thời gian bạn chọn không phù hợp\nHãy kiểm tra lại!", "TraceHistory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (cbx_perPrint.Text == "Tất cả")
                {
                    string str = "select * from HistoryPrint'";
                    DataTable dt = dtb.getData(str);
                    dtb.ShowupPrintHistory(dgv_historyPrint, dt);
                }
                else
                {
                    string str = "select * from HistoryPrint Where Ngay_Thang Between '" + dateTimePicker3.Value.ToShortDateString() + "' And '" + dateTimePicker4.Value.ToShortDateString() + "'";
                    DataTable dt = dtb.getData(str);
                    dtb.ShowupPrintHistory(dgv_historyPrint, dt);
                }        
            }
        }

        private void btn_trx_Click(object sender, EventArgs e)
        {
            dgv_historyNVL.Columns.Clear();
            DataTable dtt = new DataTable();
            var nvls = new List<NVL>() { };          
            StreamReader sr = new StreamReader(_strdatabase + "\\History\\HistoryNVL.txt");
            while (sr.EndOfStream == false)
            {
                string[] str1 = sr.ReadLine().Split('|');
                if (str1.Length == 20)
                {
                    nvls.Add(new NVL
                    {
                        model = str1[0],
                        codeNVL = str1[1],
                        maker = str1[2],
                        mkerPart = str1[3],
                        lot = str1[4],
                        temCode = str1[5],
                        ngInTemCode = str1[6],
                        tgianInTemCode = str1[7],
                        ngNhapKho = str1[8],
                        tgianNhapKho = str1[9],
                        ngCapNVL = str1[10],
                        tgianCapNVL = str1[11],
                        PDxacnhan = str1[12],
                        tgianxacnhan = str1[13],
                        ngTraNVL = str1[14],
                        tgianTraNVL = str1[15],
                        ghiChuTra = str1[16],
                        ngTraWH = str1[17],
                        tgianTraWH = str1[18],
                        ghiChuTraWH = str1[19]
                    });
                }
            }
            sr.Close();              
            
            try
            {
                dtt = CreateTable();
               
                //Chọn all
                if (cbx_model.Text == "" && cbx_malieu.Text == "" && cbx_maker.Text == "" && cbx_mkprt.Text == "" && cbx_Lot.Text == "")
                {
                    //str = "Select * From HistoryNVl
                    var str = (from nvl in nvls select nvl).OrderBy(n => n.model).ThenBy(n => n.codeNVL).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if(str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH);  
                        }                                             
                    }
                    goto jump;
                }

                //
                if (cbx_model.Text != "" && cbx_malieu.Text != "" && cbx_maker.Text != "" && cbx_mkprt.Text != "" && cbx_Lot.Text != "")
                {                   
                    //str = "Select * From HistoryNVl Where Model='" + cbx_model.Text + "' And Ma_NVL='" + cbx_malieu.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkprt.Text + "' And Lot='" + cbx_Lot.Text + "'";
                    var str = nvls.Where(x => x.model == cbx_model.Text &&
                                              x.codeNVL == cbx_malieu.Text &&
                                              x.maker == cbx_maker.Text &&
                                              x.mkerPart == cbx_mkprt.Text &&
                                              x.lot == cbx_Lot.Text).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_model.Text != "" && cbx_malieu.Text != "" && cbx_maker.Text != "" && cbx_mkprt.Text != "")
                {
                    //str = "Select * From HistoryNVL Where Model='" + cbx_model.Text + "' And Ma_NVL='" + cbx_malieu.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkprt.Text + "'";
                    var str = nvls.Where(x => x.model == cbx_model.Text &&
                                              x.codeNVL == cbx_malieu.Text &&
                                              x.maker == cbx_maker.Text &&
                                              x.mkerPart == cbx_mkprt.Text).OrderBy(n => n.lot).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_model.Text != "" && cbx_malieu.Text != "" && cbx_maker.Text != "")
                {
                    //str = "Select * From HistoryNVL Where Model='" + cbx_model.Text + "' And Ma_NVL='" + cbx_malieu.Text + "' And Maker='" + cbx_maker.Text + "'";
                    var str = nvls.Where(x => x.model == cbx_model.Text &&
                                              x.codeNVL == cbx_malieu.Text &&
                                              x.maker == cbx_maker.Text).OrderBy(n => n.mkerPart).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_model.Text != "" && cbx_malieu.Text != "")
                {
                    //str = "Select * From HistoryNVL Where Model='" + cbx_model.Text + "' And Ma_NVL='" + cbx_malieu.Text + "'";
                    var str = nvls.Where(x => x.model == cbx_model.Text &&
                                              x.codeNVL == cbx_malieu.Text).OrderBy(n => n.maker).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_model.Text != "")
                {
                    //str = "Select * From HistoryNVL Where Model='" + cbx_model.Text + "'";
                    var str = nvls.Where(x => x.model == cbx_model.Text).OrderBy(n => n.codeNVL).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                //
                if (cbx_malieu.Text != "" && cbx_maker.Text != "" && cbx_mkprt.Text != "" && cbx_Lot.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Ma_NVL='" + cbx_malieu.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkprt.Text + "' And Lot='" + cbx_Lot.Text + "'";
                    var str = nvls.Where(x => x.codeNVL == cbx_malieu.Text &&
                                              x.maker == cbx_maker.Text &&
                                              x.mkerPart == cbx_mkprt.Text &&
                                              x.lot == cbx_Lot.Text).OrderBy(n => n.model).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_malieu.Text != "" && cbx_maker.Text != "" && cbx_mkprt.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Ma_NVL='" + cbx_malieu.Text + "' And Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkprt.Text + "'";
                    var str = nvls.Where(x => x.codeNVL == cbx_malieu.Text &&
                                              x.maker == cbx_maker.Text &&
                                              x.mkerPart == cbx_mkprt.Text).OrderBy(n => n.lot).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_malieu.Text != "" && cbx_maker.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Ma_NVL='" + cbx_malieu.Text + "' And Maker='" + cbx_maker.Text + "'";
                    var str = nvls.Where(x => x.codeNVL == cbx_malieu.Text &&
                                              x.maker == cbx_maker.Text).OrderBy(n => n.mkerPart).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_malieu.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Ma_NVL='" + cbx_malieu.Text + "'";
                    var str = nvls.Where(x => x.codeNVL == cbx_malieu.Text).OrderBy(n => n.maker).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                //
                if (cbx_maker.Text != "" && cbx_mkprt.Text != "" && cbx_Lot.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkprt.Text + "' And Lot='" + cbx_Lot.Text + "'";
                    var str = nvls.Where(x => x.maker == cbx_maker.Text &&
                                              x.mkerPart == cbx_mkprt.Text &&
                                              x.lot == cbx_Lot.Text).OrderBy(n => n.codeNVL).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_maker.Text != "" && cbx_mkprt.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Maker='" + cbx_maker.Text + "' And Maker_Part='" + cbx_mkprt.Text + "'";
                    var str = nvls.Where(x => x.maker == cbx_maker.Text &&
                                              x.mkerPart == cbx_mkprt.Text).OrderBy(n => n.model).ThenBy(n => n.lot).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_maker.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Maker='" + cbx_maker.Text + "'";
                    var str = nvls.Where(x => x.maker == cbx_maker.Text).OrderBy(n => n.model).ThenBy(n => n.codeNVL).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                //
                if (cbx_mkprt.Text != "" && cbx_Lot.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Maker_Part='" + cbx_mkprt.Text + "' And Lot='" + cbx_Lot.Text + "'";
                    var str = nvls.Where(x => x.mkerPart == cbx_mkprt.Text &&
                                              x.lot == cbx_Lot.Text).OrderBy(n => n.model).ThenBy(n => n.codeNVL).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                if (cbx_mkprt.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Maker_Part='" + cbx_mkprt.Text + "'";
                    var str = nvls.Where(x => x.mkerPart == cbx_mkprt.Text).OrderBy(n => n.model).ThenBy(n => n.lot).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                //
                if (cbx_Lot.Text != "")
                {
                    //str = "Select * From HistoryNVl Where Lot='" + cbx_Lot.Text + "'";
                    var str = nvls.Where(x => x.lot == cbx_Lot.Text).OrderBy(n => n.codeNVL).ThenBy(n => n.maker).ToArray();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].model != "model")
                        {
                            assignTable(dtt, str[j].model, str[j].codeNVL, str[j].maker, str[j].mkerPart, str[j].lot,
                                    str[j].temCode, str[j].ngInTemCode, str[j].tgianInTemCode, str[j].ngNhapKho, str[j].tgianNhapKho,
                                    str[j].ngCapNVL, str[j].tgianCapNVL, str[j].PDxacnhan, str[j].tgianxacnhan, str[j].ngTraNVL, str[j].tgianTraNVL,
                                    str[j].ghiChuTra, str[j].ngTraWH, str[j].tgianTraWH, str[j].ghiChuTraWH); 
                        }  
                    }
                    goto jump;
                }

                jump:
                //dt = dtb.getData(str);
                dgv_historyNVL.DataSource = dtt;
                for (int j = 0; j < dgv_historyNVL.ColumnCount; j++)
                {
                    dgv_historyNVL.Columns[j].Width = 120;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi không thể lọc dữ liệu!", "TraceHistory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
 
        public void ShowupHistoryNVL()
        {
            dgv_historyNVL.Columns.Add("Model", "Model");
            dgv_historyNVL.Columns.Add("Ma_NVL", "Ma_NVL");
            dgv_historyNVL.Columns.Add("Maker", "Maker");
            dgv_historyNVL.Columns.Add("Maker_Part", "Maker_Part");
            dgv_historyNVL.Columns.Add("Lot", "Lot");
            dgv_historyNVL.Columns.Add("Tem_code", "Tem_code");
            dgv_historyNVL.Columns.Add("Nguoi_in_code", "Nguoi_in_code");
            dgv_historyNVL.Columns.Add("Thoi_gian_in", "Thoi_gian_in");
            dgv_historyNVL.Columns.Add("Nguoi_nhap_kho", "Nguoi_nhap_kho");
            dgv_historyNVL.Columns.Add("Thoi_gian_nhap", "Thoi_gian_nhap");
            dgv_historyNVL.Columns.Add("Nguoi_cap_NVL", "Nguoi_cap_NVL");
            dgv_historyNVL.Columns.Add("Thoi_gian_cap", "Thoi_gian_cap");
            dgv_historyNVL.Columns.Add("PD_xac_nhan", "PD_xac_nhan");
            dgv_historyNVL.Columns.Add("Thoi_gian_xn", "Thoi_gian_xn");
            dgv_historyNVL.Columns.Add("Nguoi_tra_NVL", "Nguoi_tra_NVL");
            dgv_historyNVL.Columns.Add("Thoi_gian_tra", "Thoi_gian_tra");
            dgv_historyNVL.Columns.Add("Ghi_chu_tra", "Ghi_chu_tra");
            dgv_historyNVL.Columns.Add("Nguoi_tra_WH", "Nguoi_tra_WH");
            dgv_historyNVL.Columns.Add("Thoi_gian_traWH", "Thoi_gian_traWH");
            dgv_historyNVL.Columns.Add("Ghi_chu_traWH", "Ghi_chu_traWH");

            for (int j = 0; j < dgv_historyNVL.ColumnCount; j++)
            {
                dgv_historyNVL.Columns[j].Width = 120;
            }

            dgv_historyNVL.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgv_historyNVL.ColumnHeadersHeight = dgv_historyNVL.ColumnHeadersHeight * 2;
            dgv_historyNVL.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgv_historyNVL.CellPainting += new DataGridViewCellPaintingEventHandler(dgv_historyNVL_CellPainting);
            dgv_historyNVL.Paint += new PaintEventHandler(dgv_historyNVL_Paint);
            dgv_historyNVL.Scroll += new ScrollEventHandler(dgv_historyNVL_Scroll);
            dgv_historyNVL.ColumnWidthChanged += new DataGridViewColumnEventHandler(dgv_historyNVL_ColumnWidthChanged);
        }

        private void dgv_historyNVL_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if(e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r2, true);
                e.PaintContent(r2);
                e.Handled = true;
            }
        }

        private void dgv_historyNVL_Paint(object sender, PaintEventArgs e)
        {
            string[] infHeader = { "Thông tin BOM","WH-KTZ", "KTZ-PD", "PDxacnhan", "PD-KTZ", "KTZ-WH"};
            for(int j = 0; j < 18;)
            {
                if(j == 0)
                {
                    Rectangle r1 = dgv_historyNVL.GetCellDisplayRectangle(j, -1, true);
                    int w2 = dgv_historyNVL.GetCellDisplayRectangle(j + 1, -1, true).Width;
                    r1.X += 1;
                    r1.Y += 1;                    
                    r1.Width = r1.Width + (5 * w2) - 2;
                    r1.Height = r1.Height / 2 - 2;
                    e.Graphics.FillRectangle(new SolidBrush(Color.Gray), r1);
                    StringFormat format = new StringFormat();                   
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(infHeader[0],
                    dgv_historyNVL.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgv_historyNVL.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                    j += 6;
                }
                else if (j == 6)
                {
                    Rectangle r1 = dgv_historyNVL.GetCellDisplayRectangle(j, -1, true);
                    int w2 = dgv_historyNVL.GetCellDisplayRectangle(j + 1, -1, true).Width;
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + (3 * w2) - 2;
                    r1.Height = r1.Height / 2 - 2;
                    e.Graphics.FillRectangle(new SolidBrush(Color.Yellow), r1);
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(infHeader[1],
                    dgv_historyNVL.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgv_historyNVL.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                    j += 4;
                }
                else if(j == 10)
                {
                    Rectangle r1 = dgv_historyNVL.GetCellDisplayRectangle(j, -1, true);
                    int w2 = dgv_historyNVL.GetCellDisplayRectangle(j + 1, -1, true).Width;
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + (1 * w2) - 2;
                    r1.Height = r1.Height / 2 - 2;
                    e.Graphics.FillRectangle(new SolidBrush(Color.Violet), r1);
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(infHeader[2],
                    dgv_historyNVL.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgv_historyNVL.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                    j += 2;
                }
                else if (j == 12)
                {
                    Rectangle r1 = dgv_historyNVL.GetCellDisplayRectangle(j, -1, true);
                    int w2 = dgv_historyNVL.GetCellDisplayRectangle(j + 1, -1, true).Width;
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + (1 * w2) - 2;
                    r1.Height = r1.Height / 2 - 2;
                    e.Graphics.FillRectangle(new SolidBrush(Color.SkyBlue), r1);
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(infHeader[3],
                    dgv_historyNVL.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgv_historyNVL.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                    j += 2;
                } 
                else if (j == 14)
                {
                    Rectangle r1 = dgv_historyNVL.GetCellDisplayRectangle(j, -1, true);
                    int w2 = dgv_historyNVL.GetCellDisplayRectangle(j + 1, -1, true).Width;
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + (2 * w2) - 2;
                    r1.Height = r1.Height / 2 - 2;
                    e.Graphics.FillRectangle(new SolidBrush(Color.SandyBrown), r1);
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(infHeader[4],
                    dgv_historyNVL.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgv_historyNVL.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                    j += 3;
                } 
                else if(j == 17)
                {
                    Rectangle r1 = dgv_historyNVL.GetCellDisplayRectangle(j, -1, true);
                    int w2 = dgv_historyNVL.GetCellDisplayRectangle(j + 1, -1, true).Width;
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + (2 * w2) - 2;
                    r1.Height = r1.Height / 2 - 2;
                    e.Graphics.FillRectangle(new SolidBrush(Color.LawnGreen), r1);
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(infHeader[5],
                    dgv_historyNVL.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(dgv_historyNVL.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                    j += 3;
                }
            }
        }

        private void dgv_historyNVL_Scroll(object sender, ScrollEventArgs e)
        {
            Rectangle rtHeader = dgv_historyNVL.DisplayRectangle;
            rtHeader.Height = dgv_historyNVL.ColumnHeadersHeight / 2;
            dgv_historyNVL.Invalidate(rtHeader);
        }

        private void dgv_historyNVL_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Rectangle rtHeader = dgv_historyNVL.DisplayRectangle;
            rtHeader.Height = dgv_historyNVL.ColumnHeadersHeight / 2;
            dgv_historyNVL.Invalidate(rtHeader);
        }

        public bool chk = true;
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage3"])
            {                                
                var nvls = new List<NVL>() { };               
                StreamReader sr = new StreamReader(_strdatabase + "\\History\\HistoryNVL.txt");
                while (sr.EndOfStream == false)
                {
                    string[] str1 = sr.ReadLine().Split('|');
                    if (str1.Length == 20)
                    {
                        nvls.Add(new NVL
                        {
                            model = str1[0],
                            codeNVL = str1[1],
                            maker = str1[2],
                            mkerPart = str1[3],
                            lot = str1[4],
                            temCode = str1[5],
                            ngInTemCode = str1[6],
                            tgianInTemCode = str1[7],
                            ngNhapKho = str1[8],
                            tgianNhapKho = str1[9],
                            ngCapNVL = str1[10],
                            tgianCapNVL = str1[11],
                            PDxacnhan = str1[12],
                            tgianxacnhan = str1[13],
                            ngTraNVL = str1[14],
                            tgianTraNVL = str1[15],
                            ghiChuTra = str1[16],
                            ngTraWH = str1[17],
                            tgianTraWH = str1[18],
                            ghiChuTraWH = str1[19]
                        });
                    }
                }
                sr.Close();
                //model
                if(cbx_model.Items.Count == 0)
                {
                    var str_model = nvls.GroupBy(x => x.model).Select(g => g.First()).ToArray();
                    for (int j = 1; j < str_model.Length; j++)
                    {
                        cbx_model.Items.Add(str_model[j].model);
                    }
                }

                //ma NVL
                if(cbx_malieu.Items.Count == 0)
                {
                    var str_maNVL = nvls.GroupBy(x => x.codeNVL).Select(g => g.First()).ToArray();
                    for (int j = 1; j < str_maNVL.Length; j++)
                    {
                        cbx_malieu.Items.Add(str_maNVL[j].codeNVL);
                    }
                }
                                
                //maker
                if(cbx_maker.Items.Count == 0)
                {
                    var str_maker = nvls.GroupBy(x => x.maker).Select(g => g.First()).ToArray();
                    for (int j = 1; j < str_maker.Length; j++)
                    {
                        cbx_maker.Items.Add(str_maker[j].maker);
                    }
                }
                
                //maker part
                if(cbx_mkprt.Items.Count == 0)
                {
                    var str_mkp = nvls.GroupBy(x => x.mkerPart).Select(g => g.First()).ToArray();
                    for (int j = 1; j < str_mkp.Length; j++)
                    {
                        cbx_mkprt.Items.Add(str_mkp[j].mkerPart);
                    }
                }
                
                //lot
                if(cbx_Lot.Items.Count == 0)
                {
                    var str_lot = nvls.GroupBy(x => x.lot).Select(g => g.First()).ToArray();
                    for (int j = 1; j < str_lot.Length; j++)
                    {
                        cbx_Lot.Items.Add(str_lot[j].lot);
                    }
                }                

                if (dgv_historyNVL.ColumnCount == 0 && chk == true)
                {
                    ShowupHistoryNVL();
                    chk = false;
                }                                    
            }
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage4"])
            {
                dtb1.get_cbbModel("HistoryConfirmNVL", "Ngay_thang", cbx_ntxn);
                dtb1.get_cbbModel("HistoryConfirmNVL", "Model", cbx_molxn);
                dtb1.get_cbbModel("HistoryConfirmNVL", "Ma_NVL", cbx_maNVLxn);
                dtb1.get_cbbModel("HistoryConfirmNVL", "Maker", cbx_mkxn);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cbx_model.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cbx_malieu.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cbx_maker.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cbx_mkprt.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cbx_Lot.Text = "";
        }

        private void btn_luu_Click(object sender, EventArgs e)
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
                ex.exportStockKTZZ(dgv_historyNVL, fil_name, chek);

                MessageBox.Show("Lưu thành công!", "TraceHistory", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }           
        }

        private void btn_sv_Click(object sender, EventArgs e)
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
                ex.exportStockKTZZ(dgv_historyPrint, fil_name, chek);

                MessageBox.Show("Lưu thành công!", "TraceHistory", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }           
        }

        private void button7_Click(object sender, EventArgs e)
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
                ex.exportStockKTZZ(dgv_BOMhistory, fil_name, chek);

                MessageBox.Show("Lưu thành công!", "TraceHistory", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }           
        }

        private void cbx_model_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cbx_malieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cbx_maker_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        public class NVL
        {
            public string model { set; get; }
            public string codeNVL { set; get; }
            public string maker { set; get; }
            public string mkerPart { set; get; }
            public string lot { set; get; }
            public string temCode { set; get; }
            public string ngInTemCode { set; get; }
            public string tgianInTemCode { set; get; }
            public string ngNhapKho { set; get; }
            public string tgianNhapKho { set; get; }
            public string ngCapNVL { set; get; }
            public string tgianCapNVL { set; get; }
            public string PDxacnhan { set; get; }
            public string tgianxacnhan { set; get; }
            public string ngTraNVL { set; get; }
            public string tgianTraNVL { set; get; }
            public string ghiChuTra { set; get; }
            public string ngTraWH { set; get; }
            public string tgianTraWH { set; get; }
            public string ghiChuTraWH { set; get; }
        }

        public DataTable assignTable(DataTable TableExcel, string model, string maNVL, string maker, string makerpart, string lot, string temCode, string ngInTem, string tgInTem, string ngNhapKho, string tgNhapKho, string ngCapNVL, string tgCapNVL, string PDxacnhan, string tgxacnhan, string ngTraNVL, string tgTraNVL, string ghiChuTra, string ngTraWH, string tgTraWH, string ghiChuWH)
        {
            DataRow Row;           

            Row = TableExcel.NewRow();
            Row["Model"] = model;
            Row["Ma_NVL"] = maNVL;
            Row["Maker"] = maker;
            Row["Maker_Part"] = makerpart;
            Row["Lot"] = lot;
            Row["Tem_code"] = temCode;
            Row["Nguoi_in_code"] = ngInTem;
            Row["Thoi_gian_in"] = tgInTem;
            Row["Nguoi_nhap_kho"] = ngNhapKho;
            Row["Thoi_gian_nhap"] = tgNhapKho;
            Row["Nguoi_cap_NVL"] = ngCapNVL;
            Row["Thoi_gian_cap"] = tgCapNVL;
            Row["PD_xac_nhan"] = PDxacnhan;
            Row["Thoi_gian_xn"] = tgxacnhan;
            Row["Nguoi_tra_NVL"] = ngTraNVL;
            Row["Thoi_gian_tra"] = tgTraNVL;
            Row["Ghi_chu_tra"] = ghiChuTra;
            Row["Nguoi_tra_WH"] = ngTraWH;
            Row["Thoi_gian_traWH"] = tgTraWH;
            Row["Ghi_chu_traWH"] = ghiChuWH;

            TableExcel.Rows.Add(Row);
            return TableExcel;
        }

        public DataTable CreateTable()
        {
            DataTable TableExcel = new DataTable();
            DataColumn column;

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Model";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Ma_NVL";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Maker";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Maker_Part";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Lot";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Tem_code";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Nguoi_in_code";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Thoi_gian_in";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Nguoi_nhap_kho";
            TableExcel.Columns.Add(column);


            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Thoi_gian_nhap";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Nguoi_cap_NVL";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Thoi_gian_cap";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "PD_xac_nhan";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Thoi_gian_xn";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Nguoi_tra_NVL";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Thoi_gian_tra";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Ghi_chu_tra";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Nguoi_tra_WH";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Thoi_gian_traWH";
            TableExcel.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(String);
            column.ColumnName = "Ghi_chu_traWH";
            TableExcel.Columns.Add(column);

            return TableExcel;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            cbx_ntxn.Text = "";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            cbx_molxn.Text = "";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            cbx_maNVLxn.Text = "";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            cbx_mkxn.Text = "";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dgv_NVLxn.Columns.Clear();
            string strSel = string.Empty;
            //Chọn all
            if(cbx_ntxn.Text == "" && cbx_molxn.Text == "" && cbx_maNVLxn.Text == "" && cbx_mkxn.Text == "")
            {
                strSel = "Select * from HistoryConfirmNVL";
                goto jumpxn;
            }
            //
            if (cbx_ntxn.Text != "" && cbx_molxn.Text != "" && cbx_maNVLxn.Text != "" && cbx_mkxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Ngay_thang='" + cbx_ntxn.Text + "' and Model='" + cbx_molxn.Text + "' and Ma_NVL='" + cbx_maNVLxn.Text + "' and Maker='" + cbx_mkxn.Text + "'";
                goto jumpxn;
            }
            //
            if (cbx_ntxn.Text != "" && cbx_molxn.Text != "" && cbx_maNVLxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Ngay_thang='" + cbx_ntxn.Text + "' and Model='" + cbx_molxn.Text + "' and Ma_NVL='" + cbx_maNVLxn.Text + "'";
                goto jumpxn;
            }
            //
            if (cbx_ntxn.Text != "" && cbx_molxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Ngay_thang='" + cbx_ntxn.Text + "' and Model='" + cbx_molxn.Text + "'";
                goto jumpxn;
            }
            //
            if (cbx_ntxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Ngay_thang='" + cbx_ntxn.Text + "'";
                goto jumpxn;
            }
            //
            if (cbx_molxn.Text != "" && cbx_maNVLxn.Text != "" && cbx_mkxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Model='" + cbx_molxn.Text + "' and Ma_NVL='" + cbx_maNVLxn.Text + "' and Maker='" + cbx_mkxn.Text + "'";
                goto jumpxn;
            }
            //
            if (cbx_molxn.Text != "" && cbx_maNVLxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Model='" + cbx_molxn.Text + "' and Ma_NVL='" + cbx_maNVLxn.Text + "'";
                goto jumpxn;
            }
            //
            if (cbx_molxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Model='" + cbx_molxn.Text + "'";
                goto jumpxn;
            }
            //
            if (cbx_maNVLxn.Text != "" && cbx_mkxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Ma_NVL='" + cbx_maNVLxn.Text + "' and Maker='" + cbx_mkxn.Text + "'";
                goto jumpxn;
            }
            //
            if (cbx_maNVLxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Ma_NVL='" + cbx_maNVLxn.Text + "'";
                goto jumpxn;
            }
            //
            if (cbx_mkxn.Text != "")
            {
                strSel = "Select * from HistoryConfirmNVL where Maker='" + cbx_mkxn.Text + "'";
                goto jumpxn;
            }
        jumpxn:
            dt = dtb.getData(strSel);
            dgv_NVLxn.DataSource = dt;
            for (int j = 0; j < dgv_NVLxn.ColumnCount; j++)
            {                
                if(j < 4)
                {
                    dgv_NVLxn.Columns[j].Width = 100;
                }
                else if(j >= 4 && j < 8)
                {
                    dgv_NVLxn.Columns[j].Width = 160;
                }
                else
                {
                    dgv_NVLxn.Columns[j].Width = 120;
                }                
            }
        }

        private void button8_Click(object sender, EventArgs e)
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
                ex.exportStockKTZZ(dgv_NVLxn, fil_name, chek);

                MessageBox.Show("Lưu thành công!", "TraceHistory", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }           
        }
    }
}
