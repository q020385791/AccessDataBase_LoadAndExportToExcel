using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace AccCount
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void TxtSelectByText_Click(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+ label1.Text + "");
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            var sqlquery = "";
            try
            {
                sqlquery = txtQuery.Text;
                OleDbDataAdapter Adapter = new OleDbDataAdapter();
                DataSet dataSet = new DataSet();
                //OleDbCommand cmd = new OleDbCommand(sql, connection);
                Adapter.SelectCommand = new OleDbCommand(sqlquery, conn);
                Adapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
                dataGridView1.Refresh();
                dataGridView1.Columns[0].Selected = true;
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Btnexport_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            save.FileName = "Excel Export demo";
            save.Filter = "*.xlsx|*.xlsx";
            if (save.ShowDialog() != DialogResult.OK) return;

            // Excel 物件
            Excel.Application xls = null;
            try
            {
                xls = new Excel.Application();
                // Excel WorkBook
                Excel.Workbook book = xls.Workbooks.Add();
                Excel.Worksheet Sheet = xls.ActiveSheet;
                // 把 DataGridView 資料塞進 Excel 內
                DataGridView2Excel(Sheet);
                // 儲存檔案
                book.SaveAs(save.FileName);
                MessageBox.Show("轉換完成");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                xls.Quit();
            }
        }
        private void DataGridView2Excel(Excel.Worksheet Sheet)
        {
            var Rows=dataGridView1.Rows.Count;
            var ColumnCount = dataGridView1.Columns.Count;

            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    try
                    {

                        if (!string.IsNullOrEmpty(dataGridView1[j, i].Value.ToString()))
                        {
                           
                            Sheet.Cells[i +2, j+1] = dataGridView1[j, i].Value.ToString();
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
            //插入標頭
            for (int i = 0; i < ColumnCount; i++)
            {
                Sheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }
        }
        private void BtnGetFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog flog = new OpenFileDialog();
            flog.Filter = "All Files (*.*)|*.*";
            flog.FilterIndex = 1;
            flog.Multiselect = true;
            if (flog.ShowDialog() == DialogResult.OK)
            {
                string sFileName = flog.FileName;
                string[] arrAllFiles = flog.FileNames; //used when Multiselect = true       
                label1.Text = sFileName;
            }
        }
    }
}
