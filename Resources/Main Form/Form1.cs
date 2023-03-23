using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;
using ComponentOwl.BetterListView;
using System.Runtime.Serialization.Formatters.Binary;
using AxMSTSCLib;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ManageMaterialPBA
{
    public partial class Form1 : Form
    {
        //Bien form1  
        public string str_database = string.Empty;
        private List<string> axMsRdpcArray = null;
        Ping p = new Ping();
        public static string _user = "";
        string Acc = "";
        string part = string.Empty;
        public int count_Out;
        public int posX;
        public int posY;
        public bool doiRoi = true;
        public string datTim = string.Empty, dMon = string.Empty;
        public bool cfrm = false;        
        //Class
        database_1 dtb1;
        database dtb;
        ClsExcel excel = new ClsExcel();               
        //WH-KTZ
        DataTable dtransWK, dtrans1WK;        
        public bool arrgPerWK;
        public string partEnterWK;
        public int rel_con = 0;
        public bool nhapMkrp = false;
        public bool chkMkp_WK = true;
        //KTZ-PD
        DataTable transportKP, transport1KP; 
        public bool arrgPerKP;
        public string perEnterKP;
        public bool cfrfifo = false, nhapCodeKP = false;
        public string dDay = string.Empty, dShift = string.Empty;
        public bool chkScCode_KP = true;
        public int sttKP = 0;
        //PD confirm
        DataTable dtblePDxn, dtble1PDxn;
        public bool arrgPerPDxn;
        public string perEnterPDxn;
        public bool nhapCodePDxn = false;
        public bool chkScCode_PDxn = true, chkMkp_PDxn = true;
        public int sttPDxn = 0;
        //PD-KTZ
        DataTable tranPK, tran1PK;
        public bool arrgPerPK;
        public string perEnterPK;
        public bool nhapCodePK = false;
        public bool chkScCode_PK = true;
        public int sttPK = 0;
        //KTZ-WH
        DataTable reltranKW, reltran1KW;
        public bool arrgPerKW;
        public string partEnterKW;
        public bool nhapCodeKW = false;
        public bool chkScCode_KW = true;
        public int sttKW = 0;
        //NVL Special
        DataTable solPst, solPst1;
        public bool arrgPerSP;
        public string partEnterSP;
        public bool scCodeSP = false;
        public int sttSP = 0;

        public Form1()
        {
            InitializeComponent();
            Init();
            this.ActiveControl = txt_user;
            axMsRdpcArray = new List<string>();
            datTim = getYearMonthDay();
            dMon = getYearMonth();
            dDay = find_day().ToShortDateString();
            dShift = find_shift();
        }        

        protected void Init()
        {
            btn_change_pw.Hide();
            stl_nameUser.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            str_database = GetLink();
            dtb1 = new database_1(str_database);
            dtb = new database(str_database);
            this.Icon = Properties.Resources.Gakuseisean_Aire_Developer;
            //Control tab
            ((Control)this.KTZlayNVLWH).Enabled = false;
            tabControl1.TabPages.Remove(KTZlayNVLWH);
            ((Control)this.KTZgiaoNVLLine).Enabled = false;
            tabControl1.TabPages.Remove(KTZgiaoNVLLine);
            ((Control)this.Linexacnhan).Enabled = false;
            tabControl1.TabPages.Remove(Linexacnhan);
            ((Control)this.LinetraNVLKTZ).Enabled = false;
            tabControl1.TabPages.Remove(LinetraNVLKTZ);
            ((Control)this.KTZtraNVLWH).Enabled = false;
            tabControl1.TabPages.Remove(KTZtraNVLWH);
            ((Control)this.SpecialMaterial).Enabled = false;
            tabControl1.TabPages.Remove(SpecialMaterial);
            //Khoa them tai khoan
            bOMListToolStripMenuItem.Visible = false;
            sMDToolStripMenuItem1.Enabled = false;
            //Set vi tri cursor
            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            //Check database connect
            OleDbConnection cnn = dtb.GetConnection();
            DataTable dt = new DataTable();
            btn_login.Text = "Đăng nhập";
            if (cnn != null)
            {
                stt_database.ForeColor = Color.Black;
                stt_database.BackColor = Color.Green;
                stt_database.Text = "Database avaiable";
            }
            else
            {
                stt_database.ForeColor = Color.Black;
                stt_database.BackColor = Color.Red;
                stt_database.Text = "Database not avaiable";
            }
            //Folder history
            if (!System.IO.Directory.Exists(str_database + "\\History"))
            {
                System.IO.Directory.CreateDirectory(str_database + "\\History");
            }
            if (!System.IO.Directory.Exists(str_database + "\\History\\In_Out"))
            {
                System.IO.Directory.CreateDirectory(str_database + "\\History\\In_Out");
            }
            if (!System.IO.Directory.Exists(str_database + "\\History\\WH"))
            {
                System.IO.Directory.CreateDirectory(str_database + "\\History\\WH");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void btn_login_Click_1(object sender, EventArgs e)
        {
            string kind = "";
            if (btn_login.Text == "Đăng nhập")
            {
                if (txt_user.Text == "" || txt_pass.Text == "" || txt_IDemploy.Text == "")
                {
                    MessageBox.Show("Tên đăng nhập/mật khẩu/ID bị trống!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (dtb.login_admin(txt_user.Text, txt_pass.Text, txt_IDemploy.Text, kind) == true) // admin đăng nhập
                {
                    bOMListToolStripMenuItem.Visible = true;

                    count_Out = 900;
                    timer2.Start();

                    sMDToolStripMenuItem1.Enabled = true;
                    _user = txt_user.Text;
                    btn_login.Text = "Đăng xuất";
                    btn_change_pw.Show();

                    txt_user.Enabled = false;
                    txt_pass.Enabled = false;
                    txt_IDemploy.Enabled = false;
                    txt_IDemploy.Text = "";
                    stl_nameUser.Text = dtb.get_name(_user, txt_pass.Text);
                    Acc = stl_nameUser.Text;
                    part = "admin";

                    //Thay doi password monthly                   
                    if (Convert.ToDateTime(dtb.get_DateChangepass(txt_user.Text, txt_pass.Text)) < DateTime.Now.AddDays(-30))
                    {
                        sMDToolStripMenuItem1.Enabled = false;
                        doiRoi = false;
                        bool Isopen = false;
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Text == "Change Password")
                            {
                                Isopen = true;
                                f.BringToFront();
                                break;
                            }
                        }
                        if (Isopen == false)
                        {
                            Change_pw changPw = new Change_pw(this, txt_user.Text, str_database);
                            changPw.Show();
                        }
                    }
                }
                else if (dtb.login_manager(txt_user.Text, txt_pass.Text, txt_IDemploy.Text, kind) == true) // manager đăng nhập
                {
                    count_Out = 900;
                    timer2.Start();

                    sMDToolStripMenuItem1.Enabled = true;
                    _user = txt_user.Text;
                    btn_login.Text = "Đăng xuất";
                    btn_change_pw.Show();

                    txt_user.Enabled = false;
                    txt_pass.Enabled = false;
                    txt_IDemploy.Enabled = false;
                    txt_IDemploy.Text = "";
                    stl_nameUser.Text = dtb.get_name(_user, txt_pass.Text);
                    Acc = stl_nameUser.Text;
                   part = "manager";

                    //Thay doi password monthly
                    if (Convert.ToDateTime(dtb.get_DateChangepass(txt_user.Text, txt_pass.Text)) < DateTime.Now.AddDays(-30))
                    {
                        doiRoi = false;
                        sMDToolStripMenuItem1.Enabled = false;
                        bool Isopen = false;
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Text == "Change Password")
                            {
                                Isopen = true;
                                f.BringToFront();
                                break;
                            }
                        }
                        if (Isopen == false)
                        {
                            Change_pw changPw = new Change_pw(this, txt_user.Text, str_database);
                            changPw.Show();
                        }
                    }

                }
                else if (dtb.login_part(txt_user.Text, txt_pass.Text, txt_IDemploy.Text, "CPE") == true) // user đăng nhập
                {
                    count_Out = 900;
                    timer2.Start();

                    sMDToolStripMenuItem1.Enabled = true;
                    _user = txt_user.Text;
                    btn_login.Text = "Đăng xuất";
                    btn_change_pw.Show();

                    txt_user.Enabled = false;
                    txt_pass.Enabled = false;
                    txt_IDemploy.Enabled = false;
                    txt_IDemploy.Text = "";
                    stl_nameUser.Text = dtb.get_name(_user, txt_pass.Text);
                    part = "CPE";
                    Acc = stl_nameUser.Text;

                    //Thay doi password monthly
                    if (Convert.ToDateTime(dtb.get_DateChangepass(txt_user.Text, txt_pass.Text)) < DateTime.Now.AddDays(-30))
                    {
                        sMDToolStripMenuItem1.Enabled = false;
                        doiRoi = false;
                        bool Isopen = false;
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Text == "Change Password")
                            {
                                Isopen = true;
                                f.BringToFront();
                                break;
                            }
                        }
                        if (Isopen == false)
                        {
                            Change_pw changPw = new Change_pw(this, txt_user.Text, str_database);
                            changPw.Show();
                        }
                    }
                }
                else if (dtb.login_part(txt_user.Text, txt_pass.Text, txt_IDemploy.Text, "PD") == true) // user đăng nhập
                {
                    count_Out = 900;
                    timer2.Start();

                    sMDToolStripMenuItem1.Enabled = true;
                    _user = txt_user.Text;
                    btn_login.Text = "Đăng xuất";
                    btn_change_pw.Show();

                    txt_user.Enabled = false;
                    txt_pass.Enabled = false;
                    txt_IDemploy.Enabled = false;
                    txt_IDemploy.Text = "";
                    part = "PD";
                    stl_nameUser.Text = dtb.get_name(_user, txt_pass.Text);
                    Acc = stl_nameUser.Text;

                    //Thay doi password monthly
                    if (Convert.ToDateTime(dtb.get_DateChangepass(txt_user.Text, txt_pass.Text)) < DateTime.Now.AddDays(-30))
                    {
                        doiRoi = false;
                        sMDToolStripMenuItem1.Enabled = false;
                        bool Isopen = false;
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Text == "Change Password")
                            {
                                Isopen = true;
                                f.BringToFront();
                                break;
                            }
                        }
                        if (Isopen == false)
                        {
                            Change_pw changPw = new Change_pw(this, txt_user.Text, str_database);
                            changPw.Show();
                        }
                    }
                }
                else if (dtb.login_part(txt_user.Text, txt_pass.Text, txt_IDemploy.Text, "KTZ") == true) // user đăng nhập
                {
                    count_Out = 900;
                    timer2.Start();

                    sMDToolStripMenuItem1.Enabled = true;
                    _user = txt_user.Text;
                    btn_login.Text = "Đăng xuất";
                    btn_change_pw.Show();

                    txt_user.Enabled = false;
                    txt_pass.Enabled = false;
                    txt_IDemploy.Enabled = false;
                    txt_IDemploy.Text = "";
                    part = "KTZ";
                    stl_nameUser.Text = dtb.get_name(_user, txt_pass.Text);
                    Acc = stl_nameUser.Text;


                    //Thay doi password monthly
                    if (Convert.ToDateTime(dtb.get_DateChangepass(txt_user.Text, txt_pass.Text)) < DateTime.Now.AddDays(-30))
                    {
                        doiRoi = false;
                        sMDToolStripMenuItem1.Enabled = false;
                        bool Isopen = false;
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Text == "Change Password")
                            {
                                Isopen = true;
                                f.BringToFront();
                                break;
                            }
                        }
                        if (Isopen == false)
                        {
                            Change_pw changPw = new Change_pw(this, txt_user.Text, str_database);
                            changPw.Show();
                        }
                    }
                }
                else if (dtb.login_part(txt_user.Text, txt_pass.Text, txt_IDemploy.Text, "QA") == true) // user đăng nhập
                {
                    count_Out = 900;
                    timer2.Start();

                    sMDToolStripMenuItem1.Enabled = true;
                    _user = txt_user.Text;
                    btn_login.Text = "Đăng xuất";
                    btn_change_pw.Show();

                    txt_user.Enabled = false;
                    txt_pass.Enabled = false;
                    txt_IDemploy.Enabled = false;
                    txt_IDemploy.Text = "";
                    part = "QA";
                    stl_nameUser.Text = dtb.get_name(_user, txt_pass.Text);
                    Acc = stl_nameUser.Text;

                    //Thay doi password monthly
                    if (Convert.ToDateTime(dtb.get_DateChangepass(txt_user.Text, txt_pass.Text)) < DateTime.Now.AddDays(-30))
                    {
                        sMDToolStripMenuItem1.Enabled = false;
                        doiRoi = false;
                        bool Isopen = false;
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Text == "Change Password")
                            {
                                Isopen = true;
                                f.BringToFront();
                                break;
                            }
                        }
                        if (Isopen == false)
                        {
                            Change_pw changPw = new Change_pw(this, txt_user.Text, str_database);
                            changPw.Show();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập/mật khẩu/ID. Hãy thử lại!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                bOMListToolStripMenuItem.Visible = false;
                sMDToolStripMenuItem1.Enabled = false;
                txt_user.Enabled = true;
                txt_pass.Enabled = true;
                txt_IDemploy.Enabled = true;
                btn_login.Text = "Đăng nhập";
                txt_user.Text = "";
                txt_user.Focus();
                txt_pass.Text = "";
                txt_IDemploy.Text = "";
                _user = "";
                btn_change_pw.Hide();
                part = string.Empty;
                Init();
                ((Control)this.KTZlayNVLWH).Enabled = false;
                tabControl1.TabPages.Remove(KTZlayNVLWH);
                ((Control)this.KTZgiaoNVLLine).Enabled = false;
                tabControl1.TabPages.Remove(KTZgiaoNVLLine);
                ((Control)this.Linexacnhan).Enabled = false;
                tabControl1.TabPages.Remove(Linexacnhan);
                ((Control)this.LinetraNVLKTZ).Enabled = false;
                tabControl1.TabPages.Remove(LinetraNVLKTZ);
                ((Control)this.KTZtraNVLWH).Enabled = false;
                tabControl1.TabPages.Remove(KTZtraNVLWH);
            }
        }

        #region Congcu
        private void bOMListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "BOM")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                BOM bom = new BOM(txt_user.Text, txt_pass.Text, str_database);
                bom.Show();
            }
        }

        private void addAccountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "AddUser")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                AddUser addUser = new AddUser(this, str_database);
                addUser.Show();
            }
        }

        private void deleteAccountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "DeleteUser")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                DeleteUser deleteUser = new DeleteUser(this, str_database);
                deleteUser.Show();
            }
        }

        private void btn_change_pw_Click_1(object sender, EventArgs e)
        {
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "Change Password")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                Change_pw changPw = new Change_pw(this, txt_user.Text, str_database);
                changPw.Show();
                txt_pass.Text = "";
            }
        }

        private void truyXuấtLịchSửToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "TraceHistory")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                TraceHistory hs = new TraceHistory(str_database);
                hs.Show();
            }
        }

        private void kiểmTraStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockNVL")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockNVL stk = new StockNVL(str_database);
                stk.Show();
            }
        }
        #endregion

        #region Chuongtrinh
        private void inoutMaterialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if ((part == "admin" || part == "manager" || part == "CPE"))
            //{
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "Barcode")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    Barcode ab = new Barcode(txt_user.Text, txt_pass.Text, str_database);
                    ab.Show();
                }              
            //}
            //else
            //{
            //    MessageBox.Show("Bạn không có quyền truy cập hạng mục này\nHoặc trang dữ liệu đã được mở rồi!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}           
        }

        private void inCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((part == "admin" || part == "manager" || part == "KTZ") && ((Control)this.KTZlayNVLWH).Enabled == false)
            {
                ((Control)this.KTZlayNVLWH).Enabled = true;
                tabControl1.TabPages.Add(KTZlayNVLWH);
                tabControl1.SelectedTab = tabControl1.TabPages["KTZlayNVLWH"];
                //Load model
                dtb1.get_cbbModel("Model", "Model_name", cbx_Model_OWK);
                //hide input manual
                txt_manualInputWk.Hide();
                txt_manualInputWk.Enabled = false;
                btn_inputManualWk.Hide();
                btn_inputManualWk.Enabled = false;
                //con trỏ chuột đếm tgian out
                posX = Cursor.Position.X;
                posY = Cursor.Position.Y;
                //Get thông tin khi đổi password
                DataTable dtNewPass = dtb1.GetNewPass(txt_user.Text, txt_pass.Text);
                dtb1.DeleteDataNewPass();
                if (dtNewPass.Rows.Count > 0)
                {
                    partEnterWK = dtb1.get_PerLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString(), "part");                    
                    arrgPerWK = dtb1.get_RightLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString());                   
                }
                else
                {
                    partEnterWK = dtb1.get_PerLogin(txt_user.Text, txt_pass.Text, "part");
                    arrgPerWK = dtb1.get_RightLogin(txt_user.Text, txt_pass.Text);
                }
                //hinh anh
                picBx1.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBx1.SizeMode = PictureBoxSizeMode.StretchImage;
                picBx2.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBx2.SizeMode = PictureBoxSizeMode.StretchImage;
                //History folder
                if (!System.IO.Directory.Exists(str_database + "\\History\\WH\\Order\\" + dMon))
                {
                    System.IO.Directory.CreateDirectory(str_database + "\\History\\WH\\Order\\" + dMon);
                }
                //clear dgv
                dgv_WH_Ktz.Columns.Clear();
            }     
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập hạng mục này\nHoặc trang dữ liệu đã được mở rồi!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void wHKTZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((part == "admin" || part == "manager" || part == "KTZ") && ((Control)this.KTZgiaoNVLLine).Enabled == false)
            {
                sttKP = 0;
                ((Control)this.KTZgiaoNVLLine).Enabled = true;
                tabControl1.TabPages.Add(KTZgiaoNVLLine);
                tabControl1.SelectedTab = tabControl1.TabPages["KTZgiaoNVLLine"];
                //Khoa PO
                rbt_NewPO.Checked = false;
                rbt_OldPO.Checked = false;
                //Load data model, PD
                dtb1.get_cbbModel("Model", "Model_name", cbx_ModelKP);
                dtb1.get_part(cbx_PD, "PD");
                //Hide input manual
                txt_manualInputKp.Hide();
                txt_manualInputKp.Enabled = false;
                btn_enterKp.Hide();
                btn_enterKp.Enabled = false;
                //con trỏ chuột đếm tgian out
                posX = Cursor.Position.X;
                posY = Cursor.Position.Y;
                //Get thông tin khi đổi password
                DataTable dtNewPass = dtb1.GetNewPass(txt_user.Text, txt_pass.Text);
                dtb1.DeleteDataNewPass();
                if (dtNewPass.Rows.Count > 0)
                {
                    arrgPerKP = dtb1.get_RightLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString());
                    perEnterKP = dtb1.get_PerLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString(), "part");                    
                }
                else
                {
                    arrgPerKP = dtb1.get_RightLogin(txt_user.Text, txt_pass.Text);
                    perEnterKP = dtb1.get_PerLogin(txt_user.Text, txt_pass.Text, "part");                    
                }
                //Hinh anh
                picBx1KP.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBx1KP.SizeMode = PictureBoxSizeMode.StretchImage;
                picBx2KP.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBx2KP.SizeMode = PictureBoxSizeMode.StretchImage; 
                //Hien thi stock line
                DataTable dt_sl = dtb1.search_stock("KtzGiaoPd1", true);
                dtb1.show_StockLinee(dgv_viewStkLine, dt_sl);
                dgv_viewStkLine.Columns["Ngay_thang"].Visible = false;
                dgv_viewStkLine.Columns["Ca_kip"].Visible = false;
                dgv_viewStkLine.Columns["Line"].Visible = false;
                dgv_viewStkLine.Columns["Model"].Visible = false;
                dgv_viewStkLine.Columns["KTZ"].Visible = false;
                dgv_viewStkLine.Columns["PD"].Visible = false; 
                //History folder
                if (!System.IO.Directory.Exists(str_database + "\\History\\In_Out\\Ktz_to_PD\\" + dMon))
                {
                    System.IO.Directory.CreateDirectory(str_database + "\\History\\In_Out\\Ktz_to_PD\\" + dMon);
                }
                //clear dgv
                dgv_Ktz_Pd.Columns.Clear();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập hạng mục này\nHoặc trang dữ liệu đã được mở rồi!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }              

        private void lineKTZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((part == "admin" || part == "manager" || part == "PD") && ((Control)this.Linexacnhan).Enabled == false)
            {
                sttPDxn = 0;
                ((Control)this.Linexacnhan).Enabled = true;
                tabControl1.TabPages.Add(Linexacnhan);
                tabControl1.SelectedTab = tabControl1.TabPages["Linexacnhan"];
                //Load data model, KTZ
                dtb1.get_cbbModel("Model", "Model_name", cbx_modelPDxn);
                dtb1.get_part(cbx_KTZ_PDxn, "KTZ");
                //Hide input manual
                txt_inpManulPDxn.Hide();
                txt_inpManulPDxn.Enabled = false;
                btn_enterPDxn.Hide();
                btn_enterPDxn.Enabled = false;
                txt_inpAutoPDxn.Enabled = false;
                //con trỏ chuột đếm tgian out
                posX = Cursor.Position.X;
                posY = Cursor.Position.Y;
                //Get thông tin khi đổi password
                DataTable dtNewPass = dtb1.GetNewPass(txt_user.Text, txt_pass.Text);
                dtb1.DeleteDataNewPass();
                if (dtNewPass.Rows.Count > 0)
                {
                    arrgPerPDxn = dtb1.get_RightLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString());
                    perEnterPDxn = dtb1.get_PerLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString(), "part");
                }
                else
                {
                    arrgPerPDxn = dtb1.get_RightLogin(txt_user.Text, txt_pass.Text);
                    perEnterPDxn = dtb1.get_PerLogin(txt_user.Text, txt_pass.Text, "part");
                }
                //hinh anh
                picPDxn1.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picPDxn1.SizeMode = PictureBoxSizeMode.StretchImage;
                picPDxn2.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picPDxn2.SizeMode = PictureBoxSizeMode.StretchImage;
                //Hien thi stock line
                DataTable dt_sl3 = dtb1.search_stock("KtzGiaoPd1", true);
                dtb1.show_StockLinee(dgv_stkLinePDxn, dt_sl3);
                dgv_stkLinePDxn.Columns["Ngay_thang"].Visible = false;
                dgv_stkLinePDxn.Columns["Ca_kip"].Visible = false;
                dgv_stkLinePDxn.Columns["Line"].Visible = false;
                dgv_stkLinePDxn.Columns["Model"].Visible = false;
                dgv_stkLinePDxn.Columns["KTZ"].Visible = false;
                dgv_stkLinePDxn.Columns["PD"].Visible = false; 
                //History folder
                if (!System.IO.Directory.Exists(str_database + "\\History\\In_Out\\PD_xacnhan\\" + dMon))
                {
                    System.IO.Directory.CreateDirectory(str_database + "\\History\\In_Out\\PD_xacnhan\\" + dMon);
                }
                //clear dgv
                dgv_PDxn.Columns.Clear();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập hạng mục này\nHoặc trang dữ liệu đã được mở rồi!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kTZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((part == "admin" || part == "manager" || part == "PD") && ((Control)this.LinetraNVLKTZ).Enabled == false)
            {
                sttPK = 0;
                ((Control)this.LinetraNVLKTZ).Enabled = true;
                tabControl1.TabPages.Add(LinetraNVLKTZ);
                tabControl1.SelectedTab = tabControl1.TabPages["LinetraNVLKTZ"];
                //Load data model, KTZ
                dtb1.get_cbbModel("Model", "Model_name", cbx_ModelPK);
                dtb1.get_part(cbx_Ktz, "KTZ");
                //Hide input manual
                txt_manualInputPk.Hide();
                txt_manualInputPk.Enabled = false;
                btn_EnterPK.Hide();
                btn_EnterPK.Enabled = false;
                //con trỏ chuột đếm tgian out
                posX = Cursor.Position.X;
                posY = Cursor.Position.Y;
                //Get thông tin khi đổi password
                DataTable dtNewPass = dtb1.GetNewPass(txt_user.Text, txt_pass.Text);
                dtb1.DeleteDataNewPass();
                if (dtNewPass.Rows.Count > 0)
                {
                    arrgPerPK = dtb1.get_RightLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString());
                    perEnterPK = dtb1.get_PerLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString(), "part");
                }
                else
                {
                    arrgPerPK = dtb1.get_RightLogin(txt_user.Text, txt_pass.Text);
                    perEnterPK = dtb1.get_PerLogin(txt_user.Text, txt_pass.Text, "part");
                }
                //hinh anh
                picBx1PK.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBx1PK.SizeMode = PictureBoxSizeMode.StretchImage;
                picBx2PK.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBx2PK.SizeMode = PictureBoxSizeMode.StretchImage;
                //Hien thi stock line
                DataTable dt_sl2 = dtb1.search_stock("KtzGiaoPd1", true);
                dtb1.show_StockLinee(dgv_stkLine, dt_sl2);
                dgv_stkLine.Columns["Ngay_thang"].Visible = false;
                dgv_stkLine.Columns["Ca_kip"].Visible = false;
                dgv_stkLine.Columns["Line"].Visible = false;
                dgv_stkLine.Columns["Model"].Visible = false;
                dgv_stkLine.Columns["KTZ"].Visible = false;
                dgv_stkLine.Columns["PD"].Visible = false;
                //History folder
                if (!System.IO.Directory.Exists(str_database + "\\History\\In_Out\\PD_return_Ktz\\" + dMon))
                {
                    System.IO.Directory.CreateDirectory(str_database + "\\History\\In_Out\\PD_return_Ktz\\" + dMon);
                }
                //clear dgv
                dgv_Pd_Ktz.Columns.Clear();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập hạng mục này\nHoặc trang dữ liệu đã được mở rồi!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kTZTrảNVLWHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((part == "admin" || part == "manager" || part == "KTZ") && ((Control)this.KTZtraNVLWH).Enabled == false)
            {
                sttKW = 0;
                ((Control)this.KTZtraNVLWH).Enabled = true;
                tabControl1.TabPages.Add(KTZtraNVLWH);
                tabControl1.SelectedTab = tabControl1.TabPages["KTZtraNVLWH"];
                //Load data model
                dtb1.get_cbbModel("Model", "Model_name", cbx_reMol);
                //Hide input manual
                txt_manualInputKw.Hide();
                txt_manualInputKw.Enabled = false;
                btn_enterCodeKw.Hide();
                btn_enterCodeKw.Enabled = false;
                //con trỏ chuột đếm tgian out
                posX = Cursor.Position.X;
                posY = Cursor.Position.Y;
                //Get thông tin khi đổi password
                DataTable dtNewPass = dtb1.GetNewPass(txt_user.Text, txt_pass.Text);
                dtb1.DeleteDataNewPass();
                if (dtNewPass.Rows.Count > 0)
                {
                    partEnterKW = dtb1.get_PerLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString(), "part");
                    arrgPerKW = dtb1.get_RightLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString());
                }
                else
                {
                    partEnterKW = dtb1.get_PerLogin(txt_user.Text, txt_pass.Text, "part");
                    arrgPerKW = dtb1.get_RightLogin(txt_user.Text, txt_pass.Text);      
                }
                //hinh anh
                picBoxRe1.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBoxRe1.SizeMode = PictureBoxSizeMode.StretchImage;
                picBoxRe2.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBoxRe2.SizeMode = PictureBoxSizeMode.StretchImage;
                //History folder
                if (!System.IO.Directory.Exists(str_database + "\\History\\WH\\Return\\" + dMon))
                {
                    System.IO.Directory.CreateDirectory(str_database + "\\History\\WH\\Return\\" + dMon);
                }
                //clear dgv
                dgv_returnWH.Columns.Clear();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập hạng mục này\nHoặc trang dữ liệu đã được mở rồi!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_clsWHKTZ_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            ((Control)this.KTZlayNVLWH).Enabled = false;
            tabControl1.TabPages.Remove(KTZlayNVLWH);
            cbx_Model_OWK.Text = "";
            txtQtRow.Text = "";
            txt_manualInputWk.Text = "";
            txt_autoInputWk.Text = "";
            cb_manualInputWk.Checked = false;
            rbtnReload.Checked = false;
            chkMkp_WK = true;
            dgv_WH_Ktz.Columns.Clear();
        }

        private void btn_clsKTZgiaoLine_Click(object sender, EventArgs e)
        {
            ((Control)this.KTZgiaoNVLLine).Enabled = false;
            tabControl1.TabPages.Remove(KTZgiaoNVLLine);
            cbx_ModelKP.Text = "";
            cbx_PD.Text = "";         
            txt_manualInputKp.Text = "";
            txt_autoInputKp.Text = "";
            cb_inputCodeKp.Checked = false;
            radbtn_reLoadKP.Checked = false;
            chkScCode_KP = true;
            dgv_Ktz_Pd.Columns.Clear();
        }

        private void btn_clsLinexn_Click(object sender, EventArgs e)
        {
            ((Control)this.Linexacnhan).Enabled = false;
            tabControl1.TabPages.Remove(Linexacnhan);
            cbx_modelPDxn.Text = "";
            cbx_KTZ_PDxn.Text = "";
            txt_scnCodePDxn.Text = "";
            txt_inpManulPDxn.Text = "";
            txt_inpAutoPDxn.Text = "";
            chb_nhaptayPDxn.Checked = false;
            rtb_reloadPDxn.Checked = false;
            chkScCode_PDxn = true;
            chkMkp_PDxn = true;
            dgv_PDxn.Columns.Clear();
        }

        private void btn_clsLinetraKTZ_Click(object sender, EventArgs e)
        {
            ((Control)this.LinetraNVLKTZ).Enabled = false;
            tabControl1.TabPages.Remove(LinetraNVLKTZ);
            cbx_ModelPK.Text = "";
            cbx_Ktz.Text = "";
            txt_manualInputPk.Text = "";
            txt_autoInputPk.Text = "";
            cb_inputPk.Checked = false;
            radbtn_reLoadPK.Checked = false;
            chkScCode_PK = true;
            dgv_Pd_Ktz.Columns.Clear();
        }

        private void btn_clsKTZtraWH_Click(object sender, EventArgs e)
        {
            ((Control)this.KTZtraNVLWH).Enabled = false;
            tabControl1.TabPages.Remove(KTZtraNVLWH);
            cbx_reMol.Text = "";
            txt_manualInputKw.Text = "";
            txt_autoInputKw.Text = "";
            cb_manualInputKw.Checked = false;
            rdbtn_reloadRe.Checked = false;
            chkScCode_KW = true;
            dgv_returnWH.Columns.Clear();
        }       
        #endregion        

        #region Control_PC 
        private void timer2_Tick(object sender, EventArgs e)
        {
            count_Out--;
            toolStripStatusLabel1.Text = count_Out.ToString();
            if (count_Out == 0)
            {             
                //WH-KTZ   
                if (((Control)this.KTZlayNVLWH).Enabled == true)
                {
                    if (dgv_WH_Ktz.Columns.Count > 0)
                    {
                        excel.ExportTxt(dgv_WH_Ktz, str_database + "\\tem\\" + cbx_Model_OWK.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadWH-KTZ.txt");
                    }
                    //close tab
                    ((Control)this.KTZlayNVLWH).Enabled = false;
                    tabControl1.TabPages.Remove(KTZlayNVLWH);
                }
                //KTZ-PD
                if (((Control)this.KTZgiaoNVLLine).Enabled == true)
                {
                    if (dgv_Ktz_Pd.Columns.Count > 0)
                    {
                        excel.ExportTxt(dgv_Ktz_Pd, str_database + "\\tem\\" + cbx_ModelKP.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadKTZ-PD.txt");
                    }
                    //close tab
                    ((Control)this.KTZgiaoNVLLine).Enabled = false;
                    tabControl1.TabPages.Remove(KTZgiaoNVLLine);
                }
                //PD confirm
                if (((Control)this.Linexacnhan).Enabled == true)
                {
                    if (dgv_PDxn.Columns.Count > 0)
                    {
                        excel.ExportTxt(dgv_PDxn, str_database + "\\tem\\" + cbx_modelPDxn.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPDxacnhan.txt");
                    }
                    //close tab
                    ((Control)this.Linexacnhan).Enabled = false;
                    tabControl1.TabPages.Remove(Linexacnhan);
                }
                //PD-KTZ
                if (((Control)this.LinetraNVLKTZ).Enabled == true)
                {
                    if (dgv_Pd_Ktz.Columns.Count > 0)
                    {
                        excel.ExportTxt(dgv_Pd_Ktz, str_database + "\\tem\\" + cbx_ModelPK.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPD-KTZ.txt");
                    }
                    //close tab
                    ((Control)this.LinetraNVLKTZ).Enabled = false;
                    tabControl1.TabPages.Remove(LinetraNVLKTZ);
                }
                //KTZ-WH
                if (((Control)this.KTZtraNVLWH).Enabled == true)
                {
                    if (dgv_returnWH.Columns.Count > 0)
                    {
                        excel.ExportTxt(dgv_returnWH, str_database + "\\tem\\" + cbx_reMol.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadReturnWH.txt");
                    }
                    //close tab
                    ((Control)this.KTZtraNVLWH).Enabled = false;
                    tabControl1.TabPages.Remove(KTZtraNVLWH);
                }
                //NVL Special   
                if (((Control)this.SpecialMaterial).Enabled == true)
                {
                    if (dgv_SoPst.Columns.Count > 0)
                    {
                        excel.ExportTxt(dgv_SoPst, str_database + "\\tem\\" + cbx_molSoPst.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "Reload_" + cbx_NvlNam.Text + ".txt");
                    }
                    //close tab
                    ((Control)this.SpecialMaterial).Enabled = false;
                    tabControl1.TabPages.Remove(SpecialMaterial);
                }

                //txt_user.Enabled = true;
                //txt_pass.Enabled = true;
                //txt_IDemploy.Enabled = true;
                //btn_login.Text = "Đăng nhập";
                //txt_user.Text = "";
                //txt_pass.Text = "";
                //txt_IDemploy.Text = "";
                //txt_user.Focus();
                //AcceptButton = btn_login;
                //_user = "";
                //btn_change_pw.Hide();
                //part = string.Empty;
                //timer2.Stop();
                //Init();
                this.Close();
            }
        }

        public void active_Form()
        {
            timer2.Start();
        }

        private const int WF_MOUSEMOVE = 0x0200;
        public bool PreFilterMess(ref Message m)
        {
            bool act = m.Msg == 0x100 || m.Msg == 0x101;
            act = act || m.Msg == 0x10;
            if (act)
            {
                active_Form();
            }
            return false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private async void stt_database_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(500);
            if (stt_database.Text != "")
            {
                if (stt_database.Text == "Database not avaiable")
                {
                    MessageBox.Show("Lỗi kết nối database, chương trình không thể hoạt động.\nHãy khởi động lại chương trình!", "Chú ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dOENewModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (doiRoi == false)
            {
                MessageBox.Show("Đăng nhập lại và đổi mật khẩu!", "Thay đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion   
                                                           
        public string GetLink()
        {           
            string str = string.Empty;
            StreamReader sr_Link = new StreamReader(@Application.StartupPath + "\\Link_Database.txt");
            while(sr_Link.EndOfStream == false)
            {
                string strSr = sr_Link.ReadLine();
                if(strSr != "" && strSr != null)
                {
                    str = strSr;
                }
            }
            return str;
        }
                
        public string[] inFWK = new string[5];
        public bool xnCPE = false;
        public string mtrlWK = string.Empty, codWK = string.Empty, mkWK = string.Empty, mkpWK = string.Empty, qtyRllWK = string.Empty;
        public string[] get_InfInpWK(DataTable dt, TextBox mkprtRaw, CheckBox typeInput)
        {
            if (mkprtRaw.Text != "")
            {               
                int havMakprt = 0;
                bool chkSub = false;
                string strSubB = string.Empty, strSubA = string.Empty;
                foreach (DataRow dtr in dt.Rows)
                {
                    if (mkprtRaw.Text == dtr[5].ToString() || mkprtRaw.Text == dtr[8].ToString())
                    {
                        havMakprt++;
                        inFWK[0] = dtr[2].ToString();//material
                        inFWK[1] = dtr[3].ToString();//code
                        inFWK[2] = dtr[4].ToString();//maker
                        inFWK[3] = dtr[5].ToString();//maker part
                        inFWK[4] = dtr[7].ToString();//Qty in one Roll
                        chkSub = false;
                    }
                    else if (mkprtRaw.Text.Contains(dtr[5].ToString()))//maker part thua ky tu
                    {
                        havMakprt++;
                        int start = mkprtRaw.Text.IndexOf(dtr[5].ToString(), 0);
                        strSubB = mkprtRaw.Text.Substring(0, start);
                        strSubA = mkprtRaw.Text.Substring(start + dtr[5].ToString().Length, mkprtRaw.Text.Length - (start + dtr[5].ToString().Length));
                        mtrlWK = dtr[2].ToString();//material
                        codWK = dtr[3].ToString();//code
                        mkWK = dtr[4].ToString();//maker
                        mkpWK = dtr[5].ToString();//maker part
                        qtyRllWK = dtr[7].ToString();//Qty in one Roll
                        chkSub = true;
                    }
                    else//maker part thieu ky tu
                    {
                        
                    }
                }
                if (havMakprt == 0)
                {
                    if (typeInput.Checked == false)
                    {
                        mkprtRaw.Text = "";
                    }
                    MessageBox.Show("Không tìm được maker part trong BOM \nHoặc maker part chưa được update vào BOM!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkMkp_WK = true;
                }
                else if (havMakprt > 1)
                {
                    Array.Clear(inFWK, 0, inFWK.Length);
                    mtrlWK = string.Empty;
                    codWK = string.Empty;
                    mkWK = string.Empty;
                    mkpWK = string.Empty; 
                    qtyRllWK = string.Empty;
                    if (typeInput.Checked == false)
                    {
                        mkprtRaw.Text = "";
                    }
                    MessageBox.Show("Trùng maker part. Thông báo CPE kiểm tra lại BOM!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkMkp_WK = true;
                }
                else
                {
                    if(chkSub == true)
                    {
                        DialogResult rels = MessageBox.Show("Maker part thừa ký tự : " + strSubB + ", " + strSubA + "\nLiên hệ CPE xác nhận?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (rels == DialogResult.Yes)
                        {
                            xnCPE = true;
                            bool Isopen = false;
                            foreach (Form f in Application.OpenForms)
                            {
                                if (f.Text == "ConfirmNVL")
                                {
                                    Isopen = true;
                                    f.BringToFront();
                                    break;
                                }
                            }
                            if (Isopen == false)
                            {
                                ConfirmNVL cfmNVL = new ConfirmNVL(this, strSubB, strSubA, mtrlWK, codWK, mkWK, mkpWK, qtyRllWK, cbx_Model_OWK.Text, mkprtRaw.Text);
                                cfmNVL.Show();
                                timer1.Enabled = true;
                                count_timer = 0;
                                timer1.Start();
                            }
                        }
                        else
                        {                           
                            if (typeInput.Checked == false)
                            {
                                mkprtRaw.Text = "";
                            }
                            chkMkp_WK = true;
                        }
                    }
                }

                return inFWK;
            }
            else
            {
                string[] inEmmpty = new string[5];
                return inEmmpty;
            }
        }

        public string[] get_InfInpKP(DataTable dt, TextBox mkprtRaw, CheckBox typeInput)
        {
            if (mkprtRaw.Text != "")
            {
                string[] inF = new string[5];
                int havMakprt = 0;
                foreach (DataRow dtr in dt.Rows)
                {
                    string[] temCod = mkprtRaw.Text.Split('+');
                    if (temCod[0] == dtr[1].ToString() && temCod[1] == dtr[5].ToString() && temCod[2] == dtr[3].ToString() && temCod[3] == dtr[4].ToString())
                    {
                        havMakprt++;
                        inF[0] = dtr[0].ToString();//material
                        inF[1] = dtr[1].ToString();//code
                        inF[2] = dtr[2].ToString();//maker
                        inF[3] = dtr[3].ToString();//maker part
                        inF[4] = dtr[6].ToString();//Qty in one Roll
                    }
                }
                if (havMakprt == 0)
                {
                    if (typeInput.Checked == false)
                    {
                        mkprtRaw.Text = "";
                    }
                    MessageBox.Show("Không tìm mã NVL " + mkprtRaw.Text.Substring(0, 11) + " trong stock FIFO!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkScCode_KP = true;
                }
                else if (havMakprt > 1)
                {
                    Array.Clear(inF, 0, inF.Length);
                    if (typeInput.Checked == false)
                    {
                        mkprtRaw.Text = "";
                    }
                    MessageBox.Show("Trùng tem code. Thông báo CPE kiểm tra lại stock FIFO!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkScCode_KP = true;
                }
                else
                { }

                return inF;
            }
            else
            {
                string[] inEmmpty = new string[6];
                return inEmmpty;
            }
        }

        public string[] get_InfInpKW(DataTable dt, TextBox mkprtRaw, CheckBox typeInput)
        {
            if (mkprtRaw.Text != "")
            {
                string[] inF = new string[5];
                int havMakprt = 0;
                foreach (DataRow dtr in dt.Rows)
                {
                    string[] temCod = mkprtRaw.Text.Split('+');
                    if (temCod[0] == dtr[1].ToString() && temCod[1] == dtr[5].ToString() && temCod[2] == dtr[3].ToString() && temCod[3] == dtr[4].ToString())
                    {
                        havMakprt++;
                        inF[0] = dtr[0].ToString();//material
                        inF[1] = dtr[1].ToString();//code
                        inF[2] = dtr[2].ToString();//maker
                        inF[3] = dtr[3].ToString();//maker part
                        inF[4] = dtr[6].ToString();//Qty in one Roll
                    }
                }
                if (havMakprt == 0)
                {
                    if (typeInput.Checked == false)
                    {
                        mkprtRaw.Text = "";
                    }
                    MessageBox.Show("Không tìm mã NVL " + mkprtRaw.Text.Substring(0, 11) + " trong stock FIFO!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkScCode_KW = true;
                }
                else if (havMakprt > 1)
                {
                    Array.Clear(inF, 0, inF.Length);
                    if (typeInput.Checked == false)
                    {
                        mkprtRaw.Text = "";
                    }
                    MessageBox.Show("Trùng tem code. Thông báo CPE kiểm tra lại stock FIFO!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkScCode_KW = true;
                }
                else
                { }

                return inF;
            }
            else
            {
                string[] inEmmpty = new string[6];
                return inEmmpty;
            }
        }

        public string[] get_InfInpPDxn(DataTable dt, string code, string mkp, TextBox txtmkp, TextBox txtcode, CheckBox typeInput)
        {
            if (txtmkp.Text != "" && txtcode.Text != "")
            {
                string[] inF = new string[5];
                int havMakprt = 0;

                foreach (DataRow dtr in dt.Rows)
                {
                    if (code == dtr[3].ToString() && txtmkp.Text.Contains(dtr[5].ToString()))
                    {
                        havMakprt++;
                        inF[0] = dtr[2].ToString();//Material
                        inF[1] = dtr[3].ToString();//code
                        inF[2] = dtr[4].ToString();//maker
                        inF[3] = dtr[5].ToString();//makerpart
                        inF[4] = dtr[7].ToString();//Qty in one Roll
                    }
                }

                if (havMakprt == 0)
                {                   
                    if (typeInput.Checked == false)//auto
                    {
                        txtmkp.Text = "";
                        txtmkp.Enabled = false;
                        txtcode.Text = "";
                        txtcode.Focus();
                        chkScCode_PDxn = true;
                        chkMkp_PDxn = true;  
                    }
                    else
                    {
                        txtmkp.Text = "";
                        txtmkp.Enabled = false;
                        txtcode.Text = "";
                        txtcode.Focus();
                        chkScCode_PDxn = true;
                    }
                    MessageBox.Show("Xảy ra lỗi :\n1. Không tìm được maker part trong BOM \n2. KTZ đã dán sai tem Code vào cuộn liệu!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                     
                }
                else if (havMakprt > 1)
                {
                    Array.Clear(inF, 0, inF.Length);
                    if (typeInput.Checked == false)//auto
                    {
                        txtmkp.Text = "";
                        txtmkp.Enabled = false;
                        txtcode.Text = "";
                        txtcode.Focus();
                        chkScCode_PDxn = true;
                        chkMkp_PDxn = true;
                    }
                    else
                    {
                        txtmkp.Text = "";
                        txtmkp.Enabled = false;
                        txtcode.Text = "";
                        txtcode.Focus();
                        chkScCode_PDxn = true;
                    }
                    MessageBox.Show("Trùng maker part. Thông báo CPE kiểm tra lại BOM!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                { }

                return inF;
            }
            else
            {
                string[] inEmpty = new string[5];
                return inEmpty;
            }
        }

        public string[] get_InfInpSP(DataTable dt,  string model, string tenNvl, TextBox codeWH, TextBox mkr)
        {
            if (codeWH.Text != "" && mkr.Text != "")
            {
                string[] inF = new string[5];
                int havMakprt = 0, codeSame = 0, makerSame = 0;
                foreach (DataRow dtr in dt.Rows)
                {
                    if (codeWH.Text.Contains(dtr[4].ToString()) && mkr.Text == dtr[3].ToString())
                    {
                        havMakprt++;
                        inF[0] = dtr[0].ToString();//Model
                        inF[1] = dtr[1].ToString();//Ten NVL
                        inF[2] = dtr[2].ToString();//maker
                        inF[3] = dtr[3].ToString();//maker detail
                        inF[4] = codeWH.Text;//code
                    }
                    if(codeWH.Text.Contains(dtr[4].ToString()))
                    {
                        codeSame++;
                    }
                    if(mkr.Text == dtr[3].ToString())
                    {
                        makerSame++;
                    }
                }

                if (havMakprt == 0)
                {
                    codeWH.Text = "";
                    mkr.Text = "";
                    mkr.Enabled = false;
                    if (codeSame == 0 && makerSame == 0)
                    {
                        MessageBox.Show("Xảy ra lỗi :\nKhông tìm được NVL trong BOM \nVà WH đã dán sai tem Code vào NVL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if(codeSame != 0 && makerSame == 0)
                    {
                        MessageBox.Show("Xảy ra lỗi : \nKhông tìm được NVL trong BOM!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if(codeSame == 0 && makerSame != 0)
                    {
                        MessageBox.Show("Xảy ra lỗi :\nWH đã dán sai tem Code vào NVL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (havMakprt > 1)
                {
                    Array.Clear(inF, 0, inF.Length);
                    codeWH.Text = "";
                    mkr.Text = "";
                    mkr.Enabled = false;
                    MessageBox.Show("Trùng maker part. Thông báo CPE kiểm tra lại BOM!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                { }

                return inF;
            }
            else
            {
                string[] inEmpty = new string[5];
                return inEmpty;
            }
        }

        public string[] get_InfInpPK(DataTable dt, string code, string mkp, TextBox txt1, CheckBox typeInput)
        {
            if (txt1.Text != "")
            {
                string[] inF = new string[5];
                int havMakprt = 0;

                foreach (DataRow dtr in dt.Rows)
                {
                    if (code == dtr[3].ToString() && mkp == dtr[5].ToString())
                    {
                        havMakprt++;
                        inF[0] = dtr[2].ToString();//Material
                        inF[1] = dtr[3].ToString();//code
                        inF[2] = dtr[4].ToString();//maker
                        inF[3] = dtr[5].ToString();//makerpart
                        inF[4] = dtr[7].ToString();//Qty in one Roll
                    }
                }

                if (havMakprt == 0)
                {
                    if (typeInput.Checked == false)
                    {
                        txt1.Text = "";
                    }
                    MessageBox.Show("Không tìm được maker part trong BOM \nHoặc maker part chưa được update vào BOM!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkScCode_PK = true;
                }
                else if (havMakprt > 1)
                {
                    Array.Clear(inF, 0, inF.Length);
                    if (typeInput.Checked == false)
                    {
                        txt1.Text = "";
                    }
                    MessageBox.Show("Trùng maker part. Thông báo CPE kiểm tra lại BOM!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    chkScCode_PK = true;
                }
                else
                { }

                return inF;
            }
            else
            {
                string[] inEmpty = new string[5];
                return inEmpty;
            }
        }        

        public string find_hour()
        {
            string hour_cap = string.Empty;
            hour_cap = DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
            return hour_cap;
        }

        public bool TimeBetween(DateTime time, DateTime startDateTime, DateTime endDateTime)
        {
            // get TimeSpan
            TimeSpan start = new TimeSpan(startDateTime.Hour, startDateTime.Minute, 0);
            TimeSpan end = new TimeSpan(endDateTime.Hour, endDateTime.Minute, 0);

            // convert datetime to a TimeSpan
            TimeSpan now = time.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        public string find_shift()
        {
            string shift;
            DateTime dateTime = DateTime.Now;

            DateTime startDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 8, 0, 0);
            DateTime endDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 20, 0, 0);

            if (TimeBetween(dateTime, startDateTime, endDateTime))
            {
                shift = "Ngày";
            }
            else
            {
                shift = "Đêm";
            }
            return shift;
        }

        public DateTime find_day()
        {
            DateTime dateTime = DateTime.Now;

            DateTime startDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 20, 0, 1);
            DateTime endDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);

            TimeSpan start = new TimeSpan(startDateTime.Hour, startDateTime.Minute, startDateTime.Second);
            TimeSpan end = new TimeSpan(endDateTime.Hour, endDateTime.Minute, endDateTime.Second);

            // convert datetime to a TimeSpan
            TimeSpan now = dateTime.TimeOfDay;

            if (start <= now && now <= end)
            {
                dateTime = DateTime.Now.AddDays(1);
            }
            else
            {
                dateTime = DateTime.Now;
            }
            return dateTime;
        }

        public string getYearMonthDay()
        {
            string str = string.Empty;
            if (DateTime.Now.Month < 10)
            {
                if (DateTime.Now.Day < 10)
                {
                    str = DateTime.Now.Year.ToString() + "-0" + DateTime.Now.Month.ToString() + "-0" + DateTime.Now.Day.ToString();
                }
                else
                {
                    str = DateTime.Now.Year.ToString() + "-0" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                }
            }
            else
            {
                if (DateTime.Now.Day < 10)
                {
                    str = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-0" + DateTime.Now.Day.ToString();
                }
                else
                {
                    str = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                }
            }          
            return str;
        }

        public string getYearMonth()
        {
            string str = string.Empty;
            if (DateTime.Now.Month < 10)
                str = DateTime.Now.Year.ToString() + "-0" + DateTime.Now.Month.ToString();
            else
                str = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString();
            return str;
        }

        public int count_timer = 0;
        public bool chkWKAgain = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Stop timer
            count_timer++;
            if (count_timer == 300)
            {
                timer1.Stop();
            }
            //WH-KTZ nhap tay
            if (cfrm == true && nhapMkrp == true)
            {
                count_timer = 0;
                nhapMkrp = false;
                cfrm = false;
                timer1.Stop();

                txt_manualInputWk.Visible = true;
                txt_manualInputWk.Enabled = true;
                txt_manualInputWk.Focus();
                btn_inputManualWk.Visible = true;
                btn_inputManualWk.Enabled = true;
                lbl_inputCodeWk.Text = "Nhập MakerPart";
            }
            //WH-KTZ xác nhận Maker part NVL
            if(cfrm == true && xnCPE == true)
            {
                count_timer = 0;
                cfrm = false;
                xnCPE = false;
                timer1.Stop();

                if (cb_manualInputWk.Checked == true)
                {
                    chkWKAgain = true;
                    chek_input(txt_manualInputWk);
                }
                else
                {
                    chkWKAgain = true;
                    chek_input(txt_autoInputWk);
                }
            }
            //KTZ-PD nhap tay
            if (cfrm == true && nhapCodeKP == true)
            {
                count_timer = 0;
                cfrm = false;
                nhapCodeKP = false;
                timer1.Stop();

                txt_manualInputKp.Visible = true;
                txt_manualInputKp.Enabled = true;
                txt_manualInputKp.Focus();
                btn_enterKp.Visible = true;
                btn_enterKp.Enabled = true;
                lbl_inputKp.Text = "Nhập Code";
            }
            //KTZ-PD xac nhan InOut no FI-FO
            if (cfrfifo == true)
            {
                count_timer = 0;
                cfrfifo = false;
                timer1.Stop();

                if (cb_inputCodeKp.Checked == true)
                {
                    input_KP(txt_manualInputKp, true);
                }
                else
                {
                    input_KP(txt_autoInputKp, true);
                }
            }
            //PDxannhan nhap tay
            if (cfrm == true && nhapCodePDxn == true)
            {
                count_timer = 0;
                cfrm = false;
                nhapCodePDxn = false;
                timer1.Stop();
             
                txt_inpManulPDxn.Visible = true;
                txt_inpManulPDxn.Enabled = true;
                txt_inpManulPDxn.Text = "";
                txt_inpManulPDxn.Focus();
                txt_inpAutoPDxn.Visible = false;
                txt_inpAutoPDxn.Enabled = false;
                txt_inpAutoPDxn.Text = "";
                btn_enterPDxn.Visible = true;
                btn_enterPDxn.Enabled = true;
                lbl_inputPDxn.Text = "Nhập MakerPart";
            }
            //PD-KTZ nhap tay
            if (cfrm == true && nhapCodePK == true)
            {
                count_timer = 0;
                cfrm = false;
                nhapCodePK = false;
                timer1.Stop();

                txt_manualInputPk.Visible = true;
                txt_manualInputPk.Enabled = true;
                txt_manualInputPk.Focus();
                btn_EnterPK.Visible = true;
                btn_EnterPK.Enabled = true;
                lbl_inputCodePK.Text = "Nhập Code";
            }
            //KTZ-WH nhap tay
            if(cfrm == true && nhapCodeKW == true)
            {
                count_timer = 0;
                cfrm = false;
                nhapCodeKW = false;
                timer1.Stop();

                txt_manualInputKw.Visible = true;
                txt_manualInputKw.Enabled = true;
                txt_manualInputKw.Focus();
                btn_enterCodeKw.Visible = true;
                btn_enterCodeKw.Enabled = true;
                lbl_inputCodeKw.Text = "Nhập Code";
            }
        }

        private void timer_reLoad_Tick(object sender, EventArgs e)
        {
            //WH-KTZ
            if (((Control)this.KTZlayNVLWH).Enabled == true)
            {
                if (dgv_WH_Ktz.Columns.Count > 0)
                {
                    excel.ExportTxt(dgv_WH_Ktz, str_database + "\\tem\\" + cbx_Model_OWK.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadWH-KTZ.txt");
                }
            }
            //KTZ-PD
            if(((Control)this.KTZgiaoNVLLine).Enabled == true)
            {
                if(dgv_Ktz_Pd.Columns.Count > 0)
                {
                    excel.ExportTxt(dgv_Ktz_Pd, str_database + "\\tem\\" + cbx_ModelKP.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadKTZ-PD.txt");
                }               
            }
            //PD confirm
            if(((Control)this.Linexacnhan).Enabled == true)
            {
                if(dgv_PDxn.Columns.Count > 0)
                {
                    excel.ExportTxt(dgv_PDxn, str_database + "\\tem\\" + cbx_modelPDxn.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPDxacnhan.txt");
                }
            }            
            //PD-KTZ
            if(((Control)this.LinetraNVLKTZ).Enabled == true)
            {
                if(dgv_Pd_Ktz.Columns.Count > 0)
                {
                    excel.ExportTxt(dgv_Pd_Ktz, str_database + "\\tem\\" + cbx_ModelPK.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPD-KTZ.txt");
                }
            }
            //KTZ-WH
            if(((Control)this.KTZtraNVLWH).Enabled == true)
            {
                if(dgv_returnWH.Columns.Count > 0)
                {
                    excel.ExportTxt(dgv_returnWH, str_database + "\\tem\\" + cbx_reMol.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadReturnWH.txt");
                }
            }
            //NVL Special
            if (((Control)this.SpecialMaterial).Enabled == true)
            {
                if (dgv_SoPst.Columns.Count > 0)
                {
                    excel.ExportTxt(dgv_SoPst, str_database + "\\tem\\" + cbx_molSoPst.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "Reload_" + cbx_NvlNam.Text + ".txt");
                }
            }
            tool_saving.BackColor = Color.White;
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

        //=============================================================WH-KTZ===============================================================================
        #region WH-KTZ
        private void cbx_Model_OWK_SelectedIndexChanged(object sender, EventArgs e)
        {            
            dtb1.delete_Transport("OrderWH");
            dgv_WH_Ktz.Columns.Clear();

            dtransWK = dtb1.loadtransportWH(cbx_Model_OWK.Text);
            txtQtRow.Focus(); 
        }

        private async void txt_autoInputWk_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            if (txt_autoInputWk.Text != "" && chkMkp_WK == true)
            {
                chkMkp_WK = false;
                if (cbx_Model_OWK.Text == "")
                {
                    MessageBox.Show("Bạn chưa chọn Model!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_autoInputWk.Text = "";
                    cbx_Model_OWK.Focus();
                    chkMkp_WK = true;
                }
                else
                {
                    txt_manualInputWk.Text = "";
                    if (arrgPerWK == false)//OP
                    {
                        timer_reLoad.Start();
                        //Gọi hàm kiểm tra thông tin input 
                        chkWKAgain = false;
                        chek_input(txt_autoInputWk); 
                    }
                    else//admin, manager
                    {
                        DialogResult rel_ar = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "OrderWH", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rel_ar == DialogResult.OK)
                        {
                            timer_reLoad.Start();
                            //Gọi hàm kiểm tra thông tin input
                            chkWKAgain = false;
                            chek_input(txt_autoInputWk); 
                        }
                    }
                }
            }
        }

        private void btn_inputManualWk_Click(object sender, EventArgs e)
        {
            if (txt_autoInputWk.Text == "")
            {                
                if (cbx_Model_OWK.Text == "" || txt_manualInputWk.Text == "")
                {
                    MessageBox.Show("Hãy kiểm tra lại thông tin Model/Input!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbx_Model_OWK.Focus();
                }
                else
                {
                    if (arrgPerWK == false)
                    {
                        timer_reLoad.Start();
                        //Gọi hàm check thông tin input 
                        chkWKAgain = false;
                        chek_input(txt_manualInputWk);                        
                    }
                    else
                    {
                        DialogResult rel_ar = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "OrderWH", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rel_ar == DialogResult.OK)
                        {
                            timer_reLoad.Start();
                            //Gọi hàm kiểm tra thông tin input
                            chkWKAgain = false;
                            chek_input(txt_manualInputWk);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy xóa mục Scan Code trước khi Enter code tay!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public int qtyInRoll = 0;
        public void chek_input(TextBox txt)
        {
            Clipboard.Clear();
            //Biến convert dòng
            rel_con = 0;
            bool chk_con = false;
            //Biến dòng khi nhảy Lot
            int iR = 0;
            //Biến báo đã có code trong DataGirdView trùng
            bool havCodSm = false;
            //Convert txt Dong
            chk_con = int.TryParse(txtQtRow.Text, out rel_con);
            if (chk_con == false)
            {
                rel_con = 1;
            }

            //Lọc từ BOM theo makerpart, nếu trùng all thông tin > 2 lần trong dataTable -> báo PE xác nhận 
            string[] infFromBom = new string[5];
            if(chkWKAgain == false)
            {
                infFromBom = get_InfInpWK(dtransWK, txt, cb_manualInputWk);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    infFromBom[i] = inFWK[i];
                }
                Array.Clear(inFWK, 0, inFWK.Length);
                dtransWK = dtb1.loadtransportWH(cbx_Model_OWK.Text);
            }
            
            if (infFromBom[0] != null && infFromBom[1] != null && infFromBom[2] != null && infFromBom[3] != null && infFromBom[4] != null)
            {       
                //Điền data vào datagridview
                if (dgv_WH_Ktz.Columns.Count == 0 || dgv_WH_Ktz.Rows.Count == 0)//dgv chua co data
                {
                    dtb1.insert_transOrderWH2v2(cbx_Model_OWK.Text, infFromBom[0], infFromBom[1], infFromBom[2], infFromBom[3], "", infFromBom[4], "", stl_nameUser.Text, rel_con);
                    txtQtRow.Text = "";
                    txt.Enabled = false;
                    havCodSm = true;
                    dtrans1WK = dtb1.loadtransport_tableWH(cbx_Model_OWK.Text, "OrderWH");
                }   
                else
                {
                    MessageBox.Show("Chương trình đang xảy ra lỗi process. Liên hệ ngay CPE", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    chkMkp_WK = true;
                }

                if (havCodSm == true)
                {
                    //Hiển thị
                    dgv_WH_Ktz.Columns.Clear();
                    dtb1.show_OrderWH(dgv_WH_Ktz, dtrans1WK);                 
                    //Not sort
                    foreach (DataGridViewColumn col in dgv_WH_Ktz.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    //Qty in roll
                    qtyInRoll = int.Parse(infFromBom[4]);
                    //Nhảy chuột về lot
                    //for (int i = 0; i < dgv_WH_Ktz.RowCount - 1; i++)
                    //{
                    //    if (dgv_WH_Ktz.Rows[i].Cells["Lot"].Value.ToString() == "")
                    //    {
                    //        dgv_WH_Ktz.Rows[i].Cells["Lot"].Selected = true;
                    //        iR = dgv_WH_Ktz.SelectedCells[0].OwningRow.Index;
                    //        dgv_WH_Ktz.CurrentCell = dgv_WH_Ktz["Lot", iR];
                    //        dgv_WH_Ktz.BeginEdit(true);
                    //    }
                    //}
                    if (dgv_WH_Ktz.Rows[0].Cells["Lot"].Value.ToString() == "")
                    {
                        dgv_WH_Ktz.Rows[0].Cells["Lot"].Selected = true;
                        iR = dgv_WH_Ktz.SelectedCells[0].OwningRow.Index;
                        dgv_WH_Ktz.CurrentCell = dgv_WH_Ktz["Lot", iR];
                        dgv_WH_Ktz.BeginEdit(true);
                    }

                    string strIma1 = string.Empty;
                    string strIma2 = string.Empty;
                    #region
                    switch (infFromBom[2])
                    {
                        case "RENESAS":
                            strIma1 = infFromBom[2] + "1";
                            strIma2 = infFromBom[2] + "2";
                            break;

                        case "STMICRO":
                            strIma1 = infFromBom[2] + "1";
                            strIma2 = infFromBom[2] + "2";
                            break;

                        case "TI":
                            strIma1 = infFromBom[2] + "1";
                            strIma2 = infFromBom[2] + "2";
                            break;
                        default:
                            strIma1 = infFromBom[2];
                            strIma2 = string.Empty;
                            break;
                    }
                    #endregion

                    //Hiển thị hình ảnh tiêu chuẩn Lot           
                    picBx1.Visible = true;
                    lbl_Lot1.Visible = true;

                    picBx1.Image = new Bitmap(str_database + "\\Picture\\" + strIma1 + ".PNG");
                    picBx1.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (strIma2 != string.Empty)
                    {
                        picBx2.Visible = true;
                        lbl_Lot2.Visible = true;
                        picBx2.Image = new Bitmap(str_database + "\\Picture\\" + strIma2 + ".PNG");
                        picBx2.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        picBx2.Visible = false;
                        lbl_Lot2.Visible = false;
                    }
                    havCodSm = false;
                    //txt.Text = "";
                }
                else
                {
                    //if (cb_manualInputWk.Checked == false)
                    //{
                    //    txt.Text = "";
                    //}
                }
            }
        }

        private void cb_manualInputWk_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_manualInputWk.Checked == true)
            {
                AcceptButton = btn_inputManualWk;
                if (arrgPerWK == true)
                {
                    txt_manualInputWk.Visible = true;                    
                    txt_manualInputWk.Enabled = true;
                    txt_manualInputWk.Focus();
                    btn_inputManualWk.Visible = true;
                    btn_inputManualWk.Enabled = true;
                    lbl_inputCodeWk.Text = "Nhập MakerPart";
                }
                else
                {
                    nhapMkrp = true;
                    bool Isopen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "ConfirmWHKTZ")
                        {
                            Isopen = true;
                            f.BringToFront();
                            break;
                        }
                    }
                    if (Isopen == false)
                    {
                        ConfirmWHKTZ confirmAd = new ConfirmWHKTZ(this);
                        confirmAd.Show();
                        count_timer = 0;
                        timer1.Start();
                    }
                }
            }
            else
            {
                txt_manualInputWk.Hide();
                txt_manualInputWk.Enabled = false;
                txt_manualInputWk.Text = "";
                txt_autoInputWk.Focus();
                btn_inputManualWk.Visible = false;
                btn_inputManualWk.Enabled = false;
                lbl_inputCodeWk.Text = "Scan MakerPart";
            }
        }

        private void rbtnReload_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnReload.Checked == true)
            {
                try
                {
                    DataTable dt_reload = new DataTable();
                    string strPath = string.Empty;                    
                    strPath = str_database + "\\tem\\" + cbx_Model_OWK.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadWH-KTZ.txt";
                    
                    StreamReader sr = new StreamReader(strPath);
                    string[] colName = sr.ReadLine().Split(',');
                    for (int j = 0; j < colName.Length - 1; j++)
                    {
                        dt_reload.Columns.Add(colName[j]);
                    }

                    string newLine;
                    while ((newLine = sr.ReadLine()) != null)
                    {
                        DataRow dtr = dt_reload.NewRow();
                        string[] values = newLine.Split(',');
                        if (values[0] != "")
                        {
                            for (int i = 0; i < values.Length - 1; i++)
                            {
                                dtr[i] = values[i];
                            }
                            dt_reload.Rows.Add(dtr);
                        }
                    }
                    sr.Close();

                    dgv_WH_Ktz.Columns.Clear();
                    dtb1.show_OrderWH(dgv_WH_Ktz, dt_reload);                   
                    //Not sort
                    foreach (DataGridViewColumn col in dgv_WH_Ktz.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    //reset
                    if (cb_manualInputWk.Checked == true)
                    {
                        txt_manualInputWk.Enabled = false;
                    }
                    else
                    {
                        txt_autoInputWk.Enabled = false;
                    }
                    rbtnReload.Checked = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Data Re-load trống. Hãy tiếp tục thao tác!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rbtnReload.Checked = false;
                }
            }
        }

        private void btn_deleteOWH_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Array.Clear(inFWK, 0, inFWK.Length);
            try
            {               
                int t = dgv_WH_Ktz.CurrentRow.Index;
                int y = dgv_WH_Ktz.CurrentCell.RowIndex;
                if (t >= 0 || y >= 0)
                {
                    int a = 0;
                    if (txtQtRow.Text == "")
                    {
                        a = 1;
                    }
                    else
                    {
                        a = int.Parse(txtQtRow.Text);
                    }
                    //string[] str = dgv_WH_Ktz.Rows[t].Cells["Tem_code"].Value.ToString().Split('+');
                    //if(str.Length == 4)
                    //{
                    //    string strFifo = str[0] + "+" +
                    //                 dgv_WH_Ktz.Rows[t].Cells["Maker"].Value.ToString() + "+" +
                    //                 str[2] + "+" +
                    //                 str[3] + "+" +
                    //                 str[1];
                    //    dtb1.del_filLog("FI-FO", strFifo, a);
                    //    dtb1.del_filLog("NewCode", dgv_WH_Ktz.Rows[t].Cells["Tem_code"].Value.ToString(), a);
                    //}                   

                    for (int i = 0; i < a; i++)
                    {
                        DataRow drDel = dtrans1WK.Rows[t];
                        dtrans1WK.Rows.Remove(drDel);
                        //Xoa database
                        dtb1.delete_Transport("OrderWH");
                        //Enable txt
                        if (dgv_WH_Ktz.RowCount == 0)
                        {
                            if (cb_manualInputWk.Checked == true)
                            {
                                txt_manualInputWk.Enabled = true;
                                txt_manualInputWk.Focus();
                            }
                            else
                            {
                                txt_autoInputWk.Enabled = true;
                                txt_autoInputWk.Focus();
                            }
                        }
                    }
                    txtQtRow.Text = "";
                }
                if(dgv_WH_Ktz.RowCount == 0)
                {
                    txt_autoInputWk.Text = "";
                    chkMkp_WK = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Click vào ô bất kỳ đó để xóa!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_ConfirmWHKtz_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Array.Clear(inFWK, 0, inFWK.Length);
            //In new code
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "NewCode")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                NewCode newCode = new NewCode(dgv_WH_Ktz, txt_user.Text, txt_pass.Text, cbx_Model_OWK.Text, stl_nameUser.Text, part, dMon, datTim, this, str_database);
                newCode.Show();
            }            
            //Reset data
            rbtnReload.Checked = false;
            timer_reLoad.Stop();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            //Hiển thị new form
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockKTZ2")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockKTZ2 stk2 = new StockKTZ2(str_database);
                stk2.Show();
            }
        }

        private void btn_ktra1_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            if ((DateTime.Compare(dTiPic1WK.Value, dTiPic2WK.Value) > 0) || (cbx_Model_OWK.Text.Length == 0))
            {
                MessageBox.Show("Hãy xem lại Model/thời gian bạn muốn kiểm tra!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string[] hisroryCheck = GetHistory(dTiPic1WK, dTiPic2WK);
                //Tìm history theo ngày -> tổng hợp -> Hiển thị Excel
                //Get file name trong khoảng time đã chọn
                int num_filOk = 0;
                int num_file = dtb1.count_file(str_database + "\\History\\WH\\Order\\", hisroryCheck);
                string[] nam_file = dtb1.get_filOK(num_file, str_database + "\\History\\WH\\Order\\", hisroryCheck, cbx_Model_OWK.Text, dTiPic1WK.Text, dTiPic2WK.Text, num_filOk);
                //Sprire.XLS -> merge all file 
                //Open file merged(save tạm ra đâu đó)
                //Save as file merged nếu muốn
                dtb1.merg_Excel(str_database + "\\History\\WH\\Order\\", hisroryCheck, nam_file, nam_file.Length, datTim, "WH-KTZ", true, str_database);
            } 
        }

        public string[] GetHistory(DateTimePicker dtp1, DateTimePicker dtp2)
        {
            int qtyMonth = (dtp2.Value.Year - dtp1.Value.Year) * 12 + (dtp2.Value.Month - dtp1.Value.Month) + 1;
            string[] historyCheck = new string[qtyMonth];

            if (dtp1.Value.Year == dtp2.Value.Year)
            {
                int n = 0;
                for (int i = dtp1.Value.Month; i <= dtp2.Value.Month; i++)
                {
                    historyCheck[n] = dtp1.Value.Year.ToString() + "-" + i.ToString("00");
                    n++;
                }
            }
            else
            {
                bool chkYear = false;
                int newYear = 0;
                for (int j = 0; j < qtyMonth; j++)
                {
                    DateTime dt = dtp1.Value.AddMonths(j);
                    if (dt.Year == dtp2.Value.Year && dt.Month > dtp2.Value.Month)
                    {
                        break;
                    }

                    if (dt.Month == 12)
                    {
                        chkYear = true;
                        newYear++;
                    }
                    if ((chkYear == true && dt.Month != 12) || (newYear > 0))
                    {
                        historyCheck[j] = dt.Year.ToString() + "-" + dt.Month.ToString("00");
                    }
                    else
                    {
                        historyCheck[j] = dtp1.Value.Year.ToString() + "-" + dt.Month.ToString("00");
                    }
                }
            }
            return historyCheck;
        }

        public int qtyAct = 0, iqcTest = 0; 
        private void dgv_WH_Ktz_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Thread.Sleep(100);
            int column = dgv_WH_Ktz.CurrentCell.ColumnIndex;            
            if (column == 8 && dgv_WH_Ktz.CurrentRow.Cells["IQC_test"].Value.ToString() != "")
            {
                qtyAct = int.Parse(dgv_WH_Ktz.CurrentRow.Cells["So_luong_nhap"].Value.ToString());
                bool chk = int.TryParse(dgv_WH_Ktz.CurrentRow.Cells["IQC_test"].Value.ToString(), out iqcTest);
                if (chk == true)
                {
                    if (iqcTest > 0)
                    {
                        dgv_WH_Ktz.CurrentRow.Cells["So_luong_nhap"].Value = (qtyInRoll - iqcTest).ToString();
                    }
                    else
                    {

                        dgv_WH_Ktz.CurrentRow.Cells["So_luong_nhap"].Value = (qtyInRoll).ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Mục IQC_test phải nhập là số nguyên dương!", "OrderWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (column == 8 && dgv_WH_Ktz.CurrentRow.Cells["IQC_test"].Value.ToString() == "")
            {
                dgv_WH_Ktz.CurrentRow.Cells["So_luong_nhap"].Value = (qtyInRoll).ToString();
            }
        }

        public string crtUpperChar(string strChar)
        {
            string nStr = string.Empty;
            foreach (char c in strChar)
            {
                nStr = nStr + char.ToUpper(c);
            }
            return nStr;
        }

        private void dgv_WH_Ktz_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                  
        }

        private void dgv_WH_Ktz_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Thread.Sleep(100);
            int column = dgv_WH_Ktz.CurrentCell.ColumnIndex;
            if (column == 5 && dgv_WH_Ktz.CurrentRow.Cells["Lot"].Value.ToString() != "")
            {
                dgv_WH_Ktz.CurrentRow.Cells["Lot"].Value = crtUpperChar(dgv_WH_Ktz.CurrentRow.Cells["Lot"].Value.ToString());
                dgv_WH_Ktz.CurrentRow.Cells["Tem_Code"].Value = dgv_WH_Ktz.CurrentRow.Cells["Ma_NVL"].Value.ToString() + "+" + dtb1.get_time() + "+" + dgv_WH_Ktz.CurrentRow.Cells["Maker_Part"].Value.ToString() + "+" + dgv_WH_Ktz.CurrentRow.Cells["Lot"].Value.ToString();
            }                          
        }

        private void lbl_Lot1_Click(object sender, EventArgs e)
        {
            if (arrgPerWK == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerWK, str_database);
                    infPicture.Show();
                }
            }
        }

        private void lbl_Lot2_Click(object sender, EventArgs e)
        {
            if (arrgPerWK == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerWK, str_database);
                    infPicture.Show();
                }
            }
        }

        private void cbx_Model_OWK_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        } 

        private void txt_manualInputWk_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_autoInputWk_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_inputManualWk_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_deleteOWH_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_ConfirmWHKtz_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btnExport_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_ktra1_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void dgv_WH_Ktz_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }
        #endregion

        //=============================================================KTZ-PD===============================================================================
        #region KTZ-PD
        private void cbx_Model_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_PD.Text = "";
            dtb1.delete_Transport("KtzGiaoPd1_Per");
            dtb1.delete_Transport("Ktz_Pd_tranfer");
            dgv_Ktz_Pd.Columns.Clear();

            string[] strMolRun = dtb1.get_modelRun();
            int errMolRun = 0;
            if (strMolRun[0] != "none")
            {
                for (int i = 0; i < strMolRun.Length; i++)
                {
                    if (cbx_ModelKP.Text == strMolRun[i])
                    {
                        errMolRun = 0;
                    }
                    else
                    {
                        errMolRun++;
                    }
                }
                if (errMolRun == 0)
                {
                    //transportKP = dtb1.LoadBOM(cbx_ModelKP.Text);
                }
                else
                {
                    MessageBox.Show("Bạn đang chọn sai Model. Line đang chạy Model khác!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbx_ModelKP.Text = "";
                }
            }
            else
            {
                //transportKP = dtb1.LoadBOM(cbx_ModelKP.Text);
            }             
        }

        private void rbt_NewPO_CheckedChanged(object sender, EventArgs e)
        {
            if (rbt_NewPO.Checked == true)
            {
                if (dtb1.get_NVLLine("KtzGiaoPd1") == false)
                {
                    MessageBox.Show("NVL của PO cũ vẫn còn tồn trên Line, chưa thể chọn PO mới!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rbt_OldPO.Checked = true;
                }
                else if (dtb1.get_NVLLine("PDxacnhanStock") == false)
                {
                    MessageBox.Show("NVL cấp ra Line chưa được PD xác nhận, chưa thể chọn PO mới!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rbt_OldPO.Checked = true;
                }
                else
                {
                    rbt_NewPO.Checked = true;
                }
            }
        }

        private void rbt_OldPO_CheckedChanged(object sender, EventArgs e)
        {
            if (rbt_OldPO.Checked == true)
            {
                string[] strModelRun = dtb1.get_modelRun();
                int errModelRun = 0;
                if (strModelRun[0] != "none")
                {
                    for (int i = 0; i < strModelRun.Length; i++)
                    {
                        if (cbx_ModelKP.Text == strModelRun[i])
                        {
                            errModelRun = 0;
                        }
                        else
                        {
                            errModelRun++;
                        }
                    }
                    if (errModelRun != 0)
                    {
                        MessageBox.Show("Model của PO đang chạy khác NVL đang tồn trên Line!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        rbt_OldPO.Checked = true;
                    }
                }
                else
                {
                    if(dtb1.get_NVLLine("PDxacnhanStock") == false)
                    {
                        rbt_OldPO.Checked = true;
                    }
                    else
                    {
                        MessageBox.Show("Không có NVL đang tồn trên Line!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rbt_NewPO.Checked = true;
                    }                    
                }
            }            
        }

        private void cbx_PD_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_autoInputKp.Focus();
        }

        private async void txt_autoInputKp_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(1500);
            if (txt_autoInputKp.Text != "" && chkScCode_KP == true)
            {
                chkScCode_KP = false;
                if ((cbx_ModelKP.Text == "") || (rbt_NewPO.Checked == false && rbt_OldPO.Checked == false) || cbx_PD.Text == "")
                {
                    MessageBox.Show("Bạn điền thiếu thông tin Model/PO/PD!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_autoInputKp.Text = "";
                    cbx_ModelKP.Focus();
                    chkScCode_KP = true;
                }
                else
                {
                    txt_manualInputKp.Text = "";
                    //Check double Input
                    if (dtb1.chekNewCodeInputed(txt_autoInputKp.Text) == false)
                    {
                        txt_autoInputKp.ResetText();
                        MessageBox.Show("Trùng cuộn liệu đã cấp Line. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputKp.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_KP = true;
                    }
                    //check code cho PD xac nhan
                    else if (dtb1.chekdoubleCodePDxacnhan(txt_autoInputKp.Text) == false)
                    {
                        txt_autoInputKp.ResetText();
                        MessageBox.Show("Trùng cuộn liệu đang chờ PD xác nhận. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputKp.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_KP = true;
                    }
                    //check format code
                    else if (dtb1.chk_formInput(txt_autoInputKp.Text) == false)
                    {
                        txt_autoInputKp.ResetText();
                        MessageBox.Show("Sai format code input. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputKp.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_KP = true;
                    }
                    //check NVL holding
                    else if (dtb1.chekNVLHolding(txt_autoInputKp.Text) == false)
                    {
                        txt_autoInputKp.ResetText();
                        MessageBox.Show("Cuộn liệu bị holding. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputKp.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_KP = true;
                    }
                    else
                    {
                        if (arrgPerKP == false)//OP
                        {
                            timer_reLoad.Start();
                            //Gọi hàm check thông tin input
                            input_KP(txt_autoInputKp, false);
                        }
                        else//admin, manager
                        {
                            DialogResult rel_ar = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "In/Out Material", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (rel_ar == DialogResult.OK)
                            {
                                timer_reLoad.Start();
                                //Gọi hàm check thông tin input
                                input_KP(txt_autoInputKp, false);
                            }
                        }
                    }
                }
            }
        }

        private void btn_enterKp_Click(object sender, EventArgs e)
        {
            if (txt_autoInputKp.Text == "")
            {
                if ((cbx_ModelKP.Text == "") || (rbt_NewPO.Checked == false && rbt_OldPO.Checked == false) || txt_manualInputKp.Text == "" || cbx_PD.Text == "")
                {
                    MessageBox.Show("Bạn điền thiếu thông tin Model/PO/PD/Input!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbx_ModelKP.Focus();
                }
                else
                {
                    //Check double Input
                    if (dtb1.chekNewCodeInputed(txt_manualInputKp.Text) == false)
                    {
                        //txt_manualInputKp.ResetText();
                        MessageBox.Show("Trùng cuộn liệu đã cấp Line. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputKp.Focus();//Trỏ chuột tại textBox Input
                    }
                    //Check code cho PD xac nhan
                    else if (dtb1.chekdoubleCodePDxacnhan(txt_manualInputKp.Text) == false)
                    {
                        //txt_manualInputKp.ResetText();
                        MessageBox.Show("Trùng cuộn liệu đang chờ PD xác nhận. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputKp.Focus();//Trỏ chuột tại textBox Input
                    }
                    //check format code
                    else if (dtb1.chk_formInput(txt_manualInputKp.Text) == false)
                    {
                        //txt_manualInputKp.ResetText();
                        MessageBox.Show("Sai format code input. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputKp.Focus();//Trỏ chuột tại textBox Input
                    }
                    //check NVL holding
                    else if (dtb1.chekNVLHolding(txt_manualInputKp.Text) == false)
                    {
                        //txt_manualInputKp.ResetText();
                        MessageBox.Show("Cuộn liệu bị holding. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputKp.Focus();//Trỏ chuột tại textBox Input
                    }
                    else
                    {
                        if (arrgPerKP == false)
                        {
                            timer_reLoad.Start();
                            //Gọi hàm check input manual
                            input_KP(txt_manualInputKp, false);
                        }
                        else
                        {
                            DialogResult rel_arKP = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "In/Out Material", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (rel_arKP == DialogResult.OK)
                            {
                                timer_reLoad.Start();
                                //Gọi hàm check input manual
                                input_KP(txt_manualInputKp, false);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy xóa mục Scan Code trước khi Enter code tay!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void input_KP(TextBox txt, bool chkFifo)
        {
            //Biến hour_cap
            string h_cap = find_hour();
            //Biến xác nhận maker part same
            bool mkpSam1 = false;
            //Biến báo đã có code trong DataGirdView trùng
            bool havCodSm1 = false;
            //tach chuoi và so sanh trong stock
            string[] str_Inp = txt.Text.Split('+');
            //Kiem tra time input la min
            string minTimInp = string.Empty;
            if (chkFifo == false)
            {
                minTimInp = dtb1.get_InputKp(str_Inp[0], str_Inp[1].Substring(0, 8), str_Inp[2], str_Inp[1]);
            }
            else
            {
                minTimInp = "true";
            }

            if (minTimInp == "false")
            {
                DialogResult relFiFo = MessageBox.Show("Mã NVL " + str_Inp[0] + " còn Stock cũ hơn. Bạn vẫn muốn tiếp tục cấp NVL này?", "In/Out Material", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (relFiFo == DialogResult.OK)
                {
                    //Hiển thị new form
                    bool Isopen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "ConfirmFiFo")
                        {
                            Isopen = true;
                            f.BringToFront();
                            break;
                        }
                    }
                    if (Isopen == false)
                    {
                        ConfirmFiFo ff = new ConfirmFiFo(this);
                        ff.Show();
                        count_timer = 0;
                        timer1.Start();
                    }
                }
                else
                {
                    if (cfrfifo == false)
                    {
                        txt.Text = "";
                        dtb1.del_filLog("PDxacnhan", str_Inp[1], 1, str_database);
                        chkScCode_KP = true;
                    }
                }
            }
            else if (minTimInp == "No Date")
            {
                txt.Text = "";
                MessageBox.Show("Không tồn tại Mã NVL " + str_Inp[0] + " ,ngày nhập kho = " + str_Inp[1] + " trong stock FIFO!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chkScCode_KP = true;
            }
            else if (minTimInp == "No code")
            {
                txt.Text = "";
                MessageBox.Show("Không tồn tại Mã NVL " + str_Inp[0] + " trong stock FIFO!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chkScCode_KP = true;
            }
            else if (minTimInp == "Fail Access")
            {
                txt.Text = "";
                MessageBox.Show("Không thể truy xuất Stock FIFO!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chkScCode_KP = true;
            }
            else
            {
                transportKP = dtb1.LoadStockFIFO(str_Inp[0], str_Inp[2], str_Inp[3], str_Inp[1]);                
                string[] infFromBom = get_InfInpKP(transportKP, txt, cb_inputCodeKp);
                if (infFromBom[0] != null && infFromBom[1] != null && infFromBom[2] != null && infFromBom[3] != null && infFromBom[4] != null)
                {
                    //Kiểm tra SDI code, maker theo mnaker part
                    if (dtb1.chekScanMakPrtSame(infFromBom[3]) == false)//đã có input maker part rồi
                    {
                        mkpSam1 = true;
                    }
                    else//chưa input maker part lần nào
                    {
                        dtb1.savMakPrt(infFromBom[3]);
                        mkpSam1 = false;
                    }

                    sttKP++;
                    //Điền data vào DataGridView
                    if (dgv_Ktz_Pd.Columns.Count == 0 || dgv_Ktz_Pd.Rows.Count == 0)
                    {
                        dtb1.delete_Transport("Ktz_Pd_tranfer");
                        if (mkpSam1 == true)
                        {
                            txt.Text = "";
                            MessageBox.Show("Kiểm tra lại file log (Makerpart) trong StartPath!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            chkScCode_KP = true;
                        }
                        else
                        {
                            dtb1.insert_transKPv2(sttKP.ToString(), dDay, dShift, cbx_ModelKP.Text, infFromBom[0], infFromBom[1], infFromBom[2], infFromBom[3], str_Inp[3], infFromBom[4], Acc, cbx_PD.Text, txt.Text);
                            havCodSm1 = true;
                            transport1KP = dtb1.LoadDatabase("Ktz_Pd_tranfer", cbx_ModelKP.Text, dDay);
                            //Hien thi 
                            dgv_Ktz_Pd.Columns.Clear();
                            dtb1.show_ktzGiaoPd(dgv_Ktz_Pd, transport1KP);
                        }                        
                    }
                    else//dgv da co data
                    {
                        DataRow dtrwKP = transport1KP.NewRow();
                        dtrwKP["STT"] = sttKP.ToString();
                        dtrwKP["Ngay_thang"] = dDay;
                        dtrwKP["Ca_kip"] = dShift;
                        dtrwKP["Line"] = "SMD";
                        dtrwKP["Model"] = cbx_ModelKP.Text;
                        dtrwKP["Mo_ta"] = infFromBom[0];
                        dtrwKP["Ma_NVL"] = infFromBom[1];
                        dtrwKP["Maker"] = infFromBom[2];
                        dtrwKP["Maker_Part"] = infFromBom[3];
                        dtrwKP["Lot"] = str_Inp[3];
                        dtrwKP["So_luong_cap"] = infFromBom[4];
                        dtrwKP["Tem_code"] = txt.Text;
                        dtrwKP["KTZ"] = stl_nameUser.Text;
                        dtrwKP["PD"] = cbx_PD.Text;
                        transport1KP.Rows.Add(dtrwKP);
                        transport1KP.AcceptChanges();                       
                        havCodSm1 = true;                        
                    }

                    if (havCodSm1 == true)
                    {                        
                        //An bot datagridvuew 
                        dgv_Ktz_Pd.Columns["Ngay_thang"].Visible = false;
                        dgv_Ktz_Pd.Columns["Ca_kip"].Visible = false;
                        dgv_Ktz_Pd.Columns["Line"].Visible = false;
                        dgv_Ktz_Pd.Columns["Model"].Visible = false;
                        dgv_Ktz_Pd.Columns["KTZ"].Visible = false;
                        dgv_Ktz_Pd.Columns["PD"].Visible = false;
                        //Auto scroll
                        if(dgv_Ktz_Pd.RowCount > 10)
                        {
                            dgv_Ktz_Pd.FirstDisplayedScrollingRowIndex = dgv_Ktz_Pd.RowCount - 1 ;
                        }                        
                        //Not sort
                        foreach (DataGridViewColumn col in dgv_Ktz_Pd.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        //Save input
                        dtb1.savPDxacnhan(txt.Text);
                        //Save data FIFO
                        dtb1.savFIFO(infFromBom[1] + "+" + infFromBom[2] + "+" + infFromBom[3] + "+" + str_Inp[3] + "+" + str_Inp[1]);
                        //Standard Lot
                        string strIma1 = string.Empty;
                        string strIma2 = string.Empty;
                        #region
                        switch (infFromBom[2])
                        {
                            case "RENESAS":
                                strIma1 = infFromBom[2] + "1";
                                strIma2 = infFromBom[2] + "2";
                                break;

                            case "STMICRO":
                                strIma1 = infFromBom[2] + "1";
                                strIma2 = infFromBom[2] + "2";
                                break;

                            case "TI":
                                strIma1 = infFromBom[2] + "1";
                                strIma2 = infFromBom[2] + "2";
                                break;
                            default:
                                strIma1 = infFromBom[2];
                                strIma2 = string.Empty;
                                break;
                        }
                        #endregion

                        picBx1KP.Visible = true;
                        lblKP_Pic1.Visible = true;

                        picBx1KP.Image = new Bitmap(str_database + "\\Picture\\" + strIma1 + ".PNG");
                        picBx1KP.SizeMode = PictureBoxSizeMode.StretchImage;

                        if (strIma2 != string.Empty)
                        {
                            picBx2KP.Visible = true;
                            lblKP_Pic2.Visible = true;
                            picBx2KP.Image = new Bitmap(str_database + "\\Picture\\" + strIma2 + ".PNG");
                            picBx2KP.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        else
                        {
                            picBx2KP.Visible = false;
                            lblKP_Pic2.Visible = false;
                        }
                        havCodSm1 = false;
                        txt.Text = "";
                        if (cb_inputCodeKp.Checked == false)
                        {
                            chkScCode_KP = true;
                        }
                    }
                    else
                    {
                        if (cb_inputCodeKp.Checked == false)
                        {
                            txt.Text = "";
                            chkScCode_KP = true;
                        }
                    }
                }
            }
        }

        private void cb_inputCodeKp_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_inputCodeKp.Checked == true)
            {
                AcceptButton = btn_enterKp;
                if (arrgPerKP == true)
                {
                    txt_manualInputKp.Visible = true;
                    txt_manualInputKp.Enabled = true;
                    txt_manualInputKp.Focus();
                    btn_enterKp.Visible = true;
                    btn_enterKp.Enabled = true;
                    lbl_inputKp.Text = "Nhập Code";
                }
                else
                {
                    nhapCodeKP = true;
                    bool Isopen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "ConfirmInOut")
                        {
                            Isopen = true;
                            f.BringToFront();
                            break;
                        }
                    }
                    if (Isopen == false)
                    {
                        ConfirmInOut confirmAd = new ConfirmInOut(this, str_database);
                        confirmAd.Show();
                        count_timer = 0;
                        timer1.Start();
                    }
                }
            }
            else
            {
                txt_manualInputKp.Hide();
                txt_manualInputKp.Enabled = false;
                txt_manualInputKp.Text = "";
                txt_autoInputKp.Focus();
                btn_enterKp.Visible = false;
                btn_enterKp.Enabled = false;
                lbl_inputKp.Text = "Scan Code";
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            txt_autoInputKp.Focus();
            try
            {
                // Nếu dùng dataGridView2.SelectedRows.Count thì phải click vào đầu hàng
                // Nếu dùng dataGridView2.CurrentRow.Index thì click vào bất kì vị trí có thể xóa hàng đó

                if (this.dgv_Ktz_Pd.CurrentRow.Index >= 0)
                {
                    string[] str = dgv_Ktz_Pd.Rows[dgv_Ktz_Pd.CurrentRow.Index].Cells["Tem_code"].Value.ToString().Split('+');
                    string strFifo = str[0] + "+" +
                                     dgv_Ktz_Pd.Rows[dgv_Ktz_Pd.CurrentRow.Index].Cells["Maker"].Value.ToString() + "+" +
                                     str[2] + "+" +
                                     str[3] + "+" +
                                     str[1];
                    dtb1.del_filLog("FI-FO", strFifo, 1, str_database);
                    dtb1.del_filLog("PDxacnhan", dgv_Ktz_Pd.Rows[dgv_Ktz_Pd.CurrentRow.Index].Cells["Tem_code"].Value.ToString(), 1, str_database);
                    dtb1.del_filLog("MakerPart", dgv_Ktz_Pd.Rows[dgv_Ktz_Pd.CurrentRow.Index].Cells["Maker_Part"].Value.ToString(), 1, str_database);

                    DataRow drToDelete = transport1KP.Rows[dgv_Ktz_Pd.CurrentRow.Index];
                    transport1KP.Rows.Remove(drToDelete);
                }
                if(dgv_Ktz_Pd.RowCount == 0)
                {
                    chkScCode_KP = true;
                    sttKP = 0;
                }
            }
            catch
            {
                MessageBox.Show("Click vào đầu hàng đó để xóa!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_ConfirmKtz_Click(object sender, EventArgs e)
        {
            if (dtb1.checkKtz_PD(dgv_Ktz_Pd) == true)//Kiểm tra all thông tin
            {
                MessageBox.Show("Các thông tin đang để trống hoặc sai!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult traloi = MessageBox.Show("Bạn có chắc chắn giao NVL cho PD?", "In/Out Material", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (traloi == DialogResult.Yes)
                {
                    DataTable dtSource = (DataTable)dgv_Ktz_Pd.DataSource;
                    //Giảm stock sau khi KTZ giao NVL cho PD
                    string[] err = dtb1.Reduce_StokKtz2(dgv_Ktz_Pd, datTim, "Ma_NVL", "Lot", "So_luong_cap", "In/Out Material", dtSource, stl_nameUser.Text, "KTZ-PD", dDay, dShift, cbx_ModelKP.Text, cbx_PD.Text, str_database);
                    if (err[0] == null)
                    {
                        dgv_Ktz_Pd.Columns.Clear();
                        dtb1.show_ktzGiaoPd(dgv_Ktz_Pd, dtSource);
                        dgv_Ktz_Pd.Columns["Ngay_thang"].Visible = false;
                        dgv_Ktz_Pd.Columns["Ca_kip"].Visible = false;
                        dgv_Ktz_Pd.Columns["Line"].Visible = false;
                        dgv_Ktz_Pd.Columns["Model"].Visible = false;
                        dgv_Ktz_Pd.Columns["KTZ"].Visible = false;
                        dgv_Ktz_Pd.Columns["PD"].Visible = false;
                        //Not sort
                        foreach (DataGridViewColumn col in dgv_Ktz_Pd.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        //Xoa Stock FIFO
                        if (dtb1.del_FIFO(str_database) == true)
                        {
                            //DataTable trung gian lưu thông tin theo form c/s -> gán vào file .CSV
                            DataTable tb_Excel = new DataTable();
                            tb_Excel = dtb1.LoadDatabase("PDxacnhanStock", cbx_ModelKP.Text, dDay);
                            string content = "Ngay_thang, Ca_kip, Line, Model, Mo_ta, Ma_NVL, Maker, Maker Part, Lot, So_luong_cap, Tem_code, KTZ, PD\n";
                            bool chekExitFil = excel.checkExitLog(str_database + "\\History\\In_Out\\Ktz_to_PD\\" + dMon + "\\" + datTim + "_" + cbx_ModelKP.Text + ".csv");
                            if (excel.Export_CSV(tb_Excel, str_database + "\\History\\In_Out\\Ktz_to_PD\\" + dMon + "\\" + datTim + "_" + cbx_ModelKP.Text + ".csv", chekExitFil, content) == true)
                            {
                                MessageBox.Show("Tạo LogFile thành công!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (File.Exists(str_database + "\\tem\\" + cbx_ModelKP.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadKTZ-PD.txt"))
                                {
                                    File.Delete(str_database + "\\tem\\" + cbx_ModelKP.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadKTZ-PD.txt");
                                }
                                //update lich su                                       
                                //try
                                //{
                                #region
                                //var nvls = new List<NVL>() { };
                                //StreamReader sr = new StreamReader(str_database + "\\History\\HistoryNVL.txt");
                                //while (sr.EndOfStream == false)
                                //{
                                //    string[] str = sr.ReadLine().Split('|');
                                //    if (str.Length == 20)
                                //    {
                                //        nvls.Add(new NVL
                                //        {
                                //            model = str[0],
                                //            codeNVL = str[1],
                                //            maker = str[2],
                                //            mkerPart = str[3],
                                //            lot = str[4],
                                //            temCode = str[5],
                                //            ngInTemCode = str[6],
                                //            tgianInTemCode = str[7],
                                //            ngNhapKho = str[8],
                                //            tgianNhapKho = str[9],
                                //            ngCapNVL = str[10],
                                //            tgianCapNVL = str[11],
                                //            PDxacnhan = str[12],
                                //            tgianxacnhan = str[13],
                                //            ngTraNVL = str[14],
                                //            tgianTraNVL = str[15],
                                //            ghiChuTra = str[16],
                                //            ngTraWH = str[17],
                                //            tgianTraWH = str[18],
                                //            ghiChuTraWH = str[19]
                                //        });
                                //    }
                                //}
                                //sr.Close();

                                //for (int h = 0; h < dgv_Ktz_Pd.RowCount; h++)
                                //{
                                //    if (dgv_Ktz_Pd.Rows[h].Cells["Mo_ta"].Value != null && dgv_Ktz_Pd.Rows[h].Cells["Mo_ta"].Value.ToString() != "")
                                //    {
                                //        string temCode = dgv_Ktz_Pd.Rows[h].Cells["Tem_code"].Value.ToString();
                                //        foreach (var nn in nvls.Where(x => x.temCode == temCode))
                                //        {
                                //            nn.ngCapNVL = stl_nameUser.Text;
                                //            nn.tgianCapNVL = DateTime.Now.ToString();
                                //        }
                                //    }                                               
                                //}

                                //FileStream fs = new FileStream(str_database + "\\History\\HistoryNVL.txt", FileMode.Create);
                                //StreamWriter sw = new StreamWriter(fs);
                                //foreach (var item in nvls)
                                //{
                                //    sw.WriteLine(item.model + "|" +
                                //                 item.codeNVL + "|" +
                                //                 item.maker + "|" +
                                //                 item.mkerPart + "|" +
                                //                 item.lot + "|" +
                                //                 item.temCode + "|" +
                                //                 item.ngInTemCode + "|" +
                                //                 item.tgianInTemCode + "|" +
                                //                 item.ngNhapKho + "|" +
                                //                 item.tgianNhapKho + "|" +
                                //                 item.ngCapNVL + "|" +
                                //                 item.tgianCapNVL + "|" +
                                //                 item.PDxacnhan + "|" +
                                //                 item.tgianxacnhan + "|" +
                                //                 item.ngTraNVL + "|" +
                                //                 item.tgianTraNVL + "|" +
                                //                 item.ghiChuTra + "|" +
                                //                 item.ngTraWH + "|" +
                                //                 item.tgianTraWH + "|" +
                                //                 item.ghiChuTraWH);
                                //}
                                //sw.Close();
                                //fs.Close();
                                #endregion
                                //hien thi stock line
                                dgv_viewStkLine.Columns.Clear();
                                DataTable dt_sl = dtb1.search_stock("KtzGiaoPd1", false);
                                dtb1.show_StockLinee(dgv_viewStkLine, dt_sl);
                                dgv_viewStkLine.Columns["Ngay_thang"].Visible = false;
                                dgv_viewStkLine.Columns["Ca_kip"].Visible = false;
                                dgv_viewStkLine.Columns["Line"].Visible = false;
                                dgv_viewStkLine.Columns["Model"].Visible = false;
                                dgv_viewStkLine.Columns["KTZ"].Visible = false;
                                dgv_viewStkLine.Columns["PD"].Visible = false;
                                //update model running
                                dtb1.upModeRun(dDay, dShift, cbx_ModelKP.Text);
                                //Xoa stock = 0
                                dtb1.Del_StockZero("Stock_KTZ", "So_luong");
                                dtb1.delete_Transport("Ktz_Pd_tranfer");
                                dtb1.delete_Transport("PDxacnhanStock");
                                //Reset data                        
                                //dgv_Ktz_Pd.Columns.Clear();
                                cbx_PD.Text = "";
                                picBx1KP.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                picBx1KP.SizeMode = PictureBoxSizeMode.StretchImage;
                                picBx2KP.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                picBx2KP.SizeMode = PictureBoxSizeMode.StretchImage;
                                picBx1KP.Visible = true;
                                lblKP_Pic1.Visible = true;
                                picBx2KP.Visible = true;
                                lblKP_Pic2.Visible = true;
                                rbt_NewPO.Checked = false;
                                rbt_OldPO.Checked = false;
                                radbtn_reLoadKP.Checked = false;
                                chkScCode_KP = true;
                                sttKP = 0;
                                timer_reLoad.Stop();
                                //xoa file .log
                                try
                                {
                                    string[] files = Directory.GetFiles(str_database + "\\Log\\Duplicate\\");
                                    int m = 0;
                                    foreach (string fil in files)
                                    {
                                        if (files[m].Contains("Input_Line") || files[m].Contains("PDxacnhan") || files[m].Contains("Input_Ktz") || files[m].Contains("NVL_Holding"))
                                        {
                                            goto jumpm;
                                        }
                                        File.Delete(fil);
                                    jumpm:
                                        m++;
                                    }
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Xảy ra lỗi xóa file .log (FI-FO, MakerPart)!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                //}
                                //catch (Exception)
                                //{
                                //    MessageBox.Show("Xảy ra lỗi cập nhật history NVL!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //}                                                                                                   
                            }
                            else
                            {
                                MessageBox.Show("Xảy ra lỗi xuất logfile!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Xảy ra lỗi Xóa Stock FI-FO!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        dgv_Ktz_Pd.Columns.Clear();
                        dtb1.show_ktzGiaoPd(dgv_Ktz_Pd, dtSource);
                        dgv_Ktz_Pd.Columns["Ngay_thang"].Visible = false;
                        dgv_Ktz_Pd.Columns["Ca_kip"].Visible = false;
                        dgv_Ktz_Pd.Columns["Line"].Visible = false;
                        dgv_Ktz_Pd.Columns["Model"].Visible = false;
                        dgv_Ktz_Pd.Columns["KTZ"].Visible = false;
                        dgv_Ktz_Pd.Columns["PD"].Visible = false;
                        //Not sort
                        foreach (DataGridViewColumn col in dgv_Ktz_Pd.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        if (err[0] == "error")//datagirdview dgv_Ktz_Pd trống
                        {
                            MessageBox.Show("Bạn chưa nhập data. Nội dung hiển thị trống!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else//datagirdview dgv_Ktz_Pd input âm stock KTZ
                        {
                            foreach (DataGridViewRow dgr in dgv_Ktz_Pd.Rows)
                            {
                                if (dgr.Cells["Mo_ta"].Value != null && dgr.Cells["Mo_ta"].Value.ToString() != "")
                                {
                                    for (int i = 0; i < err.Length; )
                                    {
                                        if (dgr.Cells["Ma_NVL"].Value.ToString() == err[i] && dgr.Cells["Lot"].Value.ToString() == err[i + 1])
                                        {
                                            dgr.Cells["Tem_code"].Style.BackColor = Color.Red;
                                        }
                                        i = i + 2;
                                    }
                                }
                            }
                        }
                    }
                }
            }             
        }       

        private void btnStockLinee_Click(object sender, EventArgs e)
        {
            //Show Stock Line
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockLine2")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockLine2 s_line2 = new StockLine2(str_database);
                s_line2.Show();
            }
        }

        private void btnStockKtz_Click(object sender, EventArgs e)
        {
            //Hiển thị new form
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockKTZ2")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockKTZ2 stk2 = new StockKTZ2(str_database);
                stk2.Show();
            }
        }

        private void radbtn_reLoadKP_CheckedChanged(object sender, EventArgs e)
        {
            if (radbtn_reLoadKP.Checked == true)
            {
                try
                {
                    DataTable dt_reload = new DataTable();
                    StreamReader sr = new StreamReader(str_database + "\\tem\\" + cbx_ModelKP.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadKTZ-PD.txt");
                    string[] colName = sr.ReadLine().Split(',');
                    for (int j = 0; j < colName.Length - 1; j++)
                    {
                        dt_reload.Columns.Add(colName[j]);
                    }

                    string newLine;
                    while ((newLine = sr.ReadLine()) != null)
                    {
                        DataRow dtr = dt_reload.NewRow();
                        string[] values = newLine.Split(',');
                        if (values[0] != "")
                        {
                            for (int i = 0; i < values.Length - 1; i++)
                            {
                                dtr[i] = values[i];
                            }
                            dt_reload.Rows.Add(dtr);
                        }
                    }
                    sr.Close();

                    dgv_Ktz_Pd.Columns.Clear();
                    dtb1.show_ktzGiaoPd(dgv_Ktz_Pd, dt_reload);
                    //An bot datagridvuew 
                    dgv_Ktz_Pd.Columns["Ngay_thang"].Visible = false;
                    dgv_Ktz_Pd.Columns["Ca_kip"].Visible = false;
                    dgv_Ktz_Pd.Columns["Line"].Visible = false;
                    dgv_Ktz_Pd.Columns["Model"].Visible = false;
                    dgv_Ktz_Pd.Columns["KTZ"].Visible = false;
                    dgv_Ktz_Pd.Columns["PD"].Visible = false;
                    //Not sort
                    foreach (DataGridViewColumn col in dgv_Ktz_Pd.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    radbtn_reLoadKP.Checked = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Data Re-load trống. Hãy tiếp tục thao tác!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    radbtn_reLoadKP.Checked = false;
                }                
            }
        }

        private void btn_ktraKP_Click(object sender, EventArgs e)
        {
            if ((DateTime.Compare(dTKP_Pic1.Value, dTKP_Pic2.Value) > 0) || (cbx_ModelKP.Text.Length == 0))
            {
                MessageBox.Show("Hãy xem lại Model/thời gian bạn muốn kiểm tra!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string[] hisroryCheck = GetHistory(dTKP_Pic1, dTKP_Pic2);
                //Tìm history theo ngày -> tổng hợp -> Hiển thị Excel
                //Get file name trong khoảng time đã chọn
                int num_filOk = 0;
                int num_file = dtb1.count_file(str_database + "\\History\\In_Out\\Ktz_to_PD\\", hisroryCheck);
                string[] nam_file = dtb1.get_filOK(num_file, str_database + "\\History\\In_Out\\Ktz_to_PD\\", hisroryCheck, cbx_ModelKP.Text, dTKP_Pic1.Text, dTKP_Pic2.Text, num_filOk);
                //Sprire.XLS -> merge all file 
                //Open file merged(save tạm ra đâu đó)
                //Save as file merged nếu muốn
                dtb1.merg_Excel(str_database + "\\History\\In_Out\\Ktz_to_PD\\", hisroryCheck, nam_file, nam_file.Length, datTim, "KTZ-PD", false, str_database);     
            }         
        }

        private void lblKP_Pic1_Click(object sender, EventArgs e)
        {
            if (arrgPerKP == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerKP, str_database);
                    infPicture.Show();
                }
            }
        }

        private void lblKP_Pic2_Click(object sender, EventArgs e)
        {
            if (arrgPerKP == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerKP, str_database);
                    infPicture.Show();
                }
            }
        }        

        private void dgv_Ktz_Pd_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (cb_inputCodeKp.Checked == true)
            {
                txt_manualInputKp.Text = "";
                txt_manualInputKp.Focus();
            }
            else
            {
                txt_autoInputKp.Text = "";
                txt_autoInputKp.Focus();
            }   
        }

        private void dgv_Ktz_Pd_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Thread.Sleep(300);
            int column = dgv_Ktz_Pd.CurrentCell.ColumnIndex;
            if (column == 10 && dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Value.ToString() != "")
            {
                int m = 0;
                bool chkgv = int.TryParse(dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Value.ToString(), out m);
                if (chkgv == false || m < 0)
                {
                    MessageBox.Show("Mục So_luong_cap phải là số dương. Không nhập chữ hoặc số âm!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Style.BackColor = Color.Red;
                    btn_ConfirmKtz.Enabled = false;
                }
                else
                {
                    int tt = dtb1.getData_qty2(dgv_Ktz_Pd.CurrentRow.Cells["Ma_NVL"].Value.ToString(), dgv_Ktz_Pd.CurrentRow.Cells["Maker_Part"].Value.ToString(), dgv_Ktz_Pd.CurrentRow.Cells["Lot"].Value.ToString());
                    string[] str = dgv_Ktz_Pd.CurrentRow.Cells["Tem_code"].Value.ToString().Split('+');
                    int tt_code = dtb1.getData_qty2(dgv_Ktz_Pd.CurrentRow.Cells["Ma_NVL"].Value.ToString(), dgv_Ktz_Pd.CurrentRow.Cells["Maker_Part"].Value.ToString(), dgv_Ktz_Pd.CurrentRow.Cells["Lot"].Value.ToString(), str[1]);
                    if (tt < int.Parse(dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Value.ToString()))
                    {
                        MessageBox.Show("NVL " + dgv_Ktz_Pd.CurrentRow.Cells["Ma_NVL"].Value.ToString() + ", Lot " + dgv_Ktz_Pd.CurrentRow.Cells["Lot"].Value.ToString() + " stock KTZ (" + tt.ToString() + ") < So_luong_cap (" + dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Value.ToString() + ") \nHãy kiểm tra lại số lượng input line!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Style.BackColor = Color.Red;
                        btn_ConfirmKtz.Enabled = false;
                    }
                    else if (tt_code < int.Parse(dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Value.ToString()))
                    {
                        MessageBox.Show("NVL " + dgv_Ktz_Pd.CurrentRow.Cells["Ma_NVL"].Value.ToString() + ", Tem_code " + dgv_Ktz_Pd.CurrentRow.Cells["Tem_code"].Value.ToString() + " stock KTZ (" + tt_code.ToString() + ") < So_luong_cap (" + dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Value.ToString() + ") \nHãy kiểm tra lại số lượng input line!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Style.BackColor = Color.Red;
                        btn_ConfirmKtz.Enabled = false;
                    }
                    else
                    {
                        dgv_Ktz_Pd.CurrentRow.Cells["So_luong_cap"].Style.BackColor = Color.White;
                        btn_ConfirmKtz.Enabled = true;
                    }
                }
            }
        }

        private void cbx_Model_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void cbx_PD_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_manualInputKp_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_autoInputKp_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_delete_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_ConfirmKtz_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btnStockLinee_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btnStockKtz_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_ktraKP_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void dgv_Ktz_Pd_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }        

        private void dgv_viewStkLine_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }
        #endregion

        //=============================================================PD confirm===============================================================================
        #region PD confirm
        private void cbx_modelPDxn_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_KTZ_PDxn.Text = "";
            dtb1.delete_Transport("PDxacnhan");
            dgv_PDxn.Columns.Clear();            

            //string[] strMolRun = dtb1.get_modelRun();
            //int errMolRun = 0;
            //if (strMolRun[0] != "none")
            //{
            //    for (int i = 0; i < strMolRun.Length; i++)
            //    {
            //        if (cbx_modelPDxn.Text == strMolRun[i])
            //        {
            //            errMolRun = 0;
            //        }
            //        else
            //        {
            //            errMolRun++;
            //        }
            //    }
            //    if (errMolRun == 0)
            //    {
                    dtblePDxn = dtb1.LoadBOM(cbx_modelPDxn.Text);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Bạn đang chọn sai Model. Line đang chạy Model khác!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        cbx_modelPDxn.Text = "";
            //    }                
            //}
            //else
            //{
            //    MessageBox.Show("Không có NVL đang tồn trên Line!", "PDxacnhan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            //    dtblePDxn = dtb1.LoadBOM(cbx_modelPDxn.Text);
            //}

                    if ((DateTime.Compare(DateTime.Parse(dtimPic1_com.Value.ToShortDateString()), DateTime.Parse(dtimPic2_com.Value.ToShortDateString())) > 0) || (cbx_modelPDxn.Text.Length == 0))
                    {
                        MessageBox.Show("Hãy xem lại Model/thời gian bạn muốn kiểm tra!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //Summary
                        string[] hisroryCheck = GetHistory(dtimPic1_com, dtimPic2_com);
                        int num_filOk = 0;
                        int num_file = dtb1.count_file(str_database + "\\History\\In_Out\\Ktz_to_PD\\", hisroryCheck);
                        string[] nam_file = dtb1.get_filOK(num_file, str_database + "\\History\\In_Out\\Ktz_to_PD\\", hisroryCheck, cbx_modelPDxn.Text, dtimPic1_com.Text, dtimPic2_com.Text, num_filOk);
                        pathCompare = dtb1.merg_Excel3(str_database + "\\History\\In_Out\\Ktz_to_PD\\", hisroryCheck, nam_file, nam_file.Length, datTim, "PDxacnhan", false, str_database);
                    }
        }

        private void cbx_KTZ_PDxn_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_scnCodePDxn.Focus();
        }

        private async void txt_scnCodePDxn_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(2000);
            if (txt_scnCodePDxn.Text != "" && chkScCode_PDxn == true)
            {
                chkScCode_PDxn = false;
                if (chb_nhaptayPDxn.Checked == true)
                {
                    txt_inpManulPDxn.Enabled = true;
                    txt_inpManulPDxn.Focus();
                    txt_inpAutoPDxn.Enabled = false;
                    txt_inpAutoPDxn.Visible = false;
                }
                else
                {
                    txt_inpAutoPDxn.Enabled = true;
                    txt_inpAutoPDxn.Focus();
                    txt_inpManulPDxn.Enabled = false;
                    txt_inpManulPDxn.Visible = false;
                }                   
            }
            else if (txt_scnCodePDxn.Text == "" && chkScCode_PDxn == false)
            {
                chkScCode_PDxn = true;
                if (chb_nhaptayPDxn.Checked == true)
                {
                    txt_inpManulPDxn.Enabled = false;
                    txt_inpAutoPDxn.Enabled = false;
                    txt_inpAutoPDxn.Visible = false;
                }
                else
                {
                    txt_inpAutoPDxn.Enabled = false;
                    txt_inpManulPDxn.Enabled = false;
                    txt_inpManulPDxn.Visible = false;
                }
            }
        }

        private async void txt_inpAutoPDxn_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            if (txt_inpAutoPDxn.Text != "" && chkMkp_PDxn == true)
            {
                chkMkp_PDxn = false;
                if (cbx_modelPDxn.Text == "" || cbx_KTZ_PDxn.Text == "")
                {
                    MessageBox.Show("Bạn chưa chọn Model/KTZ!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbx_modelPDxn.Focus();
                    if (chb_nhaptayPDxn.Checked == false)//auto
                    {
                        txt_inpAutoPDxn.Text = "";
                        txt_inpAutoPDxn.Enabled = false;
                        txt_scnCodePDxn.Text = "";
                        txt_scnCodePDxn.Focus();
                        chkScCode_PDxn = true;
                        chkMkp_PDxn = true;
                    }
                    else
                    {
                        txt_inpAutoPDxn.Text = "";
                        txt_inpAutoPDxn.Enabled = false;
                        txt_scnCodePDxn.Text = "";
                        txt_scnCodePDxn.Focus();
                        chkScCode_PDxn = true;
                    }
                }
                else
                {
                    txt_inpManulPDxn.Text = "";
                    if (arrgPerPDxn == false)//OP
                    {
                        //Gọi hàm kiểm tra thông tin input
                        PDxacnhan(txt_inpAutoPDxn);                        
                    }
                    else//admin, manager
                    {
                        DialogResult rel_ar = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "PDxacnhan", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rel_ar == DialogResult.OK)
                        {
                            //Gọi hàm kiểm tra thông tin input                            
                            PDxacnhan(txt_inpAutoPDxn);   
                        }
                    }
                }
            }
        }

        private void btn_enterPDxn_Click(object sender, EventArgs e)
        {
            if (txt_inpAutoPDxn.Text == "")
            {
                if (cbx_modelPDxn.Text == "" || txt_inpManulPDxn.Text == "" || cbx_KTZ_PDxn.Text == "")
                {
                    MessageBox.Show("Hãy kiểm tra lại thông tin Model/Input/KTZ!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbx_modelPDxn.Focus();
                    if (chb_nhaptayPDxn.Checked == false)//auto
                    {
                        txt_inpManulPDxn.Text = "";
                        txt_inpManulPDxn.Enabled = false;
                        txt_scnCodePDxn.Text = "";
                        txt_scnCodePDxn.Focus();
                        chkScCode_PDxn = true;
                        chkMkp_PDxn = true;
                    }
                    else
                    {
                        txt_inpManulPDxn.Text = "";
                        txt_inpManulPDxn.Enabled = false;
                        txt_scnCodePDxn.Text = "";
                        txt_scnCodePDxn.Focus();
                        chkScCode_PDxn = true;
                    }
                }
                else
                {
                    txt_inpManulPDxn.Text = crtUpperChar(txt_inpManulPDxn.Text);
                    if (arrgPerPDxn == false)
                    {
                        //Gọi hàm kiểm tra thông tin input
                        PDxacnhan(txt_inpManulPDxn);
                    }
                    else
                    {
                        DialogResult rel_ar = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "PDxacnhan", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rel_ar == DialogResult.OK)
                        {                            
                            //Gọi hàm kiểm tra thông tin input
                            PDxacnhan(txt_inpManulPDxn);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy xóa mục Scan Code trước khi Enter code tay!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void PDxacnhan(TextBox txt)
        {
            if (dtb1.chk_formInput(txt_scnCodePDxn.Text) == false)//sai format code
            {
                txt.Text = "";
                txt.Enabled = false;
                MessageBox.Show("Sai format code input. Hãy kiểm tra lại!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_scnCodePDxn.Focus();
                txt_scnCodePDxn.Text = "";
                chkScCode_PDxn = true;
                chkMkp_PDxn = true;           
            }
            else if (dtb1.chekNewCodeInputed(txt_scnCodePDxn.Text, str_database) == false)//da input r
            {
                txt.Text = "";
                txt.Enabled = false;
                MessageBox.Show("Cuộn liệu đã được input rồi!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_scnCodePDxn.Focus();
                txt_scnCodePDxn.Text = "";
                chkScCode_PDxn = true;
                chkMkp_PDxn = true;
            }
            else if(dtb1.chekdoubleCodePDxacnhan(txt_scnCodePDxn.Text, str_database) == true)
            {
                txt.Text = "";
                txt.Enabled = false;
                MessageBox.Show("Cuộn liệu chưa được KTZ giao!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_scnCodePDxn.Focus();
                txt_scnCodePDxn.Text = "";
                chkScCode_PDxn = true;
                chkMkp_PDxn = true;
            }
            else if (dtb1.chekNVLHolding(txt_scnCodePDxn.Text, str_database) == false)
            {
                txt.Text = "";
                txt.Enabled = false;
                MessageBox.Show("Cuộn liệu đang holding!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_scnCodePDxn.Focus();
                txt_scnCodePDxn.Text = "";
                chkScCode_PDxn = true;
                chkMkp_PDxn = true;
            }
            else
            {
                //Cat chuoi new code: Ma_NVL + DateTime + MakerPart + Lot
                string[] nCode = txt_scnCodePDxn.Text.Split('+');
                //Check data txt input
                string[] infFromBom = new string[5];
                infFromBom = get_InfInpPDxn(dtblePDxn, nCode[0], nCode[2], txt, txt_scnCodePDxn, chb_nhaptayPDxn);
                if (infFromBom[0] != null && infFromBom[1] != null && infFromBom[2] != null && infFromBom[3] != null && infFromBom[4] != null)
                {
                    if (nCode[0] == infFromBom[1] && txt.Text.Contains(nCode[2]))//trùng code và maker part
                    {
                        sttPDxn++;
                        //Điền data vào datagridview
                        if (dgv_PDxn.Columns.Count == 0 || dgv_PDxn.Rows.Count == 0)//dgv chua co data
                        {
                            //run timer reload
                            timer_reLoad.Start();
                            tool_saving.BackColor = Color.Green;
                            //xoa database cu
                            dtb1.delete_Transport("PDxacnhan");
                            //Insert OrderWH
                            dtb1.insert_tranPDxn(sttPDxn.ToString(), dDay, dShift, "SMD", cbx_modelPDxn.Text, infFromBom[0], infFromBom[1], infFromBom[2], infFromBom[3], nCode[3], infFromBom[4], txt_scnCodePDxn.Text, cbx_KTZ_PDxn.Text, stl_nameUser.Text);
                            //Load all data vao dataTable
                            dtble1PDxn = dtb1.LoadDatabase("PDxacnhan", cbx_modelPDxn.Text, dDay);
                            //Hiển thị
                            dgv_PDxn.Columns.Clear();
                            dtb1.show_PDxacnhan(dgv_PDxn, dtble1PDxn);
                        }
                        else
                        {
                            tool_saving.BackColor = Color.Green;
                            DataRow dtrwPDxn = dtble1PDxn.NewRow();
                            dtrwPDxn["STT"] = sttPDxn.ToString();
                            dtrwPDxn["Ngay_thang"] = dDay;
                            dtrwPDxn["Ca_kip"] = dShift;
                            dtrwPDxn["Line"] = "SMD";
                            dtrwPDxn["Model"] = cbx_modelPDxn.Text;
                            dtrwPDxn["Mo_ta"] = infFromBom[0];
                            dtrwPDxn["Ma_NVL"] = infFromBom[1];
                            dtrwPDxn["Maker"] = infFromBom[2];
                            dtrwPDxn["Maker_Part"] = infFromBom[3];
                            dtrwPDxn["Lot"] = nCode[3];
                            dtrwPDxn["So_luong_cap"] = infFromBom[4];
                            dtrwPDxn["Tem_code"] = txt_scnCodePDxn.Text;
                            dtrwPDxn["KTZ"] = cbx_KTZ_PDxn.Text;
                            dtrwPDxn["PD"] = stl_nameUser.Text;
                            dtble1PDxn.Rows.Add(dtrwPDxn);
                            dtble1PDxn.AcceptChanges();  
                        }                        
                        //An bot datagridvuew 
                        dgv_PDxn.Columns["Ngay_thang"].Visible = false;
                        dgv_PDxn.Columns["Ca_kip"].Visible = false;
                        dgv_PDxn.Columns["Line"].Visible = false;
                        dgv_PDxn.Columns["Model"].Visible = false;
                        dgv_PDxn.Columns["KTZ"].Visible = false;
                        dgv_PDxn.Columns["PD"].Visible = false;
                        //Auto scroll
                        if (dgv_PDxn.RowCount > 7)
                        {
                            dgv_PDxn.FirstDisplayedScrollingRowIndex = dgv_PDxn.RowCount - 1;
                        }  
                        //Not sort
                        foreach (DataGridViewColumn col in dgv_PDxn.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        //Luu new code
                        dtb1.savNwCodInputed(txt_scnCodePDxn.Text, str_database);
                        //Hinh ảnh
                        string strIma1 = string.Empty;
                        string strIma2 = string.Empty;

                        switch (infFromBom[2])
                        {
                            case "RENESAS":
                                strIma1 = infFromBom[2] + "1";
                                strIma2 = infFromBom[2] + "2";
                                break;

                            case "STMICRO":
                                strIma1 = infFromBom[2] + "1";
                                strIma2 = infFromBom[2] + "2";
                                break;

                            case "TI":
                                strIma1 = infFromBom[2] + "1";
                                strIma2 = infFromBom[2] + "2";
                                break;
                            default:
                                strIma1 = infFromBom[2];
                                strIma2 = string.Empty;
                                break;
                        }

                        picPDxn1.Visible = true;
                        lbl_PDxn1.Visible = true;

                        picPDxn1.Image = new Bitmap(str_database + "\\Picture\\" + strIma1 + ".PNG");
                        picPDxn1.SizeMode = PictureBoxSizeMode.StretchImage;

                        if (strIma2 != string.Empty)
                        {
                            picPDxn2.Visible = true;
                            lbl_PDxn2.Visible = true;
                            picPDxn2.Image = new Bitmap(str_database + "\\Picture\\" + strIma2 + ".PNG");
                            picPDxn2.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        else
                        {
                            picPDxn2.Visible = false;
                            lbl_PDxn2.Visible = false;
                        }
                        //Reset
                        if (chb_nhaptayPDxn.Checked == true)
                        {
                            txt.Text = "";
                            txt.Enabled = false;
                            txt_scnCodePDxn.Text = "";
                            txt_scnCodePDxn.Focus();
                            chkScCode_PDxn = true;
                        }
                        else
                        {
                            txt.Text = "";
                            txt.Enabled = false;
                            txt_scnCodePDxn.Text = "";
                            txt_scnCodePDxn.Focus();
                            chkScCode_PDxn = true;
                            chkMkp_PDxn = true;
                        }
                    }
                    else
                    {
                        //Reset
                        if (chb_nhaptayPDxn.Checked == true)
                        {
                            txt.Text = "";
                            txt.Enabled = false;
                            txt_scnCodePDxn.Text = "";
                            txt_scnCodePDxn.Focus();
                            chkScCode_PDxn = true;
                        }
                        else
                        {
                            txt.Text = "";
                            txt.Enabled = false;
                            txt_scnCodePDxn.Text = "";
                            txt_scnCodePDxn.Focus();
                            chkScCode_PDxn = true;
                            chkMkp_PDxn = true;
                        }
                        MessageBox.Show("KTZ đã dán sai tem Code vào cuộn liệu\nHãy thông tin ngay cho Leader!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void chb_nhaptayPDxn_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_nhaptayPDxn.Checked == true)
            {
                AcceptButton = btn_enterPDxn;
                //if (txt_scnCodePDxn.Text != "")
                //{
                    if (arrgPerPDxn == true)
                    {
                        txt_inpManulPDxn.Visible = true;
                        txt_inpManulPDxn.Enabled = true;
                        txt_inpManulPDxn.Text = "";
                        txt_inpManulPDxn.Focus();
                        txt_inpAutoPDxn.Visible = false;
                        txt_inpAutoPDxn.Enabled = false;
                        txt_inpAutoPDxn.Text = "";
                        btn_enterPDxn.Visible = true;
                        btn_enterPDxn.Enabled = true;
                        lbl_inputPDxn.Text = "Nhập MakerPart";
                    }
                    else
                    {
                        nhapCodePDxn = true;
                        bool Isopen = false;
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Text == "ConfirmInOut")
                            {
                                Isopen = true;
                                f.BringToFront();
                                break;
                            }
                        }
                        if (Isopen == false)
                        {
                            ConfirmInOut confirmAd = new ConfirmInOut(this, str_database);
                            confirmAd.Show();
                            count_timer = 0;
                            timer1.Start();
                        }
                    }
                //} 
                //else
                //{
                //    txt_inpManulPDxn.Visible = true;
                //    txt_inpManulPDxn.Enabled = false;
                //    txt_inpAutoPDxn.Visible = false;
                //    txt_inpAutoPDxn.Enabled = false;
                //    btn_enterPDxn.Visible = true;
                //    btn_enterPDxn.Enabled = true;
                //    lbl_inputPDxn.Text = "Nhập MakerPart";
                //}
            }
            else
            {
                btn_enterPDxn.Visible = false;
                btn_enterPDxn.Enabled = false;
                lbl_inputPDxn.Text = "Scan MakerPart";

                //if (txt_scnCodePDxn.Text != "")
                //{
                    txt_inpManulPDxn.Visible = false;
                    txt_inpManulPDxn.Enabled = false;
                    txt_inpManulPDxn.Text = "";
                    txt_inpAutoPDxn.Visible = true;
                    txt_inpAutoPDxn.Enabled = true;
                    txt_inpAutoPDxn.Text = "";
                    txt_inpAutoPDxn.Focus();                   
                //}
                //else
                //{
                //    txt_inpManulPDxn.Visible = false;
                //    txt_inpManulPDxn.Enabled = false;
                //    txt_inpAutoPDxn.Visible = true;
                //    txt_inpAutoPDxn.Enabled = false;
                //}               
            }
        }

        private void rtb_reloadPDxn_CheckedChanged(object sender, EventArgs e)
        {
            if (rtb_reloadPDxn.Checked == true)
            {
                if(dgv_PDxn.RowCount == 0)
                {
                    timer_reLoad.Start();
                    try
                    {
                        //DataTable dt_reload = new DataTable();
                        dtble1PDxn = new DataTable();
                        StreamReader sr = new StreamReader(str_database + "\\tem\\" + cbx_modelPDxn.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPDxacnhan.txt");
                        string[] colName = sr.ReadLine().Split(',');
                        for (int j = 0; j < colName.Length - 1; j++)
                        {
                            dtble1PDxn.Columns.Add(colName[j]);
                        }

                        string newLine;
                        while ((newLine = sr.ReadLine()) != null)
                        {
                            DataRow dtr = dtble1PDxn.NewRow();
                            string[] values = newLine.Split(',');
                            if (values[0] != "")
                            {
                                for (int i = 0; i < values.Length - 1; i++)
                                {
                                    dtr[i] = values[i];
                                }
                                dtble1PDxn.Rows.Add(dtr);
                            }
                        }
                        sr.Close();

                        dgv_PDxn.Columns.Clear();
                        dtb1.show_PDxacnhan(dgv_PDxn, dtble1PDxn);
                        //An bot data
                        dgv_PDxn.Columns["Ngay_thang"].Visible = false;
                        dgv_PDxn.Columns["Ca_kip"].Visible = false;
                        dgv_PDxn.Columns["Line"].Visible = false;
                        dgv_PDxn.Columns["Model"].Visible = false;
                        dgv_PDxn.Columns["KTZ"].Visible = false;
                        dgv_PDxn.Columns["PD"].Visible = false;
                        //Not sort
                        foreach (DataGridViewColumn col in dgv_PDxn.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        txt_scnCodePDxn.Enabled = true;
                        if (chb_nhaptayPDxn.Checked == true)
                        {
                            txt_inpManulPDxn.Enabled = false;
                        }
                        else
                        {
                            txt_inpAutoPDxn.Enabled = false;
                        }
                        rtb_reloadPDxn.Checked = false;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Data Re-load trống. Hãy tiếp tục thao tác!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rtb_reloadPDxn.Checked = false;
                    }
                }
                else
                {
                    MessageBox.Show("Bạn không thể tải dữ liệu lúc này!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtb_reloadPDxn.Checked = false;
                }               
            }           
        }

        private void btn_delPDxn_Click(object sender, EventArgs e)
        {
            txt_scnCodePDxn.Focus();;
            try
            {
                // Nếu dùng dataGridView2.SelectedRows.Count thì phải click vào đầu hàng
                // Nếu dùng dataGridView2.CurrentRow.Index thì click vào bất kì vị trí có thể xóa hàng đó

                if (this.dgv_PDxn.CurrentRow.Index >= 0)
                {
                    dtb1.del_filLog("Input_Line", dgv_PDxn.Rows[dgv_PDxn.CurrentRow.Index].Cells["Tem_code"].Value.ToString(), 1, str_database);

                    DataRow drToDelete = dtble1PDxn.Rows[dgv_PDxn.CurrentRow.Index];
                    dtble1PDxn.Rows.Remove(drToDelete);
                }
                if(dgv_PDxn.RowCount == 0)
                {
                    chkMkp_PDxn = true;
                    chkScCode_PDxn = true;
                    sttPDxn = 0;
                }
            }
            catch
            {
                MessageBox.Show("Click vào đầu hàng đó để xóa!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_xnPDxn_Click(object sender, EventArgs e)
        {
            //Kiem tra dien du data
            if (dtb1.checkPDnhan(dgv_PDxn) == true)//kiểm tra datagirdview dc điền đủ thông tin
            {
                MessageBox.Show("Các thông tin đang để trống hoặc sai!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int betWen = GapPDKTZ(dgv_PDxn);
                //Check thong tin da nhap vs data KTZ giao
                if (CheckData(dgv_PDxn) == true)
                {
                    if(betWen > 0)
                    {
                        DialogResult rell = MessageBox.Show("Số cuộn NVL Line xác nhận thiếu so với C/S KTZ giao : " + betWen.ToString() + "\nBạn có muốn tiếp tục lưu logfile?", "PDxacnhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if(rell == DialogResult.Yes)
                        {
                            //Luu thong tin (tạo logfile)
                            if (dtb1.insert_PDxn(dgv_PDxn, dDay, dShift, Acc, cbx_KTZ_PDxn.Text, cbx_modelPDxn.Text) == true)
                            {
                                bool chekExitFil = excel.checkExitLog(str_database + "\\History\\In_Out\\PD_xacnhan\\" + dMon + "\\" + datTim + "_" + cbx_modelPDxn.Text + ".csv");
                                if (excel.exportLogfilePDxn(dgv_PDxn, str_database + "\\History\\In_Out\\PD_xacnhan\\" + dMon + "\\" + datTim + "_" + cbx_modelPDxn.Text + ".csv", chekExitFil, 2, 1) == true)
                                {
                                    MessageBox.Show("Tạo LogFile thành công!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    if (File.Exists(str_database + "\\tem\\" + cbx_modelPDxn.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPDxacnhan.txt"))
                                    {
                                        File.Delete(str_database + "\\tem\\" + cbx_modelPDxn.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPDxacnhan.txt");
                                    }
                                    //update lich su
                                    #region
                                    try
                                    {
                                        var nvls = new List<NVL>() { };
                                        StreamReader sr = new StreamReader(str_database + "\\History\\HistoryNVL.txt");
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

                                        for (int k = 0; k < dgv_PDxn.RowCount; k++)
                                        {
                                            if (dgv_PDxn.Rows[k].Cells["Mo_ta"].Value != null && dgv_PDxn.Rows[k].Cells["Mo_ta"].Value.ToString() != "")
                                            {
                                                string temCode = dgv_PDxn.Rows[k].Cells["Tem_code"].Value.ToString();
                                                foreach (var nn in nvls.Where(x => x.temCode == temCode))
                                                {
                                                    nn.PDxacnhan = stl_nameUser.Text;
                                                    nn.tgianxacnhan = DateTime.Now.ToString();
                                                }
                                            }
                                        }

                                        FileStream fs = new FileStream(str_database + "\\History\\HistoryNVL.txt", FileMode.Create);
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

                                        //hien thi stock line
                                        dgv_stkLinePDxn.Columns.Clear();
                                        DataTable dt_sl4 = dtb1.search_stock("KtzGiaoPd1", false);
                                        dtb1.show_StockLinee(dgv_stkLinePDxn, dt_sl4);
                                        dgv_stkLinePDxn.Columns["Ngay_thang"].Visible = false;
                                        dgv_stkLinePDxn.Columns["Ca_kip"].Visible = false;
                                        dgv_stkLinePDxn.Columns["Line"].Visible = false;
                                        dgv_stkLinePDxn.Columns["Model"].Visible = false;
                                        dgv_stkLinePDxn.Columns["KTZ"].Visible = false;
                                        dgv_stkLinePDxn.Columns["PD"].Visible = false;
                                        //Reset data
                                        dtb1.delete_Transport("PDxacnhanStock");
                                        dtb1.delete_Transport("PDxacnhanStock_1");
                                        dtb1.delete_Transport("PDxacnhan");
                                        cbx_KTZ_PDxn.Text = "";
                                        picPDxn1.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                        picPDxn1.SizeMode = PictureBoxSizeMode.StretchImage;
                                        picPDxn2.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                        picPDxn2.SizeMode = PictureBoxSizeMode.StretchImage;
                                        picPDxn1.Visible = true;
                                        lbl_PDxn1.Visible = true;
                                        picPDxn2.Visible = true;
                                        lbl_PDxn2.Visible = true;
                                        rtb_reloadPDxn.Checked = false;
                                        chkMkp_PDxn = true;
                                        chkScCode_PDxn = true;
                                        sttPDxn = 0;
                                        timer_reLoad.Stop();
                                        tool_saving.BackColor = Color.White;
                                        //xoa file .log
                                        try
                                        {
                                            string[] files = Directory.GetFiles(str_database + "\\Log\\Duplicate\\");
                                            int m = 0;
                                            foreach (string fil in files)
                                            {
                                                if (files[m].Contains("Input_Line") || files[m].Contains("Input_Ktz") || files[m].Contains("NVL_Holding"))
                                                {
                                                    goto jumpm;
                                                }
                                                else if (files[m].Contains("PDxacnhan"))
                                                {
                                                    foreach (DataGridViewRow dgr in dgv_PDxn.Rows)
                                                    {
                                                        if (dgr.Cells["Mo_ta"].Value != null && dgr.Cells["Mo_ta"].Value.ToString() != "")
                                                        {
                                                            dtb1.del_filLog("PDxacnhan", dgr.Cells["Tem_code"].Value.ToString(), 1, str_database);
                                                        }
                                                    }
                                                    dgv_PDxn.Columns.Clear();
                                                    goto jumpm;
                                                }
                                                File.Delete(fil);
                                            jumpm:
                                                m++;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            MessageBox.Show("Xảy ra lỗi xóa file .log (FI-FO, MakerPart)!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show("Xảy ra lỗi cập nhật history NVL!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    MessageBox.Show("Xảy ra lỗi xuất logfile!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Xảy ra lỗi cập nhật database!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else if(betWen == 0)
                    {
                        //Luu thong tin (tạo logfile)
                        if (dtb1.insert_PDxn(dgv_PDxn, dDay, dShift, Acc, cbx_KTZ_PDxn.Text, cbx_modelPDxn.Text) == true)
                        {                           
                            bool chekExitFil = excel.checkExitLog(str_database + "\\History\\In_Out\\PD_xacnhan\\" + dMon + "\\" + datTim + "_" + cbx_modelPDxn.Text + ".csv");
                            if (excel.exportLogfilePDxn(dgv_PDxn, str_database + "\\History\\In_Out\\PD_xacnhan\\" + dMon + "\\" + datTim + "_" + cbx_modelPDxn.Text + ".csv", chekExitFil, 2, 1) == true)
                            {
                                MessageBox.Show("Tạo LogFile thành công!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (File.Exists(str_database + "\\tem\\" + cbx_modelPDxn.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPDxacnhan.txt"))
                                {
                                    File.Delete(str_database + "\\tem\\" + cbx_modelPDxn.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPDxacnhan.txt");
                                }
                                //update lich su
                                #region
                                try
                                {
                                    var nvls = new List<NVL>() { };
                                    StreamReader sr = new StreamReader(str_database + "\\History\\HistoryNVL.txt");
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

                                    for (int k = 0; k < dgv_PDxn.RowCount; k++)
                                    {
                                        if (dgv_PDxn.Rows[k].Cells["Mo_ta"].Value != null && dgv_PDxn.Rows[k].Cells["Mo_ta"].Value.ToString() != "")
                                        {
                                            string temCode = dgv_PDxn.Rows[k].Cells["Tem_code"].Value.ToString();
                                            foreach (var nn in nvls.Where(x => x.temCode == temCode))
                                            {
                                                nn.PDxacnhan = stl_nameUser.Text;
                                                nn.tgianxacnhan = DateTime.Now.ToString();
                                            }
                                        }
                                    }

                                    FileStream fs = new FileStream(str_database + "\\History\\HistoryNVL.txt", FileMode.Create);
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

                                    //hien thi stock line
                                    dgv_stkLinePDxn.Columns.Clear();
                                    DataTable dt_sl4 = dtb1.search_stock("KtzGiaoPd1", false);
                                    dtb1.show_StockLinee(dgv_stkLinePDxn, dt_sl4);
                                    dgv_stkLinePDxn.Columns["Ngay_thang"].Visible = false;
                                    dgv_stkLinePDxn.Columns["Ca_kip"].Visible = false;
                                    dgv_stkLinePDxn.Columns["Line"].Visible = false;
                                    dgv_stkLinePDxn.Columns["Model"].Visible = false;
                                    dgv_stkLinePDxn.Columns["KTZ"].Visible = false;
                                    dgv_stkLinePDxn.Columns["PD"].Visible = false;
                                    //Reset data
                                    dtb1.delete_Transport("PDxacnhanStock");
                                    dtb1.delete_Transport("PDxacnhanStock_1");
                                    dtb1.delete_Transport("PDxacnhan");
                                    cbx_KTZ_PDxn.Text = "";
                                    picPDxn1.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                    picPDxn1.SizeMode = PictureBoxSizeMode.StretchImage;
                                    picPDxn2.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                    picPDxn2.SizeMode = PictureBoxSizeMode.StretchImage;
                                    picPDxn1.Visible = true;
                                    lbl_PDxn1.Visible = true;
                                    picPDxn2.Visible = true;
                                    lbl_PDxn2.Visible = true;
                                    rtb_reloadPDxn.Checked = false;
                                    chkMkp_PDxn = true;
                                    chkScCode_PDxn = true;
                                    sttPDxn = 0;
                                    timer_reLoad.Stop();
                                    tool_saving.BackColor = Color.White;
                                    //xoa file .log
                                    try
                                    {
                                        string[] files = Directory.GetFiles(str_database + "\\Log\\Duplicate\\");
                                        int m = 0;
                                        foreach (string fil in files)
                                        {
                                            if (files[m].Contains("Input_Line") || files[m].Contains("Input_Ktz") || files[m].Contains("NVL_Holding"))
                                            {
                                                goto jumpm;
                                            }
                                            else if (files[m].Contains("PDxacnhan"))
                                            {
                                                foreach (DataGridViewRow dgr in dgv_PDxn.Rows)
                                                {
                                                    if (dgr.Cells["Mo_ta"].Value != null && dgr.Cells["Mo_ta"].Value.ToString() != "")
                                                    {
                                                        dtb1.del_filLog("PDxacnhan", dgr.Cells["Tem_code"].Value.ToString(), 1, str_database);
                                                    }
                                                }
                                                dgv_PDxn.Columns.Clear();
                                                goto jumpm;
                                            }
                                            File.Delete(fil);
                                        jumpm:
                                            m++;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show("Xảy ra lỗi xóa file .log (FI-FO, MakerPart)!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Xảy ra lỗi cập nhật history NVL!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                #endregion
                            }
                            else
                            {
                                MessageBox.Show("Xảy ra lỗi xuất logfile!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Xảy ra lỗi cập nhật database!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("PD xác nhận nhiều hơn với C/S bàn giao của KTZ!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (betWen > 0)
                    {
                        MessageBox.Show("Xảy ra lỗi!\n1. NVL bị bôi đỏ không có trong C/S bàn giao của KTZ!\n2. NVL bôi vàng không đúng với C/S bàn giao của KTZ!\n3. Số cuộn NVL Line xác nhận thiếu so với C/S KTZ giao là " + betWen.ToString(), "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (betWen == 0)
                    {
                        MessageBox.Show("Xảy ra lỗi!\n1. NVL bị bôi đỏ không có trong C/S bàn giao của KTZ!\n2. NVL bôi vàng không đúng với C/S bàn giao của KTZ!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("PD xác nhận nhiều hơn với C/S bàn giao của KTZ!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        public string pathCompare = string.Empty;
        public string[,] arrNVL = new string[2000, 15];//1000 dong=1000 cuộn liệu, 13 cot 
        public StreamReader srSourceCom;
        public bool CheckData(DataGridView dgv)
        {
            //Get data NVL KTZ da giao
            int rw = 0;
            try
            {
                srSourceCom = new StreamReader(pathCompare);
                while (srSourceCom.EndOfStream == false)
                {
                    int col = 0;
                    string str1 = srSourceCom.ReadLine();
                    string[] str2 = str1.Split(',');
                    for (int i = 0; i < str2.Length; i++)
                    {
                        arrNVL[rw, col] = str2[i];
                        col++;
                    }
                    rw++;
                }
                srSourceCom.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi đọc logfile KTZ giao NVL PD", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                srSourceCom.Close();
            }

            //So sanh
            int maxRow = arrNVL.GetLength(0);
            int errExist = 0, errMatching = 0, err = 0;
            try
            {
                foreach (DataGridViewRow dgr in dgv.Rows)
                {
                    if (dgr.Cells["Mo_ta"].Value != null && dgr.Cells["Mo_ta"].Value.ToString() != "")
                    {
                        for (int i = 0; i < maxRow; i++)
                        {
                            if (arrNVL[i, 0] != null && arrNVL[i, 0] != "")
                            {
                                if (dgr.Cells["Tem_code"].Value.ToString() == arrNVL[i, 10])
                                {
                                    errExist = 0;
                                    //if (dgr.Cells["Line"].Value.ToString() != arrNVL[i, 2])
                                    //{
                                    //    errMatching++;
                                    //    dgr.Cells["Line"].Style.BackColor = Color.Yellow;
                                    //    break;
                                    //}
                                    if (dgr.Cells["Model"].Value.ToString() != arrNVL[i, 3])
                                    {
                                        errMatching++;
                                        dgr.Cells["Model"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    //else if (dgr.Cells["Mo_ta"].Value.ToString() != arrNVL[i, 4])
                                    //{
                                    //    errMatching++;
                                    //    dgr.Cells["Mo_ta"].Style.BackColor = Color.Yellow;
                                    //    break;
                                    //}
                                    else if (dgr.Cells["Ma_NVL"].Value.ToString() != arrNVL[i, 5])
                                    {
                                        errMatching++;
                                        dgr.Cells["Ma_NVL"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else if (dgr.Cells["Maker"].Value.ToString() != arrNVL[i, 6])
                                    {
                                        errMatching++;
                                        dgr.Cells["Maker"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else if (dgr.Cells["Maker_Part"].Value.ToString() != arrNVL[i, 7])
                                    {
                                        errMatching++;
                                        dgr.Cells["Maker_Part"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else if (dgr.Cells["Lot"].Value.ToString() != arrNVL[i, 8])
                                    {
                                        errMatching++;
                                        dgr.Cells["Lot"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else if (dgr.Cells["So_luong_cap"].Value.ToString() != arrNVL[i, 9])
                                    {
                                        errMatching++;
                                        dgr.Cells["So_luong_cap"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else
                                    {
                                        errMatching = 0;
                                        //dgr.Cells["Line"].Style.BackColor = Color.White;
                                        dgr.Cells["Model"].Style.BackColor = Color.White;
                                        //dgr.Cells["Mo_ta"].Style.BackColor = Color.White;
                                        dgr.Cells["Ma_NVL"].Style.BackColor = Color.White;
                                        dgr.Cells["Maker"].Style.BackColor = Color.White;
                                        dgr.Cells["Maker_Part"].Style.BackColor = Color.White;
                                        dgr.Cells["Lot"].Style.BackColor = Color.White;
                                        dgr.Cells["So_luong_cap"].Style.BackColor = Color.White;
                                        break;
                                    }
                                }
                                else if(dgr.Cells["Tem_code"].Value.ToString().Contains(arrNVL[i, 11].Substring(1, arrNVL[i, 11].Length - 1)))
                                {
                                    errExist = 0;
                                    //if (dgr.Cells["Line"].Value.ToString() != arrNVL[i, 2])
                                    //{
                                    //    errMatching++;
                                    //    dgr.Cells["Line"].Style.BackColor = Color.Yellow;
                                    //    break;
                                    //}
                                    if (dgr.Cells["Model"].Value.ToString() != arrNVL[i, 3])
                                    {
                                        errMatching++;
                                        dgr.Cells["Model"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    //else if (dgr.Cells["Mo_ta"].Value.ToString() != arrNVL[i, 4])
                                    //{
                                    //    errMatching++;
                                    //    dgr.Cells["Mo_ta"].Style.BackColor = Color.Yellow;
                                    //    break;
                                    //}
                                    else if (dgr.Cells["Ma_NVL"].Value.ToString() != arrNVL[i, 5])
                                    {
                                        errMatching++;
                                        dgr.Cells["Ma_NVL"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else if (dgr.Cells["Maker"].Value.ToString() != arrNVL[i, 6])
                                    {
                                        errMatching++;
                                        dgr.Cells["Maker"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else if (!dgr.Cells["Maker_Part"].Value.ToString().Contains(arrNVL[i, 7].Substring(1, arrNVL[i, 7].Length - 1)))
                                    {
                                        errMatching++;
                                        dgr.Cells["Maker_Part"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else if (dgr.Cells["Lot"].Value.ToString() != arrNVL[i, 9])
                                    {
                                        errMatching++;
                                        dgr.Cells["Lot"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else if (dgr.Cells["So_luong_cap"].Value.ToString() != arrNVL[i, 10])
                                    {
                                        errMatching++;
                                        dgr.Cells["So_luong_cap"].Style.BackColor = Color.Yellow;
                                        break;
                                    }
                                    else
                                    {
                                        errMatching = 0;
                                        //dgr.Cells["Line"].Style.BackColor = Color.White;
                                        dgr.Cells["Model"].Style.BackColor = Color.White;
                                        //dgr.Cells["Mo_ta"].Style.BackColor = Color.White;
                                        dgr.Cells["Ma_NVL"].Style.BackColor = Color.White;
                                        dgr.Cells["Maker"].Style.BackColor = Color.White;
                                        dgr.Cells["Maker_Part"].Style.BackColor = Color.White;
                                        dgr.Cells["Lot"].Style.BackColor = Color.White;
                                        dgr.Cells["So_luong_cap"].Style.BackColor = Color.White;
                                        break;
                                    }
                                }
                                else
                                {
                                    errExist++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        //PD ko tìm thấy NVL KTZ giao
                        if (errExist != 0)
                        {
                            dgr.Cells["Tem_code"].Style.BackColor = Color.Red;
                            err++;
                        }
                        else
                        {
                            dgr.Cells["Tem_code"].Style.BackColor = Color.White;
                        }

                        //Data khong matching giưa PD và KTZ
                        if (errMatching != 0)
                        {
                            err++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi so sánh C/S giao nhận giữa KTZ và PD", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            //Tra ve kq ktra
            if(err != 0)
            {                
                return false;
            }
            else
            {
                return true;
            }
        }

        public int GapPDKTZ(DataGridView dgv)
        {
            int rw1 = 0;
            //Get data NVL KTZ da giao
            try
            {
                //DirectoryInfo dir = new DirectoryInfo(str_database + "\\History\\In_Out\\Ktz_to_PD\\" + dMon);
                //if (dir.Exists)
                //{
                //    foreach (FileInfo fIn in dir.GetFiles())
                //    {
                //        if (fIn.Name.Contains(datTim + "_" + cbx_modelPDxn.Text + ".csv"))
                //        {
                            StreamReader srSource = new StreamReader(pathCompare);
                            while (srSource.EndOfStream == false)
                            {
                                string str1 = srSource.ReadLine();
                                string[] str2 = str1.Split(',');
                                if (str2[0] != "" && str2[0] != "Ngay_thang")
                                {
                                    rw1++;
                                }
                            }
                            srSource.Close();
                //        }
                //    }
                //}
            }
            catch (Exception)
            {
                rw1 = 0;
            }

            int rw2 = 0;
            //Get data NVL PD da nhan
            try
            {
                DirectoryInfo dir2 = new DirectoryInfo(str_database + "\\History\\In_Out\\PD_xacnhan\\" + dMon);
                if (dir2.Exists)
                {
                    foreach (FileInfo fIn2 in dir2.GetFiles())
                    {
                        if (fIn2.Name.Contains(datTim + "_" + cbx_modelPDxn.Text + ".csv"))
                        {
                            StreamReader srSource2 = new StreamReader(fIn2.DirectoryName + "\\" + fIn2);
                            while (srSource2.EndOfStream == false)
                            {
                                string str12 = srSource2.ReadLine();
                                string[] str22 = str12.Split(',');
                                if (str22[0] != "" && str22[0] != "Ngay_thang")
                                {
                                    rw2++;
                                }
                            }
                            srSource2.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                rw2 = 0;
            }

            if(dgv.RowCount == 0)
            {
                return rw1 - rw2;
            }
            else
            {
                return rw1 - rw2 - (dgv.RowCount - 1);
            }            
        }

        private void btn_chkData_Click(object sender, EventArgs e)
        {
            if ((DateTime.Compare(DateTime.Parse(dtimPic1_com.Value.ToShortDateString()), DateTime.Parse(dtimPic2_com.Value.ToShortDateString())) > 0) || (cbx_modelPDxn.Text.Length == 0))
            {
                MessageBox.Show("Hãy xem lại Model/thời gian bạn muốn kiểm tra!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //Summary
                string[] hisroryCheck = GetHistory(dtimPic1_com, dtimPic2_com);
                int num_filOk = 0;
                int num_file = dtb1.count_file(str_database + "\\History\\In_Out\\Ktz_to_PD\\", hisroryCheck);
                string[] nam_file = dtb1.get_filOK(num_file, str_database + "\\History\\In_Out\\Ktz_to_PD\\", hisroryCheck, cbx_modelPDxn.Text, dtimPic1_com.Text, dtimPic2_com.Text, num_filOk);
                pathCompare = dtb1.merg_Excel3(str_database + "\\History\\In_Out\\Ktz_to_PD\\", hisroryCheck, nam_file, nam_file.Length, datTim, "PDxacnhan", false, str_database);
            }

            int betWen = GapPDKTZ(dgv_PDxn);
            //Check thong tin da nhap vs data KTZ giao
            if (CheckData(dgv_PDxn) == true)
            {
                if (betWen > 0)
                {
                    MessageBox.Show("Dữ liệu bạn nhập đúng với C/S bàn giao của KTZ!\nSố cuộn NVL Line xác nhận thiếu so với C/S KTZ giao là " + betWen.ToString(), "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if(betWen == 0)
                {
                    MessageBox.Show("Dữ liệu bạn nhập đúng với C/S bàn giao của KTZ!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("PD xác nhận nhiều hơn với C/S bàn giao của KTZ!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (betWen > 0)
                {
                    MessageBox.Show("Xảy ra lỗi!\n1. NVL bị bôi đỏ không có trong C/S bàn giao của KTZ!\n2. NVL bôi vàng không đúng với C/S bàn giao của KTZ!\n3. Số cuộn NVL Line xác nhận thiếu so với C/S KTZ giao là " + betWen.ToString(), "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if( betWen == 0)
                {
                    MessageBox.Show("Xảy ra lỗi!\n1. NVL bị bôi đỏ không có trong C/S bàn giao của KTZ!\n2. NVL bôi vàng không đúng với C/S bàn giao của KTZ!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Error);                
                }
                else
                {
                    MessageBox.Show("PD xác nhận nhiều hơn với C/S bàn giao của KTZ!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }  
        } 

        private void btn_chkLinePDxn_Click(object sender, EventArgs e)
        {
            //Show Stock Line
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockLine2")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockLine2 s_line2 = new StockLine2(str_database);
                s_line2.Show();
            }
        }

        private void btn_ktPDxn_Click(object sender, EventArgs e)
        {
            if ((DateTime.Compare(DateTime.Parse(dTiPic1_PDxn.Value.ToShortDateString()), DateTime.Parse(dTiPic2_PDxn.Value.ToShortDateString())) > 0) || (cbx_modelPDxn.Text.Length == 0))
            {
                MessageBox.Show("Hãy xem lại Model/thời gian bạn muốn kiểm tra!", "PDxacnhan", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string[] hisroryCheck = GetHistory(dTiPic1_PDxn, dTiPic2_PDxn);
                //Tìm history theo ngày -> tổng hợp -> Hiển thị Excel
                //Get file name trong khoảng time đã chọn
                int num_filOk = 0;
                int num_file = dtb1.count_file(str_database + "\\History\\In_Out\\PD_xacnhan\\", hisroryCheck);
                string[] nam_file = dtb1.get_filOK(num_file, str_database + "\\History\\In_Out\\PD_xacnhan\\", hisroryCheck, cbx_modelPDxn.Text, dTiPic1_PDxn.Text, dTiPic2_PDxn.Text, num_filOk);
                //Sprire.XLS -> merge all file 
                //Open file merged(save tạm ra đâu đó)
                //Save as file merged nếu muốn
                dtb1.merg_Excel2(str_database + "\\History\\In_Out\\PD_xacnhan\\", hisroryCheck, nam_file, nam_file.Length, datTim, "PDxacnhan", false, str_database);    
            }
        }

        private void lbl_PDxn1_Click(object sender, EventArgs e)
        {
            if (arrgPerPDxn == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerPDxn, str_database);
                    infPicture.Show();
                }
            }
        }

        private void lbl_PDxn2_Click(object sender, EventArgs e)
        {
            if (arrgPerPDxn == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerPDxn, str_database);
                    infPicture.Show();
                }
            }
        }

        private void dgv_PDxn_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            txt_scnCodePDxn.Focus();
        }

        private void cbx_modelPDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void cbx_KTZ_PDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_scnCodePDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_inpManulPDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_inpAutoPDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_enterPDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_delPDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_xnPDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_chkLinePDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_ktPDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void dgv_PDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void dgv_stkLinePDxn_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }
        #endregion

        //=============================================================PD-KTZ===============================================================================
        #region PD-KTZ
        private void cbx_ModelPK_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_Ktz.Text = "";
            dtb1.delete_Transport("Pd_Ktz");
            dtb1.delete_Transport("Pd_Ktz_tranfer");
            dgv_Pd_Ktz.Columns.Clear();

            //string[] strMolRun = dtb1.get_modelRun();
            //int errMolRun = 0;
            //if (strMolRun[0] != "none")
            //{
            //    for (int i = 0; i < strMolRun.Length; i++)
            //    {
            //        if (cbx_ModelPK.Text == strMolRun[i])
            //        {
            //            errMolRun = 0;
            //        }
            //        else
            //        {
            //            errMolRun++;
            //        }
            //    }
            //    if (errMolRun == 0)
            //    {
                    tranPK = dtb1.LoadBOM(cbx_ModelPK.Text);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Bạn đang chọn sai Model. Line đang chạy Model khác!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        cbx_ModelPK.Text = "";
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Không có NVL đang tồn trên Line!", "In/Out Material", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            //    tranPK = dtb1.LoadBOM(cbx_ModelPK.Text);
            //}           
        }

        private void cbx_Ktz_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_autoInputPk.Focus();
        }

        private async void txt_autoInputPk_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(2000);
            if (txt_autoInputPk.Text != "" && chkScCode_PK == true)
            {
                chkScCode_PK = false;
                if ((cbx_ModelPK.Text == "") || (cbx_Ktz.Text == ""))
                {
                    MessageBox.Show("Bạn điền thiếu thông tin Model/KTZ!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_autoInputPk.Text = "";
                    cbx_ModelPK.Focus();
                    chkScCode_PK = true;
                }
                else
                {
                    txt_manualInputPk.Text = "";
                    //Check double Input
                    if (dtb1.chekdoubleCode1(txt_autoInputPk.Text, str_database) == false)
                    {
                        txt_autoInputPk.ResetText();
                        MessageBox.Show("Trùng cuộn liệu đã return về KTZ. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputPk.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_PK = true;
                    }
                    //Check code cho PD xac nhan
                    else if (dtb1.chekdoubleCodePDxacnhan(txt_autoInputPk.Text, str_database) == false)
                    {
                        txt_autoInputPk.ResetText();
                        MessageBox.Show("Cuộn liệu đang chờ PD xác nhận. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputPk.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_PK = true;
                    }
                    //check format code
                    else if (dtb1.chk_formInput(txt_autoInputPk.Text) == false)
                    {
                        txt_autoInputPk.ResetText();
                        MessageBox.Show("Sai format code input. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputPk.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_PK = true;
                    }
                    //check code da dc input line va chua return ve Ktz
                    else if (dtb1.chekNewCodeInputed(txt_autoInputPk.Text, str_database) == true)
                    {
                        txt_autoInputPk.ResetText();
                        MessageBox.Show("NVL chưa được input Line, không thể trả về KTZ. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputPk.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_PK = true;
                    }                    
                    else
                    {
                        if (arrgPerPK == false)//OP
                        {
                            //Gọi hàm check thông tin input
                            input_PK(txt_autoInputPk);
                        }
                        else//admin, manager
                        {
                            DialogResult rel_ar = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "In/Out Material", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (rel_ar == DialogResult.OK)
                            {
                                //Gọi hàm check thông tin input
                                input_PK(txt_autoInputPk);
                            }
                        }
                    }
                }
            }
        }

        private void btn_EnterPK_Click(object sender, EventArgs e)
        {
            if (txt_autoInputPk.Text == "")
            {
                if (cbx_ModelPK.Text == "" || txt_manualInputPk.Text == "" || cbx_Ktz.Text == "")
                {
                    MessageBox.Show("Bạn điền thiếu thông tin Model/Ktz/Input!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbx_ModelPK.Focus();
                }
                else
                {
                    txt_manualInputPk.Text = crtUpperChar(txt_manualInputPk.Text);
                    //Check double Input
                    if (dtb1.chekdoubleCode1(txt_manualInputPk.Text, str_database) == false)
                    {
                        //txt_manualInputPk.ResetText();
                        MessageBox.Show("Trùng cuộn liệu đã return về KTZ. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputPk.Focus();//Trỏ chuột tại textBox Input
                    }
                    //Check code cho PD xac nhan
                    else if (dtb1.chekdoubleCodePDxacnhan(txt_manualInputPk.Text, str_database) == false)
                    {
                        //txt_manualInputPk.ResetText();
                        MessageBox.Show("Cuộn liệu đang chờ PD xác nhận. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputPk.Focus();//Trỏ chuột tại textBox Input
                    }
                    //check format code
                    else if (dtb1.chk_formInput(txt_manualInputPk.Text) == false)
                    {
                        //txt_manualInputPk.ResetText();
                        MessageBox.Show("Sai format code input. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputPk.Focus();//Trỏ chuột tại textBox Input
                    }
                    //check code da dc input line va chua return ve Ktz
                    else if (dtb1.chekNewCodeInputed(txt_manualInputPk.Text, str_database) == true)
                    {
                        //txt_manualInputPk.ResetText();
                        MessageBox.Show("NVL chưa được input Line, không thể trả về KTZ. Hãy kiểm tra lại!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputPk.Focus();//Trỏ chuột tại textBox Input
                    }                    
                    else
                    {
                        if (arrgPerPK == false)
                        {
                            //Gọi hàm check thông tin input
                            input_PK(txt_manualInputPk);
                        }
                        else
                        {
                            DialogResult rel_arPK = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "In/Out Material", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (rel_arPK == DialogResult.OK)
                            {
                                //Gọi hàm check thông tin input
                                input_PK(txt_manualInputPk);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy xóa mục Scan Code trước khi Enter code tay!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void input_PK(TextBox txt)
        {
            //Biến dòng khi nhảy Slg_tra_KTZ
            int iR = 0;
            //Biến xác nhận maker part same
            bool mkpSam2 = false;
            //Biến báo đã có code trong DataGirdView trùng
            bool havCodSm2 = false;
            //tach chuoi và so sanh trong stock
            string[] str_Inp = txt.Text.Split('+');
            string[] infFromBom = get_InfInpPK(tranPK, str_Inp[0], str_Inp[2], txt, cb_inputPk);
            if (infFromBom[0] != null && infFromBom[1] != null && infFromBom[2] != null && infFromBom[3] != null && infFromBom[4] != null)
            {
                //Kiểm tra SDI code, maker theo mnaker part
                if (dtb1.chekScanMakPrtSame(infFromBom[3], str_database) == false)//đã có input maker part rồi
                {
                    mkpSam2 = true;
                }
                else//chưa input maker part lần nào
                {
                    dtb1.savMakPrt(infFromBom[3], str_database);
                    mkpSam2 = false;
                }

                sttPK++;
                //Điền data vào datagridview
                if (dgv_Pd_Ktz.Columns.Count == 0 || dgv_Pd_Ktz.Rows.Count == 0)
                {
                    //run timer reload
                    timer_reLoad.Start();
                    tool_saving.BackColor = Color.Green;
                    //xoa database cu
                    dtb1.delete_Transport("PD_Ktz_tranfer");
                    if (mkpSam2 == true)
                    {
                        txt.Text = "";
                        MessageBox.Show("Kiểm tra lại file log (Makerpart) trong StartPath!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        chkScCode_PK = true;
                    }
                    else
                    {
                        dtb1.insert_trans_PdKtzv2(sttPK.ToString(), dDay, dShift, cbx_ModelPK.Text, infFromBom[0], infFromBom[1], infFromBom[2], infFromBom[3], str_Inp[3], Acc, cbx_Ktz.Text, txt.Text);
                        havCodSm2 = true;
                        tran1PK = dtb1.LoadDatabase("PD_Ktz_tranfer", cbx_ModelPK.Text, dDay);
                        //Hien thi dgv
                        dgv_Pd_Ktz.Columns.Clear();
                        dtb1.show_Pd_Ktz(dgv_Pd_Ktz, tran1PK);                       
                    }                   
                }
                else//dgv da co data
                {
                    tool_saving.BackColor = Color.Green;
                    DataRow dtrwPK = tran1PK.NewRow();
                    dtrwPK["STT"] = sttPK.ToString();
                    dtrwPK["Ngay_thang"] = dDay;
                    dtrwPK["Ca_kip"] = dShift;
                    dtrwPK["Line"] = "SMD";
                    dtrwPK["Model"] = cbx_ModelPK.Text;
                    dtrwPK["Mo_ta"] = infFromBom[0];
                    dtrwPK["Ma_NVL"] = infFromBom[1];
                    dtrwPK["Maker"] = infFromBom[2];
                    dtrwPK["Maker_Part"] = infFromBom[3];
                    dtrwPK["Lot"] = str_Inp[3];
                    dtrwPK["Slg_tra_KTZ"] = "";
                    dtrwPK["Slg_ton_Line"] = "";
                    dtrwPK["Tem_code"] = txt.Text;
                    dtrwPK["Giai_thich"] = "";
                    dtrwPK["Ghi_chu"] = "";
                    dtrwPK["PD"] = stl_nameUser.Text;
                    dtrwPK["KTZ"] = cbx_Ktz.Text;
                    tran1PK.Rows.Add(dtrwPK);
                    tran1PK.AcceptChanges();
                    havCodSm2 = true;
                }

                if (havCodSm2 == true)
                {   
                    //An bot datagridview
                    dgv_Pd_Ktz.Columns["Ngay_thang"].Visible = false;
                    dgv_Pd_Ktz.Columns["Ca_kip"].Visible = false;
                    dgv_Pd_Ktz.Columns["Line"].Visible = false;
                    dgv_Pd_Ktz.Columns["Model"].Visible = false;
                    dgv_Pd_Ktz.Columns["Tem_code"].Visible = false;
                    dgv_Pd_Ktz.Columns["PD"].Visible = false;
                    dgv_Pd_Ktz.Columns["KTZ"].Visible = false;
                    //Auto scroll
                    if (dgv_Pd_Ktz.RowCount > 7)
                    {
                        dgv_Pd_Ktz.FirstDisplayedScrollingRowIndex = dgv_Pd_Ktz.RowCount - 1;
                    }  
                    //Not sort
                    foreach (DataGridViewColumn col in dgv_Pd_Ktz.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    //nhap code vao input_ktz
                    dtb1.savInput1(txt.Text, str_database);
                    //Save data FIFO
                    //dtb1.savFIFO(infFromBom[1] + "+" + infFromBom[2] + "+" + infFromBom[3] + "+" + str_Inp[3] + "+" + str_Inp[1]);
                    //Nhảy chuột về lot
                    foreach (DataGridViewRow dgr in dgv_Pd_Ktz.Rows)
                    {
                        if (dgr.Cells["Mo_ta"].Value != null && dgr.Cells["Mo_ta"].Value.ToString() != "")
                        {
                            if (dgr.Cells["Slg_tra_KTZ"].Value.ToString() == "")
                            {
                                dgr.Cells["Slg_tra_KTZ"].Selected = true;
                                iR = dgv_Pd_Ktz.SelectedCells[0].OwningRow.Index;
                                dgv_Pd_Ktz.CurrentCell = dgv_Pd_Ktz["Slg_tra_KTZ", iR];
                                dgv_Pd_Ktz.BeginEdit(true);
                            }
                        }                       
                    }

                    //Standard Lot
                    string strIma1 = string.Empty;
                    string strIma2 = string.Empty;
                    #region
                    switch (infFromBom[2])
                    {
                        case "RENESAS":
                            strIma1 = infFromBom[2] + "1";
                            strIma2 = infFromBom[2] + "2";
                            break;

                        case "STMICRO":
                            strIma1 = infFromBom[2] + "1";
                            strIma2 = infFromBom[2] + "2";
                            break;

                        case "TI":
                            strIma1 = infFromBom[2] + "1";
                            strIma2 = infFromBom[2] + "2";
                            break;
                        default:
                            strIma1 = infFromBom[2];
                            strIma2 = string.Empty;
                            break;
                    }
                    #endregion

                    picBx1PK.Visible = true;
                    lblPK_Pic1.Visible = true;

                    picBx1PK.Image = new Bitmap(str_database + "\\Picture\\" + strIma1 + ".PNG");
                    picBx1PK.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (strIma2 != string.Empty)
                    {
                        picBx2PK.Visible = true;
                        lblPK_Pic2.Visible = true;
                        picBx2PK.Image = new Bitmap(str_database + "\\Picture\\" + strIma2 + ".PNG");
                        picBx2PK.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        picBx2PK.Visible = false;
                        lblPK_Pic2.Visible = false;
                    }
                    havCodSm2 = false;
                    txt.Text = "";
                    if (cb_inputPk.Checked == false)
                    {
                        chkScCode_PK = true;
                    }
                }
                else
                {
                    if (cb_inputPk.Checked == false)
                    {
                        txt.Text = "";
                        chkScCode_PK = true;
                    }
                }
            }
        }

        private void cb_inputPk_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_inputPk.Checked == true)
            {
                AcceptButton = btn_EnterPK;
                if (arrgPerPK == true)
                {
                    txt_manualInputPk.Visible = true;
                    txt_manualInputPk.Enabled = true;
                    txt_manualInputPk.Focus();
                    btn_EnterPK.Visible = true;
                    btn_EnterPK.Enabled = true;
                    lbl_inputCodePK.Text = "Nhập Code";
                }
                else
                {
                    nhapCodePK = true;
                    bool Isopen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "ConfirmInOut")
                        {
                            Isopen = true;
                            f.BringToFront();
                            break;
                        }
                    }
                    if (Isopen == false)
                    {
                        ConfirmInOut confirmAd = new ConfirmInOut(this, str_database);
                        confirmAd.Show();
                        count_timer = 0;
                        timer1.Start();
                    }            
                }
            }
            else
            {
                txt_manualInputPk.Hide();
                txt_manualInputPk.Enabled = false;
                txt_manualInputPk.Text = "";
                txt_autoInputPk.Focus();
                btn_EnterPK.Visible = false;
                btn_EnterPK.Enabled = false;
                lbl_inputCodePK.Text = "Scan Code";
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            txt_autoInputPk.Focus();
            try
            {
                // Nếu dùng dataGridView2.SelectedRows.Count thì phải click vào đầu hàng
                // Nếu dùng dataGridView2.CurrentRow.Index thì click vào bất kì vị trí có thể xóa hàng đó

                if (this.dgv_Pd_Ktz.CurrentRow.Index >= 0)
                {
                    string[] str = dgv_Pd_Ktz.Rows[dgv_Pd_Ktz.CurrentRow.Index].Cells["Tem_code"].Value.ToString().Split('+');
                    string strFifo = str[0] + "+" +
                                     dgv_Pd_Ktz.Rows[dgv_Pd_Ktz.CurrentRow.Index].Cells["Maker"].Value.ToString() + "+" +
                                     str[2] + "+" +
                                     str[3] + "+" +
                                     str[1];
                    //dtb1.del_filLog("FI-FO", strFifo, 1);
                    //dtb1.del_filLog("Input_Ktz", dgv_Pd_Ktz.Rows[dgv_Pd_Ktz.CurrentRow.Index].Cells["Tem_code"].Value.ToString(), 1);
                    dtb1.del_filLog("MakerPart", dgv_Pd_Ktz.Rows[dgv_Pd_Ktz.CurrentRow.Index].Cells["Maker_Part"].Value.ToString(), 1, str_database);

                    DataRow drToDelete = tran1PK.Rows[dgv_Pd_Ktz.CurrentRow.Index];
                    tran1PK.Rows.Remove(drToDelete);
                }
                if(dgv_Pd_Ktz.RowCount == 0)
                {
                    chkScCode_PK = true;
                    sttPK = 0;
                }
            }
            catch
            {
                MessageBox.Show("Click vào đầu hàng đó để xóa!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            if (dtb1.checkPD_Ktz(dgv_Pd_Ktz) == true)
            {
                MessageBox.Show("Các thông tin đang để trống hoặc sai!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult traloi = MessageBox.Show("Bạn có chắc chắn muốn trả số NVL này?", "In/Out Materail", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (traloi == DialogResult.Yes)
                {
                    if(chk_cloPO.CheckState == CheckState.Indeterminate)
                    {
                        MessageBox.Show("Thiếu thông tin xác nhận!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        chk_cloPO.ForeColor = Color.Red;
                    }
                    else
                    {
                        chk_cloPO.ForeColor = Color.Black;
                        //Update StockFIFO
                        if (dtb1.up_FIFO2(dgv_Pd_Ktz) == true)
                        {
                            //Tăng stock sau khi PD tra NVL
                            if (dtb1.upStokKtz2(dgv_Pd_Ktz, datTim) == true)
                            {
                                //Update database
                                if (dtb1.insert_PdReturn_table(dgv_Pd_Ktz, dDay, dShift, cbx_Ktz.Text, Acc, cbx_ModelPK.Text, dgv_stkLine) == true)
                                {
                                    //Tạo logfile                             
                                    bool chekExitFil = excel.checkExitLog(str_database + "\\History\\In_Out\\PD_return_Ktz\\" + dMon + "\\" + datTim + "_" + cbx_ModelPK.Text + ".csv");
                                    if (excel.exportLogfile(dgv_Pd_Ktz, str_database + "\\History\\In_Out\\PD_return_Ktz\\" + dMon + "\\" + datTim + "_" + cbx_ModelPK.Text + ".csv", chekExitFil, 2, 1) == true)
                                    {
                                        MessageBox.Show("Tạo LogFile thành công!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        if (File.Exists(str_database + "\\tem\\" + cbx_ModelPK.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPD-KTZ.txt"))
                                        {
                                            File.Delete(str_database + "\\tem\\" + cbx_ModelPK.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPD-KTZ.txt");
                                        }

                                        //update lich su + Xoa code Stock line = 0
                                        #region
                                        try
                                        {
                                            #region
                                            var nvls = new List<NVL>() { };
                                            StreamReader sr = new StreamReader(str_database + "\\History\\HistoryNVL.txt");
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
                                                        tgianTraWH = str[17],
                                                        ghiChuTraWH = str[19]
                                                    });
                                                }
                                            }
                                            sr.Close();
                                            #endregion
                                            OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + str_database + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                                            cnn.Open();
                                            string strIn = string.Empty;
                                            for (int h = 0; h < dgv_Pd_Ktz.RowCount; h++)
                                            {
                                                //xac dinh nvl holding
                                                bool nvlHolding = false;
                                                if (dgv_Pd_Ktz.Rows[h].Cells["Mo_ta"].Value != null && dgv_Pd_Ktz.Rows[h].Cells["Mo_ta"].Value.ToString() != "")
                                                {
                                                    //=============update database NVL chua tra
                                                    //string maNVL = dgv_Pd_Ktz.Rows[h].Cells["Ma_NVL"].Value.ToString();
                                                    string temCode = dgv_Pd_Ktz.Rows[h].Cells["Tem_code"].Value.ToString();
                                                    string ghiChu = string.Empty;
                                                    if (dgv_Pd_Ktz.Rows[h].Cells["Giai_thich"].Value.ToString() == "NVL NG cần holding")
                                                    {
                                                        ghiChu = dgv_Pd_Ktz.Rows[h].Cells["Giai_thich"].Value.ToString();
                                                        nvlHolding = true;
                                                    }
                                                    else if (dgv_Pd_Ktz.Rows[h].Cells["Giai_thich"].Value.ToString() == "Khác")
                                                    {
                                                        ghiChu = dgv_Pd_Ktz.Rows[h].Cells["Ghi_chu"].Value.ToString();

                                                        if ((dgv_Pd_Ktz.Rows[h].Cells["Ghi_chu"].Value.ToString().Contains("Hold"))
                                                        || (dgv_Pd_Ktz.Rows[h].Cells["Ghi_chu"].Value.ToString().Contains("hold"))
                                                        || (dgv_Pd_Ktz.Rows[h].Cells["Ghi_chu"].Value.ToString().Contains("HOLD")))
                                                        {
                                                            nvlHolding = true;
                                                        }
                                                        else
                                                        {
                                                            nvlHolding = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ghiChu = dgv_Pd_Ktz.Rows[h].Cells["Giai_thich"].Value.ToString();
                                                        nvlHolding = false;
                                                    }

                                                    if (int.Parse(dgv_Pd_Ktz.Rows[h].Cells["Slg_tra_KTZ"].Value.ToString()) > 0)//stock ktz > 0 -> stock line = 0
                                                    {
                                                        foreach (var nn in nvls.Where(x => x.temCode == temCode))
                                                        {
                                                            nn.ngTraNVL = stl_nameUser.Text;
                                                            nn.tgianTraNVL = DateTime.Now.ToString();
                                                            nn.ghiChuTra = ghiChu;
                                                        }
                                                        //strIn = "Update HistoryNVL Set Nguoi_tra_NVL ='" + toolStripStatusLabel2.Text + "', Thoi_gian_tra ='" + DateTime.Now.ToString() + "', Ghi_chu_tra ='" + ghiChu + "' Where Model ='" + cbx_Model2.Text +
                                                        //                                                                                                                                  "' And Ma_NVL ='" + maNVL +
                                                        //                                                                                                                                  "' And Tem_code ='" + temCode + "'";

                                                        //OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                                                        //cmdIn.ExecuteNonQuery();
                                                        if (nvlHolding == true)
                                                        {
                                                            dtb1.savHolding(dgv_Pd_Ktz.Rows[h].Cells["Tem_code"].Value.ToString(), str_database);
                                                        }
                                                        //Xoa stock line = 0 trong buffer database NVL chua tra
                                                        string strDel = "Delete * From NVLChuaTra Where Tem_code ='" + dgv_Pd_Ktz.Rows[h].Cells["Tem_code"].Value.ToString() + "'";
                                                        OleDbCommand cmdDel = new OleDbCommand(strDel, cnn);
                                                        cmdDel.ExecuteNonQuery();
                                                        //xoa code stock line = 0
                                                        dtb1.del_stockLine_zero(dgv_Pd_Ktz.Rows[h].Cells["Ma_NVL"].Value.ToString(), dgv_Pd_Ktz.Rows[h].Cells["Maker_Part"].Value.ToString(), dgv_Pd_Ktz.Rows[h].Cells["Lot"].Value.ToString(), dgv_Pd_Ktz.Rows[h].Cells["Tem_code"].Value.ToString());
                                                        //xoa code trong input_Line
                                                        dtb1.del_filLog("Input_Line", dgv_Pd_Ktz.Rows[h].Cells["Tem_code"].Value.ToString(), 1, str_database);
                                                    }
                                                    else//stock ktz = 0 -> stock line > 0
                                                    {
                                                        foreach (var nn in nvls.Where(x => x.temCode == temCode))
                                                        {
                                                            nn.ngTraNVL = "Line keep";
                                                            nn.tgianTraNVL = DateTime.Now.ToString();
                                                            nn.ghiChuTra = ghiChu;
                                                        }
                                                        //strIn = "Update HistoryNVL Set Nguoi_tra_NVL ='Line keep', Thoi_gian_tra ='" + DateTime.Now.ToString() + "', Ghi_chu_tra ='" + ghiChu + "' Where Model ='" + cbx_Model2.Text +
                                                        //                                                                                                                                  "' And Ma_NVL ='" + maNVL +
                                                        //                                                                                                                                  "' And Tem_code ='" + temCode + "'";
                                                        //OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                                                        //cmdIn.ExecuteNonQuery();

                                                        if (nvlHolding == true)
                                                        {
                                                            dtb1.savHolding(dgv_Pd_Ktz.Rows[h].Cells["Tem_code"].Value.ToString(), str_database);
                                                        }
                                                        //chuyen code stock line > 0 vao database khac
                                                        string strIns2 = "Insert Into NVLChuaTra Values('" + dDay + "','"
                                                                                                           + dShift + "','"
                                                                                                           + "SMD" + "','"
                                                                                                           + cbx_ModelPK.Text + "','"
                                                                                                           + dgv_Pd_Ktz.Rows[h].Cells["Mo_ta"].Value.ToString() + "','"
                                                                                                           + dgv_Pd_Ktz.Rows[h].Cells["Ma_NVL"].Value.ToString() + "','"
                                                                                                           + dgv_Pd_Ktz.Rows[h].Cells["Maker"].Value.ToString() + "','"
                                                                                                           + dgv_Pd_Ktz.Rows[h].Cells["Maker_Part"].Value.ToString() + "','"
                                                                                                           + dgv_Pd_Ktz.Rows[h].Cells["Lot"].Value.ToString() + "','"
                                                                                                           + dgv_Pd_Ktz.Rows[h].Cells["Slg_ton_Line"].Value.ToString() + "','"
                                                                                                           + dgv_Pd_Ktz.Rows[h].Cells["Tem_code"].Value.ToString() + "','"
                                                                                                           + dgv_Pd_Ktz.Rows[h].Cells["Giai_thich"].Value.ToString() + "','"
                                                                                                           + dgv_Pd_Ktz.Rows[h].Cells["Ghi_chu"].Value.ToString() + "')";
                                                        OleDbCommand cmdIns = new OleDbCommand(strIns2, cnn);
                                                        cmdIns.ExecuteNonQuery();
                                                        //xoa code trong input_Ktz
                                                        dtb1.del_filLog("Input_Ktz", dgv_Pd_Ktz.Rows[h].Cells["Tem_code"].Value.ToString(), 1, str_database);
                                                    }
                                                }
                                            }
                                            cnn.Close();
                                            #region
                                            FileStream fs = new FileStream(str_database + "\\History\\HistoryNVL.txt", FileMode.Create);
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
                                            #endregion
                                            if(chk_cloPO.CheckState == CheckState.Checked)
                                            {
                                                dtb1.delete_Transport("KtzGiaoPd1", "Model", cbx_ModelPK.Text);
                                            }
                                            else if (chk_cloPO.CheckState == CheckState.Unchecked)
                                            {
                                                dtb1.delete_Transport("KtzGiaoPd1", cbx_ModelPK.Text, dgv_Pd_Ktz);
                                            }
                                            chk_cloPO.CheckState = CheckState.Indeterminate;
                                            //Hien thi stock line
                                            dgv_stkLine.Columns.Clear();
                                            DataTable dt_sl = dtb1.search_stock("KtzGiaoPd1", false);
                                            dtb1.show_StockLinee(dgv_stkLine, dt_sl);
                                            dgv_stkLine.Columns["Ngay_thang"].Visible = false;
                                            dgv_stkLine.Columns["Ca_kip"].Visible = false;
                                            dgv_stkLine.Columns["Line"].Visible = false;
                                            dgv_stkLine.Columns["Model"].Visible = false;
                                            dgv_stkLine.Columns["KTZ"].Visible = false;
                                            dgv_stkLine.Columns["PD"].Visible = false;
                                            //xoa model dang chay
                                            //if (dgv_stkLine.Rows.Count == 1)
                                            //{
                                            //    dtb1.delete_Transport("KtzPd_ModelRun");
                                            //}
                                            //Reset data
                                            dgv_Pd_Ktz.Columns.Clear();
                                            cbx_Ktz.Text = "";
                                            picBx1PK.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                            picBx1PK.SizeMode = PictureBoxSizeMode.StretchImage;
                                            picBx2PK.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                            picBx2PK.SizeMode = PictureBoxSizeMode.StretchImage;
                                            picBx1PK.Visible = true;
                                            lblPK_Pic1.Visible = true;
                                            picBx2PK.Visible = true;
                                            lblPK_Pic2.Visible = true;
                                            radbtn_reLoadPK.Checked = false;
                                            chkScCode_PK = true;
                                            sttPK = 0;
                                            timer_reLoad.Stop();
                                            tool_saving.BackColor = Color.White;
                                            File.Delete(str_database + "\\Log\\Duplicate\\MakerPart.log");
                                            //Xoa database
                                            dtb1.delete_Transport("Pd_Ktz_tranfer");
                                            dtb1.delete_Transport("Pd_Ktz");
                                            //Xoa stock = 0
                                            dtb1.Del_StockZero("Stock_KTZ4", "So_luong");
                                            //xoa file .log
                                            try
                                            {
                                                string[] files = Directory.GetFiles(str_database + "\\Log\\Duplicate\\");
                                                int t = 0;
                                                foreach (string fil in files)
                                                {
                                                    if (files[t].Contains("Input_Line") || files[t].Contains("PDxacnhan") || files[t].Contains("Input_Ktz") || files[t].Contains("NVL_Holding"))
                                                    {
                                                        goto jumpt;
                                                    }
                                                    File.Delete(fil);
                                                jumpt:
                                                    t++;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                MessageBox.Show("Xảy ra lỗi xóa file .log (FI-FO, MakerPart, Input_Ktz)!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            MessageBox.Show("Xảy ra lỗi cập nhật history NVL!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        MessageBox.Show("Xảy ra lỗi xuất logfile!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Xảy ra lỗi cập nhật database!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Xảy ra lỗi cập nhật Stock KTZ!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Xảy ra lỗi cập nhật Stock FI-FO!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }                    
                }
            }             
        }        

        private void btn_StkLine_Click(object sender, EventArgs e)
        {
            //Show Stock Line
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockLine2")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockLine2 s_line2 = new StockLine2(str_database);
                s_line2.Show();
            }
        }

        private void radbtn_reLoadPK_CheckedChanged(object sender, EventArgs e)
        {
            if (radbtn_reLoadPK.Checked == true)
            {
                if(dgv_Pd_Ktz.RowCount == 0)
                {
                    timer_reLoad.Start();
                    try
                    {
                        //DataTable dt_reload = new DataTable();
                        tran1PK = new DataTable();
                        StreamReader sr = new StreamReader(str_database + "\\tem\\" + cbx_ModelPK.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadPD-KTZ.txt");
                        string[] colName = sr.ReadLine().Split(',');
                        for (int j = 0; j < colName.Length - 1; j++)
                        {
                            tran1PK.Columns.Add(colName[j]);
                        }

                        string newLine;
                        while ((newLine = sr.ReadLine()) != null)
                        {
                            DataRow dtr = tran1PK.NewRow();
                            string[] values = newLine.Split(',');
                            if (values[0] != "")
                            {
                                for (int i = 0; i < values.Length - 1; i++)
                                {
                                    dtr[i] = values[i];
                                }
                                tran1PK.Rows.Add(dtr);
                            }
                        }
                        sr.Close();

                        dgv_Pd_Ktz.Columns.Clear();
                        dtb1.show_Pd_Ktz(dgv_Pd_Ktz, tran1PK);
                        //An bot datagridview
                        dgv_Pd_Ktz.Columns["Ngay_thang"].Visible = false;
                        dgv_Pd_Ktz.Columns["Ca_kip"].Visible = false;
                        dgv_Pd_Ktz.Columns["Line"].Visible = false;
                        dgv_Pd_Ktz.Columns["Model"].Visible = false;
                        dgv_Pd_Ktz.Columns["Tem_code"].Visible = false;
                        dgv_Pd_Ktz.Columns["PD"].Visible = false;
                        dgv_Pd_Ktz.Columns["KTZ"].Visible = false;
                        //Not sort
                        foreach (DataGridViewColumn col in dgv_Pd_Ktz.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        radbtn_reLoadPK.Checked = false;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Data Re-load trống. Hãy tiếp tục thao tác!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        radbtn_reLoadPK.Checked = false;
                    }
                }
                else
                {
                    MessageBox.Show("Bạn không thể tải dữ liệu lúc này!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    radbtn_reLoadPK.Checked = false;
                }               
            }
        }

        private void btn_KiemtraPK_Click(object sender, EventArgs e)
        {
            if ((DateTime.Compare(DateTime.Parse(dTPK_Pic1.Value.ToShortDateString()), DateTime.Parse(dTPK_Pic2.Value.ToShortDateString())) > 0) || (cbx_ModelPK.Text.Length == 0))
            {
                MessageBox.Show("Hãy xem lại Model/thời gian bạn muốn kiểm tra!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string[] hisroryCheck = GetHistory(dTPK_Pic1, dTPK_Pic2);
                //Tìm history theo ngày -> tổng hợp -> Hiển thị Excel
                //Get file name trong khoảng time đã chọn
                int num_filOk = 0;
                int num_file = dtb1.count_file(str_database + "\\History\\In_Out\\PD_return_Ktz\\", hisroryCheck);
                string[] nam_file = dtb1.get_filOK(num_file, str_database + "\\History\\In_Out\\PD_return_Ktz\\", hisroryCheck, cbx_ModelPK.Text, dTPK_Pic1.Text, dTPK_Pic2.Text, num_filOk);
                //Sprire.XLS -> merge all file 
                //Open file merged(save tạm ra đâu đó)
                //Save as file merged nếu muốn
                dtb1.merg_Excel2(str_database + "\\History\\In_Out\\PD_return_Ktz\\", hisroryCheck, nam_file, nam_file.Length, datTim, "PD-KTZ", false, str_database);
            }      
        }

        private void lblPK_Pic1_Click(object sender, EventArgs e)
        {
            if (arrgPerPK == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerPK, str_database);
                    infPicture.Show();
                }
            }
        }

        private void lblPK_Pic2_Click(object sender, EventArgs e)
        {
            if (arrgPerPK == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerPK, str_database);
                    infPicture.Show();
                }
            }
        }

        private void dgv_Pd_Ktz_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Thread.Sleep(300);
            int column = dgv_Pd_Ktz.CurrentCell.ColumnIndex;
            if ((column == 10 && dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Value.ToString() != ""))
            {
                if (!Isnumber(dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Value.ToString()))
                {
                    MessageBox.Show("Mục Slg_tra_KTZ phải là số dương. Không nhập chữ hoặc số âm!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Style.BackColor = Color.Red;
                    btn_Confirm.Enabled = false;
                }
                else
                {
                    //int tt_qty = 0;
                    ////tinh stock
                    //if (chkb_NVLchuatra.Checked == false)
                    //{
                    //    tt_qty = dtb1.getData_qty(cbx_ModelPK.Text, dgv_Pd_Ktz.CurrentRow.Cells["Ma_NVL"].Value.ToString(), dgv_Pd_Ktz.CurrentRow.Cells["Tem_code"].Value.ToString(), "So_luong_cap", "KtzGiaoPd1");
                    //}
                    //else
                    //{
                    //    tt_qty = dtb1.getData_qty(cbx_ModelPK.Text, dgv_Pd_Ktz.CurrentRow.Cells["Ma_NVL"].Value.ToString(), dgv_Pd_Ktz.CurrentRow.Cells["Tem_code"].Value.ToString(), "Slg_ton_Line", "NVLChuaTra");
                    //}
                    //check qty tra KTZ
                    if ((int.Parse(dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Value.ToString()) >= 0)) //&& (int.Parse(dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Value.ToString()) <= tt_qty)) //stock ktz > 0 -> stock line = 0
                    {
                        dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Style.BackColor = Color.White;
                        dgv_Pd_Ktz.CurrentRow.Cells["Giai_thich"].Value = "";
                        dgv_Pd_Ktz.CurrentRow.Cells["Ghi_chu"].Value = "";
                        btn_Confirm.Enabled = true;                       
                    }                   
                    else
                    {
                        dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Style.BackColor = Color.Red;
                        btn_Confirm.Enabled = false;
                        MessageBox.Show("Slg_tra_KTZ không là số âm!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //MessageBox.Show("Slg_tra_KTZ nhập vào (" + dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Value.ToString() + ") nhiều hơn stock Line (" + tt_qty.ToString() + ")!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                   
                }
            }
            else if ((column == 11 && dgv_Pd_Ktz.CurrentRow.Cells["Slg_ton_Line"].Value.ToString() != "" && dgv_Pd_Ktz.CurrentRow.Cells["Slg_ton_Line"].Value.ToString() != "0"))
            {
                int tt_qty = 0;
                //tinh stock
                if(chkb_NVLchuatra.Checked == false)
                {
                    tt_qty = dtb1.getData_qty(cbx_ModelPK.Text, dgv_Pd_Ktz.CurrentRow.Cells["Ma_NVL"].Value.ToString(), dgv_Pd_Ktz.CurrentRow.Cells["Tem_code"].Value.ToString(), "So_luong_cap", "KtzGiaoPd1");
                }
                else
                {
                    tt_qty = dtb1.getData_qty(cbx_ModelPK.Text, dgv_Pd_Ktz.CurrentRow.Cells["Ma_NVL"].Value.ToString(), dgv_Pd_Ktz.CurrentRow.Cells["Tem_code"].Value.ToString(), "Slg_ton_Line", "NVLChuaTra");
                }
                //so sanh
                int slgKTZ = int.Parse(dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Value.ToString());
                int slgLine = int.Parse(dgv_Pd_Ktz.CurrentRow.Cells["Slg_ton_Line"].Value.ToString());
                int tt_act = slgKTZ + slgLine;
                if (tt_act > tt_qty)
                {
                    dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Style.BackColor = Color.Red;
                    dgv_Pd_Ktz.CurrentRow.Cells["Slg_ton_Line"].Style.BackColor = Color.Red;
                    btn_Confirm.Enabled = false;
                    MessageBox.Show("Slg_tra_KTZ + Slg_ton_Line (" + tt_act.ToString() + ") nhiều hơn stock Line (" + tt_qty.ToString() + ")!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(slgLine < 0)
                {
                    dgv_Pd_Ktz.CurrentRow.Cells["Slg_ton_Line"].Style.BackColor = Color.Red;
                    btn_Confirm.Enabled = false; 
                    MessageBox.Show("Mục Slg_ton_Line phải là số nguyên dương!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if((slgKTZ > 0 && slgLine > 0))
                {
                    dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Style.BackColor = Color.Red;
                    dgv_Pd_Ktz.CurrentRow.Cells["Slg_ton_Line"].Style.BackColor = Color.Red;
                    btn_Confirm.Enabled = false;
                    MessageBox.Show("Slg_tra_KTZ và Slg_ton_Line không đồng thời > 0!", "In/Out Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Style.BackColor = Color.White;
                    dgv_Pd_Ktz.CurrentRow.Cells["Slg_ton_Line"].Style.BackColor = Color.White;
                    btn_Confirm.Enabled = true;
                }
            }
            else if ((column == 13 && dgv_Pd_Ktz.CurrentRow.Cells["Giai_thich"].Value.ToString() != ""))
            {
                if (dgv_Pd_Ktz.CurrentRow.Cells["Giai_thich"].Value.ToString() == "Khác")
                {
                    dgv_Pd_Ktz.CurrentRow.Cells["Ghi_chu"].ReadOnly = false;
                    dgv_Pd_Ktz.CurrentCell = dgv_Pd_Ktz.CurrentRow.Cells["Ghi_chu"];
                }
                else
                {
                    dgv_Pd_Ktz.CurrentRow.Cells["Ghi_chu"].Value = "";
                    dgv_Pd_Ktz.CurrentRow.Cells["Ghi_chu"].ReadOnly = true;
                }
            }

            if (dgv_Pd_Ktz.CurrentRow.Cells["Slg_tra_KTZ"].Value.ToString() != "" || dgv_Pd_Ktz.CurrentRow.Cells["Slg_ton_Line"].Value.ToString() != "")
            {
                if (cb_inputPk.Checked == true)
                {
                    txt_manualInputPk.Text = "";
                    txt_manualInputPk.Focus();
                }
                else
                {
                    txt_autoInputPk.Text = "";
                    txt_autoInputPk.Focus();
                }
            }
        }

        public bool Isnumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private void cbx_ModelPK_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void cbx_Ktz_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_manualInputPk_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_autoInputPk_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_EnterPK_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_Clear_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_Confirm_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_StkLine_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_KiemtraPK_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }        

        private void dgv_Pd_Ktz_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }
        #endregion

        //=============================================================KTZ-WH===============================================================================
        #region KTZ-WH
        private void cbx_reMol_SelectedIndexChanged(object sender, EventArgs e)
        {            
            dtb1.delete_Transport("ReturnWH");
            dgv_returnWH.Columns.Clear();

            //reltranKW = dtb1.LoadBOM(cbx_reMol.Text);
            txt_autoInputKw.Focus();
        }

        private async void txt_autoInputKw_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(1500);
            if (txt_autoInputKw.Text != "" && chkScCode_KW == true)
            {
                chkScCode_KW = false;
                if (cbx_reMol.Text == "")
                {
                    MessageBox.Show("Bạn điền thiếu thông tin Model!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_autoInputKw.Text = "";
                    cbx_reMol.Focus();
                    chkScCode_KW = true;
                }
                else
                {
                    txt_manualInputKw.Text = "";
                    if (dtb1.chekdoubleCode2(txt_autoInputKw.Text) == false)//check double input
                    {
                        txt_autoInputKw.ResetText();
                        MessageBox.Show("Trùng cuộn liệu đã nhập trước đó. Hãy kiểm tra lại!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputKw.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_KW = true;
                    }
                    else if (dtb1.chk_formInput(txt_autoInputKw.Text) == false)//check format code
                    {
                        txt_autoInputKw.ResetText();
                        MessageBox.Show("Sai format code input. Hãy kiểm tra lại!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputKw.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_KW = true;
                    }
                    else if (dtb1.get_extinctCode(txt_autoInputKw.Text) == false)//check tồn tại trong stock KTZ4
                    {
                        txt_autoInputKw.ResetText();
                        MessageBox.Show("Không tồn tại code đã input. Hãy kiểm tra lại!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_autoInputKw.Focus();//Trỏ chuột tại textBox Input
                        chkScCode_KW = true;
                    }
                    else
                    {
                        if (arrgPerKW == false)//OP
                        {
                            timer_reLoad.Start();
                            //Gọi hàm kiểm tra thông tin input
                            inp_Re(txt_autoInputKw);
                        }
                        else//admin, manager
                        {
                            DialogResult rel_ar = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "ReturnWH", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (rel_ar == DialogResult.OK)
                            {
                                timer_reLoad.Start();
                                //Gọi hàm kiểm tra thông tin input
                                inp_Re(txt_autoInputKw);
                            }
                        }
                    }
                }
            }
        }

        private void btn_enterCodeKw_Click(object sender, EventArgs e)
        {
            if (txt_autoInputKw.Text == "")
            {
                if (cbx_reMol.Text == "" || txt_manualInputKw.Text == "")
                {
                    MessageBox.Show("Hãy kiểm tra lại thông tin Input!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbx_reMol.Focus();
                }
                else
                {
                    if (dtb1.chekdoubleCode2(txt_manualInputKw.Text) == false)//check double input
                    {
                        //txt_manualInputKw.ResetText();
                        MessageBox.Show("Trùng cuộn liệu đã nhập trước đó. Hãy kiểm tra lại!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputKw.Focus();//Trỏ chuột tại textBox Input
                    }
                    else if (dtb1.chk_formInput(txt_manualInputKw.Text) == false)//check format code
                    {
                        //txt_manualInputKw.ResetText();
                        MessageBox.Show("Sai format code input. Hãy kiểm tra lại!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputKw.Focus();//Trỏ chuột tại textBox Input
                    }
                    else if (dtb1.get_extinctCode(txt_manualInputKw.Text) == false)//check tồn tại trong stock KTZ4
                    {
                        //txt_manualInputKw.ResetText();
                        MessageBox.Show("Không tồn tại code đã input trong stock. Hãy kiểm tra lại!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_manualInputKw.Focus();//Trỏ chuột tại textBox Input
                    }
                    else
                    {
                        if (arrgPerKW == false)
                        {
                            timer_reLoad.Start();
                            //Gọi hàm check thông tin input
                            inp_Re(txt_manualInputKw);
                        }
                        else
                        {
                            DialogResult rel_ar = MessageBox.Show("Bạn đang làm công việc của OP. Bạn có muốn tiếp tục?", "ReturnWH", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (rel_ar == DialogResult.OK)
                            {
                                timer_reLoad.Start();
                                //Gọi hàm kiểm tra thông tin input
                                inp_Re(txt_manualInputKw);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy xóa mục Scan Code trước khi Enter code tay!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void inp_Re(TextBox txt)
        {
            //Biến xác nhận maker part same
            bool mkpSamrl = false;
            //Biến báo đã có code trong DataGirdView trùng
            bool havCodSmrl = false;
            //tach chuoi và so sanh trong stock
            string[] str_Inp = txt.Text.Split('+');
            reltranKW = dtb1.LoadStockFIFO(str_Inp[0], str_Inp[2], str_Inp[3], str_Inp[1]);
            //Lọc từ BOM theo makerpart, nếu trùng all thông tin > 2 lần trong dataTable -> báo PE xác nhận                      
            string[] infFromBom = get_InfInpKW(reltranKW, txt, cb_manualInputKw);
            if (infFromBom[0] != null && infFromBom[1] != null && infFromBom[2] != null && infFromBom[3] != null && infFromBom[4] != null)
            {
                //Kiểm tra maker part xem có dc scan >=2 lần              
                if (dtb1.chekScanMakPrtSame(infFromBom[3]) == false)//đã có input maker part rồi
                {
                    mkpSamrl = true;
                }
                else//chưa input maker part lần nào
                {
                    dtb1.savMakPrt(infFromBom[3]);
                    mkpSamrl = false;
                }

                sttKW++;
                //Điền data vào datagridview
                if (dgv_returnWH.Columns.Count == 0 || dgv_returnWH.Rows.Count == 0)//dgv chua co data
                {
                    dtb1.delete_Transport("ReturnWH");
                    if (mkpSamrl == true)//da ton tai new makerpart
                    {
                        txt.Text = "";
                        MessageBox.Show("Kiểm tra lại file log (Makerpart) trong StartPath!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        chkScCode_KW = true;
                    }
                    else//input new makerpart
                    {
                        dtb1.insert_transReWH2v2(sttKW.ToString(), infFromBom[0], infFromBom[1], infFromBom[2], infFromBom[3], str_Inp[3], infFromBom[4], txt.Text, stl_nameUser.Text);
                        havCodSmrl = true;
                        reltran1KW = dtb1.loadtransport_tableWH("ReturnWH");
                        //Hiển thị
                        dgv_returnWH.Columns.Clear();
                        dtb1.show_ReturnWH(dgv_returnWH, reltran1KW);
                    }                    
                }
                else//dgv da co data
                {                   
                    DataRow dtrwKW = reltran1KW.NewRow();
                    dtrwKW["STT"] = sttKW.ToString();
                    dtrwKW["Mo_ta"] = infFromBom[0];
                    dtrwKW["Ma_NVL"] = infFromBom[1];
                    dtrwKW["Maker"] = infFromBom[2];
                    dtrwKW["Maker_Part"] = infFromBom[3];
                    dtrwKW["Lot"] = str_Inp[3];
                    dtrwKW["So_luong_tra"] = infFromBom[4];
                    dtrwKW["Tem_code"] = txt.Text;
                    dtrwKW["Giai_thich"] = "";
                    dtrwKW["Ghi_chu"] = "";
                    reltran1KW.Rows.Add(dtrwKW);
                    reltran1KW.AcceptChanges();
                    havCodSmrl = true;
                }

                if (havCodSmrl == true)
                {
                    //Auto scroll
                    if (dgv_returnWH.RowCount > 22)
                    {
                        dgv_returnWH.FirstDisplayedScrollingRowIndex = dgv_returnWH.RowCount - 1;
                    }  
                    //Not sort
                    foreach (DataGridViewColumn col in dgv_returnWH.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    //Save data FIFO                               
                    dtb1.savFIFO(infFromBom[1] + "+" + infFromBom[2] + "+" + infFromBom[3] + "+" + str_Inp[3] + "+" + str_Inp[1]);
                    //hien thi label
                    string strIma1 = string.Empty;
                    string strIma2 = string.Empty;
                    #region
                    switch (infFromBom[2])
                    {
                        case "RENESAS":
                            strIma1 = infFromBom[2] + "1";
                            strIma2 = infFromBom[2] + "2";
                            break;

                        case "STMICRO":
                            strIma1 = infFromBom[2] + "1";
                            strIma2 = infFromBom[2] + "2";
                            break;

                        case "TI":
                            strIma1 = infFromBom[2] + "1";
                            strIma2 = infFromBom[2] + "2";
                            break;
                        default:
                            strIma1 = infFromBom[2];
                            strIma2 = string.Empty;
                            break;
                    }
                    #endregion

                    picBoxRe1.Visible = true;
                    lbl_picRe1.Visible = true;

                    picBoxRe1.Image = new Bitmap(str_database + "\\Picture\\" + strIma1 + ".PNG");
                    picBoxRe1.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (strIma2 != string.Empty)
                    {
                        picBoxRe2.Visible = true;
                        lbl_picRe2.Visible = true;
                        picBoxRe2.Image = new Bitmap(str_database + "\\Picture\\" + strIma2 + ".PNG");
                        picBoxRe2.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        picBoxRe2.Visible = false;
                        lbl_picRe2.Visible = false;
                    }
                    havCodSmrl = false;
                    txt.Text = "";
                    if (cb_manualInputKw.Checked == false)
                    {
                        chkScCode_KW = true;
                    }
                }
                else
                {
                    if (cb_manualInputKw.Checked == false)
                    {
                        txt.Text = "";
                        chkScCode_KW = true;
                    }
                }
            }
        }

        private void cb_manualInputKw_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_manualInputKw.Checked == true)
            {
                AcceptButton = btn_enterCodeKw;
                if (arrgPerKW == true)
                {
                    txt_manualInputKw.Visible = true;
                    txt_manualInputKw.Enabled = true;
                    txt_manualInputKw.Focus();
                    btn_enterCodeKw.Visible = true;
                    btn_enterCodeKw.Enabled = true;
                    lbl_inputCodeKw.Text = "Nhập Code";
                }
                else
                {
                    nhapCodeKW = true;
                    bool Isopen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Text == "ConfirmWHKTZ")
                        {
                            Isopen = true;
                            f.BringToFront();
                            break;
                        }
                    }
                    if (Isopen == false)
                    {
                        ConfirmWHKTZ confirmAd = new ConfirmWHKTZ(this);
                        confirmAd.Show();
                        count_timer = 0;
                        timer1.Start();
                    }
                }
            }
            else
            {
                txt_manualInputKw.Hide();
                txt_manualInputKw.Enabled = false;
                txt_manualInputKw.Text = "";
                txt_autoInputKw.Focus();
                btn_enterCodeKw.Visible = false;
                btn_enterCodeKw.Enabled = false;
                lbl_inputCodeKw.Text = "Scan Code";
            }
        }

        private void btn_xoaRe_Click(object sender, EventArgs e)
        {
            txt_autoInputKw.Focus();
            try
            {
                if (this.dgv_returnWH.CurrentRow.Index >= 0)
                {
                    string[] str = dgv_returnWH.Rows[dgv_returnWH.CurrentRow.Index].Cells["Tem_code"].Value.ToString().Split('+');
                    string strFifo = str[0] + "+" +
                                     dgv_returnWH.Rows[dgv_returnWH.CurrentRow.Index].Cells["Maker"].Value.ToString() + "+" +
                                     str[2] + "+" +
                                     str[3] + "+" +
                                     str[1];
                    dtb1.del_filLog("FI-FO", strFifo, 1, str_database);
                    //dtb1.del_filLog("Return_WH", dgv_returnWH.Rows[dgv_returnWH.CurrentRow.Index].Cells["Tem_code"].Value.ToString(), 1);
                    dtb1.del_filLog("MakerPart", dgv_returnWH.Rows[dgv_returnWH.CurrentRow.Index].Cells["Maker_Part"].Value.ToString(), 1, str_database);

                    DataRow drDel = reltran1KW.Rows[dgv_returnWH.CurrentRow.Index];
                    reltran1KW.Rows.Remove(drDel);
                }
                if(dgv_returnWH.RowCount == 0)
                {
                    chkScCode_KW = true;
                    sttKW = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Click vào đầu hàng đó để xóa!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_ConfRe_Click(object sender, EventArgs e)
        {
            if (dtb1.checkReWH(dgv_returnWH) == true)//kiểm tra datagirdview dc điền đủ thông tin
            {
                MessageBox.Show("Các thông tin đang để trống hoặc sai!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult anReWH = MessageBox.Show("Bạn muốn lưu thông tin đã nhập?", "ReturnWH", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (anReWH == DialogResult.Yes)
                {
                    DataTable dtSource = (DataTable)dgv_returnWH.DataSource;
                    //Giảm stock sau khi return WH
                    string[] err = dtb1.Reduce_StokKtz2(dgv_returnWH, datTim, "Ma_NVL", "Lot", "So_luong_tra", "ReturnWH", dtSource, stl_nameUser.Text, "KTZ-WH", dDay, dShift, cbx_reMol.Text, "", str_database);
                    if (err[0] == null)
                    {
                        dgv_returnWH.Columns.Clear();
                        dtb1.show_ReturnWH(dgv_returnWH, dtSource);
                        //Not sort
                        foreach (DataGridViewColumn col in dgv_returnWH.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        //Xoa Stock FIFO
                        if (dtb1.del_FIFO(str_database) == true)
                        {
                            //lưu logfile KTZ-WH ngay tra
                            //DataTable trung gian lưu thông tin theo form c/s -> gán vào file .CSV
                            DataTable tb_Excel = new DataTable();
                            tb_Excel = dtb1.LoadDatabase("ReturnWH_Logfile");
                            string content = "Mo_ta, Ma_NVL, Maker, Maker_Part, Lot, So_luong_tra, Tem_code, Giai_thich, Ghi_chu, Nguoi_tra\n";
                            bool chekExitFil = excel.checkExitLog(str_database + "\\History\\WH\\Return\\" + dMon + "\\" + datTim + "_" + cbx_reMol.Text + ".csv");
                            if (excel.Export_CSV(tb_Excel, str_database + "\\History\\WH\\Return\\" + dMon + "\\" + datTim + "_" + cbx_reMol.Text + ".csv", chekExitFil, content) == true)
                            {
                                MessageBox.Show("Tạo LogFile thành công!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (File.Exists(str_database + "\\tem\\" + cbx_reMol.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadReturnWH.txt"))
                                {
                                    File.Delete(str_database + "\\tem\\" + cbx_reMol.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadReturnWH.txt");
                                }
                                //update lich sư                              
                                //try
                                //{
                                #region
                                //var nvls = new List<NVL>() { };
                                //StreamReader sr = new StreamReader(str_database + "\\History\\HistoryNVL.txt");
                                //while (sr.EndOfStream == false)
                                //{
                                //    string[] str = sr.ReadLine().Split('|');
                                //    if (str.Length == 20)
                                //    {
                                //        nvls.Add(new NVL
                                //        {
                                //            model = str[0],
                                //            codeNVL = str[1],
                                //            maker = str[2],
                                //            mkerPart = str[3],
                                //            lot = str[4],
                                //            temCode = str[5],
                                //            ngInTemCode = str[6],
                                //            tgianInTemCode = str[7],
                                //            ngNhapKho = str[8],
                                //            tgianNhapKho = str[9],
                                //            ngCapNVL = str[10],
                                //            tgianCapNVL = str[11],
                                //            PDxacnhan = str[12],
                                //            tgianxacnhan = str[13],
                                //            ngTraNVL = str[14],
                                //            tgianTraNVL = str[15],
                                //            ghiChuTra = str[16],
                                //            ngTraWH = str[17],
                                //            tgianTraWH = str[18],
                                //            ghiChuTraWH = str[19]
                                //        });
                                //    }
                                //}
                                //sr.Close();

                                //for (int h = 0; h < dgv_returnWH.RowCount; h++)
                                //{
                                //    if (dgv_returnWH.Rows[h].Cells["Mo_ta"].Value != null && dgv_returnWH.Rows[h].Cells["Mo_ta"].Value.ToString() != "")
                                //    {
                                //        string temCode = dgv_returnWH.Rows[h].Cells["Tem_code"].Value.ToString();
                                //        string ghiChu = string.Empty;
                                //        if (dgv_returnWH.Rows[h].Cells["Giai_thich"].Value.ToString() == "Khác")
                                //        {
                                //            ghiChu = dgv_returnWH.Rows[h].Cells["Ghi_chu"].Value.ToString();
                                //        }
                                //        else
                                //        {
                                //            ghiChu = dgv_returnWH.Rows[h].Cells["Giai_thich"].Value.ToString();
                                //        }

                                //        foreach (var nn in nvls.Where(x => x.temCode == temCode))
                                //        {
                                //            nn.ngTraWH = stl_nameUser.Text;
                                //            nn.tgianTraWH = DateTime.Now.ToString();
                                //            nn.ghiChuTraWH = ghiChu;
                                //        }
                                //    }                                       
                                //}

                                //FileStream fs = new FileStream(str_database + "\\History\\HistoryNVL.txt", FileMode.Create);
                                //StreamWriter sw = new StreamWriter(fs);
                                //foreach (var item in nvls)
                                //{
                                //    sw.WriteLine(item.model + "|" +
                                //                 item.codeNVL + "|" +
                                //                 item.maker + "|" +
                                //                 item.mkerPart + "|" +
                                //                 item.lot + "|" +
                                //                 item.temCode + "|" +
                                //                 item.ngInTemCode + "|" +
                                //                 item.tgianInTemCode + "|" +
                                //                 item.ngNhapKho + "|" +
                                //                 item.tgianNhapKho + "|" +
                                //                 item.ngCapNVL + "|" +
                                //                 item.tgianCapNVL + "|" +
                                //                 item.PDxacnhan + "|" +
                                //                 item.tgianxacnhan + "|" +
                                //                 item.ngTraNVL + "|" +
                                //                 item.tgianTraNVL + "|" +
                                //                 item.ghiChuTra + "|" +
                                //                 item.ngTraWH + "|" +
                                //                 item.tgianTraWH + "|" +
                                //                 item.ghiChuTraWH);
                                //}
                                //sw.Close();
                                //fs.Close();
                                #endregion
                                //Reset data
                                dgv_returnWH.Columns.Clear();
                                picBoxRe1.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                picBoxRe1.SizeMode = PictureBoxSizeMode.StretchImage;
                                picBoxRe2.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                                picBoxRe2.SizeMode = PictureBoxSizeMode.StretchImage;
                                picBoxRe1.Visible = true;
                                lbl_picRe1.Visible = true;
                                picBoxRe2.Visible = true;
                                lbl_picRe2.Visible = true;
                                rdbtn_reloadRe.Checked = false;
                                chkScCode_KW = true;
                                sttKW = 0;
                                timer_reLoad.Stop();
                                dtb1.delete_Transport("ReturnWH");
                                dtb1.delete_Transport("ReturnWH_Logfile");
                                //Xoa stock = 0
                                dtb1.Del_StockZero("Stock_KTZ", "So_luong");
                                //xoa file .log
                                try
                                {
                                    string[] files = Directory.GetFiles(str_database + "\\Log\\Duplicate\\");
                                    int n = 0;
                                    foreach (string fil in files)
                                    {
                                        if (files[n].Contains("Input_Line") || files[n].Contains("PDxacnhan") || files[n].Contains("Input_Ktz") || files[n].Contains("NVL_Holding"))
                                        {
                                            goto jumpn;
                                        }
                                        File.Delete(fil);
                                    jumpn:
                                        n++;
                                    }
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Xảy ra lỗi xóa file .log (FI-FO, MakerPart, Return_WH)!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Xảy ra lỗi xuất logfile!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Xảy ra lỗi Xóa Stock FI-FO!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        dgv_returnWH.Columns.Clear();
                        dtb1.show_ReturnWH(dgv_returnWH, dtSource);
                        //Not sort
                        foreach (DataGridViewColumn col in dgv_returnWH.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        if (err[0] == "error")
                        {
                            MessageBox.Show("Bạn chưa nhập data. Nội dung hiển thị trống!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            foreach (DataGridViewRow dgr in dgv_returnWH.Rows)
                            {
                                for (int i = 0; i < err.Length; )
                                {
                                    if (dgr.Cells["Mo_ta"].Value != null && dgr.Cells["Mo_ta"].Value.ToString() != "")
                                    {
                                        if (dgr.Cells["Ma_NVL"].Value.ToString() == err[i] && dgr.Cells["Lot"].Value.ToString() == err[i + 1])
                                        {
                                            dgr.Cells["Tem_code"].Style.BackColor = Color.Red;
                                        }
                                        i = i + 2;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btn_chkStkRe_Click(object sender, EventArgs e)
        {
            //Hiển thị new form
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockKTZ2")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockKTZ2 stk2 = new StockKTZ2(str_database);
                stk2.Show();
            }
        }

        private void btn_stkLineKW_Click(object sender, EventArgs e)
        {
            //Show Stock Line
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "StockLine2")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                StockLine2 s_line2 = new StockLine2(str_database);
                s_line2.Show();
            }
        }

        private void rdbtn_reloadRe_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtn_reloadRe.Checked == true)
            {
                try
                {
                    DataTable dt_reload = new DataTable();
                    StreamReader sr = new StreamReader(str_database + "\\tem\\" + cbx_reMol.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "ReloadReturnWH.txt");
                    string[] colName = sr.ReadLine().Split(',');
                    for (int j = 0; j < colName.Length - 1; j++)
                    {
                        dt_reload.Columns.Add(colName[j]);
                    }

                    string newLine;
                    while ((newLine = sr.ReadLine()) != null)
                    {
                        DataRow dtr = dt_reload.NewRow();
                        string[] values = newLine.Split(',');
                        if (values[0] != "")
                        {
                            for (int i = 0; i < values.Length - 1; i++)
                            {
                                dtr[i] = values[i];
                            }
                            dt_reload.Rows.Add(dtr);
                        }
                    }
                    sr.Close();

                    dgv_returnWH.Columns.Clear();
                    dtb1.show_ReturnWH(dgv_returnWH, dt_reload);
                    //Not sort
                    foreach (DataGridViewColumn col in dgv_returnWH.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    rdbtn_reloadRe.Checked = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Data Re-load trống. Hãy tiếp tục thao tác!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rdbtn_reloadRe.Checked = false;
                }
            }
        }

        private void btn_ktra2_Click(object sender, EventArgs e)
        {
            if ((DateTime.Compare(daTiPic_KW1.Value, daTiPic_KW2.Value) > 0) || (cbx_reMol.Text.Length == 0))
            {
                MessageBox.Show("Hãy xem lại Model/thời gian bạn muốn kiểm tra!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string[] hisroryCheck = GetHistory(daTiPic_KW1, daTiPic_KW2);
                //Tìm history theo ngày -> tổng hợp -> Hiển thị Excel
                //Get file name trong khoảng time đã chọn
                int num_filOk = 0;
                int num_file = dtb1.count_file(str_database + "\\History\\WH\\Return\\", hisroryCheck);
                string[] nam_file = dtb1.get_filOK(num_file, str_database + "\\History\\WH\\Return\\", hisroryCheck, cbx_reMol.Text, daTiPic_KW1.Text, daTiPic_KW2.Text, num_filOk);
                //Sprire.XLS -> merge all file 
                //Open file merged(save tạm ra đâu đó)
                //Save as file merged nếu muốn
                dtb1.merg_Excel(str_database + "\\History\\WH\\Return\\", hisroryCheck, nam_file, nam_file.Length, datTim, "ReturnWH", true, str_database);
            }
        }

        private void lbl_picRe1_Click(object sender, EventArgs e)
        {
            if (arrgPerKW == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerKW, str_database);
                    infPicture.Show();
                }
            }
        }

        private void lbl_picRe2_Click(object sender, EventArgs e)
        {
            if (arrgPerKW == true)//la admin or manager
            {
                bool Isopen = false;
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Text == "InformationPicture")
                    {
                        Isopen = true;
                        f.BringToFront();
                        break;
                    }
                }
                if (Isopen == false)
                {
                    InformationPicture infPicture = new InformationPicture(arrgPerKW, str_database);
                    infPicture.Show();
                }
            }
        }

        private void dgv_returnWH_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (cb_manualInputKw.Checked == true)
            {
                txt_manualInputKw.Text = "";
                txt_manualInputKw.Focus();
            }
            else
            {
                txt_autoInputKw.Text = "";
                txt_autoInputKw.Focus();
            }
        }

        private void dgv_returnWH_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Thread.Sleep(300);
            int column = dgv_returnWH.CurrentCell.ColumnIndex;
            if ((column == 6 && dgv_returnWH.CurrentRow.Cells["So_luong_tra"].Value.ToString() != ""))
            {
                int n = 0;
                bool chkgd = int.TryParse(dgv_returnWH.CurrentRow.Cells["So_luong_tra"].Value.ToString(), out n);
                if (chkgd == false || n < 0)
                {
                    MessageBox.Show("Mục So_luong_tra phải số dương. Không nhập chữ hoặc số âm!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgv_returnWH.CurrentRow.Cells["So_luong_tra"].Style.BackColor = Color.Red;
                    btn_ConfRe.Enabled = false;
                }
                else
                {
                    int tt = dtb1.getData_qty2(dgv_returnWH.CurrentRow.Cells["Ma_NVL"].Value.ToString(), dgv_returnWH.CurrentRow.Cells["Maker_Part"].Value.ToString(), dgv_returnWH.CurrentRow.Cells["Lot"].Value.ToString());
                    if (tt < int.Parse(dgv_returnWH.CurrentRow.Cells[5].Value.ToString()))
                    {
                        MessageBox.Show("NVL " + dgv_returnWH.CurrentRow.Cells["Ma_NVL"].Value.ToString() + ",Lot " + dgv_returnWH.CurrentRow.Cells["Lot"].Value.ToString() + " stock KTZ (" + tt.ToString() + ") < So_luong_tra WH (" + dgv_returnWH.CurrentRow.Cells["So_luong_tra"].Value.ToString() + ") \nHãy kiểm tra lại số lượng trả WH!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgv_returnWH.CurrentRow.Cells["So_luong_tra"].Style.BackColor = Color.Red;
                        btn_ConfRe.Enabled = false;
                    }
                    //else if (tt > int.Parse(dgv_returnWH.CurrentRow.Cells[5].Value.ToString()))
                    //{
                    //    MessageBox.Show("NVL " + dgv_returnWH.CurrentRow.Cells["Ma_NVL"].Value.ToString() + ",Lot " + dgv_returnWH.CurrentRow.Cells["Lot"].Value.ToString() + " stock KTZ (" + tt.ToString() + ") > So_luong_tra WH (" + dgv_returnWH.CurrentRow.Cells["So_luong_tra"].Value.ToString() + ") \nHãy kiểm tra lại số lượng trả WH!", "ReturnWH", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    dgv_returnWH.CurrentRow.Cells["So_luong_tra"].Style.BackColor = Color.Red;
                    //    btn_ConfRe.Enabled = false;
                    //}
                    else
                    {
                        dgv_returnWH.CurrentRow.Cells["So_luong_tra"].Style.BackColor = Color.White;
                        btn_ConfRe.Enabled = true;
                    }
                }
            }
            else if ((column == 8 && dgv_returnWH.CurrentRow.Cells["Giai_thich"].Value.ToString() != ""))
            {
                if (dgv_returnWH.CurrentRow.Cells["Giai_thich"].Value.ToString() == "Khác")
                {
                    dgv_returnWH.CurrentRow.Cells["Ghi_chu"].ReadOnly = false;
                    dgv_returnWH.CurrentCell = dgv_returnWH.CurrentRow.Cells["Giai_thich"];
                }
                else
                {
                    dgv_returnWH.CurrentRow.Cells["Ghi_chu"].Value = "";
                    dgv_returnWH.CurrentRow.Cells["Ghi_chu"].ReadOnly = true;
                }
            }
        }

        private void cbx_reMol_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_manualInputKw_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void txt_autoInputKw_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_enterCodeKw_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_xoaRe_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_ConfRe_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_chkStkRe_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void btn_ktra2_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }

        private void dgv_returnWH_MouseMove(object sender, MouseEventArgs e)
        {
            var x = posX;
            var y = posY;

            posX = Cursor.Position.X;
            posY = Cursor.Position.Y;
            if (x != posX || y != posY)
            {
                count_Out = 900;
                active_Form();
            }
        }
        #endregion

        //=============================================================NVL Special===============================================================================
        #region NVL Special
        private void cbx_molSoPst_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_NvlNam.Text = "";
        }

        private void cbx_NvlNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtb1.delete_Transport("PD_NVLSpecial");
            dgv_SoPst.Columns.Clear();
            solPst = dtb1.LoadBOMSpecial(cbx_molSoPst.Text, cbx_NvlNam.Text);
            txt_codSoPst.Focus();
            //History folder
            if (!System.IO.Directory.Exists(str_database + "\\History\\" + cbx_NvlNam.Text + "\\" + dMon))
            {
                System.IO.Directory.CreateDirectory(str_database + "\\History\\" + cbx_NvlNam.Text + "\\" + dMon);
            }
            //Hinh ảnh
            string strIma1 = cbx_molSoPst.Text + "_" + cbx_NvlNam.Text;
            string strIma2 = string.Empty;            

            picBox1_soPst.Visible = true;
            lbl_pic1SoPst.Visible = true;

            picBox1_soPst.Image = new Bitmap(str_database + "\\Picture\\" + strIma1 + ".PNG");
            picBox1_soPst.SizeMode = PictureBoxSizeMode.StretchImage;

            if (strIma2 != string.Empty)
            {
                picBox2_soPst.Visible = true;
                lbl_pic2SoPst.Visible = true;
                picBox2_soPst.Image = new Bitmap(str_database + "\\Picture\\" + strIma2 + ".PNG");
                picBox2_soPst.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                picBox2_soPst.Visible = false;
                lbl_pic2SoPst.Visible = false;
            }
        }

        private async void txt_codSoPst_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(2000);
            if(txt_codSoPst.Text.Length > 0)
            {
                if(cbx_molSoPst.Text != "" && cbx_NvlNam.Text != "")
                {
                    txt_mkrSoPst.Enabled = true;
                    txt_mkrSoPst.Focus();
                }
                else
                {
                    MessageBox.Show("Bạn điền thiếu thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_mkrSoPst.Text = "";
                    txt_mkrSoPst.Enabled = false;
                }
            }
            else
            {
                txt_mkrSoPst.Text = "";
                txt_mkrSoPst.Enabled = false;
                return;
            }
        }

        private void btn_enterSoPst_Click(object sender, EventArgs e)
        {
            if (cbx_molSoPst.Text == "" || cbx_NvlNam.Text == "" || txt_codSoPst.Text == "" || txt_mkrSoPst.Text == "")
            {
                MessageBox.Show("Hãy kiểm tra lại thông tin Model/Tên NVL/Code!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_codSoPst.Text = "";
                txt_mkrSoPst.Text = "";
                txt_mkrSoPst.Enabled = false;
            }
            else
            {
                txt_mkrSoPst.Text = crtUpperChar(txt_mkrSoPst.Text);
                //Gọi hàm kiểm tra thông tin input
                NVLSpecial(txt_codSoPst, txt_mkrSoPst);
            }  
        }

        public void NVLSpecial(TextBox codeWH, TextBox maker)
        {
            if (dtb1.chekCodeDouble(str_database, "Code" + cbx_NvlNam.Text + "_WH", txt_codSoPst.Text) == false)//da input r
            {                
                maker.Text = "";
                maker.Enabled = false;
                MessageBox.Show("NVL đã được input rồi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                codeWH.Focus();
                codeWH.Text = "";
            }            
            else
            {
                //Check data txt input
                string[] infFromBom = new string[5];
                infFromBom = get_InfInpSP(solPst, cbx_molSoPst.Text, cbx_NvlNam.Text, codeWH, maker);
                if (infFromBom[0] != null && infFromBom[1] != null && infFromBom[2] != null && infFromBom[3] != null && infFromBom[4] != null)
                {
                    sttSP++;
                    //Điền data vào datagridview
                    if (dgv_SoPst.Columns.Count == 0 || dgv_SoPst.Rows.Count == 0)//dgv chua co data
                    {
                        //run timer reload
                        timer_reLoad.Start();
                        tool_saving.BackColor = Color.Green;
                        //xoa database cu
                        dtb1.delete_Transport("PD_NVLSpecial");
                        //Insert OrderWH
                        dtb1.insert_tranSP(sttSP.ToString(), cbx_molSoPst.Text, cbx_NvlNam.Text, infFromBom[3], infFromBom[4], DateTime.Now.ToString("yyyyMMddHHmmss"), stl_nameUser.Text);
                        //Load all data vao dataTable
                        solPst1 = dtb1.LoadDatabase("PD_NVLSpecial", cbx_molSoPst.Text);
                        //Hiển thị
                        dgv_SoPst.Columns.Clear();
                        dtb1.show_SP(dgv_SoPst, solPst1);
                    }
                    else
                    {
                        tool_saving.BackColor = Color.Green;
                        DataRow dtrwSP = solPst1.NewRow();
                        dtrwSP["STT"] = sttSP.ToString();
                        dtrwSP["Model"] = cbx_molSoPst.Text;
                        dtrwSP["NVL"] = cbx_NvlNam.Text;
                        dtrwSP["Maker"] = infFromBom[3];
                        dtrwSP["Code_WH"] = infFromBom[4];
                        dtrwSP["So_luong"] = "1";
                        dtrwSP["Thoi_gian"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        dtrwSP["Nguoi_nhan"] = stl_nameUser.Text;
                        solPst1.Rows.Add(dtrwSP);
                        solPst1.AcceptChanges();
                    }
                    //Auto scroll
                    if (dgv_SoPst.RowCount > 7)
                    {
                        dgv_SoPst.FirstDisplayedScrollingRowIndex = dgv_SoPst.RowCount - 1;
                    }
                    //Not sort
                    foreach (DataGridViewColumn col in dgv_SoPst.Columns)
                    {
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    //Luu new code
                    dtb1.savDataInputed(str_database, "Code" + cbx_NvlNam.Text + "_WH", codeWH.Text);                    
                    //Reset
                    codeWH.Text = "";
                    codeWH.Focus();
                    maker.Text = "";
                    maker.Enabled = false;
                }
            }
        }

        private void rbt_reloadSoPst_CheckedChanged(object sender, EventArgs e)
        {
            if (rbt_reloadSoPst.Checked == true)
            {
                if (dgv_SoPst.RowCount == 0)
                {
                    timer_reLoad.Start();
                    try
                    {
                        //DataTable dt_reload = new DataTable();
                        solPst1 = new DataTable();
                        StreamReader sr = new StreamReader(str_database + "\\tem\\" + cbx_molSoPst.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "Reload_" + cbx_NvlNam.Text + ".txt");
                        string[] colName = sr.ReadLine().Split(',');
                        for (int j = 0; j < colName.Length - 1; j++)
                        {
                            solPst1.Columns.Add(colName[j]);
                        }

                        string newLine;
                        while ((newLine = sr.ReadLine()) != null)
                        {
                            DataRow dtr = solPst1.NewRow();
                            string[] values = newLine.Split(',');
                            if (values[0] != "")
                            {
                                for (int i = 0; i < values.Length - 1; i++)
                                {
                                    dtr[i] = values[i];
                                }
                                solPst1.Rows.Add(dtr);
                            }
                        }
                        sr.Close();

                        dgv_SoPst.Columns.Clear();
                        dtb1.show_SP(dgv_SoPst, solPst1);                        
                        //Not sort
                        foreach (DataGridViewColumn col in dgv_SoPst.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        txt_codSoPst.Enabled = true;
                        txt_mkrSoPst.Enabled = false;
                        rbt_reloadSoPst.Checked = false;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Data Re-load trống. Hãy tiếp tục thao tác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        rbt_reloadSoPst.Checked = false;
                    }
                }
                else
                {
                    MessageBox.Show("Bạn không thể tải dữ liệu lúc này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rbt_reloadSoPst.Checked = false;
                }
            }           
        }

        private void btn_delSoPst_Click(object sender, EventArgs e)
        {
            txt_codSoPst.Focus();
            try
            {
                // Nếu dùng dataGridView2.SelectedRows.Count thì phải click vào đầu hàng
                // Nếu dùng dataGridView2.CurrentRow.Index thì click vào bất kì vị trí có thể xóa hàng đó

                if (this.dgv_SoPst.CurrentRow.Index >= 0)
                {
                    File.AppendAllText(str_database + "\\History\\Delete_" + cbx_NvlNam.Text + ".txt", "\n" + dgv_SoPst.Rows[dgv_SoPst.CurrentRow.Index].Cells["Model"].Value.ToString() + "|" + dgv_SoPst.Rows[dgv_SoPst.CurrentRow.Index].Cells["NVL"].Value.ToString() + "|" + dgv_SoPst.Rows[dgv_SoPst.CurrentRow.Index].Cells["Maker"].Value.ToString() + "|" + dgv_SoPst.Rows[dgv_SoPst.CurrentRow.Index].Cells["Code_WH"].Value.ToString() + "|" + dgv_SoPst.Rows[dgv_SoPst.CurrentRow.Index].Cells["So_luong"].Value.ToString() + "|" + dgv_SoPst.Rows[dgv_SoPst.CurrentRow.Index].Cells["Thoi_gian"].Value.ToString() + "|" + dgv_SoPst.Rows[dgv_SoPst.CurrentRow.Index].Cells["Nguoi_nhan"].Value.ToString(), Encoding.UTF8);

                    dtb1.del_filLog("Code" + cbx_NvlNam.Text + "_WH", dgv_SoPst.Rows[dgv_SoPst.CurrentRow.Index].Cells["Code_WH"].Value.ToString(), 1, str_database);

                    DataRow drToDelete = solPst1.Rows[dgv_SoPst.CurrentRow.Index];
                    solPst1.Rows.Remove(drToDelete);
                }
                if (dgv_SoPst.RowCount == 1)
                {
                    sttSP = 0;
                }
            }
            catch
            {
                MessageBox.Show("Click vào đầu hàng đó để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_xnSoPst_Click(object sender, EventArgs e)
        {
            if (dtb1.checkPD_SP(dgv_SoPst) == true)
            {
                MessageBox.Show("Các thông tin đang để trống hoặc sai!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult traloi = MessageBox.Show("Bạn có chắc chắn nhập số NVL này?", "Câu hỏi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (traloi == DialogResult.Yes)
                {
                    //Update database
                    if (dtb1.insert_PdSp_table(dgv_SoPst) == true)
                    {                        
                        bool chekExitFil = excel.checkExitLog(str_database + "\\History\\" + cbx_NvlNam.Text + "\\" + dMon + "\\" + datTim + "_" + cbx_molSoPst.Text + ".csv");
                        if (excel.exportCsvNvlSpe(dgv_SoPst, str_database + "\\History\\" + cbx_NvlNam.Text + "\\" + dMon + "\\" + datTim + "_" + cbx_molSoPst.Text + ".csv", chekExitFil, "", "", 1, 0) == true)                        
                        {
                            MessageBox.Show("Tạo LogFile thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (File.Exists(str_database + "\\tem\\" + cbx_molSoPst.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "Reload_" + cbx_NvlNam.Text + ".txt"))
                            {
                                File.Delete(str_database + "\\tem\\" + cbx_molSoPst.Text + "_" + DateTime.Now.ToString("yyMMdd") + "_" + "Reload_" + cbx_NvlNam.Text + ".txt");
                            }
                            //Reset data
                            dgv_SoPst.Columns.Clear();
                            cbx_molSoPst.Text = "";                           
                            txt_codSoPst.Text = "";
                            txt_mkrSoPst.Text = "";
                            txt_mkrSoPst.Enabled = false;
                            picBox1_soPst.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                            picBox1_soPst.SizeMode = PictureBoxSizeMode.StretchImage;
                            picBox2_soPst.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                            picBox2_soPst.SizeMode = PictureBoxSizeMode.StretchImage;
                            picBox1_soPst.Visible = true;
                            lbl_pic1SoPst.Visible = true;
                            picBox2_soPst.Visible = true;
                            lbl_pic2SoPst.Visible = true;
                            rbt_reloadSoPst.Checked = false;
                            sttSP = 0;
                            timer_reLoad.Stop();
                            tool_saving.BackColor = Color.White;
                            //Xoa database
                            dtb1.delete_Transport("PD_NVLSpecial");
                            //xoa file .log
                            try
                            {
                                string[] files = Directory.GetFiles(str_database + "\\Log\\Duplicate\\");
                                int t = 0;
                                foreach (string fil in files)
                                {
                                    if (files[t].Contains("Input_Line") || files[t].Contains("PDxacnhan") || files[t].Contains("Input_Ktz") || files[t].Contains("NVL_Holding") || files[t].Contains("Code" + cbx_NvlNam.Text + "_WH"))
                                    {
                                        goto jumpt;
                                    }
                                    File.Delete(fil);
                                jumpt:
                                    t++;
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Xảy ra lỗi xóa file .log (FI-FO, MakerPart, Input_Ktz)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            cbx_NvlNam.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Xảy ra lỗi xuất logfile!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Xảy ra lỗi cập nhật database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }             
        }

        private void btn_stkSoPst_Click(object sender, EventArgs e)
        {
            //Show Stock Line
            bool Isopen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == "SolderPaste")
                {
                    Isopen = true;
                    f.BringToFront();
                    break;
                }
            }
            if (Isopen == false)
            {
                SolderPaste SP = new SolderPaste(str_database);
                SP.Show();
            }
        }

        private void btn_chkSoPst_Click(object sender, EventArgs e)
        {
            if ((DateTime.Compare(DateTime.Parse(dtTimPic1_soPst.Value.ToShortDateString()), DateTime.Parse(dtTimPic2_soPst.Value.ToShortDateString())) > 0) || (cbx_molSoPst.Text.Length == 0) || (cbx_NvlNam.Text.Length == 0))
            {
                MessageBox.Show("Hãy xem lại Model/thời gian bạn muốn kiểm tra!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string[] hisroryCheck = GetHistory(dtTimPic1_soPst, dtTimPic2_soPst);
                //Tìm history theo ngày -> tổng hợp -> Hiển thị Excel
                //Get file name trong khoảng time đã chọn
                int num_filOk = 0;
                int num_file = dtb1.count_file(str_database + "\\History\\" + cbx_NvlNam.Text + "\\", hisroryCheck);
                string[] nam_file = dtb1.get_filOK(num_file, str_database + "\\History\\" + cbx_NvlNam.Text + "\\", hisroryCheck, cbx_molSoPst.Text, dtTimPic1_soPst.Text, dtTimPic2_soPst.Text, num_filOk);
                //Sprire.XLS -> merge all file 
                //Open file merged(save tạm ra đâu đó)
                //Save as file merged nếu muốn
                dtb1.merg_Excel2(str_database + "\\History\\" + cbx_NvlNam.Text + "\\", hisroryCheck, nam_file, nam_file.Length, datTim, cbx_NvlNam.Text, false, str_database);
            }
        }

        private void btn_clsSoPst_Click(object sender, EventArgs e)
        {
            ((Control)this.SpecialMaterial).Enabled = false;
            tabControl1.TabPages.Remove(SpecialMaterial);
            cbx_molSoPst.Text = "";
            cbx_NvlNam.Text = "";
            txt_codSoPst.Text = "";
            txt_mkrSoPst.Text = "";
            rbt_reloadSoPst.Checked = false;
            dgv_SoPst.Columns.Clear();
        }        

        private void solderPasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((part == "admin" || part == "manager" || part == "PD") && ((Control)this.SpecialMaterial).Enabled == false)
            {
                sttSP = 0;
                ((Control)this.SpecialMaterial).Enabled = true;
                tabControl1.TabPages.Add(SpecialMaterial);
                tabControl1.SelectedTab = tabControl1.TabPages["SpecialMaterial"];
                //Load data model, KTZ
                dtb1.get_cbbModel("BOM_Special", "Model", cbx_molSoPst);
                dtb1.get_cbbModel("BOM_Special", "Name_Material", cbx_NvlNam);
                //Initial
                txt_codSoPst.Text = "";
                txt_mkrSoPst.Text = "";
                txt_mkrSoPst.Enabled = false;
                //con trỏ chuột đếm tgian out
                posX = Cursor.Position.X;
                posY = Cursor.Position.Y;
                //Get thông tin khi đổi password
                DataTable dtNewPass = dtb1.GetNewPass(txt_user.Text, txt_pass.Text);
                dtb1.DeleteDataNewPass();
                if (dtNewPass.Rows.Count > 0)
                {
                    arrgPerSP = dtb1.get_RightLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString());
                    partEnterSP = dtb1.get_PerLogin(txt_user.Text, dtNewPass.Rows[0].ItemArray[0].ToString(), "part");
                }
                else
                {
                    arrgPerSP = dtb1.get_RightLogin(txt_user.Text, txt_pass.Text);
                    partEnterSP = dtb1.get_PerLogin(txt_user.Text, txt_pass.Text, "part");
                }
                //hinh anh
                picBox1_soPst.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBox1_soPst.SizeMode = PictureBoxSizeMode.StretchImage;
                picBox2_soPst.Image = new Bitmap(str_database + "\\Picture\\Default.PNG");
                picBox2_soPst.SizeMode = PictureBoxSizeMode.StretchImage;                                
                //clear dgv
                dgv_SoPst.Columns.Clear();
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập hạng mục này\nHoặc trang dữ liệu đã được mở rồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion        
    }       
}