using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;
using WindowsFormsApp1.DTO;
using System.Globalization;
using System.Threading;
namespace WindowsFormsApp1
{
    public partial class TableManager : Form
    {
        public TableManager()
        {
            InitializeComponent();
            LoadTable();
            LoadCategory();
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void LoadTable()
        {   flpTable.Controls.Clear();

            List<Table> tableList =  TableDAO.Instance.LoadTableList();

            foreach(Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight};
                btn.ForeColor = Color.Black;
                if(item.Status == "Trống")
                {
                    btn.BackColor = Color.Aqua;
                }
                else
                {
                    btn.BackColor = Color.LightCoral;
                }
                btn.Click += btn_Click;
                btn.Tag = item;
                btn.Text = item.Name + Environment.NewLine + item.Status;
                flpTable.Controls.Add(btn);
            }
        }
        void ShowBill(int id)
        {   lsvBill.Items.Clear();
            float  totalPrice = 0;
            List<DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenyByTable(id) ;
            foreach(DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                lsvBill.Items.Add(lsvItem);  
                totalPrice += item.TotalPrice;
            }
            CultureInfo culture = new CultureInfo("vi-VN");

            txbTotalPrice.Text = totalPrice.ToString("c",culture);  

        }
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);

        }

   

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if(cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void button1_Click(object sender, EventArgs e) //ADD FOOD
        {
            Table table = lsvBill.Tag as Table;  

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int idFood = (cbFood.SelectedItem as Food).Id;
            int count = (int)nmFoodCount.Value;
            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(),idFood,count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }
            ShowBill(table.ID);
            LoadTable();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e) //CHECKOUT
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);

            if(idBill != -1)
            {   
                if(MessageBox.Show("Bạn có chắc thanh toán hóa đơn cho bàn " + table.Name,"Thông báo",MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill);
                    ShowBill(table.ID);
                    LoadTable();
                }
                    
            }
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountProfile f = new AccountProfile();
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();      
            f.ShowDialog();
        }

        private void TableManager_Load(object sender, EventArgs e)
        {

        }

        private void cbFood_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flpTable_Paint(object sender, PaintEventArgs e)
        {

        }

        
    }
}
