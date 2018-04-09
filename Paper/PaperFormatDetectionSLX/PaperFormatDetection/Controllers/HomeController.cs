using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaperFormatDetection.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [Route("Index")]
        [Route("Home/Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("Guide")]
        [Route("Home/Guide")]
        public ActionResult Guide()
        {
            return View();
        }
        [Route("Questions")]
        [Route("Home/Questions")]
        public ActionResult Questions()
        {
            return View();
        }
        [Route("Feedback")]
        [Route("Home/Feedback")]
        public ActionResult Feedback()
        {
            return View();
        }
        [Route("Templates")]
        [Route("Home/Templates")]
        public ActionResult Templates()
        {
            return View();
        }
        [HttpGet]
        [Route("Download/Template/{id}")]
        [Route("Home/Download/Template/{id}")]
        public FileResult DownloadTemplate(int id)
        {
            string fileName=null;
            if (id == 0)
                fileName = "大连理工大学本科毕业论文模版.doc";
            else if (id == 1)
                fileName = "大连理工大学硕士学位论文模版.doc";
            else
                fileName = "大连理工大学博士学位论文格式规范.docx";
            string filePath = Server.MapPath("/") + ("/Resource/Template/"+fileName);//路径
            return File(filePath, "text/plain", fileName); //客户端保存的名字
        }

    }
}