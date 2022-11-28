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
using System.Configuration;
namespace Hotel_N3
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Hiển thị người đăng nhập
            label9.Text = "Tài khoản đăng nhập là : \n " + ((Form)this.MdiParent).Controls["label1"].Text;
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
            string sql = "select * from khachhang";
            dataGridView1.DataSource = ketnoi.GetData(sql);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }
        //thêm khách hàng
        private void btnthem_Click(object sender, EventArgs e)
        {
            string sql = "Insert into khachhang values(@makhachhang,@tenkhachhang,@ngaysinh,@gioitinh,@quequan,@soCMND,@sdt,@quoctich)";
            string[] name = { "@makhachhang", "@tenkhachhang", "@ngaysinh", "@gioitinh", "@quequan", "@soCMND", "@sdt", "@quoctich" };
            bool gt = rdNam.Checked == true ? true : false;

            object[] value = { txtmaKH.Text, txtKH.Text, dtime1.Value, gt, txtquequan.Text, txtcmnd.Text, txtsdt.Text, txtqtich.Text };

            ketnoi.moKetNoi();
            ketnoi.updateData(sql, value, name, 8);
            loadData();
            ketnoi.dongKetNoi();
        }
        // sửa thông tin khách hàng
        private void btnsua_Click(object sender, EventArgs e)
        {
            string sql = string.Format("Update khachhang set makhachhang = @makhachhang, tenkhachhang = @tenkhachhang, ngaysinh = @ngaysinh, gioitinh = @gioitinh, quequan = @quequan, soCMND = @soCMND, sdt = @sdt, quoctich = @quoctich where makhachhang ='{0}'",txtmaKH.Text);
           
            string[] name = { "@makhachhang", "@tenkhachhang", "@ngaysinh", "@gioitinh", "@quequan", "@soCMND", "@sdt", "@quoctich" };
            
            bool gt = rdNam.Checked == true ? true : false;

            object[] value = { txtmaKH.Text, txtKH.Text, dtime1.Value, gt, txtquequan.Text, txtcmnd.Text, txtsdt.Text, txtqtich.Text };

            ketnoi.moKetNoi();
            ketnoi.updateData(sql, value, name, 8);
            loadData();
            ketnoi.dongKetNoi();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        // xóa khách hàng
        private void btnxoa_Click(object sender, EventArgs e)
        {
            int i = dataGridView1.CurrentCell.RowIndex;
            if(i >= 0)
            {
                DialogResult dr = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel);
                if(dr == DialogResult.OK)
                {
                    string ma = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string sql = string.Format("delete from khachhang where makhachhang ='{0}'", ma);
                    object[] value = { };
                    string[] name = { };

                    ketnoi.moKetNoi();
                    ketnoi.updateData(sql, value, name, 0);
                    loadData();
                    ketnoi.dongKetNoi();
                }
            }
        }
        // làm mới các ô nhập
        private void btnlammoi_Click(object sender, EventArgs e)
        {
            txtKH.Clear();
            txtmaKH.Clear();
            txtquequan.Clear();
            txtcmnd.Clear();
            txtsdt.Clear();
            txtqtich.Clear();

            txtKH.Focus();
            ketnoi.moKetNoi();
            loadData();
            ketnoi.dongKetNoi();
        }
        // Tìm kiếm khách hàng
        private void btntimkiem_Click(object sender, EventArgs e)
        {
            string sql = string.Format("select * from khachhang where makhachhang like N'%{0}%'", txttimkiem.Text);
            dataGridView1.DataSource = ketnoi.GetData(sql);
        }
        // Đẩy thông tin khách hàng lên các ô nhập
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dataGridView1.CurrentCell.RowIndex;
            txtmaKH.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
            txtKH.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            dtime1.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            string gt = dataGridView1.Rows[i].Cells[3].Value.ToString();
            if (gt == "True")
            {
                rdNam.Checked = true;
            }
            else
                rdNu.Checked = true;

            txtquequan.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            txtcmnd.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
            txtsdt.Text = dataGridView1.Rows[i].Cells[6].Value.ToString();
            txtqtich.Text = dataGridView1.Rows[i].Cells[7].Value.ToString();

        }

        private void txttimkiem_Click(object sender, EventArgs e)
        {
            txttimkiem.Clear();
        }
    }
}
