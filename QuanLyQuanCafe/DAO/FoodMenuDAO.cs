using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
   public class FoodMenuDAO
    {
        private static FoodMenuDAO instance;

        public static FoodMenuDAO Instance
        {
            get { if (instance == null) return new FoodMenuDAO(); return FoodMenuDAO.instance; }
            private set => instance = value;
        }

        public FoodMenuDAO() { }

        public List<FoodMenu> GetFoodMenuByTable (int id)
        {
            List<FoodMenu> listMenu = new List<FoodMenu>();

            string query = "SELECT f.NAME, f.price,bi.[COUNT],f.price* bi.[COUNT] AS[TotalPrice]FROM Food f, Bill b,BillInfo bi WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.status = 0 AND b.idTable = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                FoodMenu menu = new FoodMenu(item);
                listMenu.Add(menu);

            }
            return listMenu;
        }
    }
}
