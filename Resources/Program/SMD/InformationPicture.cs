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

namespace ManageMaterialPBA
{
    public partial class InformationPicture : Form
    {
        public bool _adminn;
        public string _strdatabase = string.Empty;

        public InformationPicture(bool adminn, string strdatabase)
        {
            InitializeComponent();
            _adminn = adminn;
            _strdatabase = strdatabase;
        }

        private void InformationPicture_Load(object sender, EventArgs e)
        {
            this.Location = new Point(200, 50);
            this.TopMost = true;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if(_adminn == true)
            {
                OpenFileDialog opDia = new OpenFileDialog();
                opDia.Title = "Update Image Dialog";
                opDia.InitialDirectory = @"C:\";
                opDia.Filter = "Image |*.PNG";
                opDia.FilterIndex = 1;
                string fil_name = "";
                if (opDia.ShowDialog() == DialogResult.OK)
                {
                    fil_name = opDia.FileName;
                }

                if (fil_name != "")
                {
                    string[] str = fil_name.Split('\\');
                    if (File.Exists(_strdatabase + "\\Picture\\" + str[str.Length - 1]))
                    {
                        DialogResult rel = MessageBox.Show("Đã tồn tại Image trong CSDL. Bạn có muốn thay thế?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (rel == DialogResult.OK)
                        {
                            File.Delete(_strdatabase + "\\Picture\\" + str[str.Length - 1]);
                            File.Copy(fil_name, _strdatabase + "\\Picture\\" + str[str.Length - 1]);
                        }
                    }
                    else
                    {
                        File.Copy(fil_name, _strdatabase + "\\Picture\\" + str[str.Length - 1]);
                        MessageBox.Show("Update Image thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }       
            }
        }
    }
}
