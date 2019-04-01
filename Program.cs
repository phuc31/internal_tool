using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using OutlookApp = Microsoft.Office.Interop.Outlook.Application;

namespace ConsoleApp1
{
    class Program
    {
        private static object myCI;

        static void Main(string[] args)
        {
            const string startLine = "<p style=\"padding: 0px; margin: 0px; margin - auto:0px; mso - line - height - rule: exactly; line - height:110 %; \">";
            const string endLine = "<br></p>\n";
            const string header = "<html>\n<body style=\"font - family:Arial; font - size:12pt;\">\n";
            const string bottom = "</body>\n</html>";
            const string tabCharacter = "<span style='mso-tab-count:1'>            </span>";
            int currentWeek = 14;
            string text = File.ReadAllText(@"C:\Workspace\template.html");
            string[] lines = File.ReadAllLines(@"C:\Workspace\template1.html");

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace("\t", tabCharacter);
                lines[i] = startLine + lines[i] + endLine;
            }

            text = header + string.Join("", lines) + bottom;

            int thisWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            text = string.Format(text, thisWeek, thisWeek + 1);
            OutlookApp app = new OutlookApp();
            MailItem mailItem = app.CreateItem(OlItemType.olMailItem);

            mailItem.To = "prj_mbd_rvc";
            mailItem.Subject = "Weekly report week " + currentWeek.ToString();
            //mailItem.HTMLBody = "<html>" +
            //    "<body style=font-family:Arial;font-size:12pt>This is the <strong>funcky</strong> message" +
            //    "</body></html>";
            mailItem.HTMLBody = text;
            mailItem.SaveAs(@"C:\Workspace\test.msg");
            System.Diagnostics.Process.Start(@"C:\Workspace\test.msg");
        }
    }
}
