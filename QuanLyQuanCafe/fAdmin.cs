using QuanLyQuanCafe.DAO;
using System;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
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
            LoadDateTimePicker();
            LoadListBillByDate(dtpkfromDate.Value, dtpktoDate.Value);
            LoadListFood();
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
            dtgvFood.DataSource = FoodDAO.Instance.GetListFood();
        }
        #endregion
        #region events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkfromDate.Value, dtpktoDate.Value);
        }
        #endregion

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
    }
}
