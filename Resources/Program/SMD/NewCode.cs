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
using System.Threading;
using Spire.Barcode;
using System.Drawing.Printing;


namespace ManageMaterialPBA
{
    public partial class NewCode : Form
    {
        Form1 _frm;
        database_1 dtb1;
        database dtb;
        ClsExcel excel = new ClsExcel();
        public DataGridView _dgv;
        public bool cfrm = false, btnSave = false, btnInthu = false;
        public string idCPE, psCPE, strlydo;
        public string _user = string.Empty, _pass = string.Empty, _model = string.Empty, _ngDung = string.Empty, _part = string.Empty, _dMon = string.Empty, _datTim = string.Empty;
        public string _strdatabase = string.Empty;

        public NewCode(DataGridView dgv, string user, string pass, string modell, string ngDung, string part, string dMon, string datTim, Form1 frm, string strdatabase)
        {
            InitializeComponent();
            _dgv = dgv;
            _user = user;
            _pass = pass;
            _model = modell;
            _ngDung = ngDung;
            _part = part;
            _dMon = dMon;
            _datTim = datTim;
            _frm = frm;
            _strdatabase = strdatabase;
        }

        private void NewCode_Load(object sender, EventArgs e)
        {
            dtb1 = new database_1(_strdatabase);
            dtb = new database(_strdatabase);
            this.Location = new Point(0, 0);
            //Load ảnh minh họa
            pictureBox1.Image = new Bitmap(_strdatabase + "\\Picture\\MasterTem.PNG");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            //Load config
            StreamReader sr = new StreamReader(_strdatabase + "\\PrinterConfig.ini");
            while (sr.EndOfStream == false)
            {
                string[] str = sr.ReadLine().Split('=');
                foreach (Control x in this.groupBox2.Controls)
                {
                    if (x is TextBox)
                    {
                        if (((TextBox)x).Name.Contains(str[0]))
                        {
                            ((TextBox)x).Text = str[1];
                        }
                    }
                }
            }
            sr.Close();

            //Khoa thong so            
            if (_part == "admin" || _part == "manager" || _part == "CPE")
            {
                btn_save.Enabled = true;
                btn_Inthu.Enabled = true;
            }
            else
            {
                btn_save.Enabled = false;
                btn_Inthu.Enabled = false;
            }            
            toolStripStatusLabel1.Text = _ngDung;
        }

