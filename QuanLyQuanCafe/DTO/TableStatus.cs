using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class TableStatus
    {
        public TableStatus(string name)
        {
            this.Name = name;
        }

        public TableStatus(DataRow row)
        {
            this.Name = row["status"].ToString();
        }


        private string name;

        public string Name { get => name; set => name = value; }
    }
}
