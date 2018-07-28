using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodlist = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            Load();
            // LoadAccountList();
        }
        //void LoadFoodList()
        //{
        //    string query = "select * from food";
        //    dtgvFood.DataSource = DataProvider.Instance.ExecuteQuery(query);
        //}
        //void LoadAccountList()
        //{
        //    string query = "EXEC USP_GetAccountByUserName @username ";

        //   dtgvAccount.DataSource =DataProvider.Instance.ExecuteQuery(query,new object[] { "NhungIrene"});
        //}
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region methods
        void Load()
        {
            dtgvFood.DataSource = foodlist;
            LoadDateTimePicker();
            LoadListBillByDate(dtpkfromDate.Value, dtpktoDate.Value);
            LoadListFood();
            LoadCategoryIntoCombobox(cmbFoodCategory);
            AddFoodBinding();
        }

        void LoadDateTimePicker()
        {
            DateTime today = DateTime.Now;
            dtpkfromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpktoDate.Value = dtpkfromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance1.GetBillListByDate(checkIn, checkOut);
        }

        void LoadListFood()
        {
            foodlist.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name"));
            //thay đổi thuộc tính text theo name trong dataSource
            txbFoodID.DataBindings.Add(new Binding("text", dtgvFood.DataSource, "ID"));
            nmFoodPrice.DataBindings.Add(new Binding("value", dtgvFood.DataSource, "price"));

        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCateGory();
            cb.DisplayMember = "name";
        }
        #endregion
        #region events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkfromDate.Value, dtpktoDate.Value);
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }



        #endregion

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            if (dtgvFood.SelectedCells.Count > 0)
            {
                int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["IdCategory"].Value;
                //lấy 1 ô trong datagridview
                Category category = CategoryDAO.Instance.GetCategoryById(id);

                cmbFoodCategory.SelectedItem = category;

                int index = -1;
                int i = 0;

                foreach(Category item in cmbFoodCategory.Items)
                {
                    if (item.ID == category.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }

                cmbFoodCategory.SelectedIndex = index;
            }

        }
    }
}
