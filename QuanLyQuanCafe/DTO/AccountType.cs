using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class AccountType
    {
        public AccountType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public AccountType(DataRow row)
        {
            this.Id = (int)row["type"];
            if (this.Id == 1) this.Name = "Admin";
            else this.Name = "Staff";
        }

        private string name;

        private int id;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
    }
}
