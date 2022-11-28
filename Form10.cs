using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_N3
{
    public partial class Frmphong : Form
    {
        PhongEntities1 db;
        public Frmphong()
        {
            InitializeComponent();
        }

        private void Frmphong_Load(object sender, EventArgs e)
        {
            db = new PhongEntities1();
            phongBindingSource.DataSource = db.phongs.ToList();
            label1.Text = "Tài khoản đăng nhập là : \n " + ((Form)this.MdiParent).Controls["label1"].Text;
            if (((Form)this.MdiParent).Controls["label1"].Text != "quản lý")
            {
                btnxoa.Enabled = false;
                btnthem.Enabled = false;
                btnsua.Enabled = false;
                
            }

        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            using(Form11 f11 = new Form11(null))
            {
                if (f11.ShowDialog() == DialogResult.OK)
                    phongBindingSource.DataSource = db.phongs.ToList();
            }    
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (phongBindingSource.Current == null)
                return;
            using (Form11 f11 = new Form11(phongBindingSource.Current as phong))
            {
                if (f11.ShowDialog() == DialogResult.OK)
                    phongBindingSource.DataSource = db.phongs.ToList();
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if(phongBindingSource.Current != null)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db.phongs.Remove(phongBindingSource.Current as phong);
                    phongBindingSource.RemoveCurrent();
                    db.SaveChanges();
                }
            }
            
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            txttim.Clear();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string name = this.txttim.Text;
            using(PhongEntities1 phong = new PhongEntities1())
            {
                var results = phong.phongs.Where(emp => emp.maphong.Contains(name)).ToList();
                this.phongBindingSource.DataSource = results;
            }
        }
    }
}
