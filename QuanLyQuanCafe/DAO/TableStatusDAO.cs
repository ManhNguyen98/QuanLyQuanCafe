using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableStatusDAO
    {
        private static TableStatusDAO instance;

        public static TableStatusDAO Instance
        {
            get { if (instance == null) return new TableStatusDAO(); return instance; }
            private set => instance = value;
        }

        public TableStatusDAO() { }

        
    }
}
