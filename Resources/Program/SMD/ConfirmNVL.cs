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

namespace ManageMaterialPBA
{
    public partial class ConfirmNVL : Form
    {
        string _strSubB = string.Empty, _strSubA = string.Empty;
        string _mter = string.Empty, _cod = string.Empty, _mk = string.Empty, _mkp = string.Empty, _qtyRll = string.Empty, _mol = string.Empty, _mkpAct = string.Empty;
        Form1 _frm;

        public ConfirmNVL(Form1 frm, string strSubB, string strSubA, string mter, string cod, string mk, string mkp, string qtyRll, string mol, string mkpAct) 
        {
            InitializeComponent();
            _strSubB = strSubB;
            _strSubA = strSubA;
            _mter = mter;
            _cod = cod;
            _mk = mk;
            _mkp = mkp;
            _qtyRll = qtyRll;
            _mol = mol;
            _mkpAct = mkpAct;
            _frm = frm;
        }

        private void ConfirmNVL_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            lbl_ndxn1.Text = "(1) : " + _strSubB;
            lbl_ndxn2.Text = "(2) : " + _strSubA;
        }

        private void btn_dy_Click(object sender, EventArgs e)
        {
            if ((_strSubB != "" & txt_xn1.Text != "" && _strSubA != "" && txt_xn2.Text != "")
                || (_strSubB == "" & txt_xn1.Text == "" && _strSubA != "" && txt_xn2.Text != "")
                    || (_strSubB != "" & txt_xn1.Text != "" && _strSubA == "" && txt_xn2.Text == ""))
            {
                string str_user = get_RightLogin2(txt_dn.Text, txt_mk.Text);
                if (str_user != "")
                {                        
                    //update database
                    try
                    {
                        OleDbConnection cnn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb");
                        cnn.Open();
                        //update history
                        if(_strSubB != "" && _strSubA != "")
                        {
                            //
                            string strInsB = string.Empty;
                            strInsB = "Insert Into HistoryConfirmNVL Values ('" + DateTime.Now.ToShortDateString() + "','" +
                                                                                 _mol + "','" +
                                                                                 _cod + "','" +
                                                                                 _mk + "','" +
                                                                                 _mkp + "','" +
                                                                                 _mkpAct + "','" +
                                                                                 _strSubB + "','" +
                                                                                 txt_xn1.Text + "','" +
                                                                                 str_user + "','" +
                                                                                 DateTime.Now.ToString() + "')";
                            OleDbCommand cmdB = new OleDbCommand(strInsB, cnn);
                            cmdB.ExecuteNonQuery();
                            //
                            string strInsA = string.Empty;
                            strInsA = "Insert Into HistoryConfirmNVL Values ('" + DateTime.Now.ToShortDateString() + "','" +
                                                                                 _mol + "','" +
                                                                                 _cod + "','" +
                                                                                 _mk + "','" +
                                                                                 _mkp + "','" +
                                                                                 _mkpAct + "','" +
                                                                                 _strSubA + "','" +
                                                                                 txt_xn2.Text + "','" +
                                                                                 str_user + "','" +
                                                                                 DateTime.Now.ToString() + "')";
                            OleDbCommand cmdA = new OleDbCommand(strInsA, cnn);
                            cmdA.ExecuteNonQuery();
                        }
                        else
                        {
                            string strSub = string.Empty, txt_xn = string.Empty;
                            if(_strSubA != "")
                            {
                                strSub = _strSubA;
                                txt_xn = txt_xn2.Text;
                            }
                            else
                            {
                                strSub = _strSubB;
                                txt_xn = txt_xn1.Text;
                            }
                            string strIns = string.Empty;
                            strIns = "Insert Into HistoryConfirmNVL Values ('" + DateTime.Now.ToShortDateString() + "','" +
                                                                                 _mol + "','" +
                                                                                 _cod + "','" +
                                                                                 _mk + "','" +
                                                                                 _mkp + "','" +
                                                                                 _mkpAct + "','" +
                                                                                 strSub + "','" +
                                                                                 txt_xn + "','" +
                                                                                 str_user + "','" +
                                                                                 DateTime.Now.ToString() + "')";
                            OleDbCommand cmd = new OleDbCommand(strIns, cnn);
                            cmd.ExecuteNonQuery();
                        }
                        //update BOM
                        string strUp = string.Empty;
                        strUp = "Update All_model1 Set Maker_Part_xn = '" + _mkpAct + "' Where Model='" + _mol + "' and Ma_NVL= '" + _cod + "' and Maker= '" + _mk + "' and Maker_Part='" + _mkp + "'";
                        OleDbCommand cmdUp = new OleDbCommand(strUp, cnn);
                        cmdUp.ExecuteNonQuery();
                        cnn.Close();
                        //Fill data
                        _frm.inFWK[0] = _mter;//material
                        _frm.inFWK[1] = _cod;//code
                        _frm.inFWK[2] = _mk;//maker
                        _frm.inFWK[3] = _mkp;//maker part
                        _frm.inFWK[4] = _qtyRll;//qty in roll
                        _frm.cfrm = true;
                        this.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Lỗi update history xác nhận NVL/BOM!", "Confirm NVL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }                    
                }
                else
                {
                    MessageBox.Show("Bạn không có quyền xác nhận!", "Confirm NVL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa điền nội dung xác nhận hoặc bạn đang điền thừa (1)/(2)!", "Confirm NVL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public string get_RightLogin2(string user, string pass)
        {
            string user_Login = "";
            string strSel = "Select Name_user, part From Login Where u_ser='" + user + "' And pass_word='" + pass + "'";

            DataTable dt = getData(strSel);

            foreach (DataRow dtr in dt.Rows)
            {
                if (dtr.ItemArray[1].ToString() == "CPE")
                {
                    user_Login = dtr.ItemArray[0].ToString();
                }                
            }
            return user_Login;
        }

        public DataTable getData(string str)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(str, @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb");
            da.Fill(dt);

            return dt;
        }
    }
}
