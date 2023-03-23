using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace ManageMaterialPBA
{
    public class database
    {
        public string constr = string.Empty;
        public string user;          

        public database(string str_Link)
        {
            constr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + str_Link + @"\Database.mdb";
        }

        public OleDbConnection GetConnection()
        {
            OleDbConnection con = new OleDbConnection(constr);
            con.Open();
            return con;
        }

        public DataTable getData(string str)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(str, constr);
            da.Fill(dt);

            return dt;
        }

        public bool login_part(string user, string pass, string id, string part)
        {
            string str = "select u_ser, pass_word from login where u_ser = '" + user + "' and pass_word = '" + pass + "'and part = '" + part + "' and ID_Code ='" + id + "'";

            DataTable dt = new DataTable();
            dt = getData(str);

            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool login(string user, string pass)
        {
            string str = "select u_ser, pass_word from login where u_ser = '" + user + "' and pass_word = '" + pass + "'";

            DataTable dt = new DataTable();
            dt = getData(str);

            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool login_admin(string user, string pass, string id, string kind)
        {
            string str = "select u_ser, pass_word, ID_Code, kind from login where u_ser = '" + user + "' and pass_word = '" + pass + "' and ID_Code ='" + id + "'";

            DataTable dt = new DataTable();
            dt = getData(str);

            foreach (DataRow dr in dt.Rows)
            {
                kind = dr.ItemArray[3].ToString();
            }

            if (dt.Rows.Count != 0 && kind == "admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool login_manager(string user, string pass, string id, string kind)
        {
            string str = "select u_ser, pass_word, ID_Code, kind from login where u_ser = '" + user + "' and pass_word = '" + pass + "' and ID_Code ='" + id + "'";

            DataTable dt = new DataTable();
            dt = getData(str);

            foreach (DataRow dr in dt.Rows)
            {
                kind = dr.ItemArray[3].ToString();
            }

            if (dt.Rows.Count != 0 && kind == "manager")
            {
                return true;
            }
            else
            {
                return false;
            }
        } 

        public void change_pw(string userr, string new_pw, string timme)
        {
            OleDbConnection cnn11 = new OleDbConnection(constr);
            cnn11.Open();
            string str = string.Empty;
            str = "Update login Set pass_word='" + new_pw + "', Monthly_Change ='" + timme + "' Where u_ser='" + userr + "'";

            OleDbCommand cmd11 = new OleDbCommand(str, cnn11);
            cmd11.ExecuteNonQuery();

            cnn11.Close();          
        }

        public string get_DateChangepass(string user, string pass)
        {
            string date = "";
            string str = "select Monthly_Change from login where u_ser = '" + user + "' And pass_word ='" + pass + "'";
            DataTable dt = new DataTable();
            dt = getData(str);

            // Add users vào comboBox
            foreach (DataRow dr in dt.Rows)
            {
                date = dr.ItemArray[0].ToString();
            }
            return date;
        }

        public void insert_account(string user, string pass, string kind, string hoten, string part, string id)
        {
            OleDbConnection cnn = GetConnection();
            string str = " insert into login values('" + user + "', '" + pass + "', '" + kind + "', '" + hoten + "', '" + part + "', '" + DateTime.Now.ToString("MM/dd/yy") + "', '" + id + "')";

            OleDbCommand cmd = new OleDbCommand(str, cnn);
            cmd.ExecuteNonQuery();

            cnn.Close();
        }

        public void delete(string sql_delete)
        {
            OleDbConnection cnn = GetConnection();

            OleDbCommand cmd = new OleDbCommand(sql_delete, cnn);
            cmd.ExecuteNonQuery();

            cnn.Close();
        }        

        public DataTable loadtransport()
        {
            string str = "select Line, Model, Ma_NVL, Mo_ta, Vi_tri, Diem_gan, Maker, Maker_Part, Cong_doan, So_luong, Su_dung from All_model1 order by Line, Model";

            return getData(str);
        }

        public DataTable loadtransportml(string model)
        {
            string str = "select Line, Model, Ma_NVL, Mo_ta, Vi_tri, Diem_gan, Maker, Maker_Part, Cong_doan, So_luong, Su_dung from All_model1 where Model = '" + model + "' order by Ma_NVL";

            return getData(str);
        }

        public void get_model(ComboBox cbb)
        {
            string str = "select Model from All_model1 order by Model";
            DataTable dt = new DataTable();
            dt = getData(str);

            // Add users vào comboBox
            foreach (DataRow dr in dt.Rows)
            {
                if (!cbb.Items.Contains(dr.ItemArray[0].ToString()))
                {
                    cbb.Items.Add(dr.ItemArray[0].ToString());
                }
            }
        }

        public void get_inf(ComboBox cbb, string namedataTble, string nameCol)
        {
            string str = "select " + nameCol + " from " + namedataTble + " order by " + nameCol;
            DataTable dt = new DataTable();
            dt = getData(str);

            // Add users vào comboBox
            foreach (DataRow dr in dt.Rows)
            {
                if (!cbb.Items.Contains(dr.ItemArray[0].ToString()))
                {
                    cbb.Items.Add(dr.ItemArray[0].ToString());
                }
            }
        }

        public string get_name(string nameUser, string password)
        {
            string name = "";
            string str = "select Name_user from login where u_ser = '" + nameUser + "' And pass_word ='" + password + "'";
            DataTable dt = new DataTable();
            dt = getData(str);

            // Add users vào comboBox
            foreach (DataRow dr in dt.Rows)
            {
                name = dr.ItemArray[0].ToString();
            }
            return name;
        }

        public string get_TimeChangepass()
        {
            return DateTime.Now.ToString("MM/dd/yy");
        }

        public void show_upBOM(DataGridView dgv, DataTable dt, bool bl1, bool bl2, bool bl3)
        {
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn col_line = new DataGridViewTextBoxColumn();
            col_line.DataPropertyName = "Line";
            col_line.HeaderText = "Line";
            col_line.Name = "Line";
            col_line.ReadOnly = true;
            col_line.Width = 50;
            col_line.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_line);

            DataGridViewTextBoxColumn col_mol = new DataGridViewTextBoxColumn();
            col_mol.DataPropertyName = "Model";
            col_mol.HeaderText = "Model";
            col_mol.Name = "Model";
            col_mol.ReadOnly = true;
            col_mol.Width = 80;
            col_mol.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_mol);

            DataGridViewTextBoxColumn col_Code = new DataGridViewTextBoxColumn();
            col_Code.DataPropertyName = "Ma_NVL";
            col_Code.HeaderText = "Ma_NVL";
            col_Code.Name = "Ma_NVL";
            col_Code.ReadOnly = bl3;
            if(bl3 == false)
            {
                col_Code.CellTemplate.Style.BackColor = Color.LightGray;
            }
            col_Code.Width = 80;
            col_Code.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_Code);

            DataGridViewTextBoxColumn col_Material = new DataGridViewTextBoxColumn();
            col_Material.DataPropertyName = "Mo_ta";
            col_Material.HeaderText = "Mo_ta";
            col_Material.Name = "Mo_ta";
            col_Material.ReadOnly = bl3;
            if(bl3 == false)
            {
                col_Material.CellTemplate.Style.BackColor = Color.LightGray;
            }
            col_Material.Width = 100;
            col_Material.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_Material);

            DataGridViewTextBoxColumn col_loca = new DataGridViewTextBoxColumn();
            col_loca.DataPropertyName = "Vi_tri";
            col_loca.HeaderText = "Vi_tri";
            col_loca.Name = "Vi_tri";
            col_loca.ReadOnly = bl1;
            if(bl1 == false)
            {
                col_loca.CellTemplate.Style.BackColor = Color.LightGray;
            }
            col_loca.Width = 100;
            col_loca.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_loca);

            DataGridViewTextBoxColumn col_qty = new DataGridViewTextBoxColumn();
            col_qty.DataPropertyName = "Diem_gan";
            col_qty.HeaderText = "Diem_gan";
            col_qty.Name = "Diem_gan";
            col_qty.ReadOnly = bl1;
            if(bl1 == false)
            {
                col_qty.CellTemplate.Style.BackColor = Color.LightGray;
            }
            col_qty.Width = 70;
            col_qty.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_qty);

            DataGridViewTextBoxColumn col_Maker = new DataGridViewTextBoxColumn();
            col_Maker.DataPropertyName = "Maker";
            col_Maker.HeaderText = "Maker";
            col_Maker.Name = "Maker";
            col_Maker.ReadOnly = bl2;
            if(bl2 == false)
            {
                col_Maker.CellTemplate.Style.BackColor = Color.LightGray;
            }
            col_Maker.Width = 100;
            col_Maker.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_Maker);

            DataGridViewTextBoxColumn col_MakerPart = new DataGridViewTextBoxColumn();
            col_MakerPart.DataPropertyName = "Maker_Part";
            col_MakerPart.HeaderText = "Maker_Part";
            col_MakerPart.Name = "Maker_Part";
            col_MakerPart.ReadOnly = bl1;
            if(bl1 == false)
            {
                col_MakerPart.CellTemplate.Style.BackColor = Color.LightGray;
            }
            col_MakerPart.Width = 240;
            col_MakerPart.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_MakerPart);

            DataGridViewTextBoxColumn col_prcess = new DataGridViewTextBoxColumn();
            col_prcess.DataPropertyName = "Cong_doan";
            col_prcess.HeaderText = "Cong_doan";
            col_prcess.Name = "Cong_doan";
            col_prcess.ReadOnly = true;
            col_prcess.Width = 90;
            col_prcess.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_prcess);

            DataGridViewTextBoxColumn col_QtyRll = new DataGridViewTextBoxColumn();
            col_QtyRll.DataPropertyName = "So_luong";
            col_QtyRll.HeaderText = "So_luong";
            col_QtyRll.Name = "So_luong";
            col_QtyRll.ReadOnly = bl1;
            if(bl1 == false)
            {
                col_QtyRll.CellTemplate.Style.BackColor = Color.LightGray;
            }
            col_QtyRll.Width = 100;
            col_QtyRll.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_QtyRll);

            DataGridViewComboBoxColumn col_stt = new DataGridViewComboBoxColumn();
            col_stt.Items.Add("Yes");
            col_stt.Items.Add("No");
            col_stt.FlatStyle = FlatStyle.Popup;
            col_stt.DataPropertyName = "Su_dung";
            col_stt.HeaderText = "Su_dung";
            col_stt.Name = "Su_dung";
            col_stt.Width = 95;
            col_stt.ReadOnly = bl1;
            if(bl1 == false)
            {
                col_stt.CellTemplate.Style.BackColor = Color.LightGray;
            }
            col_stt.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_stt);            

            dgv.DataSource = dt;
            dgv.ClearSelection();
        }

        public void ShowupBOMHistory(DataGridView dgv, DataTable dt)
        {
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn col_date = new DataGridViewTextBoxColumn();
            col_date.DataPropertyName = "Ngay_thang";
            col_date.HeaderText = "Ngay_thang";
            col_date.Name = "Ngay_thang";
            col_date.ReadOnly = true;
            col_date.Width = 100;
            col_date.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_date);

            DataGridViewTextBoxColumn col_mol = new DataGridViewTextBoxColumn();
            col_mol.DataPropertyName = "Model";
            col_mol.HeaderText = "Model";
            col_mol.Name = "Model";
            col_mol.ReadOnly = true;
            col_mol.Width = 100;
            col_mol.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_mol);

            DataGridViewTextBoxColumn col_maNVL = new DataGridViewTextBoxColumn();
            col_maNVL.DataPropertyName = "Ma_NVL";
            col_maNVL.HeaderText = "Ma_NVL";
            col_maNVL.Name = "Ma_NVL";
            col_maNVL.ReadOnly = true;
            col_maNVL.Width = 100;
            col_maNVL.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_maNVL);

            DataGridViewTextBoxColumn col_mk = new DataGridViewTextBoxColumn();
            col_mk.DataPropertyName = "Maker";
            col_mk.HeaderText = "Maker";
            col_mk.Name = "Maker";
            col_mk.ReadOnly = true;
            col_mk.Width = 100;
            col_mk.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_mk);

            DataGridViewTextBoxColumn col_hm = new DataGridViewTextBoxColumn();
            col_hm.DataPropertyName = "Hang_muc";
            col_hm.HeaderText = "Hang_muc";
            col_hm.Name = "Hang_muc";
            col_hm.ReadOnly = true;
            col_hm.Width = 120;
            col_hm.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_hm);

            DataGridViewTextBoxColumn col_bef = new DataGridViewTextBoxColumn();
            col_bef.DataPropertyName = "Du_lieu_truoc";
            col_bef.HeaderText = "Du_lieu_truoc";
            col_bef.Name = "Du_lieu_truoc";
            col_bef.ReadOnly = true;
            col_bef.Width = 160;
            col_bef.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_bef);

            DataGridViewTextBoxColumn col_aft = new DataGridViewTextBoxColumn();
            col_aft.DataPropertyName = "Du_lieu_sau";
            col_aft.HeaderText = "Du_lieu_sau";
            col_aft.Name = "Du_lieu_sau";
            col_aft.ReadOnly = true;
            col_aft.Width = 160;
            col_aft.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_aft);

            DataGridViewTextBoxColumn col_ld = new DataGridViewTextBoxColumn();
            col_ld.DataPropertyName = "Ly_do";
            col_ld.HeaderText = "Ly_do";
            col_ld.Name = "Ly_do";
            col_ld.ReadOnly = true;
            col_ld.Width = 220;
            col_ld.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_ld);

            DataGridViewTextBoxColumn col_ng = new DataGridViewTextBoxColumn();
            col_ng.DataPropertyName = "Nguoi_thuc_hien";
            col_ng.HeaderText = "Nguoi_thuc_hien";
            col_ng.Name = "Nguoi_thuc_hien";
            col_ng.ReadOnly = true;
            col_ng.Width = 120;
            col_ng.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_ng);

            DataGridViewTextBoxColumn col_td = new DataGridViewTextBoxColumn();
            col_td.DataPropertyName = "Thoi_diem";
            col_td.HeaderText = "Thoi_diem";
            col_td.Name = "Thoi_diem";
            col_td.ReadOnly = true;
            col_td.Width = 100;
            col_td.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_td);           

            dgv.DataSource = dt;
            dgv.ClearSelection();
        }

        public void ShowupPrintHistory(DataGridView dgv, DataTable dt)
        {
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn col_date = new DataGridViewTextBoxColumn();
            col_date.DataPropertyName = "Ngay_thang";
            col_date.HeaderText = "Ngay_thang";
            col_date.Name = "Ngay_thang";
            col_date.ReadOnly = true;
            col_date.Width = 100;
            col_date.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_date);          

            DataGridViewTextBoxColumn col_hm = new DataGridViewTextBoxColumn();
            col_hm.DataPropertyName = "Hang_muc";
            col_hm.HeaderText = "Hang_muc";
            col_hm.Name = "Hang_muc";
            col_hm.ReadOnly = true;
            col_hm.Width = 120;
            col_hm.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_hm);

            DataGridViewTextBoxColumn col_bef = new DataGridViewTextBoxColumn();
            col_bef.DataPropertyName = "Du_lieu_truoc";
            col_bef.HeaderText = "Du_lieu_truoc";
            col_bef.Name = "Du_lieu_truoc";
            col_bef.ReadOnly = true;
            col_bef.Width = 180;
            col_bef.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_bef);

            DataGridViewTextBoxColumn col_aft = new DataGridViewTextBoxColumn();
            col_aft.DataPropertyName = "Du_lieu_sau";
            col_aft.HeaderText = "Du_lieu_sau";
            col_aft.Name = "Du_lieu_sau";
            col_aft.ReadOnly = true;
            col_aft.Width = 180;
            col_aft.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_aft);

            DataGridViewTextBoxColumn col_ld = new DataGridViewTextBoxColumn();
            col_ld.DataPropertyName = "Ly_do";
            col_ld.HeaderText = "Ly_do";
            col_ld.Name = "Ly_do";
            col_ld.ReadOnly = true;
            col_ld.Width = 240;
            col_ld.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_ld);

            DataGridViewTextBoxColumn col_CPE = new DataGridViewTextBoxColumn();
            col_CPE.DataPropertyName = "CPE_xac_nhan";
            col_CPE.HeaderText = "CPE_xac_nhan";
            col_CPE.Name = "CPE_xac_nhan";
            col_CPE.ReadOnly = true;
            col_CPE.Width = 150;
            col_CPE.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_CPE);

            DataGridViewTextBoxColumn col_ng = new DataGridViewTextBoxColumn();
            col_ng.DataPropertyName = "Nguoi_thuc_hien";
            col_ng.HeaderText = "Nguoi_thuc_hien";
            col_ng.Name = "Nguoi_thuc_hien";
            col_ng.ReadOnly = true;
            col_ng.Width = 150;
            col_ng.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_ng);

            DataGridViewTextBoxColumn col_td = new DataGridViewTextBoxColumn();
            col_td.DataPropertyName = "Thoi_diem";
            col_td.HeaderText = "Thoi_diem";
            col_td.Name = "Thoi_diem";
            col_td.ReadOnly = true;
            col_td.Width = 150;
            col_td.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns.Add(col_td);

            dgv.DataSource = dt;
            dgv.ClearSelection();
        }

        public void UpChangePass(string user, string oldPass, string newPass)
        {
            OleDbConnection cnn = new OleDbConnection(constr);
            cnn.Open();
            string str = "INSERT INTO ChangePassWord VALUES ( '" + user + "', '" + oldPass + "', '" + newPass + "', 'New')";
            OleDbCommand cmd = new OleDbCommand(str, cnn);
            cmd.ExecuteNonQuery();
            cnn.Close();
        }      
    }
}
