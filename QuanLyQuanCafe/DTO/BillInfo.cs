using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
   public class BillInfo
    {
        public BillInfo (int iD, int BillID, int foodId, int count)
        {
            this.ID = iD;
            this.BillID = BillID;
            this.FoodID = foodId;
            this.Count = count;
        }

       public BillInfo (DataRow row)
        {
            this.ID = (int) row["id"];
            this.BillID = (int)row["idBill"];
            this.FoodID = (int)row["idFood"];
            this.Count = (int)row["COUNT"];
        }

        private int count;

        private int foodID;

        private int billID;

        private int iD;

        public int ID { get => iD; set => iD = value; }
        public int BillID { get => billID; set => billID = value; }
        public int FoodID { get => foodID; set => foodID = value; }
        public int Count { get => count; set => count = value; }
    }
}
