using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Hotel_N3.Class;
//using COMExcel = Microsoft.Office.Interop.Excel;

namespace Hotel_N3
{
    public partial class HDPhong : Form
    {
        public HDPhong()
        {
            InitializeComponent();
        }

        DataTable chitiethoadon;

        private void HDPhong_Load(object sender, EventArgs e)
        {
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            btnIn.Enabled = false;
            txtMaHD.ReadOnly = true;
            txtTenNV.ReadOnly = true;
            txttenKH.ReadOnly = true;
            txtTenPhong.ReadOnly = true;
            txtTenDV.ReadOnly = true;
            txtDonGia.ReadOnly = true;
            txtThanhTien.ReadOnly = true;
            txtTongtien.ReadOnly = true;
            txtgiamgia.Text = "0";
            txtTongtien.Text = "0";
            Functions.FillCombo("SELECT makhachhang, tenkhachhang FROM khachhang", cbxMaKH, "makhachhang", "makhachhang");
            cbxMaKH.SelectedIndex = -1;

            Functions.FillCombo("SELECT manhanvien, tennhanvien FROM nhanvien", cbxMaNV, "manhanvien", "tenkhachhang");
            cbxMaNV.SelectedIndex = -1;

            Functions.FillCombo("SELECT maphong, tenphong FROM phong", cbxMaPhong, "maphong", "tenkhachhang");
            cbxMaPhong.SelectedIndex = -1;

            Functions.FillCombo("SELECT madv, tendv FROM dichvu", cbxMaDV, "madv", "madv");
            cbxMaDV.SelectedIndex = -1;
            //Hiển thị thông tin của một hóa đơn được gọi từ form tìm kiếm
            if (txtMaHD.Text != "")
            {
                LoadInfoHoaDon();
                btnHuy.Enabled = true;
                btnIn.Enabled = true;
            }
            LoadDataGridView();
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.MaDichVu, b.tendv, a.SoLuong, b.giadv, a.GiamGia, a.ThanhTien FROM chitiethoadon AS a, dichvu AS b WHERE a.MaHoaDon = N'" + txtMaHD.Text + "' AND a.MaDichVu=b.madv";
            chitiethoadon = Functions.GetDataToTable(sql);
            dgvHDPhong.DataSource = chitiethoadon;
            dgvHDPhong.Columns[0].HeaderText = "Mã dịch vụ";
            dgvHDPhong.Columns[1].HeaderText = "Tên dịch vụ";
            dgvHDPhong.Columns[2].HeaderText = "Số lượng";
            dgvHDPhong.Columns[3].HeaderText = "Đơn giá";
            dgvHDPhong.Columns[4].HeaderText = "Giảm giá";
            dgvHDPhong.Columns[5].HeaderText = "Thành tiền";

            dgvHDPhong.Columns[0].Width = 80;
            dgvHDPhong.Columns[1].Width = 130;
            dgvHDPhong.Columns[2].Width = 80;
            dgvHDPhong.Columns[3].Width = 90;
            dgvHDPhong.Columns[4].Width = 90;
            dgvHDPhong.Columns[5].Width = 90;

            dgvHDPhong.AllowUserToAddRows = false;
            dgvHDPhong.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        // Nạp chi tiết hóa đơn
        private void LoadInfoHoaDon()
        {
            string str;
            str = "SELECT NgayLap FROM hoadonphong WHERE MaHoaDon = N'" + txtMaHD.Text + "'";
            dtimeNLap.Value = DateTime.Parse(Functions.GetFieldValues(str));

            str = "SELECT MaNhanVien FROM hoadonphong WHERE MaHoaDon = N'" + txtMaHD.Text + "'";
            cbxMaNV.SelectedValue = Functions.GetFieldValues(str);

            str = "SELECT MaKhachHang FROM hoadonphong WHERE MaHoaDon = N'" + txtMaHD.Text + "'";
            cbxMaKH.SelectedValue = Functions.GetFieldValues(str);

            str = "SELECT MaPhong FROM hoadonphong WHERE MaHoaDon = N'" + txtMaHD.Text + "'";
            cbxMaPhong.SelectedValue = Functions.GetFieldValues(str);

            str = "SELECT TongTien FROM hoadonphong WHERE MaHoaDon = N'" + txtMaHD.Text + "'";
            txtTongtien.Text = Functions.GetFieldValues(str);//số sang chữ 

            lbBangChu.Text = "Bằng chữ: " + Functions.ChuyenSoSangChuoi(Double.Parse(txtTongtien.Text));
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            btnHuy.Enabled = false;
            btnLuu.Enabled = true;
            btnIn.Enabled = false;
            btnThem.Enabled = false;
            ResetValues();
            txtMaHD.Text = Functions.CreateKey("H");
            LoadDataGridView();
        }
        // Nạp các giá trị control về mặc định
        private void ResetValues()
        {
            txtMaHD.Text = "";
            dtimeNLap.Value = DateTime.Now;
            cbxMaKH.Text = "";
            cbxMaPhong.Text = "";
            txtTongtien.Text = "0";
            lbBangChu.Text = "Bằng chữ: ";
            cbxMaDV.Text = "";
            txtSoLuong.Text = "";
            txtgiamgia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            double sl, SLcon, tong, Tongmoi;
            sql = "SELECT MaHoaDon FROM hoadonphong WHERE MaHoaDon=N'" + txtMaHD.Text + "'";
            if (!Functions.CheckKey(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HDBan được sinh tự động do đó không có trường hợp trùng khóa
                /*if (txtNgayLap.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập ngày bán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNgayLap.Focus();
                    return;
                }*/
                if (cbxMaNV.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbxMaNV.Focus();
                    return;
                }
                if (cbxMaKH.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbxMaKH.Focus();
                    return;
                }
                if (cbxMaPhong.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập phong", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbxMaPhong.Focus();
                    return;
                }
                sql = "INSERT INTO hoadonphong(MaHoaDon, NgayLap, MaNhanVien, MaKhachHang, MaPhong, TongTien) VALUES (N'" + txtMaHD.Text.Trim() + "','" +
                        dtimeNLap.Value + "',N'" + cbxMaNV.SelectedValue + "',N'" +
                        cbxMaKH.SelectedValue + "',N'" + cbxMaPhong.SelectedValue + "'," + txtTongtien.Text + ")";
                Functions.RunSQL(sql);
            }
            // Lưu thông tin của các mặt hàng
            if (cbxMaDV.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbxMaDV.Focus();
                return;
            }
            if ((txtSoLuong.Text.Trim().Length == 0) || (txtSoLuong.Text == "0"))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            if (txtgiamgia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập giảm giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtgiamgia.Focus();
                return;
            }
            sql = "SELECT MaDichVu FROM chitiethoadon WHERE MaDichVU=N'" + cbxMaNV.SelectedValue + "' AND MaHoaDon = N'" + txtMaHD.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã dịch vụ này đã có, bạn phải nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetValuesHang();
                cbxMaDV.Focus();
                return;
            }
            // Kiểm tra xem số lượng dv trong hotel còn đủ để cung cấp không?
            sl = Convert.ToDouble(Functions.GetFieldValues("SELECT soluong FROM dichvu WHERE madv = N'" + cbxMaDV.SelectedValue + "'"));
            if (Convert.ToDouble(txtSoLuong.Text) > sl)
            {
                MessageBox.Show("Số lượng dịch vụ còn là " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Text = "";
                txtSoLuong.Focus();
                return;
            }
            sql = "INSERT INTO chitiethoadon(MaHoaDon,MaDichVu,DonGia,SoLuong,GiamGia,ThanhTien) VALUES(N'" + txtMaHD.Text.Trim() + "',N'" + cbxMaDV.SelectedValue + "'," + txtDonGia.Text + "," + txtSoLuong.Text + "," + txtgiamgia.Text + "," + txtThanhTien.Text + ")";
            Functions.RunSQL(sql);
            LoadDataGridView();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl - Convert.ToDouble(txtSoLuong.Text);
            sql = "UPDATE dichvu SET soluong =" + SLcon + " WHERE madv= N'" + cbxMaDV.SelectedValue + "'";
            Functions.RunSQL(sql);
            // Cập nhật lại tổng tiền cho hóa đơn bán
            tong = Convert.ToDouble(Functions.GetFieldValues("SELECT TongTien FROM hoadonphong WHERE MaHoaDon = N'" + txtMaHD.Text + "'"));
            Tongmoi = tong + Convert.ToDouble(txtThanhTien.Text);

            sql = "UPDATE hoadonphong SET TongTien=" + Tongmoi + " WHERE MaHoaDon= N'" + txtMaHD.Text + "'";
            Functions.RunSQL(sql);

            txtTongtien.Text = Tongmoi.ToString();
            lbBangChu.Text = "Bằng chữ: " + Functions.ChuyenSoSangChuoi(Double.Parse(Tongmoi.ToString()));
            ResetValuesHang();
            btnHuy.Enabled = true;
            btnThem.Enabled = true;
            btnIn.Enabled = true;
        }
        // Bổ sung reset phần dịch vụ
        private void ResetValuesHang()
        {
            cbxMaDV.Text = "";
            txtSoLuong.Text = "";
            txtgiamgia.Text = "0";
            txtThanhTien.Text = "0";
        }

        private void dgvHDPhong_DoubleClick(object sender, EventArgs e)
        {
            string Madvxoa, Madvxoa2, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            if (chitiethoadon.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                //Xóa hàng và cập nhật lại số lượng hàng 
                Madvxoa = dgvHDPhong.CurrentRow.Cells["MaDichVu"].Value.ToString();
                Madvxoa2 = dgvHDPhong.CurrentRow.Cells["MaDichVu"].Value.ToString();

                SoLuongxoa = Convert.ToDouble(dgvHDPhong.CurrentRow.Cells["SoLuong"].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvHDPhong.CurrentRow.Cells["ThanhTien"].Value.ToString());
                sql = "DELETE chitiethoadon WHERE MaHoaDon=N'" + txtMaHD.Text + "' AND MaDichVu = N'" + Madvxoa + "'";
                Functions.RunSQL(sql);
                // Cập nhật lại số lượng cho các mặt hàng
                sl = Convert.ToDouble(Functions.GetFieldValues("SELECT SoLuong FROM dichvu WHERE madv = N'" + Madvxoa2 + "'"));
                slcon = sl + SoLuongxoa;
                sql = "UPDATE dichvu SET soluong =" + slcon + " WHERE madv= N'" + Madvxoa2 + "'";
                Functions.RunSQL(sql);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                tong = Convert.ToDouble(Functions.GetFieldValues("SELECT TongTien FROM hoadonphong WHERE MaHoaDon = N'" + txtMaHD.Text + "'"));
                tongmoi = tong - ThanhTienxoa;
                sql = "UPDATE hoadonphong SET TongTien =" + tongmoi + " WHERE MaHoaDon = N'" + txtMaHD.Text + "'";
                Functions.RunSQL(sql);
                txtTongtien.Text = tongmoi.ToString();
                lbBangChu.Text = "Bằng chữ: " + Functions.ChuyenSoSangChuoi(Double.Parse(tongmoi.ToString()));
                LoadDataGridView();
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT MaDichVu,SoLuong FROM chitiethoadon WHERE MaHoaDon = N'" + txtMaHD.Text + "'";
                DataTable dichvu = Functions.GetDataToTable(sql);
                for (int dv = 0; dv <= dichvu.Rows.Count - 1; dv++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(Functions.GetFieldValues("SELECT soluong FROM dichvu WHERE madv = N'" + dichvu.Rows[dv][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(dichvu.Rows[dv][1].ToString());
                    slcon = sl + slxoa;
                    sql = "UPDATE dichvu SET soluong =" + slcon + " WHERE madv= N'" + dichvu.Rows[dv][0].ToString() + "'";
                    Functions.RunSQL(sql);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE chitiethoadon WHERE MaHoaDon=N'" + txtMaHD.Text + "'";
                Functions.RunSqlDel(sql);

                //Xóa hóa đơn
                sql = "DELETE hoadonphong WHERE MaHoaDon=N'" + txtMaHD.Text + "'";
                Functions.RunSqlDel(sql);
                ResetValues();
                LoadDataGridView();
                btnHuy.Enabled = false;
                btnIn.Enabled = false;
            }
        }
        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtgiamgia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtgiamgia.Text);
            if (txtDonGia.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGia.Text);
            tt = sl * dg - sl * dg * gg / 100; ;
            txtThanhTien.Text = tt.ToString();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            // Khởi động chương trình Excel
           /* COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;//Ô trong excel
            string sql;
            int hang = 0, cot = 0;
            DataTable tblThongtinHD, tblThongtinHang;
            //exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exBook = exApp.Workbooks.Add(COMExcel.XlWebFormatting.xlWebFormattingAll);
            exSheet = exBook.Worksheets[1];
            // Định dạng chung
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:Z300"].Font.Name = "Times new roman"; //Font chữ
            exRange.Range["A1:B3"].Font.Size = 10;
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
            exRange.Range["A1:A1"].ColumnWidth = 7;
            exRange.Range["B1:B1"].ColumnWidth = 15;
            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "Blue Sea Hotel";
            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "Quy Nhơn - Bình Định";
            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại: (04)09388292";
            exRange.Range["C2:E2"].Font.Size = 16;
            exRange.Range["C2:E2"].Font.Bold = true;
            exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C2:E2"].MergeCells = true;
            exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:E2"].Value = "HÓA ĐƠN KHÁCH SẠN ";
            // Biểu diễn thông tin chung của hóa đơn bán
            sql = "SELECT a.MaHoaDon, a.NgayLap, a.TongTien, b.tenkhachhang, c.tennhanvien , d.tenphong ,d.giaphong FROM hoadonphong AS a, khachhang AS b, nhanvien AS c , phong as d WHERE a.MaHoaDon = N'" + txtMaHD.Text + "' AND a.MaKhachHang = b.makhachhang AND a.MaNhanVien = c.manhanvien AND a.MaPhong = d.maphong";
            tblThongtinHD = Functions.GetDataToTable(sql);
            exRange.Range["B6:C9"].Font.Size = 12;
            exRange.Range["B6:B6"].Value = "Mã hóa đơn:";
            exRange.Range["C6:E6"].MergeCells = true;
            exRange.Range["C6:E6"].Value = tblThongtinHD.Rows[0][0].ToString();
            exRange.Range["B7:B7"].Value = "Khách hàng:";
            exRange.Range["C7:E7"].MergeCells = true;
            exRange.Range["C7:E7"].Value = tblThongtinHD.Rows[0][3].ToString();
            exRange.Range["B8:B8"].Value = "Phòng:";
            exRange.Range["C8:E8"].MergeCells = true;
            exRange.Range["C8:E8"].Value = tblThongtinHD.Rows[0][4].ToString();
            exRange.Range["B9:B9"].Value = "Giá phòng:";
            exRange.Range["C9:E9"].MergeCells = true;
            exRange.Range["C9:E9"].Value = tblThongtinHD.Rows[0][5].ToString();
            //Lấy thông tin các mã dv
            sql = "SELECT b.tendv, a.SoLuong, b.DonGia, a.GiamGia, a.ThanhTien " +
                  "FROM chitiethoadon AS a , dichvu AS b WHERE a.MaHoaDon = N'" +
                  txtMaHD.Text + "' AND a.MaDichVu = b.madv";
            tblThongtinHang = Functions.GetDataToTable(sql);
            //Tạo dòng tiêu đề bảng
            exRange.Range["A11:F11"].Font.Bold = true;
            exRange.Range["A11:F11"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C11:F11"].ColumnWidth = 12;
            exRange.Range["A11:A11"].Value = "STT";
            exRange.Range["B11:B11"].Value = "Tên hàng";
            exRange.Range["C11:C11"].Value = "Số lượng";
            exRange.Range["D11:D11"].Value = "Đơn giá";
            exRange.Range["E11:E11"].Value = "Giảm giá";
            exRange.Range["F11:F11"].Value = "Thành tiền";
            for (hang = 0; hang < tblThongtinHang.Rows.Count; hang++)
            {
                //Điền số thứ tự vào cột 1 từ dòng 12
                exSheet.Cells[1][hang + 12] = hang + 1;
                for (cot = 0; cot < tblThongtinHang.Columns.Count; cot++)
                //Điền thông tin hàng từ cột thứ 2, dòng 12
                {
                    exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString();
                    if (cot == 3) exSheet.Cells[cot + 2][hang + 12] = tblThongtinHang.Rows[hang][cot].ToString() + "%";
                }
            }
            exRange = exSheet.Cells[cot][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = "Tổng tiền:";
            exRange = exSheet.Cells[cot + 1][hang + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = tblThongtinHD.Rows[0][2].ToString();
            exRange = exSheet.Cells[1][hang + 15]; //Ô A1 
            exRange.Range["A1:F1"].MergeCells = true;
            exRange.Range["A1:F1"].Font.Bold = true;
            exRange.Range["A1:F1"].Font.Italic = true;
            exRange.Range["A1:F1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignRight;
            exRange.Range["A1:F1"].Value = "Bằng chữ: " + Functions.ChuyenSoSangChuoi(Double.Parse(tblThongtinHD.Rows[0][2].ToString()));
            exRange = exSheet.Cells[4][hang + 17]; //Ô A1 
            exRange.Range["A1:C1"].MergeCells = true;
            exRange.Range["A1:C1"].Font.Italic = true;
            exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            DateTime d = Convert.ToDateTime(tblThongtinHD.Rows[0][1]);
            exRange.Range["A1:C1"].Value = "Bình Định, ngày " + d.Day + " tháng " + d.Month + " năm " + d.Year;
            exRange.Range["A2:C2"].MergeCells = true;
            exRange.Range["A2:C2"].Font.Italic = true;
            exRange.Range["A2:C2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:C2"].Value = "Nhân viên lập hóa đơn :";
            exRange.Range["A6:C6"].MergeCells = true;
            exRange.Range["A6:C6"].Font.Italic = true;
            exRange.Range["A6:C6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A6:C6"].Value = tblThongtinHD.Rows[0][6];
            exSheet.Name = "Hóa đơn nhập";
            exApp.Visible = true;
           */
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cbxTim.Text == "")
            {
                MessageBox.Show("Bạn phải chọn một mã hóa đơn để tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbxTim.Focus();
                return;
            }
            txtMaHD.Text = cbxTim.Text;
            LoadInfoHoaDon();
            LoadDataGridView();
            btnHuy.Enabled = true;
            btnLuu.Enabled = true;
            btnIn.Enabled = true;
            cbxTim.SelectedIndex = -1;
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
                e.Handled = false;
            else e.Handled = true;
        }

        private void cbxTim_DropDown(object sender, EventArgs e)
        {
            Functions.FillCombo("SELECT MaHoaDon FROM hoadonphong", cbxTim, "MaHoaDon", "MaHoaDon");
            cbxTim.SelectedIndex = -1;
        }

        private void HDPhong_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Xóa dữ liệu trong các điều khiển trước khi đóng Form
            ResetValues();
        }

        private void txtgiamgia_TextChanged(object sender, EventArgs e)
        {
            //Khi thay đổi giảm giá thì tính lại thành tiền
            double tt, sl, dg, gg;
            if (txtSoLuong.Text == "")
                sl = 0;
            else
                sl = Convert.ToDouble(txtSoLuong.Text);
            if (txtgiamgia.Text == "")
                gg = 0;
            else
                gg = Convert.ToDouble(txtgiamgia.Text);
            if (txtDonGia.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDonGia.Text);
            tt = sl * dg - sl * dg * gg / 100;
            txtThanhTien.Text = tt.ToString();
        }

        private void cbxMaDV_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cbxMaDV.Text == "")
            {
                txtTenDV.Text = "";
                txtDonGia.Text = "";
            }
            // Khi chọn mã Dv thì các thông tin về Dv hiện ra
            str = "SELECT tendv FROM dichvu WHERE madv =N'" + cbxMaDV.SelectedValue + "'";
            txtTenDV.Text = Functions.GetFieldValues(str);
            str = "SELECT giadv FROM dichvu WHERE madv =N'" + cbxMaDV.SelectedValue + "'";
            txtDonGia.Text = Functions.GetFieldValues(str);
        }

        private void cbxMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cbxMaKH.Text == "")
            {
                txttenKH.Text = "";

            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select tenkhachhang from khachhang where makhachhang = N'" + cbxMaKH.SelectedValue + "'";
            txttenKH.Text = Functions.GetFieldValues(str);
        }

        private void cbxMaPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cbxMaPhong.Text == "")
            {
                txtTenPhong.Text = "";
                txtGiaPhong.Text = "";
            }
            //Khi chọn Mã phòng thì các thông tin của phòng sẽ hiện ra
            str = "Select tenphong from phong where maphong = N'" + cbxMaPhong.SelectedValue + "'";
            txtTenPhong.Text = Functions.GetFieldValues(str);
            str = "Select giaphong from phong where maphong = N'" + cbxMaPhong.SelectedValue + "'";
            txtGiaPhong.Text = Functions.GetFieldValues(str);
        }

        private void cbxMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cbxMaNV.Text == "")
                txtTenNV.Text = "";
            // Khi chọn Mã nhân viên thì tên nhân viên tự động hiện ra
            str = "Select tennhanvien from nhanvien where manhanvien =N'" + cbxMaNV.SelectedValue + "'";
            txtTenNV.Text = Functions.GetFieldValues(str);
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
           
        }
    }

    }
