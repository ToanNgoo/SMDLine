using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace ManageMaterialPBA
{
    class ClsExcel
    {
        //Export Database to .csv
        public bool Export_CSV(DataTable dt, string path, bool check, string ColumnName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (check == false)
                {
                    sb.Append(ColumnName);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                        sb.Append(FormatCSV(dr[dc.ColumnName].ToString()) + ",");
                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendLine();
                }
                if (check == false)
                {
                    File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                }
                else
                {
                    File.AppendAllText(path, sb.ToString(), Encoding.UTF8);
                }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }       
        }

        public bool Export_CSV(DataTable dt, string path, string ColumnName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //if (check == false)
                //{
                    sb.Append(ColumnName);
                //}
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                        sb.Append(FormatCSV(dr[dc.ColumnName].ToString()) + ",");
                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendLine();
                }
                //if (check == false)
                //{
                    File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                //}
                //else
                //{
                //   File.AppendAllText(path, sb.ToString(), Encoding.UTF8);
                //}

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }        
        
        public static string FormatCSV(string input)
        {
            try
            {
                if (input == null)
                    return string.Empty;

                bool containsQuote = false;
                bool containsComma = false;
                int len = input.Length;
                for (int i = 0; i < len && (containsComma == false || containsQuote == false); i++)
                {
                    char ch = input[i];
                    if (ch == '"')
                        containsQuote = true;
                    else if (ch == ',')
                        containsComma = true;
                }

                if (containsQuote && containsComma)
                    input = input.Replace("\"", "\"\"");

                if (containsComma)
                    return "\"" + input + "\"";
                else
                    return input;
            }
            catch
            {
                throw;
            }
        }

        //Export dgv to .csv
        public bool exportCsvWHKtz(DataGridView dtg1, string path, bool check, string picNam, string pic, int c1, int c2)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //Export header text
                for (int i = c1; i < dtg1.Columns.Count + 1; i++)
                {
                    if (check == false)
                    {
                        sb.Append(dtg1.Columns[i - 1].HeaderText);
                        sb.Append(",");//next sang cột bên cạnh
                    }
                }

                if (check == false)
                {
                    //Thêm PIC
                    sb.Append(pic);
                    sb.Append(",");
                    sb.Append("\n");
                }

                //Export data
                for (int n = 0; n <= dtg1.Rows.Count - 1; n++)
                {
                    for (int j = c2; j < dtg1.Columns.Count; j++)
                    {
                        if (dtg1.Rows[n].Cells[j].Value != null)
                        {
                            sb.Append(dtg1.Rows[n].Cells[j].Value.ToString());
                            sb.Append(",");
                        }
                    }
                    //Thêm PIC
                    sb.Append(picNam);
                    sb.Append(",");
                    sb.Append("\n");
                }

                if (check == false)
                {
                    File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                }
                else
                {
                    File.AppendAllText(path, sb.ToString(), Encoding.UTF8);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        public bool exportCsvNvlSpe(DataGridView dtg1, string path, bool check, string picNam, string pic, int c1, int c2)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //Export header text
                for (int i = c1; i < dtg1.Columns.Count + 1; i++)
                {
                    if (check == false)
                    {
                        sb.Append(dtg1.Columns[i - 1].HeaderText);
                        sb.Append(",");//next sang cột bên cạnh
                    }
                }

                if (check == false)
                {
                    //Thêm PIC
                    sb.Append(pic);
                    sb.Append(",");
                    sb.Append("\n");
                }

                //Export data
                for (int n = 0; n < dtg1.Rows.Count - 1; n++)
                {
                    for (int j = c2; j < dtg1.Columns.Count; j++)
                    {
                        if (dtg1.Rows[n].Cells[j].Value != null)
                        {
                            sb.Append(dtg1.Rows[n].Cells[j].Value.ToString());
                            sb.Append(",");
                        }
                    }
                    //Thêm PIC
                    sb.Append(picNam);
                    sb.Append(",");
                    sb.Append("\n");
                }

                if (check == false)
                {
                    File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                }
                else
                {
                    File.AppendAllText(path, sb.ToString(), Encoding.UTF8);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Export Datagridview to .csv
        public bool exportLogfile(DataGridView dtg1, string path, bool check, int c1, int c2)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                //Export header text
                for (int i = c1; i < dtg1.Columns.Count + 1; i++)
                {
                    if (check == false)
                    {
                        sb.Append(dtg1.Columns[i - 1].HeaderText);
                        sb.Append(",");//next sang cột bên cạnh                   
                    }
                }
                if (check == false)
                {
                    sb.Append("\n");
                }

                //Export data
                for (int n = 0; n <= dtg1.Rows.Count - 1; n++)
                {
                    for (int j = c2; j < dtg1.Columns.Count; j++)
                    {
                        if (dtg1.Rows[n].Cells[j].Value != null)
                        {
                            sb.Append(dtg1.Rows[n].Cells[j].Value.ToString());
                            sb.Append(",");
                        }
                    }
                    sb.Append("\n");
                }

                if (check == false)
                {
                    File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                }
                else
                {
                    File.AppendAllText(path, sb.ToString(), Encoding.UTF8);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }           
        }

        public bool exportLogfilePDxn(DataGridView dtg1, string path, bool check, int c1, int c2)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                //Export header text
                for (int i = c1; i < dtg1.Columns.Count + 1; i++)
                {
                    if (check == false)
                    {
                        sb.Append(dtg1.Columns[i - 1].HeaderText);
                        sb.Append(",");//next sang cột bên cạnh                   
                    }
                }

                if (check == false)
                {
                    sb.Append("\n");
                }

                //Export data
                for (int n = 0; n <= dtg1.Rows.Count - 2; n++)
                {
                    for (int j = c2; j < dtg1.Columns.Count; j++)
                    {
                        if (dtg1.Rows[n].Cells[j].Value != null)
                        {
                            sb.Append(dtg1.Rows[n].Cells[j].Value.ToString());
                            sb.Append(",");
                        }
                    }
                    sb.Append("\n");
                }

                if (check == false)
                {
                    File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                }
                else
                {
                    File.AppendAllText(path, sb.ToString(), Encoding.UTF8);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Export Datagridview to .csv
        public void exportStockKTZZ(DataGridView dtg1, string path, bool check)
        {
            StringBuilder sb = new StringBuilder();

            //Export header text
            for (int i = 1; i < dtg1.Columns.Count + 1; i++)
            {
                if (check == false)
                {
                    sb.Append(dtg1.Columns[i - 1].HeaderText);
                    sb.Append(",");//next sang cột bên cạnh                   
                }
            }
            if (check == false)
            {
                sb.Append("\n");
            }

            //Export data
            for (int n = 0; n <= dtg1.Rows.Count - 1; n++)
            {
                for (int j = 0; j < dtg1.Columns.Count; j++)
                {
                    if (dtg1.Rows[n].Cells[j].Value != null)
                    {
                        sb.Append(dtg1.Rows[n].Cells[j].Value.ToString());
                        sb.Append(",");
                    }                   
                }
                sb.Append("\n");
            }

            if (check == false)
            {
                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            }
            else
            {
                File.AppendAllText(path, sb.ToString(), Encoding.UTF8);
            }
        }

        //Export Datagridview to .txt
        public void ExportTxt(DataGridView dtg1, string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            StringBuilder sb = new StringBuilder();
            //Export header text
            for (int i = 1; i < dtg1.Columns.Count + 1; i++)
            {
                sb.Append(dtg1.Columns[i - 1].HeaderText);
                sb.Append(",");//next sang cột bên cạnh                                                          
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
                        sbb.Append(",");
                    }
                }
                sw.WriteLine(sbb);
            }

            sw.Close();
            fs.Close();            
        }

        //Check ton tai file
        public bool checkExitLog(string dirpath)
        {                   
            if (!File.Exists(dirpath))
            {
                return false;//nếu chưa tồn tại trả về false
            }
            else
            {
                return true;//nếu đã tồn tại trả về true
            }
        }

        //Convert Datagirdview to Database
        public void convDtaGridToDatble(DataGridView dgv, DataTable dt)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if(col.Visible)
                {
                    dt.Columns.Add();
                }
            }
            object[] cellVal = new object[dgv.Columns.Count];
            foreach (DataGridViewRow dr in dgv.Rows)
            {
                for (int i = 0; i < dr.Cells.Count; i++)
                {
                    cellVal[i] = dr.Cells[i].Value;
                }
                dt.Rows.Add(cellVal);
            }
        }

        //Tạo folder lưu trữ
        public void createFoler(string daTim, string name, string str_dtbase)
        {
            string dirpath = str_dtbase + "\\History\\WH\\" + name + "\\" + daTim;

            DirectoryInfo dir = new DirectoryInfo(dirpath);

            //Nếu folder chưa tồn tại mới tạo mới
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        //Check folder đã tồn tại hay chưa
        public bool checkExitPO(string daTim, string name, string str_dtbase)
        {
            string dirpath = str_dtbase + "\\History\\WH\\" + name + "\\" + daTim;

            DirectoryInfo dir = new DirectoryInfo(dirpath);

            if (!dir.Exists)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
