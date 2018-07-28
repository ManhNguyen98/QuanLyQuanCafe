using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class FoodMenu
    {
        public FoodMenu(string foodName, int count, float price, float totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Count = count;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }

        public FoodMenu(DataRow row)
        {
            this.FoodName = row["Name"].ToString();
            this.Count = (int)row["COUNT"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString()); 
            this.TotalPrice = (float)Convert.ToDouble(row["TotalPrice"].ToString());
        }

        

        private float totalPrice;

        private float price;

        private int count;

        private string foodName;

        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
        public float Price { get => price; set => price = value; }
        public int Count { get => count; set => count = value; }
        public string FoodName { get => foodName; set => foodName = value; }
    }
}
