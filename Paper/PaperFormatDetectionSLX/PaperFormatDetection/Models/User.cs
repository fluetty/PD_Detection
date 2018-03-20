using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PaperFormatDetection.Models
{
    public class User
    {
        public int UserId{ get; set; }
        public string Name { get; set;}
        public string Password { get; set; }
        public string StuNum { get; set; }
        public string School { get; set; }
        public int Grade { get; set; }
        public string Phone { get; set; }
        public string StuType { get; set; }
        public List<Report> Reports { get; set; }
    }
    public class Report
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public string PaperName { get; set; }
        public DateTime DetectTime { get; set; }
        public int ErrorNum { get; set; }
        public string ReportName { get; set; }
    }
    public class DBContext:DbContext
    {
        public DBContext()  : base("name=PaperFormatDetectionDB")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Report> Reports { get; set; }
    } 
}