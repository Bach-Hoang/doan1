using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1.DAO
{
    public class MenuDAO
    {   
        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> listMenu = new List<Menu>();
            string query = "SELECT f.name , bi.count ,f.price, f.price*bi.count AS totalPrice FROM dbo.BILLINFO AS bi, dbo.Bill AS b, dbo.FOOD AS f WHERE bi.idbill = b.id AND b.status = 0 and bi.idFOOD = f.id and b.idtable = "+id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }
            return listMenu;
        }
        private static MenuDAO instance;
        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
           private set { MenuDAO.instance = value; }
        }
        private MenuDAO() { }
    }
}
