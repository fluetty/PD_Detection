using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using PaperFormatDetection.Models;
using System.Diagnostics;

namespace PaperFormatDetection.Controllers
{
    public class PaperController : Controller
    {
        private DBContext db = new DBContext();

        // GET: /Paper/
        [Route("Paper/Records")]
        public ActionResult Records()
        {
            return View(db.Reports.ToList());
        }
        [Route("Paper/Detection")]
        public ActionResult Detection()
        {
            return View();
        }

        // GET: /Paper/UploadPaper
        [HttpPost]
        [Route("Paper/UploadPaper")]
        public String Upload(FormCollection form)
        {
            int status = 1;
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                //return View("Detection");
            }
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                //return View("Detection");
            }
            else
            {
                try
                {
                    //文件大小不为0
                    file = Request.Files[0];
                    //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                    //服务器上的UpLoadFile文件夹必须有读写权限
                    string target = Server.MapPath("/") + ("/Data/Papers/");//取得目标文件夹的路径
                    string filename = file.FileName;//取得文件名字
                    string path = target + filename;//获取存储的目标地址
                    string paperType = "1";
                    file.SaveAs(path);
                    //可执行文件的目录
                    string exeEnvironmentDir = @"C:\\Users\\Zhang_weiwei\\Desktop\\PD_web\\Paper\\PaperFormatDetection\\PaperFormatDetection\\PaperFormatDetection\\bin\\Debug";
                    Process proc = new Process();
                    proc.StartInfo.FileName = exeEnvironmentDir + "/PaperFormatDetection.exe";
                    //可以用绝对路径 
                    proc.StartInfo.Arguments = path + " " + exeEnvironmentDir + "  " + paperType;
                    proc.Start();
                    proc.WaitForExit();
                    status = 0;
                }
                catch (Exception e)
                {

                }
            }
            return "{ \"status\" : " + status + " }";
        }
        [HttpGet]
        [Route("Paper/Download")]
        public FileResult download()
        {
            string filePath = Server.MapPath("/") + ("/Data/Reports/大连理工大学博士学位论文模板.docx");//路径
            return File(filePath, "text/plain", "大连理工大学博士学位论文模板.docx"); //客户端保存的名字
        }











        // POST: /Paper/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,PaperName,DetectTime,ErrorNum,ReportName")] Report report)
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
        public ActionResult Edit([Bind(Include="Id,PaperName,DetectTime,ErrorNum,ReportName")] Report report)
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
