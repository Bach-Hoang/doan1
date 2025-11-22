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
        }

        void LoadTable()
        {
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
            ShowBill(tableID);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

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
    }
}
