using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaperFormatDetection.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public string PaperName { get; set; }
        public DateTime DetectTime { get; set; }
        public int ErrorNum { get; set; }
        public string ReportName { get; set; }
    }
}