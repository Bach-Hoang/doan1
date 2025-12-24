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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {   
            string username = txbUserName.Text;
            string password = txbPassword.Text;
            if (login(username, password))
            {
                AccountDAO.LoginAccount = AccountDAO.Instance.GetAccountByUserName(username);
                TableManager f = new TableManager();
            this.Hide();
            f.ShowDialog();
            this.Show();    
            }
            
        }
        bool login(string username, string password)
        {
            return AccountDAO.Instance.Login(username, password);
        }   
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }   
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
