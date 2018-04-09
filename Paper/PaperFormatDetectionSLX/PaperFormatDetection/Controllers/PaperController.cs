using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using PaperFormatDetection.Models;

namespace PaperFormatDetection.Controllers
{
    public class PaperController : Controller
    {
        private DBContext db = new DBContext();
        // GET: /Paper/
        [Route("Paper/Records/{pageIndex=1}/{pageSize=10}")]
        public ActionResult Records(int pageIndex , int pageSize )
        {
            //获取当前分页数据集合
            if (Request.Cookies["userInfo"] == null)
            {
                return RedirectToAction("Index", "Home");//重定向
            }
            int user_id = int.Parse(Request.Cookies["userInfo"]["stuID"]);
            var reports = db.Reports
                          .Where(p => p.UserId == user_id)
                          .OrderBy(p => p.DetectTime)
                          .Skip((pageIndex - 1) * pageSize)
                          .Take(pageSize);

            //将当前ViewModel传递给视图
            return View(new ReportViewModel
            {
                Reports = reports,
                PagingInfo = new PagingInfo
                {
                    TotalItems = db.Reports.Where(p => p.UserId == user_id).Count(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                }
            });
        }

        public ActionResult Index(int pageIndex = 1, int pageSize = 5)
        {
            //获取当前分页数据集合
            var reports = db.Reports
                          .OrderBy(p => p.DetectTime)
                          .Skip((pageIndex - 1) * pageSize)
                          .Take(pageSize);

            //将当前ViewModel传递给视图
            return View(new ReportViewModel
            {
                Reports = reports,
                PagingInfo = new PagingInfo
                {
                    TotalItems = db.Reports.Count(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                }
            });
        }

        [Route("Paper/Detection")]
        public ActionResult Detection()
        {
            HttpCookie cook = Request.Cookies["userInfo"];
            if (cook != null)
            {
                ViewBag.paperType = Request.Cookies["userInfo"]["stuType"];
                return View();
            }
            else
            {
                return View("~/Views/Home/Index.cshtml");
            }
        }
        // GET: /Paper/UploadPaper
        [HttpPost]
        [Route("Paper/UploadPaper")]
        public JsonResult Upload(FormCollection form)
        {
            string status = "NO";
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                //return View("Detection");
            }
            var file = Request.Files[0];
            string _pdf=null;
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                //return View("Detection");
            }
            else
            {
                try
                {
                    string paperType=Request.Form["paperType"];
                    string target = Server.MapPath("/") + ("/Data/Papers/");//取得目标文件夹的路径
                    string filename = Util.GetTimeStamp() + "_" + file.FileName;//取得文件名字
                    string path = target + filename;//获取存储的目标地址
                    //paperType = "1";
                    file.SaveAs(path);
                    //可执行文件的目录
                    string exeEnvironmentDir = @"C:/Users/Zhang_weiwei/Desktop/PD_web/Paper/PaperFormatDetection/PaperFormatDetection/bin/Debug";
                    Process proc = new Process();
                    proc.StartInfo.FileName = exeEnvironmentDir + "/PaperFormatDetection.exe";
                    //可以用绝对路径 
                    proc.StartInfo.Arguments = path + "  " + paperType;
                    //proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.Start();
                    proc.WaitForExit();
                    //保存数据库
                    _pdf = filename.Replace(".docx", ".pdf").Replace(".doc", ".pdf");
                    if(System.IO.File.Exists(Server.MapPath("/") + ("/Data/Reports/")+_pdf))
                    {
                        Report report = new Report();
                        report.UserId = int.Parse(Request.Cookies["userInfo"]["stuID"]);
                        report.PaperName = filename;
                        report.DetectTime = DateTime.Now;
                        report.ErrorNum = 0;
                        report.ReportName = _pdf;
                        db.Reports.Add(report);
                        db.SaveChanges();
                        status = "OK";
                    }
                    else
                    {
                        status = "NO";
                    }
                }
                catch (Exception e)
                {

                }
            }
            var result = new
            {
                STATUS = status,
                REPORT = "/Data/Reports/" + _pdf
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("Paper/Download/{id}")]
        public FileResult download(int id)
        {
            string reportName = db.Reports.Find(id).ReportName;
            string filePath = Server.MapPath("/") + ("/Data/Reports/" + reportName);//路径
            return File(filePath, "text/plain", reportName); //客户端保存的名字
        }
        [HttpPost]
        [Route("Home/submitFeedback")]
        public JsonResult submitFeedback()
        {
            string s = "NO";
            if (Request.Cookies["userInfo"]==null)
                s = "UNLOGIN";
            else
            {
                try
                {
                    Feedback fd = new Feedback();
                    fd.UserId = int.Parse(Request.Cookies["userInfo"]["stuID"]);
                    fd.FeedbackTime = DateTime.Now;
                    fd.Contents = Request.Form["Contents"];
                    db.Feedbacks.Add(fd);
                    db.SaveChanges();
                    s = "OK";
                }
                catch (Exception e)
                {

                }
            }
            var result = new
            {
                status = s
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }










        // POST: /Paper/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PaperName,DetectTime,ErrorNum,ReportName")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(report);
        }

        // GET: /Paper/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: /Paper/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PaperName,DetectTime,ErrorNum,ReportName")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: /Paper/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: /Paper/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}