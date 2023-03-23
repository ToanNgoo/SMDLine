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
using Spire.Xls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace ManageMaterialPBA
{
    public partial class BOM : Form
    {
        database dtb;
        database_1 dtb1;
        ClsExcel ex = new ClsExcel();
        DataTable dt = new DataTable();
        DataTable dt_mol = new DataTable();
        public string[] strArrNg;
        public int countNg;
        public string _userr = string.Empty, _passs = string.Empty, _strdatabase = string.Empty;

        public BOM(string userr, string passs, string strdatabase)
        {
            InitializeComponent();
            _userr = userr;
            _passs = passs;
            _strdatabase = strdatabase;
        }

        private void dgv_bom_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgv_bom.ColumnHeadersDefaultCellStyle.Font = new Font(dgv_bom.Font, FontStyle.Bold);
            dgv_bom.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_bom.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void BOM_Load(object sender, EventArgs e)
        {
            dtb = new database(_strdatabase);
            dtb1 = new database_1(_strdatabase);
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width/2 - this.Width/2, 10);//Screen.PrimaryScreen.Bounds.Height/2 - this.Height/2

            //Khoa sheet
            ((Control)this.UpdateBOM).Enabled = false;
            tabControl1.TabPages.Remove(UpdateBOM);
            ((Control)this.NewBOM).Enabled = false;
            tabControl1.TabPages.Remove(NewBOM);

            //Load anh
            picMonitor.Image = new Bitmap(_strdatabase + "\\Picture\\SDIV.PNG");
            picMonitor.SizeMode = PictureBoxSizeMode.StretchImage;                       

            dtb.get_model(cbb_model);
            if(_userr != "" && _passs != "")
            {
                //Admin
                if(dtb1.get_adLogin(_userr, _passs) == true)
                {
                    côngCụToolStripMenuItem.Visible = true;
                    btn_xoa.Enabled = true;
                }
                else//Not admin
                {
                    côngCụToolStripMenuItem.Visible = false;
                    btn_xoa.Enabled = false;
                } 
            }
            else
            {
                côngCụToolStripMenuItem.Visible = false;
                btn_xoa.Enabled = false;
            }

            toolStripStatusLabel1.Text = dtb.get_name(_userr,_passs);
        }

        private void btn_exeBom_Click(object sender, EventArgs e)
        {
            DataTable bom = new DataTable();
            if(cbb_model.Text == "")
            {
                bom = dtb.loadtransport();
            }
            else
            {
                bom = dtb.loadtransportml(cbb_model.Text);
            }

            dgv_bom.DataSource = bom.DefaultView;
        }

        private void updateBOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((Control)this.UpdateBOM).Enabled == false)
            {
                ((Control)this.UpdateBOM).Enabled = true;
                tabControl1.TabPages.Add(UpdateBOM);
                tabControl1.SelectedTab = tabControl1.TabPages["UpdateBOM"];

                picUpdateBOM.Image = new Bitmap(_strdatabase + "\\Picture\\SDIV.PNG");
                picUpdateBOM.SizeMode = PictureBoxSizeMode.StretchImage;

                txt_lydo.Visible = false;
                txt_lydo.Enabled = false;
                label3.Visible = false;

                cbx_lydo.Items.Add("Thêm Code NVL mới");
                cbx_lydo.Items.Add("Thêm Maker mới");
                cbx_lydo.Items.Add("NVL được sử dụng");
                cbx_lydo.Items.Add("NVL không dùng");
                cbx_lydo.Items.Add("Khác");               

                cbb_line.Items.Add("3");
                cbb_line.Items.Add("6");
                cbb_line.Items.Add("7");

                dtb1.delete_Transport("UpdateBOM");
            }
            else
            {
                MessageBox.Show("Trang dữ liệu đã được mở rồi!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                       
        }

        private void newBOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((Control)this.NewBOM).Enabled == false)
            {
                ((Control)this.NewBOM).Enabled = true;
                tabControl1.TabPages.Add(NewBOM);
                tabControl1.SelectedTab = tabControl1.TabPages["NewBOM"];

                picNewBOM.Image = new Bitmap(_strdatabase + "\\Picture\\SDIV.PNG");
                picNewBOM.SizeMode = PictureBoxSizeMode.StretchImage;

                groupBox11.Enabled = false;
                groupBox10.Enabled = false;

                //Xoa form dien data
                dgv_nwBOM.Columns.Clear();
            }
            else
            {
                MessageBox.Show("Trang dữ liệu đã được mở rồi!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                        
        }

        private void btn_capNhat_Click(object sender, EventArgs e)
        {
            cbb_model.Items.Clear();
            dtb.get_model(cbb_model);
        }

        private void btn_clsUpdateBOM_Click(object sender, EventArgs e)
        {
            ((Control)this.UpdateBOM).Enabled = false;
            tabControl1.TabPages.Remove(UpdateBOM);
            cbb_line.Text = "";
            cbb_mol.Text = "";
            cbb_cod.Text = "";
            cbb_vendr.Text = "";
            cbx_lydo.Text = "";
            txt_lydo.Text = "";
            dgv_updBOM.Columns.Clear();
        }

        private void btn_clsNewBOM_Click(object sender, EventArgs e)
        {
            ((Control)this.NewBOM).Enabled = false;
            tabControl1.TabPages.Remove(NewBOM);
            rbtn_newForm.Checked = false;
            rbtn_availableForm.Checked = false;
            dgv_nwBOM.Columns.Clear();
        }

        //==============================================================================Update BOM==============================================================
        private void cbb_line_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Delete database  
            OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
            cnn.Open();
            string strDel = "Delete * From UpdateBOM";
            OleDbCommand cmdDel = new OleDbCommand(strDel, cnn);
            cmdDel.ExecuteNonQuery();
            cnn.Close();

            cbb_mol.Text = "";
            cbb_mol.Items.Clear();
            try
            {
                string str = "select distinct Model from All_model1 Where Line='" + cbb_line.Text + "'";
                DataTable dt = new DataTable();
                dt = dtb.getData(str);

                // Add users vào comboBox
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.ItemArray[0] != null)
                    {
                        cbb_mol.Items.Add(dr.ItemArray[0].ToString());
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn Line trước nhé!", "UpdateBOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbb_mol_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Delete database  
            OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
            cnn.Open();
            string strDel = "Delete * From UpdateBOM";
            OleDbCommand cmdDel = new OleDbCommand(strDel, cnn);
            cmdDel.ExecuteNonQuery();
            cnn.Close();

            cbb_cod.Text = "";
            cbb_cod.Items.Clear();
            try
            {
                string str = "select distinct Ma_NVL from All_model1 Where Line='" + cbb_line.Text + "' And Model='" + cbb_mol.Text + "' order by Ma_NVL";
                DataTable dt = new DataTable();
                dt = dtb.getData(str);

                // Add users vào comboBox
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.ItemArray[0] != null)
                    {
                        cbb_cod.Items.Add(dr.ItemArray[0].ToString());
                    }
                }
                cbb_cod.Items.Add("Blank");
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn Line và Model trước nhé!", "UpdateBOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbb_cod_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Delete database  
            OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
            cnn.Open();
            string strDel = "Delete * From UpdateBOM";
            OleDbCommand cmdDel = new OleDbCommand(strDel, cnn);
            cmdDel.ExecuteNonQuery();
            cnn.Close();

            cbb_vendr.Text = "";
            cbb_vendr.Items.Clear();
            try
            {
                string str = "select distinct Maker from All_model1 Where Line='" + cbb_line.Text + "' And Model='" + cbb_mol.Text + "' And Ma_NVL='" + cbb_cod.Text + "'";
                DataTable dt = new DataTable();
                dt = dtb.getData(str);

                // Add users vào comboBox
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.ItemArray[0] != null)
                    {
                        cbb_vendr.Items.Add(dr.ItemArray[0].ToString());
                    }
                }
                cbb_vendr.Items.Add("Blank");
            }
            catch (Exception)
            {
                MessageBox.Show("Chọn Line và Model và Code trước nhé!", "UpdateBOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbb_vendr_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_updBOM.Columns.Clear();
            //Hien thi datagridview
            DataTable dt = new DataTable();
            if (cbb_cod.Text != "Blank")
            {
                if (cbb_vendr.Text != "Blank")
                {
                    btn_upBOM.Text = "Update BOM";
                    //OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                    //cnn.Open();

                    string str = "select distinct Line, Model, Ma_NVL, Mo_ta, Vi_tri, Diem_gan, Maker, Maker_Part, Cong_doan, So_luong, Su_dung from All_model1 Where Line='" + cbb_line.Text + "' And Model='" + cbb_mol.Text + "' And Ma_NVL='" + cbb_cod.Text + "' And Maker='" + cbb_vendr.Text + "'";
                    dt = dtb.getData(str);

                    ExportTxt(dt, _strdatabase + "\\BOM\\Before_Update.txt", true);

                    //Hien thi datagridview
                    dtb.show_upBOM(dgv_updBOM, dt, false, true, true);
                }
                else//vendor = Blank
                {
                    btn_upBOM.Text = "Insert BOM";

                    string str = "select distinct Line, Model, Ma_NVL, Mo_ta, Vi_tri, Diem_gan, Cong_doan, Su_dung from All_model1 Where Line='" + cbb_line.Text + "' And Model='" + cbb_mol.Text + "' And Ma_NVL='" + cbb_cod.Text + "'";
                    dt = dtb.getData(str);

                    ExportTxt(dt, _strdatabase + "\\BOM\\Before_Update.txt", false);

                    //Hien thi datagridview
                    DataTable dt2 = new DataTable();
                    StreamReader sr = new StreamReader(_strdatabase + "\\BOM\\Before_Update.txt");
                    string[] colName = sr.ReadLine().Split('|');
                    for (int j = 0; j < colName.Length - 1; j++)
                    {
                        dt2.Columns.Add(colName[j]);
                    }

                    string newLine;
                    while ((newLine = sr.ReadLine()) != null)
                    {
                        DataRow dtr = dt2.NewRow();
                        string[] values = newLine.Split('|');
                        if (values[0] != "")
                        {
                            for (int i = 0; i < values.Length - 1; i++)
                            {
                                dtr[i] = values[i];
                            }
                            dt2.Rows.Add(dtr);
                        }
                    }
                    sr.Close();
                    dtb.show_upBOM(dgv_updBOM, dt2, false, false, true);
                }
            }
            else//code = blank
            {
                btn_upBOM.Text = "Insert BOM";

                OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                cnn.Open();
                string strIn = string.Empty;
                strIn = "Insert Into UpdateBOM Values('" + cbb_line.Text + "','" + cbb_mol.Text + "','"
                                                         + "" + "','" + "" + "','" + "" + "','" + "" + "','"
                                                         + "" + "','" + "" + "','" + "SMD" + "','" + "" + "','" + "" + "')";
                OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);// Khai báo và khởi tạo bộ nhớ biến cmd
                cmdIn.ExecuteNonQuery(); // thực hiện lênh SQL 
                cnn.Close();
                //Select all
                string str = "select * from UpdateBOM";
                dt = dtb.getData(str);

                ExportTxt(dt, _strdatabase + "\\BOM\\Before_Update.txt", true);

                //Hien thi datagridview
                dtb.show_upBOM(dgv_updBOM, dt, false, false, false);
            }            
        }

        public void ExportTxt(DataTable dt1, string path, bool chk)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            StringBuilder sb = new StringBuilder();
            //Export header text

            sb.Append("Line|Model|Ma_NVL|Mo_ta|Vi_tri|Diem_gan|Maker|Maker_Part|Cong_doan|So_luong|Su_dung|");

            sb.Append("\n");
            sw.WriteLine(sb);

            //Export data
            foreach (DataRow dr in dt1.Rows)
            {
                StringBuilder sbb = new StringBuilder();
                int j = 0;
                for (int i = 0; i < dt1.Columns.Count; )
                {
                    if (chk == false)
                    {
                        if (j == 6 || j == 7 || j == 9)
                        {
                            sbb.Append("");
                        }
                        else
                        {
                            sbb.Append(dr.ItemArray[i].ToString());
                            i++;
                        }
                    }
                    else
                    {
                        sbb.Append(dr.ItemArray[i].ToString());
                        i++;
                    }

                    sbb.Append("|");
                    j++;
                }
                sw.WriteLine(sbb);

                break;
            }
            sw.Close();
            fs.Close();
        }

        public void ExportTxt(DataGridView dtg1, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            StringBuilder sb = new StringBuilder();
            //Export header text
            for (int i = 1; i < dtg1.Columns.Count + 1; i++)
            {

                sb.Append(dtg1.Columns[i - 1].HeaderText);
                sb.Append("|");//next sang cột bên cạnh                                   
            }
            sb.Append("\n");
            sw.WriteLine(sb);

            //Export data
            for (int n = 0; n <= dtg1.Rows.Count - 1; n++)
            {
                StringBuilder sbb = new StringBuilder();
                for (int j = 0; j < dtg1.Columns.Count; j++)
                {
                    if (dtg1.Rows[n].Cells[j].Value != null)
                    {
                        sbb.Append(dtg1.Rows[n].Cells[j].Value.ToString());
                        sbb.Append("|");
                    }
                }
                sw.Write(sbb);
            }

            sw.Close();
            fs.Close();
        }

        private void btn_upBOM_Click(object sender, EventArgs e)
        {
            if (dtb1.checkupBOM(dgv_updBOM, txt_lydo, cbx_lydo) == true)
            {
                MessageBox.Show("Các thông tin đang để trống hoặc sai!", "UpdateBOM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult anBom = MessageBox.Show("Bạn muốn update BOM?", "UpdateBOM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (anBom == DialogResult.Yes)
                {
                    OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                    cnn.Open();

                    foreach (DataGridViewRow dgr in dgv_updBOM.Rows)
                    {
                        if (dgr.Cells[0].Value != null)
                        {
                            if (cbb_cod.Text != "Blank" && cbb_vendr.Text != "Blank")//Update
                            {
                                string strUp = "Update All_model1 Set Vi_tri='" + dgr.Cells[4].Value.ToString() +
                                                                       "', Diem_gan='" + dgr.Cells[5].Value.ToString() +
                                                                       "', Maker='" + dgr.Cells[6].Value.ToString() +
                                                                       "', Maker_Part='" + dgr.Cells[7].Value.ToString() +
                                                                       "', Cong_doan='" + dgr.Cells[8].Value.ToString() +
                                                                       "', So_luong='" + dgr.Cells[9].Value.ToString() +
                                                                       "', Su_dung='" + dgr.Cells[10].Value.ToString() +
                                                                       "' Where Line='" + cbb_line.Text + "' And Model='" + cbb_mol.Text + "' And Ma_NVL='" + cbb_cod.Text + "' And Maker='" + cbb_vendr.Text + "'";
                                OleDbCommand cmdUp = new OleDbCommand(strUp, cnn);
                                cmdUp.ExecuteNonQuery();
                                //Lưu data sau update
                                ExportTxt(dgv_updBOM, _strdatabase + "\\BOM\\After_Update.txt");
                                CompareAndHistory(_strdatabase + "\\BOM\\Before_Update.txt", _strdatabase + "\\BOM\\After_Update.txt");
                            }
                            else//Insert
                            {
                                string strIn = "Insert Into All_model1 Values('" + dgr.Cells[0].Value.ToString() + "','" + dgr.Cells[1].Value.ToString() + "','" + dgr.Cells[2].Value.ToString() + "','" + dgr.Cells[3].Value.ToString() + "','"
                                                                                 + dgr.Cells[4].Value.ToString() + "','" + dgr.Cells[5].Value.ToString() + "','" + dgr.Cells[6].Value.ToString() + "','" + dgr.Cells[7].Value.ToString() + "','"
                                                                                 + "" + "','" + dgr.Cells[8].Value.ToString() + "','" + dgr.Cells[9].Value.ToString() + "','" + dgr.Cells[10].Value.ToString() + "')";
                                OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                                cmdIn.ExecuteNonQuery();
                                //Lưu data sau update
                                ExportTxt(dgv_updBOM, _strdatabase + "\\BOM\\After_Update.txt");
                                CompareAndHistory(_strdatabase + "\\BOM\\Before_Update.txt", _strdatabase + "\\BOM\\After_Update.txt");
                            }
                        }
                    }

                    //Delete database                    
                    string strDel = "Delete * From UpdateBOM";
                    OleDbCommand cmdDel = new OleDbCommand(strDel, cnn);
                    cmdDel.ExecuteNonQuery();

                    cnn.Close();

                    dgv_updBOM.Columns.Clear();

                    //Select all
                    string str = "select Line, Model, Ma_NVL, Mo_ta, Vi_tri, Diem_gan, Maker, Maker_Part, Cong_doan, So_luong, Su_dung from All_model1 Where Line='" + cbb_line.Text + "' And Model ='" + cbb_mol.Text + "' order by Ma_NVL";
                    DataTable dt1 = dtb.getData(str);

                    //Hien thi datagridview
                    dtb.show_upBOM(dgv_updBOM, dt1, true, true, true);
                }
            }           
        }

        public void CompareAndHistory(string pathBef, string pathAft)
        {
            //Before
            StreamReader srBef = new StreamReader(pathBef);
            string[] strBef = new string[dgv_updBOM.Columns.Count];
            while (srBef.EndOfStream == false)
            {
                strBef = srBef.ReadLine().Split('|');
            }
            srBef.Close();

            //After
            StreamReader srAft = new StreamReader(pathAft);
            string[] strAft = new string[dgv_updBOM.Columns.Count];
            while (srAft.EndOfStream == false)
            {
                strAft = srAft.ReadLine().Split('|');
            }
            srAft.Close();

            //So sánh
            string[] colName = new string[dgv_updBOM.Columns.Count];
            for (int j = 0; j < colName.Length; j++)
            {
                colName[j] = dgv_updBOM.Columns[j].Name;
            }

            string strLd = string.Empty;
            if (cbx_lydo.Text == "Khác")
            {
                strLd = txt_lydo.Text;
            }
            else
            {
                strLd = cbx_lydo.Text;
            }

            bool codeChange = false, mkChange = false;
            if (strBef[2] != strAft[2])
            {
                codeChange = true;
            }
            if (strBef[3] != strAft[3])
            {
                mkChange = true;
            }

            for (int i = 0; i < strAft.Length; i++)
            {                
                if (strBef[i] != strAft[i])
                {
                    OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                    cnn.Open();
                    if (codeChange == true && mkChange == true)//code thay doi, maker thay doi (Insert)
                    {
                        string strIn = "Insert Into HistoryBOM Values('" + DateTime.Now.ToShortDateString() + "','" + cbb_mol.Text + "','" + "" + "','" + "" + "','"
                                                                         + colName[i] + "','" + strBef[i] + "','" + strAft[i] + "','" + strLd + "','"
                                                                         + toolStripStatusLabel1.Text + "','" + DateTime.Now.ToString() + "')";
                        OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                        cmdIn.ExecuteNonQuery();
                    }
                    else if(codeChange == true && mkChange == false)//code thay doi, maker ko doi
                    {
                        string strIn = "Insert Into HistoryBOM Values('" + DateTime.Now.ToShortDateString() + "','" + cbb_mol.Text + "','" + "" + "','" + "" + "','"
                                                                         + colName[i] + "','" + strBef[i] + "','" + strAft[i] + "','" + strLd + "','"
                                                                         + toolStripStatusLabel1.Text + "','" + DateTime.Now.ToString() + "')";
                        OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                        cmdIn.ExecuteNonQuery();
                    }
                    else if(codeChange == false && mkChange == true)//code ko doi, maker thay doi
                    {
                        string strIn = "Insert Into HistoryBOM Values('" + DateTime.Now.ToShortDateString() + "','" + cbb_mol.Text + "','" + strAft[2] + "','" + "" + "','"
                                                                         + colName[i] + "','" + strBef[i] + "','" + strAft[i] + "','" + strLd + "','"
                                                                         + toolStripStatusLabel1.Text + "','" + DateTime.Now.ToString() + "')";
                        OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                        cmdIn.ExecuteNonQuery();
                    }
                    else//code ko doi, maker ko doi
                    {
                        string strIn = "Insert Into HistoryBOM Values('" + DateTime.Now.ToShortDateString() + "','" + cbb_mol.Text + "','" + strAft[2] + "','" + strAft[3] + "','"
                                                                         + colName[i] + "','" + strBef[i] + "','" + strAft[i] + "','" + strLd + "','"
                                                                         + toolStripStatusLabel1.Text + "','" + DateTime.Now.ToString() + "')";
                        OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                        cmdIn.ExecuteNonQuery();
                    }                                   
                    cnn.Close();
                }
            }
        }

        private void cbx_lydo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbx_lydo.Text == "Khác")
            {
                txt_lydo.Visible = true;
                txt_lydo.Enabled = true;
                label3.Visible = true;
            }
            else
            {
                txt_lydo.Visible = false;
                txt_lydo.Enabled = false;
                label3.Visible = false;
            }
        }

        private void btn_xoaUpdateBOM_Click(object sender, EventArgs e)
        {
            try
            {
                // Nếu dùng dataGridView2.SelectedRows.Count thì phải click vào đầu hàng
                // Nếu dùng dataGridView2.CurrentRow.Index thì click vào bất kì vị trí có thể xóa hàng đó

                if (this.dgv_updBOM.CurrentRow.Index >= 0)
                {
                    dgv_updBOM.Rows.Remove(dgv_updBOM.CurrentRow);
                }
            }
            catch
            {
                MessageBox.Show("Click vào đầu hàng đó để xóa!", "UpdateBOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //==============================================================================New BOM==============================================================        
        private void rbtn_newForm_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_newForm.Checked == true)
            {
                dgv_nwBOM.Columns.Clear();
                groupBox11.Enabled = true;
            }
            else
            {
                dgv_nwBOM.Columns.Clear();
                groupBox11.Enabled = false;
            }
        }

        private void rbtn_availableForm_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtn_availableForm.Checked == true)
            {
                groupBox10.Enabled = true;
            }
            else
            {
                groupBox10.Enabled = false;
            }
        }

        private void btn_createForm_Click(object sender, EventArgs e)
        {
            timer1.Start();
            strArrNg = new string[11];
            countNg = 0;
            //Tao file excel form            
            //Mo file excel form
            Workbook wb = new Workbook();
            Worksheet ws = wb.Worksheets[0];
            DataTable dt1 = new DataTable();
            dt1.Clear();
            dt1.Columns.Add("Line");
            dt1.Columns.Add("Model");
            dt1.Columns.Add("Ma_NVL");
            dt1.Columns.Add("Mo_ta");
            dt1.Columns.Add("Vi_tri");
            dt1.Columns.Add("Diem_gan");
            dt1.Columns.Add("Maker");
            dt1.Columns.Add("Maker_Part");
            dt1.Columns.Add("Cong_doan");
            dt1.Columns.Add("So_luong");
            dt1.Columns.Add("Su_dung");
            ws.InsertDataTable(dt1, true, 1, 1);
            ws.Columns[1].ColumnWidth = 15;
            ws.Columns[2].ColumnWidth = 15;
            ws.Columns[3].ColumnWidth = 15;
            ws.Columns[6].ColumnWidth = 20;
            ws.Columns[7].ColumnWidth = 25;
            ws.Columns[8].ColumnWidth = 15;
            ws.Columns[10].ColumnWidth = 15;
            ws.Rows[0].BorderInside(LineStyleType.Thin, Color.Black);
            ws.Rows[0].BorderAround(LineStyleType.Thin, Color.Black);
            ws.Rows[0].Style.Color = Color.Yellow;
            ws.Rows[0].Style.HorizontalAlignment = HorizontalAlignType.Center;

            wb.SaveToFile(_strdatabase + "\\tem\\NewBom.xlsx", ExcelVersion.Version2007);
            Excel.Application excelBom = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wbBom = excelBom.Workbooks.Open(_strdatabase + "\\tem\\NewBom.xlsx", 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet wsBom = (Microsoft.Office.Interop.Excel.Worksheet)wbBom.Worksheets.get_Item(1);

            excelBom.Visible = true;

            //Dien data -> Save -> Load file excel filled -> Hien thi database          
            Excel.AppEvents_WorkbookBeforeSaveEventHandler WorkbookBeforeSave = new Excel.AppEvents_WorkbookBeforeSaveEventHandler(excelBom_WorkbookBeforeSave);
            excelBom.WorkbookBeforeSave += excelBom_WorkbookBeforeSave;

            Excel.AppEvents_WorkbookBeforeCloseEventHandler WorkbookBeforeClose = new Excel.AppEvents_WorkbookBeforeCloseEventHandler(excelBom_WorkbookBeforeClose);
            excelBom.WorkbookBeforeClose += excelBom_WorkbookBeforeClose;
        }

        private static void excelBom_WorkbookBeforeSave(Excel.Workbook wb, bool saveWb, ref bool cacel)
        {
            MessageBox.Show("Kiểm tra lại thông tin BOM.\nVà tắt file để load BOM vào chương trình!", "NewBom", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }

        public static bool chk;
        private static void excelBom_WorkbookBeforeClose(Excel.Workbook wb, ref bool cacel)
        {
            chk = true;
        }

        private void btn_showData_Click(object sender, EventArgs e)
        {
            strArrNg = new string[11];
            countNg = 0;
            //Load file excel filled -> Hien thi database
            dgv_nwBOM.Visible = true;
            string fname = "";
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Excel File Dialog";
            fdlg.InitialDirectory = @"C:\";
            fdlg.Filter = "Excel Files |*.xlsx|Excel Files |*.csv|All Files (*.*)|*.*";
            fdlg.FilterIndex = 3;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                fname = fdlg.FileName;
            }

            if (fname != "")
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWB = xlApp.Workbooks.Open(fname);
                Microsoft.Office.Interop.Excel._Worksheet xlWS = xlWB.Sheets[1];
                Microsoft.Office.Interop.Excel.Range XlRag = xlWS.UsedRange;

                int rowCount = XlRag.Rows.Count;
                int colCount = XlRag.Columns.Count;

                dgv_nwBOM.ColumnCount = colCount;
                dgv_nwBOM.RowCount = rowCount;

                for (int i = 1; i <= rowCount; i++)
                {
                    for (int j = 1; j <= colCount; j++)
                    {
                        if (XlRag.Cells[i, j] != null && XlRag.Cells[i, j].Value2 != null)
                        {
                            dgv_nwBOM.Rows[i - 1].Cells[j - 1].Value = XlRag.Cells[i, j].Value2.ToString();
                        }
                    }
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Marshal.ReleaseComObject(XlRag);
                Marshal.ReleaseComObject(xlWS);

                xlWB.Close();
                Marshal.ReleaseComObject(xlWB);

                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }

            if (dgv_nwBOM.Columns.Count > 0)
            {
                getNameColumns(0, 0, "Line");
                getNameColumns(1, 0, "Model");
                getNameColumns(2, 0, "Ma_NVL");
                getNameColumns(3, 0, "Mo_ta");
                getNameColumns(4, 0, "Vi_tri");
                getNameColumns(5, 0, "Diem_gan");
                getNameColumns(6, 0, "Maker");
                getNameColumns(7, 0, "Maker_Part");
                getNameColumns(8, 0, "Cong_doan");
                getNameColumns(9, 0, "So_luong");
                getNameColumns(10, 0, "Su_dung");
                if (strArrNg[0] != null)
                {
                    string toDisply = string.Join("\n", strArrNg);
                    MessageBox.Show("Trường dữ liệu bị sai.\nThay đổi lại như sau :\n" + toDisply, "NewBOM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_nmBOM_Click(object sender, EventArgs e)
        {
            if (dgv_nwBOM.Columns.Count != 11)
            {
                MessageBox.Show("Kiểm tra lại Form dữ liệu BOM chuẩn.\nDữ liệu bạn nhập đang thừa hoặc thiếu thông tin!", "NewBOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                getNameColumns(0, 0, "Line");
                getNameColumns(1, 0, "Model");
                getNameColumns(2, 0, "Ma_NVL");
                getNameColumns(3, 0, "Mo_ta");
                getNameColumns(4, 0, "Vi_tri");
                getNameColumns(5, 0, "Diem_gan");
                getNameColumns(6, 0, "Maker");
                getNameColumns(7, 0, "Maker_Part");
                getNameColumns(8, 0, "Cong_doan");
                getNameColumns(9, 0, "So_luong");
                getNameColumns(10, 0, "Su_dung");

                if (strArrNg[0] != null)
                {
                    string toDisply = string.Join("\n", strArrNg);
                    MessageBox.Show("Trường dữ liệu bị sai.\nThay đổi lại như sau :\n" + toDisply, "NewBOM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //all thong tin phai day du
                    if (dtb1.checkNwBOM(dgv_nwBOM) == true)
                    {
                        MessageBox.Show("Các thông tin đang để trống hoặc sai!", "NewBOM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        DialogResult anBom = MessageBox.Show("Bạn muốn tạo BOM New Model?", "NewBOM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (anBom == DialogResult.Yes)
                        {
                            if (chk_data(dgv_nwBOM) == false)//co loi data
                            {
                                DialogResult relBOM = MessageBox.Show("Thông tin BOM đang để trống hoặc sai!\nBạn có muốn tham khỏa file Master dữ liệu BOM?", "NewBOM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (relBOM == DialogResult.Yes)
                                {
                                    System.Diagnostics.Process.Start(_strdatabase + "\\Picture\\zMaterBOM.xlsx");
                                }
                            }
                            else
                            {
                                //update BOM database
                                OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                                cnn.Open();
                                foreach (DataGridViewRow dgr in dgv_nwBOM.Rows)
                                {
                                    if (dgr.Index == 0)
                                    {
                                        continue;
                                    }
                                    if (dgr.Cells[0].Value != null)
                                    {
                                        string strIn = "Insert Into All_model1 Values('" + dgr.Cells[0].Value.ToString() + "','" + dgr.Cells[1].Value.ToString() + "','" + dgr.Cells[2].Value.ToString() + "','"
                                                                                         + dgr.Cells[3].Value.ToString() + "','" + dgr.Cells[4].Value.ToString() + "','" + dgr.Cells[5].Value.ToString() + "','"
                                                                                         + dgr.Cells[6].Value.ToString() + "','" + dgr.Cells[7].Value.ToString() + "','" + "" + "','"
                                                                                         + dgr.Cells[8].Value.ToString() + "','"+ dgr.Cells[9].Value.ToString() + "','" + dgr.Cells[10].Value.ToString() + "')";
                                        OleDbCommand cmdIn = new OleDbCommand(strIn, cnn);
                                        cmdIn.ExecuteNonQuery();
                                    }
                                }
                                cnn.Close();
                                //reset data
                                dgv_nwBOM.Columns.Clear();
                                File.Delete(_strdatabase + "\\tem\\NewBom.xlsx");
                            }
                        }
                    }
                }
            }
        }

        public void getNameColumns(int indexCol, int indexRow, string namCol)
        {
            if (dgv_nwBOM.Rows[indexRow].Cells[indexCol].Value.ToString() != namCol)
            {
                strArrNg[countNg] = dgv_nwBOM.Rows[indexRow].Cells[indexCol].Value.ToString() + " = " + namCol;
                countNg++;
            }
        }

        public bool chk_data(DataGridView dgv)
        {
            int errData = 0;
            foreach (DataGridViewRow dgr in dgv.Rows)
            {
                if (dgr.Index == 0)
                {
                    continue;
                }
                //Line
                string masterStr = dgv.Rows[1].Cells[0].Value.ToString();
                if (dgr.Cells[0].Value.ToString() == "3" || dgr.Cells[0].Value.ToString() == "6" || dgr.Cells[0].Value.ToString() == "7")
                {
                    if (dgr.Cells[0].Value.ToString() != masterStr)
                    {
                        errData++;
                        dgr.Cells[0].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        dgr.Cells[0].Style.BackColor = Color.White;
                    }
                }
                else
                {
                    errData++;
                    dgr.Cells[0].Style.BackColor = Color.Red;
                }
                //Model
                if (dgr.Cells[1].Value.ToString() != "")
                {
                    int err = 0;
                    foreach (char c in dgr.Cells[1].Value.ToString())
                    {
                        if (!Char.IsUpper(c))
                        {
                            int kq;
                            if ((int.TryParse(c.ToString(), out kq) == false) && (c != '-'))
                            {
                                err++;
                                break;
                            }
                        }
                    }

                    if (err == 0)
                    {
                        dgr.Cells[1].Style.BackColor = Color.White;
                    }
                    else
                    {
                        errData++;
                        dgr.Cells[1].Style.BackColor = Color.Red;
                    }
                }
                //Code
                if (dgr.Cells[2].Value.ToString().Length != 11)
                {
                    errData++;
                    dgr.Cells[2].Style.BackColor = Color.Red;
                }
                else
                {
                    dgr.Cells[2].Style.BackColor = Color.White;
                }
                //Qty
                int qty;
                if (int.TryParse(dgr.Cells[5].Value.ToString(), out qty) == false)
                {
                    errData++;
                    dgr.Cells[5].Style.BackColor = Color.Red;
                }
                else
                {
                    dgr.Cells[5].Style.BackColor = Color.White;
                }
                //Process
                if (dgr.Cells[8].Value.ToString() != "SMD")
                {
                    errData++;
                    dgr.Cells[8].Style.BackColor = Color.Red;
                }
                else
                {
                    dgr.Cells[8].Style.BackColor = Color.White;
                }
                //Qty Roll
                int qtyRoll;
                if (int.TryParse(dgr.Cells[9].Value.ToString(), out qtyRoll) == false)
                {
                    errData++;
                    dgr.Cells[9].Style.BackColor = Color.Red;
                }
                else
                {
                    dgr.Cells[9].Style.BackColor = Color.White;
                }
                //Status code
                if (dgr.Cells[10].Value.ToString() != "Yes" && dgr.Cells[10].Value.ToString() != "No")
                {
                    errData++;
                    dgr.Cells[10].Style.BackColor = Color.Red;
                }
                else
                {
                    dgr.Cells[10].Style.BackColor = Color.White;
                }
            }

            if (errData == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btn_deleteAll_Click(object sender, EventArgs e)
        {
            dgv_nwBOM.Columns.Clear();
        }

        private void btn_fileMaster_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(_strdatabase + "\\Picture\\zMaterBOM.xlsx");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (chk == true)
            {
                chk = false;
                timer1.Stop();
                loadExcel(dgv_nwBOM);
            }
        }

        public void loadExcel(DataGridView dgt)
        {
            //Load file excel filled -> Hien thi database
            dgt.Visible = true;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWB = xlApp.Workbooks.Open(_strdatabase + "\\tem\\NewBom.xlsx");
            Microsoft.Office.Interop.Excel._Worksheet xlWS = xlWB.Sheets[1];
            Microsoft.Office.Interop.Excel.Range XlRag = xlWS.UsedRange;

            int rowCount = XlRag.Rows.Count;
            int colCount = XlRag.Columns.Count;

            dgt.ColumnCount = colCount;
            dgt.RowCount = rowCount;

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    if (XlRag.Cells[i, j] != null && XlRag.Cells[i, j].Value2 != null)
                    {
                        dgt.Rows[i - 1].Cells[j - 1].Value = XlRag.Cells[i, j].Value2.ToString();
                    }
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.ReleaseComObject(XlRag);
            Marshal.ReleaseComObject(xlWS);

            xlWB.Close();
            Marshal.ReleaseComObject(xlWB);

            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
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
                ex.exportStockKTZZ(dgv_bom, fil_name, chek);

                MessageBox.Show("Lưu thành công!", "BOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }   
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            DialogResult relBOM = MessageBox.Show("Bạn có chắc muốn xóa BOM model này?", "BOM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(relBOM == DialogResult.Yes)
            {
                if (cbb_model.Text != "")
                {
                    try
                    {
                        OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb"); //khai báo và khởi tạo biến cnn
                        cnn.Open();

                        string strDel = "Delete * From All_model1 where Model='" + cbb_model.Text + "'";
                        OleDbCommand cmdIn = new OleDbCommand(strDel, cnn);
                        cmdIn.ExecuteNonQuery();

                        cnn.Close();
                        MessageBox.Show("Xóa thành công!", "BOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Xóa thất bại!", "BOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn Model!", "BOM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }           
        }            
    }
}
