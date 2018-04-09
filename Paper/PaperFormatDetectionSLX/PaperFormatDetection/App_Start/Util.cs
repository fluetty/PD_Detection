using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaperFormatDetection
{
    public static class Util
    {
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }  
        public static void log(string msg)
        {
            string path = "/Resource/log.txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(msg);
            }
        }
    }
}