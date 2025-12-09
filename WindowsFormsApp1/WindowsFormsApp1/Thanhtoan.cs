using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;

namespace WindowsFormsApp1
{
    public partial class Thanhtoan : Form
    {
        private int idBill;
        private int discount;
        private float finalTotalPrice;
        public Thanhtoan(int idBill, string table, int discount, List<DTO.Menu> listMenu,DateTime dateCheckIn)
        {
            InitializeComponent();
            this.idBill = idBill;
            this.discount = discount;

        
            lbTable.Text = table; 
            lbDiscount.Text = discount.ToString() + "%"; 
            lbDatecheckin.Text = dateCheckIn.ToString("dd/MM/yyyy HH:mm:ss");

          
            lbDatecheckout.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
          
            LoadListMenu(listMenu);
        }

        void LoadListMenu(List<DTO.Menu> listMenu)
        {
            lsvBill.Items.Clear();
            float rawTotalPrice = 0;

            CultureInfo culture = new CultureInfo("vi-VN");

            foreach (DTO.Menu item in listMenu)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString("c", culture)); 
                lsvItem.SubItems.Add(item.TotalPrice.ToString("c", culture)); 
                lsvBill.Items.Add(lsvItem);

  
                rawTotalPrice += item.TotalPrice;
            }

    
            this.finalTotalPrice = rawTotalPrice - (rawTotalPrice * this.discount / 100);

            lbTotalprice.Text = rawTotalPrice.ToString("c", culture);
            lbLastprice.Text = this.finalTotalPrice.ToString("c", culture);
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận thanh toán hóa đơn này?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                BillDAO.Instance.CheckOut(idBill, discount,finalTotalPrice);
     
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Thanhtoan_Load(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void lbId_Click(object sender, EventArgs e)
        {

        }
    }
}
