using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance
        {
            get { if (instance == null) return new CategoryDAO(); return instance; }
            private  set => instance = value;
        }

        public CategoryDAO() { }

        public List<Category> GetListCateGory()
        {
            List<Category> listCategory = new List<Category>();

            string query = "SELECT * FROM FoodCategory fc";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                listCategory.Add(category);
            }

            return listCategory;
        }

        public Category GetCategoryById(int iD)
        {
            Category category = null;

            string query = "SELECT * FROM FoodCategory  where id = " + iD;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }

            return category;
        }

        public bool InsertCategory(string name)
        {
            string query = string.Format("INSERT INTO FoodCategory ( NAME ) VALUES (	N'{0}' )", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateCategory(string name,int id)
        {
            string query = string.Format("UPDATE FoodCategory SET NAME = N'{0}' WHERE id = {1}", name , id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteCategory(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from Food where idCategory = " + id);
            foreach(DataRow item in data.Rows)
            {
                Food food = new Food(item);
                BillInfoDAO.Instance.DeleteBillInfoByFoodId(food.ID);
            }

            string query = string.Format("Delete Food where idCategory = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            query = string.Format("Delete FoodCategory where id = {0}", id);
            result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
