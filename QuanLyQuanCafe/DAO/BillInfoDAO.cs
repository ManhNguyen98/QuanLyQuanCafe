﻿using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
     public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) return new BillInfoDAO(); return BillInfoDAO.instance; }
            private set => instance = value;
        }

        public BillInfoDAO() { }

        public List<BillInfo> GetListBillInfo(int iD)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM BillInfo bi WHERE bi.idBill = " + iD);

            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InserBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsetBillInfo @idBill , @idFood , @count",new object[] {idBill,idFood,count});
        }

        public void DeleteBillInfoByFoodId(int idFood)
        {
            DataProvider.Instance.ExecuteQuery("Delete BillInfo where idFood = " + idFood);
        }
    }
}
