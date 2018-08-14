using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get
            {
                if (instance == null) instance = new TableDAO();
                return TableDAO.instance;
            }
            private set { instance = value; }
        }
        private TableDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] { id1, id2 });
        }

        public static int TableWidth = 90;
        public static int TableHeigth = 90;
        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach(DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList;
        }

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

            string query = "SELECT tf.[STATUS] FROM TableFood tf WHERE tf.id =  " + iD;

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

