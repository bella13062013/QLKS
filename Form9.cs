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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }
        // Khai báo mã hóa đơn để có thể sử dụng được truyền từ form 8
        public static string mahoadon;
        ketnoi conn = new ketnoi();
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }
        Bitmap bmp;
        private void btnIn_Click(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            bmp = new Bitmap(this.Size.Width, this.Size.Height, g);
            Graphics mg = Graphics.FromImage(bmp);
            mg.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
            printPreviewDialog1.ShowDialog();

        }
        public string macthd;

        private void Form9_Load(object sender, EventArgs e)
        {
            /*using (HoaDonEntities db = new HoaDonEntities())
            {
                hoadonchitietBindingSource.DataSource = db.hoadonchitiets.ToList();
            }*/
            // Load danh sach dịch vụ lên datagridview
            dataGridView.DataSource = conn.XemDL("select machitiethd,tendv,soluong,thanhtien from hoadon,hoadonchitiet,dichvu where hoadon.mahoadon=hoadonchitiet.mahoadon " +
                "and hoadonchitiet.madichvu=dichvu.madv and hoadon.mahoadon= '" + mahoadon + "'");
            // Tính tổng tiền của hóa đơn
            txttongtien.Text = conn.XemDL("select sum(thanhtien) as thanhtien from hoadonchitiet where mahoadon = '" + mahoadon + "'").Rows[0][0].ToString();

            // Blinding dữ liệu ta textbox
            txtmachitiet.DataBindings.Clear();
            txttendichvu.DataBindings.Clear();
            txtsoluong.DataBindings.Clear();
            txtmachitiet.DataBindings.Add("Text", dataGridView.DataSource, "machitiethd");
            txttendichvu.DataBindings.Add("Text", dataGridView.DataSource, "tendv");
            txtsoluong.DataBindings.Add("Text", dataGridView.DataSource, "soluong");
        }
            private void btnluu_Click(object sender, EventArgs e)
        {
            //Lưu lại hóa đơn với tổng tiền vừa mới tính
            conn.ThucThiDL("update hoadon set tongtien='" + Convert.ToInt32(txttongtien.Text.Trim()) + "'");
            MessageBox.Show("Lưu thành công");
        }
        //đề phòng bất trắc blinding dữ liệu nếu muốn thay đổi cthd
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtmachitiet.DataBindings.Clear();
            txttendichvu.DataBindings.Clear();
            txtsoluong.DataBindings.Clear();
            txtmachitiet.DataBindings.Add("Text", dataGridView.DataSource, "machitiethd");
            txttendichvu.DataBindings.Add("Text", dataGridView.DataSource, "tendv");
            txtsoluong.DataBindings.Add("Text", dataGridView.DataSource, "soluong");
        }

        private void btncapnhat_Click(object sender, EventArgs e)
        {
            //lấy mã dv tương ứng với macthd, mahd, ténp
            string madv = conn.XemDL("select madv from dichvu where tendv=N'"+ txttendichvu.Text.Trim()+"'").Rows[0][0].ToString().Trim();
            //Tính tiền của dịch vụ khi đã thay đổi số lượng
            float tien = Convert.ToInt32(conn.XemDL("select giadv from dichvu where madv='" + madv + "'").Rows[0][0].ToString().Trim()) * Convert.ToInt32(txtsoluong.Text.Trim());
            //thực thi câu lệnh update số lượng và tiền của dịch vụ
            conn.ThucThiDL("update hoadonchitiet set soluong ='" + Convert.ToInt32(txtsoluong.Text.ToString()) + "', thanhtien='" + tien + "' " +
                "where machitiethd ='" + txtmachitiet.Text.ToString() + "' and  mahoadon ='" + mahoadon + "' and  madichvu ='" + madv + "'");
            //Load lại dữ liệu
            dataGridView.DataSource = conn.XemDL("select machitiethd, tendv, soluong, thanhtien from hoadon, hoadonchitiet, " +
                "dichvu where hoadon.mahoadon=hoadonchitiet.mahoadon and" +
                " hoadonchitiet.madichvu=dichvu.madv and hoadon.mahoadon='" + mahoadon + "'");
            //Tính lại tổng tiền
            txttongtien.Text = conn.XemDL("select sum(thanhtien) as tongtien from hoadonchitiet where mahoadon='" + mahoadon + "'").Rows[0][0].ToString();
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
