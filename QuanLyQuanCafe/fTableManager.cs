using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuanLyQuanCafe.fAccountProfile;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            LoadTable();
            LoadCategory();
            LoadComboboxTable(cmbSwitchTable);
        }
        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinCáNhânToolStripMenuItem.Text += " (" + loginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCateGory();
            cmbCategory.DataSource = listCategory;
            cmbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodsByCategoryID(id);
            cmbFood.DataSource = listFood;
            cmbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach(Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeigth};
                btn.Text = item.Name + Environment.NewLine+item.Status ;
                btn.Click += btn_Click;//tao event click cho button
                btn.Tag = item; //lay Table ID

                switch(item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.Coral;
                        break;
                }
                flpTable.Controls.Add(btn);
            }
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<FoodMenu> listBillInfo = FoodMenuDAO.Instance.GetFoodMenuByTable(id);
            float totalPrice = 0;
            foreach (FoodMenu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }

            CultureInfo culture = new CultureInfo("vi-VN");//thiết lập culture cho chương trình en-US: English
            //Thread.CurrentThread.CurrentCulture = culture;// setting luồng đang chạy theo culture
            txbTotalPrice.Text = totalPrice.ToString("c",culture);//chỉ có "c" thì mặc định theo current của máy 

        }

        void LoadComboboxTable(ComboBox cd)
        {
            cd.DataSource = TableDAO.Instance.LoadTableList();
            cd.DisplayMember = "Name";
        }
        #endregion
        #region Events
        private void btn_Click(object sender, EventArgs e)
        {
            int tableId = ((sender as Button).Tag as Table).ID;//lay Table ID
            lsvBill.Tag = (sender as Button).Tag;//luu table dang duoc chon
            ShowBill(tableId);
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinCáNhânToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")"; 
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.ShowDialog();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cmbCategory.SelectedItem as Category).ID);

            if (lsvBill.Tag != null)
            ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cmbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cmbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn!");
                return;
            }
            int idBill = BillDAO.Instance1.GetUncheckBillIDByTableID(table.ID);
            int idFood = (cmbFood.SelectedItem as Food).ID;
            int count = (int)numericUpDown1.Value;

            if (idBill == -1)//khong co bill
            {
                BillDAO.Instance1.InsertBill(table.ID);
                BillInfoDAO.Instance.InserBillInfo(BillDAO.Instance1.GetMaxIDBill(), idFood, count);//insert vao bill lớn nhất
            }
            else
            {
                BillInfoDAO.Instance.InserBillInfo(idBill, idFood, count);

            }
            ShowBill(table.ID);
            LoadTable();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance1.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            //double totalPrice = double.Parse(txbTotalPrice.Text, NumberStyles.Currency); cach chuyen khac
            double finalPrice = totalPrice - (totalPrice/100)*discount;

            if (idBill != -1 )
            {
                if(MessageBox.Show(String.Format("Bạn có chắc muốn thanh toán hóa đơn cho bàn {0}\n Tổng tiền - (Tổng tiền/100) x Giảm giá\n >> {1} - ({1}/100) x {2} = {3}",table.Name,totalPrice,discount,finalPrice),"Thông báo",MessageBoxButtons.OKCancel)==DialogResult.OK)
                {
                    BillDAO.Instance1.CheckOut(idBill,discount,(float)finalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            
            int id1 = (lsvBill.Tag as Table).ID;

            int id2 = (cmbSwitchTable.SelectedItem as Table).ID;

            if (MessageBox.Show(String.Format("Bạn có thực sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cmbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)

            {
                TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }
        }
        #endregion


    }
}
