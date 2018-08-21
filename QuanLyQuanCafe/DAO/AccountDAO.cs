using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
   public class AccountDAO
    {
        //single tone
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null) instance = new AccountDAO();
                return instance;
            }
           private set { instance = value; }
        }
        
        private AccountDAO() { }
        //
       public bool Login (string userName, string passWord)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);

            //var list = hashData.ToString();
            //list.Reverse();

            string hashPass = "";
            foreach (byte item in hashData)
            {
                hashPass += item;
            }


            string query = "USP_Login @username , @password";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, hashPass });
            return result.Rows.Count>0;
        }

        public bool UpdateAccount(string username, string displayname, string pass, string newpass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccount @userName , @displayName , @passWord , @newPassWord ",new object[] { username, displayname,pass, newpass});

            return result >0;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data =  DataProvider.Instance.ExecuteQuery("SELECT * FROM Account a WHERE a.UserName =  '" + userName +"'");
            foreach(DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("Select UserName , DisplayName  from Account ");
        }

        public bool InsertAccount(string name, string displayname, string type)
        {
            int id;
            if (Equals(type,"Admin")) id = 1;
            else id = 0;
            string query = string.Format("INSERT INTO Account (UserName , DisplayName ,	Type , password ) VALUES (	N'{0}' , N'{1}' , {2} , N'{3}')", name, displayname, id, "20720532132149213101239102231223249249135100218");
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount(string name, string displayname, string type)
        {
            int id;
            if (Equals(type, "Admin")) id = 1;
            else id = 0;
            string query = string.Format("UPDATE Account SET DisplayName = N'{0}',	Type = {1} WHERE UserName like N'{2}'",  displayname, id,name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(String UserName)
        {
            string query = string.Format("Delete Account where UserName = N'{0}'", UserName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string UserName)
        {
            string query = string.Format("Update Account Set PassWord = N'20720532132149213101239102231223249249135100218' where UserName = N'{0}'", UserName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
