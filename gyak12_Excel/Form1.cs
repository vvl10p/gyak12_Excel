using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace gyak12_Excel
{
    public partial class Form1 : Form
    {
        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;
        public Form1()
        {
            InitializeComponent();
            CreateExcel();
            CreateTable();
        }
        void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();

                xlWB = xlApp.Workbooks.Add(Missing.Value);

                xlSheet = xlWB.ActiveSheet;

                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");

                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }
        void CreateTable()
        {
            string[] fejl�cek = new string[] {
                "K�rd�s",
                "1. v�lasz",
                "2. v�laszl",
                "3. v�lasz",
                "Helyes v�lasz",
                "k�p"
            };

            for (int i = 0; i < fejl�cek.Length; i++)
            {
                xlSheet.Cells[1, 1] = fejl�cek[0];
            }

            Models.HajosContext hajosContext = new Models.HajosContext();
            var mindenK�rd�s = hajosContext.Questions.ToList();

            object[,] adatT�mb = new object[mindenK�rd�s.Count(), fejl�cek.Count()];

            for (int i = 1; i < mindenK�rd�s.Count(); i++)
            {
                adatT�mb[i, 0] = mindenK�rd�s[i].Question1;
                adatT�mb[i, 1] = mindenK�rd�s[i].Answer1;
                adatT�mb[i, 2] = mindenK�rd�s[i].Answer2;
                adatT�mb[i, 3] = mindenK�rd�s[i].Answer3;
                adatT�mb[i, 4] = mindenK�rd�s[i].CorrectAnswer;
                adatT�mb[i, 5] = mindenK�rd�s[i].Image;
            }

            int sorokSz�ma = adatT�mb.GetLength(0);
            int oszlopokSz�ma = adatT�mb.GetLength(1);

            Excel.Range adatRange = xlSheet.get_Range("A2", Type.Missing).get_Resize(sorokSz�ma, oszlopokSz�ma);
            adatRange.Value2 = adatT�mb;

            adatRange.Columns.AutoFit();

            //formatTable();
            Excel.Range fejll�cRange = xlSheet.get_Range("A1", Type.Missing).get_Resize(1, 6);
            fejll�cRange.Font.Bold = true;
            fejll�cRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            fejll�cRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            fejll�cRange.EntireColumn.AutoFit();
            fejll�cRange.RowHeight = 40;
            fejll�cRange.Interior.Color = Color.Fuchsia;
            fejll�cRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            adatRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
            

        }
    }
}