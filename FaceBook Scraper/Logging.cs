using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FaceBook_Scraper
{
    public static class Logging
    {
        public static void WriteLog(string message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Scrapper.txt", true);
                sw.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
