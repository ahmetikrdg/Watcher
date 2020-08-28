using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Threading;

namespace Wather1._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Thread thread1;
        int durum = 1;
        string textler;
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "İstek Gönderilen Site";
            dataGridView1.Columns[1].Name = "Yanıt Verme Süresi";

        }

        public static string ChromeTest(string url)
        {
            var chromeDriverServices = ChromeDriverService.CreateDefaultService();
            chromeDriverServices.HideCommandPromptWindow = true;
            var time1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var options = new ChromeOptions();
            options.AddArguments("headless");
            var driverChrome = new ChromeDriver(chromeDriverServices,options);
            driverChrome.Url = url;
             var time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; //600000
              if (time2 > 60000)
              {
                string path = string.Format(@"c:\log\ScreenShotAT{0:d_M_yyyy_HH_mm_ss}.png", DateTime.Now);
                ITakesScreenshot screenshotHandler = driverChrome as ITakesScreenshot;
                Screenshot screenshot = screenshotHandler.GetScreenshot();
                screenshot.SaveAsFile(path, ScreenshotImageFormat.Jpeg);
            }

            var result =(time2 - time1).ToString();
            return result;
        }

        /*public static string FirefowTest(string url)
        {
            var time3 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            FirefoxOptions foptions = new FirefoxOptions();
            foptions.AddArguments("--headless");
            var driverFirefox = new FirefoxDriver(foptions);
            driverFirefox.Navigate().GoToUrl(url);

            driverFirefox.Quit();
            var time4 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            var result = "-----------------------" + url + "Firefox Sayfa Yanıt Süresi: " + (time4 - time3);
            return result;
        }*/
        
        
        private void stop_Click(object sender, EventArgs e)
        {
            durum = 0;
        }

        private void btnexcel_Click(object sender, EventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            object Missing = Type.Missing;
            Workbook workbook = excel.Workbooks.Add(Missing);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
            int StartCol = 1;
            int StartRow = 1;
            for (int j = 0; j < dataGridView1.Columns.Count; j++)
            {
                Range myRange = (Range)sheet1.Cells[StartRow, StartCol + j];
                myRange.Value2 = dataGridView1.Columns[j].HeaderText;
            }
            StartRow++;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {

                    Range myRange = (Range)sheet1.Cells[StartRow + i, StartCol + j];
                    myRange.Value2 = dataGridView1[j, i].Value == null ? "" : dataGridView1[j, i].Value;
                    myRange.Select();
                }
            }
        }

        private void btnekle_Click(object sender, EventArgs e)
        {
            textler = txtekle.Text; 
        }
        private void btnstart_Click(object sender, EventArgs e)
        {
            thread1 = new Thread(new ThreadStart(Baslat));
            thread1.IsBackground = true;
            thread1.Start();
            void Baslat()
            {
                for (; ; )
                {
                    if (durum == 0)
                    {
                        break;
                    }
                    var lists = new List<Adds>()
                    {
                        new Adds(){Url="http://halic.edu.tr/tr"},
                        new Adds(){Url="http://saglikbilimleriyuksekokulu.halic.edu.tr/tr"},
                     //   new Adds(){Url=textler},

                    };

                    foreach (var item in lists)
                    {
                        var r=  ChromeTest(item.Url);  
                       
                        string[] row = new string[] { item.Url , r};
                        dataGridView1.Invoke(new System.Action(() =>
                        {
                            dataGridView1.Rows.Add(row);
                        }));
                    }
                    
                }
            }
        }

        
    }
    public class Adds
    {
        public string Url { get; set; }
    }
}

//var time1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
//var options = new ChromeOptions();
//options.AddArguments("headless");
//var driverChrome = new ChromeDriver(options);
//driverChrome.Url = "http://www.halic.edu.tr/tr";
//var time2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
// string[] row = new string[] { "http://www.halic.edu.tr/tr", "Chorome Sayfa Yanıt Süresi: ", Convert.ToString(time2 - time1) };
//dataGridView1.Invoke(new System.Action(() => { dataGridView1.Rows.Add(row); }));
//Thread.Sleep(1000);