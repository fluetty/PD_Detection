using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PaperFormatDetection.Models;

namespace PaperFormatDetection.Controllers
{
    public class UserController : Controller
    {
        private DBContext db = new DBContext();

        // GET: /User/
        [Route("User/Login")]
        public ActionResult Login()
        {
            return View("Login");
        }
        [Route("User/Register")]
        public ActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        [Route("User/Login")]
        public string Login(FormCollection form)
        {
            string name = Request.Form["username"];
            string pwd = Request.Form["password"];
            List<Models.User> u = db.Users.Where(p => p.StuNum == name && p.Password==pwd).ToList();
            int status = 1;
            if(u.Count>0)
            {
                User user = u[0];
                status = 0;
                HttpCookie cook = new HttpCookie("userInfo");
                cook.Values.Add("stuName",user.Name);
                cook.Values.Add("stuNum", user.StuNum);
                cook.Values.Add("stuType", user.StuType);
                Response.SetCookie(cook);//若已有此cookie，更新内容  
                Response.Cookies.Add(cook);//添加此cookie 
            }
            return "{ \"status\" : " + status + " }";
        }
        [HttpPost]
        [Route("User/Register")]
        public ActionResult Create(FormCollection form/*[Bind(Include = "Name,Password,StuNum,School,Grade,Phone,StuType")] User user*/)
        {
            string Name = Request.Form["Name"];
            string StuNum = Request.Form["StuNum"];
            string Password = Request.Form["Password"];
            string Phone = Request.Form["Phone"];
            string StuType = Request.Form["StuType"];
            string School = Request.Form["School"];
            string Grade = Request.Form["Grade"];
            User user=new User();
            user.Name = Name;
            user.StuNum = StuNum;
            user.Password = Password;
            user.Phone = Phone;
            user.StuType = StuType;
            user.School = School;
            user.Grade = int.Parse(Grade);
            db.Users.Add(user);
            db.SaveChanges();
            return View("Login");
        }

        [Route("User/findUserByStuNum/{StuNum}")]
        // GET: /User/Details/5
        public string Details(string StuNum)
        {
            List<Models.User> u = db.Users.Where(p => p.StuNum == StuNum).ToList();
            int status = 0;
            if (u.Count == 0)
            {
                status = 1;
            }
            return "{ \"status\" : " + status + " }";
        }

        [Route("User/Logout")]
        public ActionResult Logout()
        {
            HttpCookie cook = Request.Cookies["userInfo"];
            if (cook != null)
            {
                cook.Expires = DateTime.Now.AddDays(-1);  
            }
            return View("../Home/Index");
        }



        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        public void writelog(string str)
        {
            using (System.IO.StreamWriter sw =
    System.IO.File.AppendText(Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString()) + "\\Resource\\log.txt"))
            {
                sw.WriteLine(str);
            }
        }
    }
}
