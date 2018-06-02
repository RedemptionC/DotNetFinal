using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace finalDotNet.Controllers
{
    public class AdminController : Controller
    {
        finalModel db = new finalModel();
        // GET: Admin
        public ActionResult Index()
        {
            if((string)Session["userid"]!="admin")
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            return View();
        }
        public ActionResult Login()
        {
            return RedirectToAction("Admin", "Home");
        }
        //public ActionResult Logout()
        //{
        //    return RedirectToAction("Logout", "Home");
        //}
        //admin only
        public ActionResult register()
        {
            if ((string)Session["userid"] != "admin")
            {
                ViewBag.ErrorMessage = "only one admin";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            else
                return View();
        }
        public ActionResult Approve()
        {
            ViewBag.Title = "公告审核";

            if ((string)Session["userid"] == "admin")
            {
                var rs = from m in db.noticetable where m.isApproved == 0 select m;
                var desc = rs.OrderByDescending(i => i.submitTime);
                return View(desc.ToList());
            }
            else
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");

            }

        }
        public ActionResult Approved(int id)
        {
            if ((string)Session["userid"] != "admin")
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            var rs = db.noticetable.Find(id);
            rs.isApproved = 1;
            db.SaveChanges();
            return RedirectToAction("Approve");
        }
        public ActionResult Detail(int id)
        {
            if ((string)Session["userid"] != "admin")
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            return RedirectToAction("Detail", "Home",new { id=id});
        }
        public ActionResult noticeMag()
        {
            ViewBag.Title = "公告管理";

            if ((string)Session["userid"] != "admin")
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            var rs = from m in db.noticetable where m.isApproved == 1 select m;
            var desc = rs.OrderByDescending(i => i.submitTime);
            return View(desc.ToList());
        }
        public ActionResult noticedelete(int id)
        {
            if ((string)Session["userid"] == "admin")
            {
                var rs = db.noticetable.Find(id);
                db.noticetable.Remove(rs);
                db.SaveChanges();
                return RedirectToAction("noticeMag");
            }
            else
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");

            }
        }
        public ActionResult noticemodified(int id)
        {
            ViewBag.Title = "修改";

            if ((string)Session["userid"] == "admin")
            {
                var rs = db.noticetable.Find(id);
                return View(rs);
            }
            else
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }

        }
        [HttpPost]
        public ActionResult noticemodified(FormCollection fc,noticetable n1)
        {
            var n = db.noticetable.Find(n1.id);
            //UpdateModel(n);
            string title = fc["title"], content = fc["content"], kind = fc["kind"];
            n.title = title; n.content = content; string k = kind;
            if (k == "")
                n.kind = "all";
            else
            {
                n.kind = k;

            }
            db.SaveChanges();
            db.SaveChanges();
            return RedirectToAction("noticeMag");
        }
        public ActionResult Logout()
        {
            if ((string)Session["userid"] != "admin")
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            //登录动作在Home控制器里定义，保证在MyOrder控制器里能登出
            return RedirectToAction("Logout", "Home");
        }
        public ActionResult memberMag()
        {
            if ((string)Session["userid"] != "admin")
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
            return View(db.usertable.ToList());
        }
        public ActionResult memberdelete(string id)
        {

            if ((string)Session["userid"] == "admin")
            {
                var rs = db.usertable.Find(id);
            db.usertable.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("memberMag");
            }
            else
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }

        }
        public ActionResult membermodified(string id)
        {
            if ((string)Session["userid"] == "admin")
            {
                var rs =db.usertable.Find(id);
            return View(rs);
            }
            else
            {
                ViewBag.ErrorMessage = "ADMIN ONLY";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }

        }
        [HttpPost]
        public ActionResult membermodified(usertable u)
        {
            var rs = db.usertable.Find(u.username);
            //rs.email = u.email;
            //rs.qq = u.qq;
            UpdateModel(rs);
            //db.Entry(u).State = System.Data.Entity.EntityState.Modified;
            //这个方法确实可以，但是首先传过来的信息要完整，其次内部的机理还不是很了解
            db.SaveChanges();
            return RedirectToAction("memberMag");
                
        }
    }
}