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

        public List<TableStatus> GetListTableStatus()
        {
            List<TableStatus> listTableStatus = new List<TableStatus>();
            
            TableStatus status = new TableStatus("Trống");
            listTableStatus.Add(status);
            TableStatus status1 = new TableStatus("Có người");
            listTableStatus.Add(status1);

            return listTableStatus;
        }

        public TableStatus GetStatusById(int iD)
        {
            TableStatus status = null;

            string query = "SELECT status FROM TableFood tf WHERE tf.id =  " + iD;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                status = new TableStatus(item);
                return status;
            }

            return status;
        }
    }
}
