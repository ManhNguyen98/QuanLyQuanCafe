using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodlist = new BindingSource();
        BindingSource tablelist = new BindingSource();
        BindingSource categorylist = new BindingSource();
        BindingSource accountlist = new BindingSource();

        public Account loginAccount;
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
       

        #region methods
        void Load()
        {
            dtgvFood.DataSource = foodlist;
            dtgvTable.DataSource = tablelist;
            dtgvCategory.DataSource = categorylist;
            dtgvAccount.DataSource = accountlist;

            LoadDateTimePicker();
            LoadListBillByDate(dtpkfromDate.Value, dtpktoDate.Value);

            LoadListFood();
            LoadCategoryIntoCombobox(cmbFoodCategory);
            AddFoodBinding();

            LoadTableFood();
            AddTableBinding();
            LoadStatuIntoCombobox(cmbStatusTable);

            LoadCategory();
            AddCategoryBinding();

            AddAccountBinding();
            LoadAccount();
            LoadTypeIntoCombobox(cmbType);
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

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("text", dtgvAccount.DataSource, "username", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("text", dtgvAccount.DataSource, "displayname", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountlist.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void LoadListFood()
        {
            foodlist.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name",true,DataSourceUpdateMode.Never));
            //thay đổi thuộc tính text theo name trong dataSource
            txbFoodID.DataBindings.Add(new Binding("text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("value", dtgvFood.DataSource, "price",true,DataSourceUpdateMode.Never));

        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCateGory();
            cb.DisplayMember = "name";
        }

        List<Food> SearchFoodByName(String name)
        {
            List<Food> listfood = FoodDAO.Instance.SearchFoodByName(name);

            return listfood;
        }

        void LoadTableFood()
        {
            tablelist.DataSource = TableDAO.Instance.LoadTableList();
        }

        void AddTableBinding()
        {
            txbTable.DataBindings.Add(new Binding("text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbTableID.DataBindings.Add(new Binding("text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
           
        }

        void LoadStatuIntoCombobox(ComboBox cb)
        {
            cb.DataSource = TableStatusDAO.Instance.GetListTableStatus();
            cb.DisplayMember = "name";
        }

        void LoadCategory()
        {
            categorylist.DataSource = CategoryDAO.Instance.GetListCateGory();
        }

        void AddCategoryBinding()
        {
            txtCategoryId.DataBindings.Add(new Binding("text", dtgvCategory.DataSource, "id", true, DataSourceUpdateMode.Never));
            txtCategoryName.DataBindings.Add(new Binding("text", dtgvCategory.DataSource, "name", true, DataSourceUpdateMode.Never));
        }

        void AddAccount(string UserName, string DisplayName, string Type)
        {
            if (AccountDAO.Instance.InsertAccount(UserName,DisplayName,Type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string UserName, string DisplayName, string Type)
        {
            if (AccountDAO.Instance.UpdateAccount(UserName, DisplayName, Type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string UserName)
        {
            if (loginAccount.UserName.Equals(UserName))
            {
                MessageBox.Show("Không thể xóa chính bạn");
                return;
            }

            if (AccountDAO.Instance.DeleteAccount(UserName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPassword(string name)
        {
            if (AccountDAO.Instance.ResetPassword(name))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }

            LoadAccount();
        }
        #endregion
        #region events

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            String UserName = txbUserName.Text;
            String DisplayName = txbDisplayName.Text;
            string Type = (cmbType.SelectedItem as AccountType).Name;

            AddAccount(UserName, DisplayName, Type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            String UserName = txbUserName.Text;
            

            DeleteAccount(UserName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            String UserName = txbUserName.Text;
            String DisplayName = txbDisplayName.Text;
            string Type = (cmbType.SelectedItem as AccountType).Name;

            EditAccount(UserName, DisplayName, Type);
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            String UserName = txbUserName.Text;
            ResetPassword(UserName);
        }

        void LoadTypeIntoCombobox(ComboBox cb)
        {
            cb.DataSource = AccoutTypeDAO.Instance.GetListAccountType();
            cb.DisplayMember = "name";
        }

        private void txbUserName_TextChanged(object sender, EventArgs e)
        {
            try
            {

                string name = dtgvAccount.SelectedCells[0].OwningRow.Cells["UserName"].Value.ToString();

                AccountType type = AccoutTypeDAO.Instance.GetTypeByName(name);


                cmbType.SelectedItem = type.Name;
                int index = -1;
                int i = 0;

                foreach (AccountType item in cmbType.Items)
                {
                    if (item.Id == type.Id)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }


                cmbType.SelectedIndex = index;

            }
            catch { }
        }
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkfromDate.Value, dtpktoDate.Value);
        }

        //Thêm, sửa, xóa, tìm kiếm thức ăn
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["IdCategory"].Value;
                    //lấy 1 ô trong datagridview
                    Category category = CategoryDAO.Instance.GetCategoryById(id);

                    
                    cmbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;

                    foreach (Category item in cmbFoodCategory.Items)
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
            catch { }

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryId = (cmbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryId, price))
            {
                MessageBox.Show("Thêm thành công!");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi thêm!");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryId = (cmbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int idFood = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.UpdateFood(idFood, name, categoryId, price))
            {
                MessageBox.Show("Sửa thành công!");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi sửa!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            int idFood = Convert.ToInt32(txbFoodID.Text);

            BillInfoDAO.Instance.DeleteBillInfoByFoodId(idFood);

            if (FoodDAO.Instance.DeleteFood(idFood))
            {
                MessageBox.Show("Xóa thành công!");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi xóa!");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodlist.DataSource = SearchFoodByName(txbSearchFoodName.Text);

        }

        //Thêm, sửa, xóa bàn ăn
        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadTableFood();
        }

        private void txbTableID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                
                    int id = (int)dtgvTable.SelectedCells[0].OwningRow.Cells["ID"].Value;
     
                    TableStatus status = TableStatusDAO.Instance.GetStatusById(id);

                    
                    cmbStatusTable.SelectedItem = status;
                    int index = -1;
                    int i = 0;

                    foreach (TableStatus item in cmbStatusTable.Items)
                    {
                       if (Equals(item.Name,status.Name))
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }


                cmbStatusTable.SelectedIndex = index;
                
            }
            catch { }
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTable.Text;
            string status = (cmbStatusTable.SelectedItem as TableStatus).Name;
            

            if (TableDAO.Instance.InsertTable(name,status))
            {
                MessageBox.Show("Thêm thành công!");
                LoadTableFood();
                if (insertTable != null)
                    insertTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi thêm!");
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string name = txbTable.Text;
            string status = (cmbStatusTable.SelectedItem as TableStatus).Name;
            int id = Convert.ToInt32(txbTableID.Text);
            if (TableDAO.Instance.UpdateTable(name,status,id))
            {
                MessageBox.Show("Sửa thành công!");
                LoadTableFood();
                if (updateTable != null)
                    updateTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi sửa!");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);

            //BillInfoDAO.Instance.DeleteBillInfoByFoodId(idFood);

            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa thành công!");
                LoadTableFood();
                if (deleteTable != null)
                    deleteTable(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi xóa!");
            }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        //Thêm, sửa, xóa danh mục
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;


            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm thành công!");
                LoadCategory();
                if (insertcategory != null)
                    insertcategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi thêm!");
            }

            LoadCategoryIntoCombobox(cmbFoodCategory);
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;
            int id = Convert.ToInt32(txtCategoryId.Text);
            if (CategoryDAO.Instance.UpdateCategory(name, id))
            {
                MessageBox.Show("Sửa thành công!");
                LoadCategory();
                if (updatecategory != null)
                    updatecategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi sửa!");
            }
            LoadCategoryIntoCombobox(cmbFoodCategory);
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtCategoryId.Text);

            //BillInfoDAO.Instance.DeleteBillInfoByFoodId(idFood);

            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa thành công!");
                LoadCategory();
                if (deletecategory != null)
                    deletecategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi xóa!");
            }
            LoadCategoryIntoCombobox(cmbFoodCategory);
        }

        private event EventHandler insertcategory;
        public event EventHandler Insertcategory
        {
            add { insertcategory += value; }
            remove { insertcategory -= value; }
        }

        private event EventHandler updatecategory;
        public event EventHandler Updatecategory
        {
            add { updatecategory += value; }
            remove { updatecategory -= value; }
        }

        private event EventHandler deletecategory;
        public event EventHandler Deletecategory
        {
            add { deletecategory += value; }
            remove { deletecategory -= value; }
        }






        #endregion

        
    }
}
