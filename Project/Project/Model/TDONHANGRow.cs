﻿using Project.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace Project.Model
{
    public class TDONHANGRow:ModelRow
    {
        public string ID { set; get; }
        public DateTime NGAY { set; get; }
        public string NAME { set; get; }
        public DKHACHHANGRow DKHACHHANG { set; get; }
        public decimal TONGCONG { set; get; }
        public string NOTE { set; get; }
        public List<TDONHANGCHITIETRow> details { set; get; }   
        public TDONHANGRow()
        {
            ID = "";
            NAME = "Tự động";
            NGAY = DateTime.Now;
            DKHACHHANG = new DKHACHHANGRow();
            NOTE = "";
            details = new List<TDONHANGCHITIETRow>();
        }
        public TDONHANGRow(DataRow row)
        {
            ID = row["ID"].ToString();
            NAME = row["NAME"].ToString();
            NGAY = DateTime.Parse(row["NGAY"].ToString());
            DKHACHHANG = new DKHACHHANGRow(row["DKHACHHANGID"].ToString());
            NOTE = row["NOTE"].ToString();
            details = ChiTietHoaDon();
        }
        public TDONHANGRow(string TDONHANGID)
        {
            ID = TDONHANGID;
            if (ID.Length == 0)
            {
                ID = "";
                NGAY = DateTime.Now.Date;
                NAME = "Tự động";
                DKHACHHANG = new DKHACHHANGRow();
                TONGCONG = 0;
                NOTE = "";
                details = new List<TDONHANGCHITIETRow>();
            }
            else
            {
                DataRow row = DatabaseSql.GetFirstRow("select * from tdonhang where id = @id", attrs);
                if (row != null)
                {
                    ID = row["ID"].ToString();
                    NGAY = DateTime.Parse(row["NGAY"].ToString());
                    NAME = row["NAME"].ToString();
                    DKHACHHANG = new DKHACHHANGRow(row["DKHACHHANGID"].ToString());
                    TONGCONG = int.Parse(row["TONGCONG"].ToString());
                    NOTE = row["NOTE"].ToString();
                    details = ChiTietHoaDon();
                }
            }
        }

        private List<TDONHANGCHITIETRow> ChiTietHoaDon()
        {
            List<TDONHANGCHITIETRow> lst = new List<TDONHANGCHITIETRow>();
            DataTable dtChiTiet = DatabaseSql.GetTable(@"select tdonhangchitiet.* from tdonhangchitiet
                    inner join dmathang on dmathangid = dmathang.id where tdonhangid = @id order by dmathang.name", attrs);
            foreach (DataRow rChiTiet in dtChiTiet.Rows)
            {
                TDONHANGCHITIETRow ctRow = new TDONHANGCHITIETRow(rChiTiet);
                lst.Add(ctRow);
            }
            return lst;
        }

        public override bool Update(out string error)
        {
            bool kq = true;
            if (ID == null || ID.Length == 0)
            {
                ID = Guid.NewGuid().ToString();
                NAME = DatabaseSql.GenCode("NAME", "TDONHANG");

                DatabaseSql.ExcuteQuery(@"insert into tdonhang(id, ngay, name, dkhachhangid, tongcong, note)
                    values(@id, @ngay, @name, @dkhachhangid, @tongcong, @note)", attrs, out error);
                if (error.Length > 0) kq = false;
            }
            else
            {
                DatabaseSql.ExcuteQuery(@"update tdonhang set ngay = @ngay, name = @name, dkhachhangid = @dkhachhangid,
                    tongcong = @tongcong, note = @note where id = @id", attrs, out error);
                if (error.Length > 0) kq = false;
            }
            return kq;
        }

        //private void AddDetail(out string error)
        //{
        //    error = "";
        //    foreach (Dictionary<string, object> dic in attrsDetails)
        //    {
        //        dic["@id"] = Guid.NewGuid().ToString();
        //        Database.ExcuteQuery(@"insert into tdonhangchitiet(id, tdonhangid, dmathangid, dongia, soluong, thanhtien)
        //        values(@id, @tdonhangid, @dmathangid, @dongia, @soluong, @thanhtien)", dic, out error);
        //        if (error.Length > 0) break;
        //    }
        //}

        public override bool Delete(out string error)
        {
            DatabaseSql.ExcuteQuery("delete from tdonhang where id = @id", attrs, out error);
            if (error.Length > 0) return false;
            DatabaseSql.ExcuteQuery("delete from tdonhangchitiet where tdonhangid = @id", attrs, out error);
            if (error.Length > 0) return false;
            return true;
        }
        private Dictionary<string, object> attrs
        {
            get
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("@id", ID);
                dic.Add("@ngay", NGAY);
                dic.Add("@name", NAME);
                if (DKHACHHANG != null) dic.Add("@dkhachhangid", DKHACHHANG.ID);
                dic.Add("@tongcong", TONGCONG);
                dic.Add("@note", NOTE);
                return dic;
            }
        }

        public override DataRow ToRow(DataTable dt)
        {
            DataRow row = dt.NewRow();
            row["ID"] = ID;
            row["NGAY"] = NGAY.ToString("dd/MM/yyyy");
            row["NAME"] = NAME;
            row["DKHACHHANG_CODE"] = DKHACHHANG.CODE;
            row["DKHACHHANG_NAME"] = DKHACHHANG.NAME;
            row["TONGCONG"] = TONGCONG;
            row["NOTE"] = NOTE;
            return row;
        }
    }
}
