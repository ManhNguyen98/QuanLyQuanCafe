using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccoutTypeDAO
    {
        private static AccoutTypeDAO instance;

        public static AccoutTypeDAO Instance
        {
            get { if (instance == null) return new AccoutTypeDAO(); return instance; }
            private set => instance = value;
        }

        public AccoutTypeDAO() { }

        public List<AccountType> GetListAccountType()
        {
            List<AccountType> listAccountType = new List<AccountType>();

            AccountType type = new AccountType(1, "Admin");
            listAccountType.Add(type);
            AccountType type1 = new AccountType(0, "Staff");
            listAccountType.Add(type1);

            return listAccountType;
        }

        public AccountType GetTypeByName(string name)
        {
            AccountType type = null;

            string query = "SELECT a.type FROM Account a WHERE a.UserName like N'" + name + "'";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                type = new AccountType(item);
                return type;
            }

            return type;
        }
    }
}
