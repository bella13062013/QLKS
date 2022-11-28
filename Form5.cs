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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // Hiển thị người đăng nhập
            label5.Text = "Tài khoản đăng nhập là : \n " + ((Form)this.MdiParent).Controls["label1"].Text;
            if (((Form)this.MdiParent).Controls["label1"].Text != "quản lý")
            {
                btnxoa.Enabled = false;
                btnthem.Enabled = false;
                btnsua.Enabled = false;
                btnlammoi.Enabled = true;
            }
            ketnoi.moKetNoi();
            loadData();
            ketnoi.dongKetNoi();
        }
        public void loadData()
        {
            string sql = "select * from dichvu";
            data.DataSource = ketnoi.GetData(sql);

        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = data.CurrentCell.RowIndex;
            txtmadichvu.Text = data.Rows[i].Cells[0].Value.ToString();
            txttendichvu.Text = data.Rows[i].Cells[1].Value.ToString();
            txtdongia.Text = data.Rows[i].Cells[2].Value.ToString();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            string sql = "Insert into dichvu values(@madv,@tendv,@giadv)";

            string[] name = { "@madv", "@tendv", "@giadv" };

            object[] value = { txtmadichvu.Text, txttendichvu.Text, txtdongia.Text };

            ketnoi.moKetNoi();
            ketnoi.updateData(sql, value, name, 3);
            loadData();
            ketnoi.dongKetNoi();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            string sql = string.Format("Update dichvu set madv = @madv, tendv = @tendv, giadv = @giadv where madv ='{0}'", txtmadichvu.Text);

            string[] name = { "@madv", "@tendv", "@giadv" };

            object[] value = { txtmadichvu.Text, txttendichvu.Text, txtdongia.Text };

            ketnoi.moKetNoi();
            ketnoi.updateData(sql, value, name, 3);
            loadData();
            ketnoi.dongKetNoi();
        }

        private void btnlammoi_Click(object sender, EventArgs e)
        {
            txtmadichvu.Clear();
            txttendichvu.Clear();
            txtdongia.Clear();
            txttendichvu.Focus();

            ketnoi.moKetNoi();
            loadData();
            ketnoi.dongKetNoi();

        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            int i = data.CurrentCell.RowIndex;
            if (i >= 0)
            {
                DialogResult dr = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    string ma = data.Rows[i].Cells[0].Value.ToString();
                    string sql = string.Format("delete from dichvu where madv ='{0}'", ma);
                    object[] value = { };
                    string[] name = { };

                    ketnoi.moKetNoi();
                    ketnoi.updateData(sql, value, name, 0);
                    loadData();
                    ketnoi.dongKetNoi();
                }
            }
        }

        private void txttimkiem_Click(object sender, EventArgs e)
        {
            txttimkiem.Clear();

        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            string sql = string.Format("select * from dichvu where madv like N'%{0}%'", txttimkiem.Text);
            data.DataSource = ketnoi.GetData(sql);
        }
    }
}