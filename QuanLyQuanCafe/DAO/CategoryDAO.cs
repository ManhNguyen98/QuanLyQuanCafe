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

            string query = "SELECT* FROM FoodCategory fc";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                listCategory.Add(category);
            }

            return listCategory;
        }
    }
}
