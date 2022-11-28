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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            // Hiển thị người đăng nhập
            label9.Text = "Tài khoản đăng nhập là : \n " + ((Form)this.MdiParent).Controls["label1"].Text;
            if (((Form)this.MdiParent).Controls["label1"].Text != "quản lý")
            {
                btnxoa.Enabled = true;
                btnthem.Enabled = true;
                btnsua.Enabled = true;
                btnlammoi.Enabled = true;
            }
            ketnoi.moKetNoi();
            loadData();
            ketnoi.dongKetNoi();
        }
        public void loadData()
        {
            string sql = "select * from db_DatPhong";
            data.DataSource = ketnoi.GetData(sql);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            string sql = "Insert into db_DatPhong values(@madatphong,@maphong,@tenkh,@cmnd,@tenphong,@ngayden,@ngaydi,@tinhtrang)";
            string[] name = { "@madatphong", "@maphong", "@tenkh", "@cmnd", "@tenphong", "@ngayden", "@ngaydi", "@tinhtrang" };

            object[] value = { txtmadatphong.Text, txtmaphong.Text, txttenkhachhang.Text, txtcmnd.Text, txtenphong.Text, dateden.Value, datedi.Value, txttinhtrang.Text };

            ketnoi.moKetNoi();
            ketnoi.updateData(sql, value, name, 8);
            loadData();
            ketnoi.dongKetNoi();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            string sql = string.Format("Update db_Datphong set madatphong = @madatphong, maphong = @maphong, tenkh = @tenkh, cmnd = @cmnd, tenphong = @tenphong, ngayden = @ngayden, ngaydi = @ngaydi, tinhtrang = @tinhtrang where madatphong ='{0}'", txtmadatphong.Text);

            string[] name = { "@madatphong", "@maphong", "@tenkh", "@cmnd", "@tenphong", "@ngayden", "@ngaydi", "@tinhtrang" };

            object[] value = { txtmadatphong.Text, txtmaphong.Text, txttenkhachhang.Text, txtcmnd.Text, txtenphong.Text, dateden.Value, datedi.Value, txttinhtrang.Text };

            ketnoi.moKetNoi();
            ketnoi.updateData(sql, value, name, 8);
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
                    string sql = string.Format("delete from db_DatPhong where madatphong ='{0}'", ma);
                    object[] value = { };
                    string[] name = { };

                    ketnoi.moKetNoi();
                    ketnoi.updateData(sql, value, name, 0);
                    loadData();
                    ketnoi.dongKetNoi();
                }
            }
        }
            private void btnlammoi_Click(object sender, EventArgs e)
            {
                txtmadatphong.Clear();
                txtmaphong.Clear();
                txttenkhachhang.Clear();
                txtcmnd.Clear();
                txtenphong.Clear();
                txttinhtrang.Clear();

                txtmadatphong.Focus();
                ketnoi.moKetNoi();
                loadData();
                ketnoi.dongKetNoi();
            }

        private void txttimkiem_Click(object sender, EventArgs e)
        {
            txttimkiem.Clear();
        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            string sql = string.Format("select * from db_DatPhong where madatphong like N'%{0}%'", txttimkiem.Text);
            data.DataSource = ketnoi.GetData(sql);
        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = data.CurrentCell.RowIndex;
            txtmadatphong.Text = data.Rows[i].Cells[0].Value.ToString();
            txtmaphong.Text = data.Rows[i].Cells[1].Value.ToString();
            txttenkhachhang.Text = data.Rows[i].Cells[2].Value.ToString();
            txtcmnd.Text = data.Rows[i].Cells[3].Value.ToString();
            txtenphong.Text = data.Rows[i].Cells[4].Value.ToString();
            dateden.Text = data.Rows[i].Cells[5].Value.ToString();
            datedi.Text = data.Rows[i].Cells[6].Value.ToString();
            txttinhtrang.Text = data.Rows[i].Cells[7].Value.ToString();


        }
    }
    }

