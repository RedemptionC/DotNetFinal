using finalDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace finalDotNet.Controllers
{
    public class HomeController : Controller
    {
        /*注意与codefirst的区别*/
        //sBoardEntities dbContext = new sBoardEntities();
        finalModel db = new finalModel();
        public ActionResult Index()
        {
            ViewBag.Title = "校园公告栏";
            var rs = from m in db.noticetable where m.isApproved == 1  select m;
            var desc=rs.OrderByDescending(i => i.submitTime);
            var kindrs = (from m in db.noticetable where m.isApproved == 1 select m.kind).Distinct();
            return View(desc.ToList());
            //return View(Tuple.Create(desc.ToList(),kindrs.ToList()))
        }
        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            string kind = fc["kind"], info = fc["info"];

            if (kind != "all")
            {
                if (info != null)
                {
                    var rs = from m in db.noticetable where m.isApproved == 1 && m.kind == kind && (m.author.Contains(info) || m.title.Contains(info)) select m;
                    var desc = rs.OrderByDescending(i => i.submitTime);
                    return View(desc.ToList());
                }
                else
                {
                    var rs = from m in db.noticetable where m.isApproved == 1 && m.kind == kind select m;
                    var desc = rs.OrderByDescending(i => i.submitTime);
                    return View(desc.ToList());
                }
            }
            else
            {
                if (info != null)
                {
                    var rs = from m in db.noticetable where m.isApproved == 1  && (m.author.Contains(info) || m.title.Contains(info)) select m;
                    var desc = rs.OrderByDescending(i => i.submitTime);
                    return View(desc.ToList());
                }
                else
                {   /*这里是空值*/
                    var rs = from m in db.noticetable where m.isApproved == 1  select m;
                    var desc = rs.OrderByDescending(i => i.submitTime);
                    return View(desc.ToList());
                }
            }

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string username = fc["name"];
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_in = Encoding.UTF8.GetBytes((string)fc["password"]);
            byte[] bytes_out = sha1.ComputeHash(bytes_in);
            sha1.Dispose();
            string result = BitConverter.ToString(bytes_out);
            result = result.Replace("-", "");
            string password = result;
            var rs = from m in db.usertable where m.username.Equals(username) && m.password.Equals(password) select m;
            if (rs.ToList().Count > 0)
            {
                Session["userid"] = username;
                /*这里应该转到一个已登陆的界面，或者在分部视图里修改，或者重新写个页面？直接进入个人页面？好像不行*/
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "用户名或密码错误";
                return View("~/Views/Shared/_ErrorMessage.cshtml");
            }
        }
        public ActionResult Logout()
        {
            Session["userid"] = null;  //登出，即清除用户会话信息            
            return RedirectToAction("Index");
        }
        public ActionResult register()
        {
            ViewBag.Title = "用户注册";

            return View();
        }
        [HttpPost]
        public ActionResult register(FormCollection fc,usertable u)
        {
            //if (ModelState.IsValid)
            {
                string username1 = fc["username"];
                var rs = from m in db.usertable where m.username.Equals(username1) select m;
                if (rs.ToList().Count > 0)
                {

                    ViewBag.ErrorMessage = "用户已存在";
                    return View("~/Views/Shared/_ErrorMessage.cshtml");
                }
                else
                {
                    SHA1 sha1 = new SHA1CryptoServiceProvider();
                    byte[] bytes_in = Encoding.UTF8.GetBytes((string)fc["password"]);
                    byte[] bytes_out = sha1.ComputeHash(bytes_in);
                    sha1.Dispose();
                    string result = BitConverter.ToString(bytes_out);
                    result = result.Replace("-", "");
                    usertable user = new usertable
                    {
                        username = username1,
                        password = result,
                        qq = (string)fc["qq"],
                        email = (string)fc["email"]

                    };
                    Console.WriteLine(user.ToString());
                    db.usertable.Add(user);
                    db.SaveChanges();
                    /*此处同鲜花网站，注册成功后直接返回首页，仍然是未登录状态*/
                    return RedirectToAction("Index");
                }
            }
            //return View(u);
        }
        public ActionResult Detail(int id)
        {
            var rs = db.noticetable.Find(id);
            return View(rs);

        }
        public ActionResult Admin()
        {
            ViewBag.Title = "管理员登录";
            return View();
        }
        [HttpPost]
        public ActionResult Admin(FormCollection fc)
        {
            ViewBag.Title = "管理员登录";

            /*这里设置唯一管理员，没有建表*/
            string name = fc["name"], password = fc["password"];
            if (name == "admin" && password == "password")
            {
                Session["userid"] = name;
                /*通过链接转向管理页*/
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = "用户名或密码错误";
            return View("~/Views/Shared/_ErrorMessage.cshtml");

        }
        public int ajaxifExist(string name)
        {
            var rs = from m in db.usertable where m.username.Equals(name) select m;
            if (rs.ToList().Count > 0)
                return 1;
            else
                return 0;
        }
        public ActionResult join()
        {

            var rs = (from m in db.usertable join n in db.noticetable on m.username equals (n.author) select new Models.@join { username = m.username, title = n.title });
            List<Models.join> list = rs.ToList();
            //ViewBag.joinrs = rs.ToList();
            //ViewData["rs"] = rs;
            return View(list);
        }
        public ActionResult noticeAndUser()
        {
            var notices = from m in db.noticetable select m;
            var users = from m in db.usertable select m;
            noticeAndUser nau = new noticeAndUser
            {
                notice = notices.ToList(),
                user = users.ToList()
            };
            return View(nau);
        }
    }
}