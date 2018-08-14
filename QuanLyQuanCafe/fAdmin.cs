﻿using QuanLyQuanCafe.DAO;
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
            //cmbStatusTable.DataBindings.Add(new Binding("member", dtgvTable.DataSource, "Status"));
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

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["IdCategory"].Value;
                    //lấy 1 ô trong datagridview
                    Category category = CategoryDAO.Instance.GetCategoryById(id);

                    label14.Text = category.Name;
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

                    label8.Text = status.Name;
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
        #endregion


    }
}
