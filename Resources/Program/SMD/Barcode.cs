using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace ManageMaterialPBA
{
    public partial class Barcode : Form
    {
        database_1 dtb1;
        database dtb;
        public string dP = string.Empty;
        public bool btnSave = false, btnPrint = false;
        public bool cfrm = false, cfPrint = false, cfrAdmin = false;
        public bool btnKTZ = false, btnLine = false;
        public string txt = string.Empty;
        public string idCPE, psCPE, strlydo;
        public string xMatrix = string.Empty;
        public string yMatrix = string.Empty;
        public string sizeMatrix = string.Empty;
        public string heightChar = string.Empty;
        public string widthChar = string.Empty;
        public string xRow = string.Empty;
        public string yRow1 = string.Empty;
        public string yRow2 = string.Empty;
        public string yRow3 = string.Empty;
        public string yRow4 = string.Empty;
        public string x128 = string.Empty;
        public string y128 = string.Empty;
        public string size128 = string.Empty;
        public string height128 = string.Empty;
        public string x128Char = string.Empty;
        public string y128Char = string.Empty;
        public string userr = string.Empty, passs = string.Empty;
        public bool idScan = false;
        public string _strdatabase = string.Empty;

        public Barcode(string _user, string _pass, string strdatabase)
        {
            InitializeComponent();
            userr = _user;
            passs = _pass;
            _strdatabase = strdatabase;
        }

        private void Barcode_Load(object sender, EventArgs e)
        {
            dtb1 = new database_1(_strdatabase);
            dtb = new database(_strdatabase);
            DataTable dtNewPass = dtb1.GetNewPass(userr, passs);
            dtb1.DeleteDataNewPass();
            if (dtNewPass.Rows.Count > 0)
            {
                passs = dtNewPass.Rows[0].ItemArray[0].ToString();                
            }

            this.Location = new Point(0, 0);

            toolStripStatusLabel1.Text = dtb.get_name(userr, passs);

            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            gr_Create2.Enabled = true;

            label5.Text = "Code NVL";
            cbx_code.Visible = true;
            cbx_code.Enabled = true;
            txt_linkFile.Visible = false;
            txt_linkFile.Enabled = false;

            //Load ảnh minh họa
            pictureBox1.Image = new Bitmap(_strdatabase + "\\Picture\\MasterTem.PNG");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            //Load config
            StreamReader sr = new StreamReader(_strdatabase + "\\PrinterConfig.ini");
            while (sr.EndOfStream == false)
            {
                string[] str = sr.ReadLine().Split('=');
                foreach (Control x in this.groupBox4.Controls)
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

            //Khóa
            groupBox4.Enabled = false;
            btn_save.Enabled = false;                                   
        }         

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
            {
                inCodeLt();
            }
            else
            {
                incodLik();
            }                  
        }

        private void btn_printDemo_Click(object sender, EventArgs e)
        {
            bool chkSetup = false;
            try
            {
                xMatrix = txt_a1.Text;
                yMatrix = txt_b1.Text;
                sizeMatrix = txt_a0.Text;
                heightChar = txt_b3.Text;
                widthChar = (int.Parse(heightChar) - 5).ToString();
                xRow = txt_a2.Text;
                yRow1 = txt_b2.Text;
                yRow2 = (int.Parse(yRow1) + int.Parse(heightChar) + 10).ToString();
                yRow3 = (int.Parse(yRow2) + int.Parse(heightChar) + 10).ToString();
                yRow4 = (int.Parse(yRow3) + int.Parse(heightChar) + 10).ToString();
                x128 = txt_a3.Text;
                y128 = (int.Parse(yRow2) * 1.2).ToString();
                size128 = txt_a5.Text;
                height128 = (int.Parse(heightChar) * 2.5).ToString();
                x128Char = txt_a4.Text;
                y128Char = txt_b4.Text;
                chkSetup = true;
            }
            catch (Exception)
            {
                chkSetup = false;
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
                MessageBox.Show("Bạn hãy kiểm tra phần thông tin bôi đỏ!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (chkSetup == true)
            {
                if (dtb1.chk_formInput(cbx_code.Text) == true)
                {
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

                    }
                }
                else
                {
                    MessageBox.Show("Sai format code input. Hãy kiểm tra lại!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            //In code tai KTZ stock
            if(btnKTZ == true && cfrm == true)
            {
                try
                {
                    count_timer = 0;
                    btnKTZ = false;
                    cfrm = false;
                    timer1.Stop();
                    StreamReader sr = new StreamReader(_strdatabase + "\\Print\\KTZ\\" + dP + "_NewCode.log");
                    while (sr.EndOfStream == false)
                    {
                        string strP = sr.ReadLine();
                        cbx_code.Items.Add(strP);
                    }
                    sr.Close();
                    dP = string.Empty;
                }
                catch (Exception)
                {
                    MessageBox.Show("Xảy ra lỗi!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }
            //In code tai stock Line
            if (btnLine == true && cfrm == true)
            {
                try
                {
                    count_timer = 0;
                    btnLine = false;
                    cfrm = false;
                    timer1.Stop();
                    StreamReader sr = new StreamReader(_strdatabase + "\\Print\\Line\\" + dP + "_NewCode.log");
                    while (sr.EndOfStream == false)
                    {
                        string strP = sr.ReadLine();
                        cbx_code.Items.Add(strP);
                    }
                    sr.Close();
                    dP = string.Empty;
                }
                catch (Exception)
                {
                    MessageBox.Show("Xảy ra lỗi!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }                
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                groupBox4.Enabled = true;
                btn_save.Enabled = true;               
            }
            else
            {
                groupBox4.Enabled = false;
                btn_save.Enabled = false;
            }   
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                //Luu data
                FileStream fs_p = new FileStream(_strdatabase + "\\Print\\ChangePrinterConfig.ini", FileMode.Create);
                StreamWriter sw_p = new StreamWriter(fs_p);
                foreach (Control x in this.groupBox4.Controls)
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
                foreach (Control x in this.groupBox4.Controls)
                {
                    if (x is TextBox)
                    {
                        string str = ((TextBox)x).Name.Substring(4, 2) + "=" + ((TextBox)x).Text;
                        sw.WriteLine(str);
                    }
                }
                sw.Close();
                fs.Close();

                MessageBox.Show("Lưu file thông số thành công!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Lưu file thông số thất bại!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            for(int i = 0; i < 10; i++)
            {
                if(strBef[i] != strAft[i])
                {
                    string[] tgArr = strBef[i].Split('=');
                    OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                    cnn.Open();
                    string strIn = "Insert Into HistoryPrint Values('" + DateTime.Now.ToShortDateString() + "','" + tgArr[0] + "','" + strBef[i] + "','"
                                                                       + strAft[i] + "','" + strlydo + "','" + dtb.get_name(idCPE, psCPE) +"','"
                                                                       + toolStripStatusLabel1.Text + "','" + DateTime.Now.ToString() + "')";
                    OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                    cmdIn.ExecuteNonQuery();
                    cnn.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cbx_code.Items.Clear();
            cbx_code.Text = "";
            btnKTZ = true;
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockKTZ")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockKTZ stk = new StockKTZ(this, _strdatabase);
                stk.Show();
                timer1.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cbx_code.Items.Clear();
            cbx_code.Text = "";
            btnLine = true;
            //Show Stock Line
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockLine")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockLine s_line = new StockLine(this, _strdatabase);
                s_line.Show();
                timer1.Start();
            }
        }

        public void inCodeLt()
        {
            bool chkSetup = false;
            try
            {
                xMatrix = txt_a1.Text;
                yMatrix = txt_b1.Text;
                sizeMatrix = txt_a0.Text;
                heightChar = txt_b3.Text;
                widthChar = (int.Parse(heightChar) - 5).ToString();
                xRow = txt_a2.Text;
                yRow1 = txt_b2.Text;
                yRow2 = (int.Parse(yRow1) + int.Parse(heightChar) + 10).ToString();
                yRow3 = (int.Parse(yRow2) + int.Parse(heightChar) + 10).ToString();
                yRow4 = (int.Parse(yRow3) + int.Parse(heightChar) + 10).ToString();
                x128 = txt_a3.Text;
                y128 = (int.Parse(yRow2) * 1.2).ToString();
                size128 = txt_a5.Text;
                height128 = (int.Parse(heightChar) * 2.5).ToString();
                x128Char = txt_a4.Text;
                y128Char = txt_b4.Text;
                chkSetup = true;
            }
            catch (Exception)
            {
                chkSetup = false;
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
                MessageBox.Show("Bạn hãy kiểm tra phần thông tin bôi đỏ!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (chkSetup == true)
            {
                if (dtb1.chk_formInput(cbx_code.Text) == true)
                {
                    string[] s1 = cbx_code.Text.Split('+');
                    string s = "^XA\n^FO" + xMatrix + "," + yMatrix + "\n^BXN," + sizeMatrix + ",200\n^FD" + cbx_code.Text + "\n^FS" +
                               "\n^FO" + xRow + "," + yRow1 + "\n^A0," + heightChar + "," + widthChar + "\n^FD" + s1[0] + "\n^FS" +
                               "\n^FO" + xRow + "," + yRow2 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+" + s1[1] + "\n^FS" +
                               "\n^FO" + xRow + "," + yRow3 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+" + s1[2] + "\n^FS" +
                               "\n^FO" + xRow + "," + yRow4 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+" + s1[3] + "\n^FS" +
                               "\n^FO" + x128 + "," + y128 + "\n^BY" + size128 + "^BCN," + height128 + ",N,N,N,N\n^FD" + s1[0] + "\n^FS" +
                               "\n^FO" + x128Char + "," + y128Char + "\n^A0," + (int.Parse(heightChar) * 1.5).ToString() + "," + (int.Parse(widthChar) * 1.5).ToString() + "\n^FD" + s1[0] + "\n^FS" +
                               "\n^XZ";
                    PrintDialog pd = new PrintDialog();
                    pd.PrinterSettings = new PrinterSettings();
                    if (DialogResult.OK == pd.ShowDialog(this))
                    {
                        Clsprint.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);
                    }
                }
                else
                {
                    MessageBox.Show("Sai format code input. Hãy kiểm tra lại!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
        }

        public void incodLik()
        {
            bool chkSetup = false;
            try
            {
                xMatrix = txt_a1.Text;
                yMatrix = txt_b1.Text;
                sizeMatrix = txt_a0.Text;
                heightChar = txt_b3.Text;
                widthChar = (int.Parse(heightChar) - 5).ToString();
                xRow = txt_a2.Text;
                yRow1 = txt_b2.Text;
                yRow2 = (int.Parse(yRow1) + int.Parse(heightChar) + 10).ToString();
                yRow3 = (int.Parse(yRow2) + int.Parse(heightChar) + 10).ToString();
                yRow4 = (int.Parse(yRow3) + int.Parse(heightChar) + 10).ToString();
                x128 = txt_a3.Text;
                y128 = (int.Parse(yRow2) * 1.2).ToString();
                size128 = txt_a5.Text;
                height128 = (int.Parse(heightChar) * 2.5).ToString();
                x128Char = txt_a4.Text;
                y128Char = txt_b4.Text;
                chkSetup = true;
            }
            catch (Exception)
            {
                chkSetup = false;
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
                MessageBox.Show("Bạn hãy kiểm tra phần thông tin bôi đỏ!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (chkSetup == true)
            {
                if(txt_linkFile.Text != "" && File.Exists(txt_linkFile.Text))
                {
                    PrintDialog pd = new PrintDialog();
                    pd.PrinterSettings = new PrinterSettings();
                    if (DialogResult.OK == pd.ShowDialog(this))
                    {
                        StreamReader sr = new StreamReader(txt_linkFile.Text);
                        while (sr.EndOfStream == false)
                        {
                            string tr = sr.ReadLine();
                            if(tr != "" && tr != null)
                            {
                                string[] s1 = tr.Split('+');
                                string s = "^XA\n^FO" + xMatrix + "," + yMatrix + "\n^BXN," + sizeMatrix + ",200\n^FD" + cbx_code.Text + "\n^FS" +
                                           "\n^FO" + xRow + "," + yRow1 + "\n^A0," + heightChar + "," + widthChar + "\n^FD" + s1[0] + "\n^FS" +
                                           "\n^FO" + xRow + "," + yRow2 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+" + s1[1] + "\n^FS" +
                                           "\n^FO" + xRow + "," + yRow3 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+" + s1[2] + "\n^FS" +
                                           "\n^FO" + xRow + "," + yRow4 + "\n^A0," + heightChar + "," + widthChar + "\n^FD+" + s1[3] + "\n^FS" +
                                           "\n^FO" + x128 + "," + y128 + "\n^BY" + size128 + "^BCN," + height128 + ",N,N,N,N\n^FD" + s1[0] + "\n^FS" +
                                           "\n^FO" + x128Char + "," + y128Char + "\n^A0," + (int.Parse(heightChar) * 1.5).ToString() + "," + (int.Parse(widthChar) * 1.5).ToString() + "\n^FD" + s1[0] + "\n^FS" +
                                           "\n^XZ";
                                Thread.Sleep(500);
                                Clsprint.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);
                            }                            
                        }
                    }                           
                }
                else
                {
                    MessageBox.Show("Không tồn tại file. Hãy kiểm tra lại!", "In Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked == true)
            {
                label5.Text = "Link file";
                cbx_code.Visible = false;
                cbx_code.Enabled = false;
                txt_linkFile.Visible = true;
                txt_linkFile.Enabled = true;

                OpenFileDialog opDia = new OpenFileDialog();
                opDia.Title = "In Tem Code";
                opDia.InitialDirectory = @"C:\Users\Administrator\Documents";
                opDia.Filter = "txt |*.log";
                opDia.FilterIndex = 1;
                string fil_name = "";
                if (opDia.ShowDialog() == DialogResult.OK)
                {
                    fil_name = opDia.FileName;
                }

                if (fil_name != "")
                {
                    txt_linkFile.Text = fil_name;
                }
            }
            else
            {
                label5.Text = "Code NVL";
                cbx_code.Visible = true;
                cbx_code.Enabled = true;
                txt_linkFile.Visible = false;
                txt_linkFile.Enabled = false;
            }
        }
    }
}