        private void btn_Inthu_Click(object sender, EventArgs e)
        {
            btnInthu = true;
            if(_part != "admin" && _part == "manager")
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "ConfirmInKTZ")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    ConfirmInKTZ confirmAdKTZ = new ConfirmInKTZ(this);
                    confirmAdKTZ.Show();
                    timer1.Start();
                }   
            } 
            else
            {
                InThu();
            }
        }       

        public void InThu()
        {
            bool codeAlternative = false;
            try
            {
                string xMatrix = txt_a1.Text;
                string yMatrix = txt_b1.Text;
                string sizeMatrix = txt_a0.Text;
                string heightChar = txt_b3.Text;
                string widthChar = (int.Parse(heightChar) - 5).ToString();
                string xRow = txt_a2.Text;
                string yRow1 = txt_b2.Text;
                string yRow2 = (int.Parse(yRow1) + int.Parse(heightChar) + 10).ToString();
                string yRow3 = (int.Parse(yRow2) + int.Parse(heightChar) + 10).ToString();
                string yRow4 = (int.Parse(yRow3) + int.Parse(heightChar) + 10).ToString();
                string x128 = txt_a3.Text;
                string y128 = (int.Parse(yRow2) * 1.2).ToString();
                string size128 = txt_a5.Text;
                string height128 = (int.Parse(heightChar) * 2.5).ToString();
                string x128Char = txt_a4.Text;
                string y128Char = txt_b4.Text;

                string s = "^XA\n^FO" + xMatrix + "," + yMatrix + "\n^BXN," + sizeMatrix + ",200\n^FD0401-001110+11/11/11-11-11-11+1SS400+abcdef\n^FS" +
                           "\n^FO" + xRow + "," + yRow1 + "\n^A0," + heightChar + "," + widthChar + "\n^FD0401-001110\n^FS" +
                           "\n^FO" + xRow + "," + yRow2 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+11/11/11-11-11-11\n^FS" +
                           "\n^FO" + xRow + "," + yRow3 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+1SS400\n^FS" +
                           "\n^FO" + xRow + "," + yRow4 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+abcdef\n^FS" +
                           "\n^FO" + x128 + "," + y128 + "\n^BY" + size128 + "^BCN," + height128 + ",N,N,N,N\n^FD0401-001110\n^FS" +
                           "\n^FO" + x128Char + "," + y128Char + "\n^A0," + (int.Parse(heightChar) * 1.5).ToString() + "," + (int.Parse(widthChar) * 1.5).ToString() + "\n^FD0401-001110\n^FS" +
                           "\n^XZ";
                PrintDialog pd = new PrintDialog();
                pd.PrinterSettings = new PrinterSettings();
                if (DialogResult.OK == pd.ShowDialog(this))
                {
                    Clsprint.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);
                    codeAlternative = true;
                }
            }
            catch (Exception)
            {
                codeAlternative = false;
                dtb1.get_colorText(txt_a0);
                dtb1.get_colorText(txt_a1);
                dtb1.get_colorText(txt_a2);
                dtb1.get_colorText(txt_a3);
                dtb1.get_colorText(txt_a4);
                dtb1.get_colorText(txt_a5);
                dtb1.get_colorText(txt_b1);
                dtb1.get_colorText(txt_b2); ;
                dtb1.get_colorText(txt_b3);
                dtb1.get_colorText(txt_b4);
                MessageBox.Show("Bạn hãy kiểm tra phần thông tin bôi đỏ!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (codeAlternative == true)
            {
                DialogResult rel = MessageBox.Show("Kiểm tra code vừa in.\n OK hay NG?", "In Code", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rel == DialogResult.OK)
                {
                    btn_Inthat.Enabled = true;
                }
            }      
        }

        private void btn_Inthat_Click(object sender, EventArgs e)
        {
            bool inSucess = false;
            try
            {
                int err = 0;
                File.Delete(_strdatabase + "\\Log\\Duplicate\\NewCode.log");
                File.Delete(_strdatabase + "\\Log\\Duplicate\\FI-FO.log");
                //Luu New Code
                foreach (DataGridViewRow dgr in _dgv.Rows)
                {
                    if (dgr.Cells["Tem_code"].Value != null && dgr.Cells["Tem_code"].Value.ToString() != "")
                    {
                        if (dtb1.chekNewCode(dgr.Cells["Tem_Code"].Value.ToString()) == false)
                        {
                            dgr.Cells["Tem_Code"].Style.BackColor = Color.Red;
                            err++;
                            goto jumpchk;
                        }
                        else
                        {
                            dgr.Cells["Tem_Code"].Style.BackColor = Color.White;
                            dtb1.savNwCod(dgr.Cells["Tem_Code"].Value.ToString());//Lưu inf new code
                            //Save data FIFO     
                            //form code : Ma_NVL + DateTime + MakerPart + Lot
                            string[] strfifo = dgr.Cells["Tem_Code"].Value.ToString().Split('+');
                            dtb1.savFIFO(strfifo[0] + "+" + dgr.Cells["Maker"].Value.ToString() + "+" + strfifo[2] + "+" + strfifo[3] + "+" + strfifo[1] + "+" + dgr.Cells["So_luong_nhap"].Value.ToString() + "+" + dgr.Cells["Mo_ta"].Value.ToString());
                        }
                    }
                }
                //In
                try
                {
                    #region
                    string xMatrix = txt_a1.Text;
                    string yMatrix = txt_b1.Text;
                    string sizeMatrix = txt_a0.Text;
                    string heightChar = txt_b3.Text;
                    string widthChar = (int.Parse(heightChar) - 5).ToString();
                    string xRow = txt_a2.Text;
                    string yRow1 = txt_b2.Text;
                    string yRow2 = (int.Parse(yRow1) + int.Parse(heightChar) + 10).ToString();
                    string yRow3 = (int.Parse(yRow2) + int.Parse(heightChar) + 10).ToString();
                    string yRow4 = (int.Parse(yRow3) + int.Parse(heightChar) + 10).ToString();
                    string x128 = txt_a3.Text;
                    string y128 = (int.Parse(yRow2) * 1.2).ToString();
                    string size128 = txt_a5.Text;
                    string height128 = (int.Parse(heightChar) * 2.5).ToString();
                    string x128Char = txt_a4.Text;
                    string y128Char = txt_b4.Text;

                    PrintDialog pd = new PrintDialog();
                    pd.PrinterSettings = new PrinterSettings();
                    if (DialogResult.OK == pd.ShowDialog(this))
                    {
                        FileStream FS = new FileStream(_strdatabase + "\\Log\\Duplicate\\NewCode.log", FileMode.Open);
                        StreamReader SR = new StreamReader(FS);
                        while (SR.EndOfStream == false)
                        {
                            string s1 = SR.ReadLine();
                            string[] s2 = s1.Split('+');

                            string s = "^XA\n^FO" + xMatrix + "," + yMatrix + "\n^BXN," + sizeMatrix + ",200\n^FD" + s1 + "\n^FS" +
                                       "\n^FO" + xRow + "," + yRow1 + "\n^A0," + heightChar + "," + widthChar + "\n^FD" + s2[0] + "\n^FS" +
                                       "\n^FO" + xRow + "," + yRow2 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+" + s2[1] + "\n^FS" +
                                       "\n^FO" + xRow + "," + yRow3 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+" + s2[2] + "\n^FS" +
                                       "\n^FO" + xRow + "," + yRow4 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+" + s2[3] + "\n^FS" +
                                       "\n^FO" + x128 + "," + y128 + "\n^BY" + size128 + "^BCN," + height128 + ",N,N,N,N\n^FD" + s2[0] + "\n^FS" +
                                       "\n^FO" + x128Char + "," + y128Char + "\n^A0," + (int.Parse(heightChar) * 1.5).ToString() + "," + (int.Parse(widthChar) * 1.5).ToString() + "\n^FD" + s2[0] + "\n^FS" +
                                       "\n^XZ";
                            Thread.Sleep(500);
                            Clsprint.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);
                        }
                        SR.Close();
                        FS.Close();
                        inSucess = true;
                    }
                    #endregion
                }
                catch (Exception)
                {
                    dtb1.get_colorText(txt_a0);
                    dtb1.get_colorText(txt_a1);
                    dtb1.get_colorText(txt_a2);
                    dtb1.get_colorText(txt_a3);
                    dtb1.get_colorText(txt_a4);
                    dtb1.get_colorText(txt_a5);
                    dtb1.get_colorText(txt_b1);
                    dtb1.get_colorText(txt_b2);
                    dtb1.get_colorText(txt_b3);
                    dtb1.get_colorText(txt_b4);
                    MessageBox.Show("Xảy ra lỗi :\n1. Không tồn tại code\n2.Kiểm tra thông số bôi đỏ!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //History
                if (inSucess == true)
                {
                    #region
                    //Update history
                    FileStream fs1 = new FileStream(_strdatabase + "\\History\\HistoryNVL.txt", FileMode.Append);
                    StreamWriter sw1 = new StreamWriter(fs1);
                    for (int h = 0; h < _dgv.RowCount; h++)
                    {
                        if (_dgv.Rows[h].Cells["Mo_ta"].Value != null && _dgv.Rows[h].Cells["Mo_ta"].Value.ToString() != "")
                        {
                            string maNVL = _dgv.Rows[h].Cells["Ma_NVL"].Value.ToString();
                            string maker = _dgv.Rows[h].Cells["Maker"].Value.ToString();
                            string mkp = _dgv.Rows[h].Cells["Maker_Part"].Value.ToString();
                            string lot = _dgv.Rows[h].Cells["Lot"].Value.ToString();
                            string temCode = _dgv.Rows[h].Cells["Tem_code"].Value.ToString();
                            sw1.Write("\n");
                            sw1.WriteLine(_model + "|" +
                                          maNVL + "|" +
                                          maker + "|" +
                                          mkp + "|" +
                                          lot + "|" +
                                          temCode + "|" +
                                          _ngDung + "|" +
                                          DateTime.Now.ToString() + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "" + "|" +
                                          "");
                        }
                    }
                    sw1.Close();
                    fs1.Close();
                    //Input Stock
                    InputStockKtz();
                    try
                    {
                        string[] files = Directory.GetFiles(_strdatabase + "\\Log\\Duplicate\\");
                        int n = 0;
                        foreach (string fil in files)
                        {
                            if ((files[n].Contains("Input_Line")) || files[n].Contains("PDxacnhan") || files[n].Contains("Input_Ktz") || files[n].Contains("NVL_Holding"))
                            {
                                goto jumpn;
                            }

                            if (files[n].Contains("FI-FO"))
                            {
                                if (File.Exists(_strdatabase + "\\Log\\Duplicate\\Old\\FI-FO.log"))
                                {
                                    File.Delete(_strdatabase + "\\Log\\Duplicate\\Old\\FI-FO.log");
                                }
                                File.Move(_strdatabase + "\\Log\\Duplicate\\FI-FO.log", _strdatabase + "\\Log\\Duplicate\\Old\\FI-FO.log");
                            }

                            if (files[n].Contains("NewCode"))
                            {
                                StreamReader srLog = new StreamReader(_strdatabase + "\\Log\\Duplicate\\NewCode.log");
                                FileStream fs = new FileStream(_strdatabase + "\\Log\\Duplicate\\Old\\" + DateTime.Now.ToString("yyyyMMdd") + "_NewCode.log", FileMode.Append);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.Write("\n");
                                while (srLog.EndOfStream == false)
                                {
                                    sw.WriteLine(srLog.ReadLine());
                                }
                                sw.Close();
                                srLog.Close();
                                if (File.Exists(@"C:\Users\Administrator\Documents\" + DateTime.Now.ToString("yyyyMMdd") + "_NewCode.log"))
                                {
                                    File.Delete(@"C:\Users\Administrator\Documents\" + DateTime.Now.ToString("yyyyMMdd") + "_NewCode.log");
                                }
                                File.Copy(_strdatabase + "\\Log\\Duplicate\\NewCode.log", @"C:\Users\ngohuutoan\Desktop\New folder\" + DateTime.Now.ToString("yyyyMMdd") + "_NewCode.log");                           
                            }
                            File.Delete(fil);
                        jumpn:
                            n++;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Xảy ra lỗi xóa file .log (FI-FO, MakerPart, Newcode)!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    #endregion
                }
            jumpchk:
                if(err > 0)
                {
                    MessageBox.Show("Xảy ra lỗi trùng Tem_code. Kiểm tra lại với dữ liệu bôi đỏ", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }                
            }
            catch (Exception)
            {
                MessageBox.Show("Xảy ra lỗi :\n1. Tạo file NewCode.log\n2.Lỗi in code\n3.Lỗi update history!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                               
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.CheckState == CheckState.Checked)
            {
                btn_Inthu.Enabled = true;
                btn_save.Enabled = true;
            }
            else
            {
                btn_Inthu.Enabled = false;
                btn_save.Enabled = false;
            }
        }

        public int count_timer = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            count_timer++;
            if(count_timer == 20)
            {
                timer1.Stop();
            }
            //Luu thong so may in thay doi
            if (cfrm == true && btnSave == true)
            {
                count_timer = 0;
                cfrm = false;
                btnSave = false;
                timer1.Stop();
                try
                {
                    //Luu data
                    FileStream fs_p = new FileStream(_strdatabase + "\\Print\\ChangePrinterConfig.ini", FileMode.Create);
                    StreamWriter sw_p = new StreamWriter(fs_p);
                    foreach (Control x in this.groupBox2.Controls)
                    {
                        if (x is TextBox)
                        {
                            string str = ((TextBox)x).Name.Substring(4, 2) + "=" + ((TextBox)x).Text;
                            sw_p.WriteLine(str);
                        }
                    }
                    sw_p.Close();
                    fs_p.Close();

                    //So sanh
                    CompareAndHistory(_strdatabase + "\\PrinterConfig.ini", _strdatabase + "\\Print\\ChangePrinterConfig.ini"); 
                    
                    //Luu data
                    FileStream fs = new FileStream(_strdatabase + "\\PrinterConfig.ini", FileMode.Create);                   
                    StreamWriter sw = new StreamWriter(fs);    
                    foreach (Control x in this.groupBox2.Controls)
                    {
                        if (x is TextBox)
                        {
                            string str = ((TextBox)x).Name.Substring(4, 2) + "=" + ((TextBox)x).Text;
                            sw.WriteLine(str);
                        }
                    }
                    sw.Close(); 
                    fs.Close();
                    
                    MessageBox.Show("Lưu file thông số thành công!", "NewCode", MessageBoxButtons.OK, MessageBoxIcon.Information);                   
                }
                catch (Exception)
                {
                    MessageBox.Show("Lưu file thông số thất bại!", "NewCode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }           
            }
            //In thu
            if (cfrm == true && btnInthu == true)
            {
                count_timer = 0;
                cfrm = false;
                btnInthu = false;
                timer1.Stop();
                InThu();
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            btnSave = true;
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "ConfirmInKTZ")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                ConfirmInKTZ confirmAdKTZ = new ConfirmInKTZ(this);
                confirmAdKTZ.Show();
                timer1.Start();
            }                    
        }

        public void CompareAndHistory(string pathBef, string pathAft)
        {
            //Before
            StreamReader srBef = new StreamReader(pathBef);
            string[] strBef = new string[10];
            int b = 0;
            while (srBef.EndOfStream == false)
            {
                strBef[b] = srBef.ReadLine();
                b++;
            }
            srBef.Close();

            //After
            StreamReader srAft = new StreamReader(pathAft);
            string[] strAft = new string[10];
            int a = 0;
            while (srAft.EndOfStream == false)
            {
                strAft[a] = srAft.ReadLine();
                a++;
            }
            srAft.Close();

            //So sanh
            for (int i = 0; i < 10; i++)
            {
                if (strBef[i] != strAft[i])
                {
                    string[] tgArr = strBef[i].Split('=');
                    OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                    cnn.Open();
                    string strIn = "Insert Into HistoryPrint Values('" + DateTime.Now.ToShortDateString() + "','" + tgArr[0] + "','" + strBef[i] + "','"
                                                                       + strAft[i] + "','" + strlydo + "','" + dtb.get_name(idCPE, psCPE) + "','"
                                                                       + toolStripStatusLabel1.Text + "','" + DateTime.Now.ToString() + "')";
                    OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                    cmdIn.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        public void InputStockKtz()
        {
            if (dtb1.checkWH_Ktz(_dgv) == true)//kiểm tra datagirdview dc điền đủ thông tin
            {
                MessageBox.Show("Các thông tin đang để trống hoặc sai!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //lưu cộng dồn stock KTZ, update time cập nhật
                if (dtb1.upStokKtz(_dgv, _datTim) == true)
                {
                    //Update StockFIFO
                    if (dtb1.up_FIFO(_strdatabase) == true)
                    {
                        //lưu logfile WH-KTZ ngày order
                        //Export datagirdview sang excel .CSV
                        bool chekExitFil = excel.checkExitLog(_strdatabase + "\\History\\WH\\Order\\" + _dMon + "\\" + _datTim + "_" + _model + ".csv");
                        if (excel.exportCsvWHKtz(_dgv, _strdatabase + "\\History\\WH\\Order\\" + _dMon + "\\" + _datTim + "_" + _model + ".csv", chekExitFil, _ngDung, "Nguoi_lay", 1, 0) == true)
                        {
                            MessageBox.Show("Tạo LogFile thành công!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (File.Exists(_strdatabase + "\\tem\\" + _model + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadWH-KTZ.txt"))
                            {
                                File.Delete(_strdatabase + "\\tem\\" + _model + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadWH-KTZ.txt");
                            }

                            //update lish su
                            #region
                            try
                            {
                                var nvls = new List<NVL>() { };
                                StreamReader sr = new StreamReader(_strdatabase + "\\History\\HistoryNVL.txt");
                                while (sr.EndOfStream == false)
                                {
                                    string[] str = sr.ReadLine().Split('|');
                                    if (str.Length == 20)
                                    {
                                        nvls.Add(new NVL
                                        {
                                            model = str[0],
                                            codeNVL = str[1],
                                            maker = str[2],
                                            mkerPart = str[3],
                                            lot = str[4],
                                            temCode = str[5],
                                            ngInTemCode = str[6],
                                            tgianInTemCode = str[7],
                                            ngNhapKho = str[8],
                                            tgianNhapKho = str[9],
                                            ngCapNVL = str[10],
                                            tgianCapNVL = str[11],
                                            PDxacnhan = str[12],
                                            tgianxacnhan = str[13],
                                            ngTraNVL = str[14],
                                            tgianTraNVL = str[15],
                                            ghiChuTra = str[16],
                                            ngTraWH = str[17],
                                            tgianTraWH = str[18],
                                            ghiChuTraWH = str[19]
                                        });
                                    }
                                }
                                sr.Close();

                                for (int h = 0; h < _dgv.RowCount; h++)
                                {
                                    if (_dgv.Rows[h].Cells["Mo_ta"].Value != null && _dgv.Rows[h].Cells["Mo_ta"].Value.ToString() != "")
                                    {
                                        string temCode = _dgv.Rows[h].Cells["Tem_code"].Value.ToString();
                                        foreach (var nn in nvls.Where(x => x.temCode == temCode))
                                        {
                                            nn.ngNhapKho = _ngDung;
                                            nn.tgianNhapKho = DateTime.Now.ToString();
                                        }
                                    }
                                }

                                FileStream fs = new FileStream(_strdatabase + "\\History\\HistoryNVL.txt", FileMode.Create);
                                StreamWriter sw = new StreamWriter(fs);
                                foreach (var item in nvls)
                                {
                                    sw.WriteLine(item.model + "|" +
                                                 item.codeNVL + "|" +
                                                 item.maker + "|" +
                                                 item.mkerPart + "|" +
                                                 item.lot + "|" +
                                                 item.temCode + "|" +
                                                 item.ngInTemCode + "|" +
                                                 item.tgianInTemCode + "|" +
                                                 item.ngNhapKho + "|" +
                                                 item.tgianNhapKho + "|" +
                                                 item.ngCapNVL + "|" +
                                                 item.tgianCapNVL + "|" +
                                                 item.PDxacnhan + "|" +
                                                 item.tgianxacnhan + "|" +
                                                 item.ngTraNVL + "|" +
                                                 item.tgianTraNVL + "|" +
                                                 item.ghiChuTra + "|" +
                                                 item.ngTraWH + "|" +
                                                 item.tgianTraWH + "|" +
                                                 item.ghiChuTraWH);
                                }
                                sw.Close();
                                fs.Close();

                                //Reset data 
                                dtb1.delete_Transport("OrderWH");
                                _dgv.Columns.Clear();
                                _frm.picBx1.Image = new Bitmap(_strdatabase + "\\Picture\\Default.PNG");
                                _frm.picBx1.SizeMode = PictureBoxSizeMode.StretchImage;
                                _frm.picBx2.Image = new Bitmap(_strdatabase + "\\Picture\\Default.PNG");
                                _frm.picBx2.SizeMode = PictureBoxSizeMode.StretchImage;
                                _frm.picBx1.Visible = true;
                                _frm.lbl_Lot1.Visible = true;
                                _frm.picBx2.Visible = true;
                                _frm.lbl_Lot2.Visible = true;
                                if (_frm.cb_manualInputWk.Checked == true)
                                {
                                    _frm.txt_manualInputWk.Enabled = true;
                                    _frm.txt_manualInputWk.Text = "";
                                    _frm.txt_manualInputWk.Focus();
                                }
                                else
                                {
                                    _frm.txt_autoInputWk.Enabled = true;
                                    _frm.txt_autoInputWk.Text = "";
                                    _frm.txt_autoInputWk.Focus();
                                    _frm.chkMkp_WK = true;
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Xảy ra lỗi cập nhật history NVL!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            #endregion
                        }
                        else
                        {
                            MessageBox.Show("Xảy ra lỗi xuất logfile!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Xảy ra lỗi Update Stock FI-FO!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Xảy ra lỗi Update Stock KTZ!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                      
            }
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
    }
}
