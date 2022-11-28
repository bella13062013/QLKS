using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_N3
{
    public partial class Form11 : Form
    {
        PhongEntities1 db;
        public Form11(phong obj)
        {
            InitializeComponent();
            db = new PhongEntities1();
            if (obj == null)
            {
                phongBindingSource.DataSource = new phong();
                db.phongs.Add(phongBindingSource.Current as phong);
            }
            else
            {
                phongBindingSource.DataSource = obj;
                db.phongs.Attach(phongBindingSource.Current as phong);
            }
        }

        

        private void Form11_Load(object sender, EventArgs e)
        {
            ketnoi.moKetNoi();
            loadPhong();
            ketnoi.dongKetNoi();
            
        }
        public void loadPhong()
        {
            string sql = "select * from loaiphong";

            cbxloaiphong.DataSource = ketnoi.GetData(sql);
            cbxloaiphong.DisplayMember = "loaiphong";
            cbxloaiphong.ValueMember = "maloaiphong";
        }

        private void Form11_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(txtmaphong.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phòng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtmaphong.Focus();
                    e.Cancel = true;
                    return;
                }
                db.SaveChanges();
                e.Cancel = false;
            }
            e.Cancel = false;
        }
    }
}
