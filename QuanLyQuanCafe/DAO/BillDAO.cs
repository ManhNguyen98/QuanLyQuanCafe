using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    //Lay ra Bill tu ID Table
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance1
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
           private set { BillDAO.instance = value; }

        }

        private BillDAO() { }

        //thành công: Bill ID
        //thất bại:  -1
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Bill b WHERE b.idTable = " + id + " AND b.[STATUS] = 0");

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID1;
            }

            return -1;//khong co du lieu
        }

        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "UPDATE Bill SET dateCheckout = GETDATE() , [STATUS] = 1 , Discount = "+discount+",totalPrice = " + totalPrice +" WHERE id = " + id;

            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteQuery("Exec USP_InsertBill @idTable ", new object[] { id });
        }

        public DataTable GetBillListByDate (DateTime checkIn, DateTime checkOut)
        {
           return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

        public int GetMaxIDBill()
        {
            try
            {
               return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM Bill b");
            }
            catch
            {
                return 1;
            }
        }
    }
}
