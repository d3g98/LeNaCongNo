﻿using Project.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Project.Screens
{
    public partial class ChiTietCongNo : Form
    {
        public ChiTietCongNo()
        {
            InitializeComponent();

            detail.AutoGenerateColumns = false;
            cbKhachHang.ValueMember = "ID";
            cbKhachHang.DisplayMember = "NAME";
            cbKhachHang.DropDownStyle = ComboBoxStyle.DropDown;
            cbKhachHang.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbKhachHang.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbKhachHang.Cursor = Cursors.Default;
            cbKhachHang.DataSource = DanhSachKhachHang.List("");
            cbKhachHang.Click += CbKhachHang_Click;

            btnLoc.Click += BtnLoc_Click;
        }

        private void CbKhachHang_Click(object sender, EventArgs e)
        {
            if (cbKhachHang.Focused) cbKhachHang.DroppedDown = true;
        }

        private void BtnLoc_Click(object sender, EventArgs e)
        {
            string DKHACHHANGID = cbKhachHang.SelectedValue == null ? "" : cbKhachHang.SelectedValue.ToString();
            if (DKHACHHANGID.Length == 0)
            {
                Msg.ShowWarning("Khách hàng không được trống");
                return;
            }

            Dictionary<string, object> attrs = new Dictionary<string, object>
            {
                { "@dkhachhangid", DKHACHHANGID }
            };

            string query = @"select 0 as stt, t.* from
            (
                select ngay, '' as ngaystr, name, tongcong as numtongcong, 0 as numthanhtoan, '' as tongcong, '' as thanhtoan, '' as luyke from tdonhang where dkhachhangid = @dkhachhangid
                union all
                select ngay, '', name, 0, tongcong, '', '', '' from tthanhtoan where dkhachhangid = @dkhachhangid
            ) t
            order by ngay, name asc";
            DataTable dt = DatabaseSql.GetTable(query, attrs);

            decimal luyKe = 0;
            int stt = 1;
            foreach (DataRow row in dt.Rows)
            {
                decimal tongCong = int.Parse(row["NUMTONGCONG"].ToString()), thanhToan = int.Parse(row["NUMTHANHTOAN"].ToString());
                row["NGAYSTR"] = DateTime.Parse(row["NGAY"].ToString()).ToString("dd/MM/yyyy");
                luyKe += tongCong - thanhToan;
                row["TONGCONG"] = tongCong.ToString("n0");
                row["THANHTOAN"] = thanhToan.ToString("n0");
                row["LUYKE"] = luyKe.ToString("n0");
                row["stt"] = stt;
                stt++;
            }
            detail.DataSource = dt;
        }
    }
}
