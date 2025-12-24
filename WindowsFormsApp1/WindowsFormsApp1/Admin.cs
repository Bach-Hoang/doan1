using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;

namespace WindowsFormsApp1
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
            LoadDateTimePickerBill();
            LoadListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, 1);
        }

        
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dtgvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tcAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDateAndPage(DateTime checkIn, DateTime checkOut,int page)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListByDateAndPage(checkIn, checkOut,page);
        }
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value,1);
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";

        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 10;

            if(sumRecord % 10 != 0)
                lastPage++;
            txbPageBill.Text = lastPage.ToString(); 
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);

            if(page>1)
                page--;
            txbPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if((page*10)<sumRecord)
                page++;
            txbPageBill.Text = page.ToString();
        }
        private void ExportAdminReportToPdf(DataGridView dgv, string fromDate, string toDate)
        {
            // 1. Khởi tạo tài liệu
            Document document = new Document();
            Section section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape; // Chuyển sang khổ ngang nếu nhiều cột

            // --- TIÊU ĐỀ BÁO CÁO ---
            Paragraph title = section.AddParagraph("BÁO CÁO DOANH THU");
            title.Format.Font.Size = 18;
            title.Format.Font.Bold = true;
            title.Format.Alignment = ParagraphAlignment.Center;

            // --- THỜI GIAN (Lấy từ DateTimePicker) ---
            Paragraph period = section.AddParagraph($"Giai đoạn: {fromDate} - {toDate}");
            period.Format.Alignment = ParagraphAlignment.Center;
            period.Format.SpaceAfter = 15;

            // --- BẢNG DỮ LIỆU (Lấy từ DataGridView) ---
            Table table = section.AddTable();
            table.Borders.Width = 0.5;
            table.Rows.Alignment = RowAlignment.Center;
            // Tự động tạo cột dựa trên DataGridView
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                table.AddColumn(Unit.FromCentimeter(18.0 / dgv.ColumnCount)); // Chia đều độ rộng
            }

            // Tiêu đề cột
            Row headerRow = table.AddRow();
            headerRow.Shading.Color = Colors.LightGray;
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                headerRow.Cells[i].AddParagraph(dgv.Columns[i].HeaderText);
            }

            // Dữ liệu từ DataGridView
            foreach (DataGridViewRow dgvRow in dgv.Rows)
            {
                if (!dgvRow.IsNewRow)
                {
                    Row row = table.AddRow();
                    for (int i = 0; i < dgv.ColumnCount; i++)
                    {
                        string value = dgvRow.Cells[i].Value?.ToString() ?? "";
                        row.Cells[i].AddParagraph(value);
                    }
                }
            }

            // 3. Render và lưu file
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();

            string filename = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";
            pdfRenderer.PdfDocument.Save(filename);

            // Mở file sau khi tạo
            Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Lấy giá trị chuỗi từ DateTimePicker
            string fromDate = dtpkFromDate.Value.ToString("dd/MM/yyyy");
            string toDate = dtpkToDate.Value.ToString("dd/MM/yyyy");

            // dgvBill là tên DataGridView hiển thị doanh thu của bạn
            ExportAdminReportToPdf(dtgvBill, fromDate, toDate);
        }
    }
}
