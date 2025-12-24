using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics; // Dùng để mở file sau khi tạo
using System.Drawing;
using System.Globalization;
using System.IO;
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
            
            lbnamenv.Text = AccountDAO.LoginAccount.DisplayName;
            lbphonenumber.Text = AccountDAO.LoginAccount.PhoneNumber;
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
                List<DTO.Menu> listMenuChiTiet = GetListMenuFromListView(lsvBill);
                string maHoaDon = lbId.Text; 
                string tenBan = lbTable.Text; 

                
                string gioVao = lbDatecheckin.Text; 
                string gioRa = lbDatecheckout.Text;


                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("vi-VN");

                
                double tongTien = 0;
                double.TryParse(lbTotalprice.Text, System.Globalization.NumberStyles.Currency, culture, out tongTien);

                double giamGia = 0;
                
                string giamGiaText = lbDiscount.Text.Replace("%", "").Trim();
                double.TryParse(giamGiaText, out giamGia);

                double thanhTien = 0;
                double.TryParse(lbLastprice.Text, System.Globalization.NumberStyles.Currency, culture, out thanhTien);
                ExportInvoiceToPdf(listMenuChiTiet, maHoaDon, tenBan, gioVao, gioRa, tongTien, giamGia, thanhTien, AccountDAO.LoginAccount.DisplayName, AccountDAO.LoginAccount.PhoneNumber);
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
        
 

    private List<DTO.Menu> GetListMenuFromListView(ListView lsvBill)
    {
        List<DTO.Menu> listMenu = new List<DTO.Menu>();
        CultureInfo culture = new CultureInfo("vi-VN"); 

        foreach (ListViewItem item in lsvBill.Items)
        {
            

           
            string FoodName = item.SubItems[0].Text;

           
            int count = 0;
            int.TryParse(item.SubItems[1].Text, out count);
            int Count = count;

           
            string priceText = item.SubItems[2].Text;
            float price = 0;
           
            float.TryParse(priceText, NumberStyles.Currency, culture, out price);
            float Price = price;

            
            float totalPrice = 0;
            float.TryParse(item.SubItems[3].Text, NumberStyles.Currency, culture, out totalPrice);
            float TotalPrice = totalPrice;

                DTO.Menu menu = new DTO.Menu(FoodName, count, Price, TotalPrice );

                listMenu.Add(menu);
        }

        return listMenu;
    }


    private void ExportInvoiceToPdf(List<DTO.Menu> listMenu, string maHD, string tenBan, string gioVao, string gioRa, double tongTien, double giamGia, double thanhTien,string displayName,string sdt)
    {
        
        Document document = new Document();
        document.Info.Title = "Hóa Đơn Thanh Toán - " + tenBan;

        
        Section section = document.AddSection();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.TopMargin = Unit.FromCentimeter(2);

        
        Paragraph title = section.AddParagraph("HÓA ĐƠN THANH TOÁN");
        title.Format.Font.Size = 18;
        title.Format.Font.Bold = true;
        title.Format.Alignment = ParagraphAlignment.Center;
        title.Format.SpaceAfter = 10;

        section.AddParagraph($"Nhân viên: {displayName}");
        section.AddParagraph($"Số điện thoại NV: {sdt}");
        section.AddParagraph("Nhà hàng: Gemini Restaurant").Format.Font.Bold = true;
        section.AddParagraph("Địa chỉ: 123 Đường ABC, Hà Nội");
        section.AddParagraph($"Ngày: {DateTime.Now.ToString("dd/MM/yyyy")}");
        section.AddParagraph($"Mã hóa đơn: {maHD}"); 
        section.AddParagraph().Format.SpaceAfter = 5;

        
        Table infoTable = section.AddTable();
        infoTable.AddColumn(Unit.FromCentimeter(8));
        infoTable.AddColumn(Unit.FromCentimeter(8));

        Row row1 = infoTable.AddRow();
        row1.Cells[0].AddParagraph("Bàn: " + tenBan).Format.Font.Bold = true;
        row1.Cells[1].AddParagraph("Tổng tiền: " + tongTien);

        Row row2 = infoTable.AddRow();
        row2.Cells[0].AddParagraph("Giờ vào: " + gioVao);
        row2.Cells[1].AddParagraph("Giảm giá: " + giamGia);

        Row row3 = infoTable.AddRow();
        row3.Cells[0].AddParagraph("Giờ ra: " + gioRa);
        row3.Cells[1].AddParagraph("Thành tiền: " + thanhTien).Format.Font.Bold = true;

        section.AddParagraph().Format.SpaceAfter = 15;

       
        section.AddParagraph("DANH SÁCH MÓN ĂN").Format.Font.Bold = true;
        Table table = section.AddTable();
        table.Borders.Width = 0.5;

        table.AddColumn(Unit.FromCentimeter(6.5)); 
        table.AddColumn(Unit.FromCentimeter(1.5)); 
        table.AddColumn(Unit.FromCentimeter(4));  
        table.AddColumn(Unit.FromCentimeter(4));  

        Row headerRow = table.AddRow();
        headerRow.Shading.Color = Colors.LightGray;
        headerRow.Format.Font.Bold = true;
        headerRow.Cells[0].AddParagraph("Tên món");
        headerRow.Cells[1].AddParagraph("SL");
        headerRow.Cells[2].AddParagraph("Đơn giá");
        headerRow.Cells[3].AddParagraph("Thành tiền");

        CultureInfo culture = new CultureInfo("vi-VN");
        foreach (var item in listMenu)
        {
            Row dataRow = table.AddRow();
            dataRow.Cells[0].AddParagraph(item.FoodName);
            dataRow.Cells[1].AddParagraph(item.Count.ToString());
            dataRow.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            dataRow.Cells[2].AddParagraph(item.Price.ToString("c", culture));
            dataRow.Cells[3].AddParagraph(item.TotalPrice.ToString("c", culture));
        }

        
        section.AddParagraph().Format.SpaceBefore = 10;
        Paragraph footerPrice = section.AddParagraph();
        footerPrice.Format.Alignment = ParagraphAlignment.Right;
        footerPrice.AddText($"Tổng tiền: {tongTien}\n");
        footerPrice.AddText($"Giảm giá: {giamGia}\n");

        Paragraph finalPricePara = section.AddParagraph();
        finalPricePara.Format.Alignment = ParagraphAlignment.Right;
        finalPricePara.Format.Font.Size = 14;
        finalPricePara.Format.Font.Bold = true;
        finalPricePara.AddText($"THÀNH TIỀN: {thanhTien}");

        section.AddParagraph("\nCảm ơn quý khách và hẹn gặp lại!");
        section.LastParagraph.Format.Alignment = ParagraphAlignment.Center;
        section.LastParagraph.Format.Font.Italic = true;

       
        PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
        pdfRenderer.Document = document;
        pdfRenderer.RenderDocument();

        string filename = "Invoice_" + tenBan.Replace(" ", "") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";
        pdfRenderer.PdfDocument.Save(filename);

        Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
    }
}
}
