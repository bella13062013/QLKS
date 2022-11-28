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
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }
        ketnoi conn = new ketnoi();
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            // Tạo hóa đơn với tên vừa chọn
           // conn.ThucThiDL("insert into hoadon values('" + txtmahoadon.Text.Trim()+"',N'" + txttenkh.Text.Trim() + "',N'" + cbxsophong.SelectedValue + "',N'" + txtgiaphong.Text + "',N'" + cbxloaithe.SelectedValue + "',N'" + date1.Value + "'," + 0 + "')");
            conn.ThucThiDL("insert into hoadon values ('"+ txtmahoadon.Text.Trim() + "',N'" + txttenkh.Text.Trim() + "','" + cbxsophong.SelectedValue +"','" + txtgiaphong.Text + "','" + cbxloaithe.SelectedValue + "','" + date1.Value + "','" + 0 + "')");
            // Thực thi tạo từng chi tiết hóa đơn với các dịch vụ vừa chọn
            for (int i=0;i<listBox2.Items.Count;i++)// quét các item trong listbox2
            {
                // Lấy mã dịch vụ tương ứng với tên dịch vụ (lưu ý tên dịch vụ không được trùng)
                string madv1 = conn.XemDL("select madv from dichvu where tendv=N'" + listBox2.Items[i].ToString().Trim() + "'").Rows[0][0].ToString().Trim();
                //Tính tiền mỗi dịch vụ tương ứng với giá thành của nó
                string tien = conn.XemDL("select giadv from dichvu where madv='" + madv1  + "'").Rows[0][0].ToString().Trim();
                //thực hiện lệnh thêm cthd cho sản phẩm vừa chọn bao gồm: macthd, mahd, masp, soluong, tien
                conn.ThucThiDL("insert into hoadonchitiet values ('"+ i.ToString() + "','" + txtmahoadon.Text.ToString().Trim() + "','" + madv1 + "','" + 1 + "','" + Convert.ToInt32(tien) + "')");
            }
            // Lấy mã hóa đơn để truyền sang form 9
            Form9.mahoadon = txtmahoadon.Text.Trim();
            Form9 f9 = new Form9();
            f9.ShowDialog();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            // Thêm tên dịch vụ của các dịch vụ có vào Listbox 1
            string sql = "select * from dichvu";
            for(int i=0;i<conn.XemDL(sql).Rows.Count;i++)
            {
                listBox1.Items.Add(conn.XemDL(sql).Rows[i][1].ToString());
            }
            ketnoi.moKetNoi();
            loadPhong();
            loadTT();
            ketnoi.dongKetNoi();
        }
        public void loadPhong()
        {
            string sql = "select * from phong";

            cbxsophong.DataSource = ketnoi.GetData(sql);
            cbxsophong.DisplayMember = "maphong";
            cbxsophong.ValueMember = "maphong";
        }
        public void loadTT()
        {
            string sql = "select * from loaithanhtoan";

            cbxloaithe.DataSource = ketnoi.GetData(sql);
            cbxloaithe.DisplayMember = "loaithanhtoan";
            cbxloaithe.ValueMember = "loaithanhtoan";
        }
        private void btnchon_Click(object sender, EventArgs e)
        {
            while (listBox1.SelectedItems.Count > 0)//Nếu có lựa chọn 1 item
            {
                listBox2.Items.Add(listBox1.SelectedItem);//Thêm item đó bên Listbox2 
                listBox1.Items.Remove(listBox1.SelectedItem);//xóa item vừa thêm bên listbox2 ở listbox1
            }
        }

        private void btnbo_Click(object sender, EventArgs e)
        {
            //Ngược lại
            while (listBox2.SelectedItems.Count > 0)
            {
                listBox1.Items.Add(listBox2.SelectedItem);
                listBox2.Items.Remove(listBox2.SelectedItem);
            }
        }

        private void btnlammoi_Click(object sender, EventArgs e)
        {
            //Thực hiện lại từ đầu
            //Xóa trắng dữ liệu trên 2 listbox
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            string sql = "select * from dichvu";

            for (int i = 0; i < conn.XemDL(sql).Rows.Count; i++)
            {
                listBox1.Items.Add(conn.XemDL(sql).Rows[i][1].ToString());
            }
        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            // Quản lý cách tìm kiếm
            listBox1.Items.Clear();
            string cachtim = cbxcachtim.SelectedItem.ToString();
            switch(cachtim)
            {
                case "Mã dịch vụ":
                    string sql1 = "select * from dichvu where madv like '%" + txttimkiem.Text.Trim() + "%'";
                    for (int i = 0;i<conn.XemDL(sql1).Rows.Count;i++)
                    {
                        listBox1.Items.Add(conn.XemDL(sql1).Rows[i][1].ToString().Trim());
                    }
                    break;
                case "Tên dịch vụ":
                    string sql = "select * from dichvu where tendv like N'%" + txttimkiem.Text.Trim() + "%'";
                    for (int i = 0; i < conn.XemDL(sql).Rows.Count; i++)
                    {
                        listBox1.Items.Add(conn.XemDL(sql).Rows[i][1].ToString().Trim());
                    }
                    break;

                    default:
                    MessageBox.Show("Bạn chưa chọn cách tìm kiếm." + cachtim);
                    break;

            }
        }

        private void cbxcachtim_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
