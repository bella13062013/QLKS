using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Hotel_N3
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {

            // Hiển thị người đăng nhập
            label10.Text = "Tài khoản đăng nhập là : \n " + ((Form)this.MdiParent).Controls["label1"].Text;
            if (((Form)this.MdiParent).Controls["label1"].Text != "quản lý")
            {
                btnxoa.Enabled = false;
                btnthem.Enabled = false;
                btnsua.Enabled = false;
                btnlammoi.Enabled = true;
            }
            ketnoi.moKetNoi();
            loadData();
            loadPhong();
            ketnoi.dongKetNoi();
        }
        public void loadData()
        {
            string sql = "select * from nhanvien";
            data.DataSource = ketnoi.GetData(sql);
        }
        public void loadPhong()
        {
            string sql = "select * from loainhanvien";

            cbxChucVu.DataSource = ketnoi.GetData(sql);
            cbxChucVu.DisplayMember = "chucvu";
            cbxChucVu.ValueMember = "machucvu";

        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = data.CurrentCell.RowIndex;
            txtmanhanvien.Text = data.Rows[i].Cells[0].Value.ToString();
            txttennhanvien.Text = data.Rows[i].Cells[1].Value.ToString();
            string gt = data.Rows[i].Cells[2].Value.ToString();
            if (gt == "True")
            {
                rdNam.Checked = true;
            }
            else
                rdNu.Checked = true;

            cbxChucVu.SelectedValue = data.Rows[i].Cells[3].Value.ToString();
            txtDiaChi.Text = data.Rows[i].Cells[4].Value.ToString();
            txtSDT.Text = data.Rows[i].Cells[5].Value.ToString();
            datevlam.Text = data.Rows[i].Cells[6].Value.ToString();

            diachianh.Text = data.Rows[i].Cells[7].Value.ToString();

            string pathAnh = ConfigurationManager.AppSettings.Get("duongdananh") + "\\" + diachianh.Text;
            if (File.Exists(pathAnh))
            {
                pichienanh.Image = Image.FromFile(pathAnh);
            }
            else
                pichienanh.Image = null;
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            string sql = "Insert into nhanvien values(@manhanvien,@tennhanvien,@gioitinh,@machucvu,@diachi,@sdt,@ngayvaolam,@picture)";

            string[] name = { "@manhanvien", "@tennhanvien", "@gioitinh", "@machucvu", "@diachi", "@sdt", "@ngayvaolam", "@picture" };

            bool gt = rdNam.Checked == true ? true : false;

            object[] value = { txtmanhanvien.Text,txttennhanvien.Text,gt,cbxChucVu.SelectedValue,txtDiaChi.Text,txtSDT.Text,datevlam.Value,diachianh.Text };

            ketnoi.moKetNoi();
            ketnoi.updateData(sql, value, name, 8);
            loadData();
            ketnoi.dongKetNoi();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            string sql = string.Format("Update nhanvien set manhanvien = @manhanvien, tennhanvien = @tennhanvien, gioitinh = @gioitinh, machucvu = @machucvu, diachi = @diachi, sdt = @sdt, ngayvaolam = @ngayvaolam , picture = @picture where manhanvien ='{0}'", txtmanhanvien.Text);

            string[] name = { "@manhanvien", "@tennhanvien", "@gioitinh", "@machucvu", "@diachi", "@sdt", "@ngayvaolam", "@picture" };

            bool gt = rdNam.Checked == true ? true : false;

            object[] value = { txtmanhanvien.Text, txttennhanvien.Text, gt, cbxChucVu.SelectedValue, txtDiaChi.Text, txtSDT.Text, datevlam.Value, diachianh.Text };

            ketnoi.moKetNoi();
            ketnoi.updateData(sql, value, name, 8);
            loadData();
            ketnoi.dongKetNoi();
        }

        private void btnlammoi_Click(object sender, EventArgs e)
        {
            txtmanhanvien.Clear();
            txttennhanvien.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();

            txttennhanvien.Focus();
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
                    string sql = string.Format("delete from nhanvien where manhanvien ='{0}'", ma);
                    object[] value = { };
                    string[] name = { };

                    ketnoi.moKetNoi();
                    ketnoi.updateData(sql, value, name, 0);
                    loadData();
                    ketnoi.dongKetNoi();
                }
            }
        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            string sql = string.Format("select * from nhanvien where manhanvien like N'%{0}%'", txttimkiem.Text);
            data.DataSource = ketnoi.GetData(sql);
        }

        private void btnchonanh_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();
            if (oFile.ShowDialog() == DialogResult.OK)
            {
                pichienanh.Image = Image.FromFile(oFile.FileName);
                diachianh.Text = Path.GetFileName(oFile.FileName);
            }
        }

        private void txttimkiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void txttimkiem_Click(object sender, EventArgs e)
        {
            txttimkiem.Clear();
        }
    }
}
