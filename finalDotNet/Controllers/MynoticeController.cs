using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalDotNet.Controllers
{
    public class MynoticeController : Controller
    {
        finalModel db = new finalModel();
        // GET: Mynotice
        public ActionResult Index()
        {
            ViewBag.Title = "我的公告";

            if (Session["userid"] == null)
            {
                ViewBag.ErrorMessage = "请先登陆";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            string author = Session["userid"].ToString();
            var rs = from m in db.noticetable where m.author == author select m;
            var desc = rs.OrderByDescending(i => i.submitTime);
            return View(desc);
        }
        public ActionResult Login()
        {
            return RedirectToAction("Login", "Home");
        }
        public ActionResult register()
        {
            return RedirectToAction("register", "Home");
        }
        public ActionResult Logout()
        {
            //登录动作在Home控制器里定义，保证在MyOrder控制器里能登出
            return RedirectToAction("Logout", "Home");
        }
        public ActionResult Addnotice()
        {
            if (Session["userid"] == null)
            {
                ViewBag.ErrorMessage = "请先登陆";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Addnotice(FormCollection fc,noticetable n1)
        {
            //if (ModelState.IsValid)
            //{
                noticetable n = new noticetable();
                n.author = (string)Session["userid"];
                n.title = fc["title"];
                string k= fc["kind"];
            if (k == "")
                n.kind = "all";
            else
            {
            n.kind = k;

            }
                n.content = fc["content"];
                n.isApproved = 0;
                n.submitTime = DateTime.Now;
                n.id = Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
                db.noticetable.Add(n);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //else
            //{
            //    return RedirectToAction("Addnotice");
            //}

        }
        public ActionResult Detail(int id)
        {
            ViewBag.Title = "详细";

            if ((string)Session["userid"] == null)
            {
                ViewBag.ErrorMessage = "请先登录";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            return RedirectToAction("Detail", "Home", new { id = id });

        }
        public ActionResult Delete(int id)
        {
            if (Session["userid"] == null)
            {
                ViewBag.ErrorMessage = "请先登陆";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            var rs = db.noticetable.Find(id);
            db.noticetable.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Modified(int id)
        {
            ViewBag.Title = "修改";

            if (Session["userid"] == null)
            {
                ViewBag.ErrorMessage = "请先登陆";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            var rs = db.noticetable.Find(id);
            //UpdateModel(rs);
            //db.SaveChanges();
            return View(rs);
        }
        [HttpPost]
        public ActionResult Modified(noticetable n,FormCollection fc)
        {
            var n1 = db.noticetable.Find(n.id);
            //UpdateModel(n);
            string title = fc["title"], content = fc["content"], kind = fc["kind"];
            n1.title = title; n1.content = content;
            string k = kind;
            if (k == "")
                n.kind = "all";
            else
            {
                n.kind = k;

            }
            //db.Entry(n).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}