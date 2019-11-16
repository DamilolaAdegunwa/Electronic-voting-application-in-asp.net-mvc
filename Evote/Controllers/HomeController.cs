using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Evote.Models;
namespace Evote.Controllers
{
    public class HomeController : Controller
    {
        hrEntities db = new hrEntities();
        public ActionResult test()
        {

            return View();
        }
        public ActionResult Logout()
        {
            Session["Username"] = "";
            Session["Password"] = "";
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            TempData["MessageType"] = "success";
            TempData["Message"] = "You have successfully logged out!!";
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            var model = new SignIn();
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(SignIn model)
        {
            var Username = model.Name;
            var Password = model.Password;
            Session["Username"] = "";
            Session["Password"] = "";
            foreach (var person in db.EligibleMembers)
            {
                if(Username.ToString().ToLower() == person.email.ToLower() && Password.ToString() == person.password)
                {
                    Session["Username"] = person.email;
                    Session["Password"] = person.password;
                    //TempData["MessageType"] = "success";
                    //TempData["Message"] = "Your login was successful!!";
                    return RedirectToAction("Homepage");
                }
            }
            TempData["MessageType"] = "danger";
            TempData["Message"] = "Invalid Username or Password!!";
            return View(model);
        }
        public FileContentResult GetFile(int id)
        {
            var ev = db.Contestants.Find(id);
            return File(ev.EligibleMember.ImageContent, ev.EligibleMember.ImageFileType);
        }
        public FileContentResult GetCampaignFile(int id)
        {
            var ev = db.Contestants.Find(id);
            return File(ev.CampaignPictureContent, ev.CampaignPictureFileType);
        }
        public FileContentResult GetMemberFile(int id)
        {
            var ev = db.EligibleMembers.Find(id);
            return File(ev.ImageContent, ev.ImageFileType);
        }
        public ActionResult Homepage()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            if (Session["Username"]?.ToString() == null || Session["Password"]?.ToString() == null)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please input your username and password!!";
                return Redirect("Login");
            }
            else
            {
                foreach (var person in db.EligibleMembers)
                {
                    if (Session["Username"].ToString().ToLower() == person.email.ToLower() && Session["Password"].ToString() == person.password)
                    {
                        ViewBag.memberId = person.Id;
                        return View();
                    }
                }
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid username and password combination!!";
                return Redirect("Login");
            }

            //return View();
        }
        [HttpPost]
        public ActionResult Homepage(int contestantId, int VoteSessionId)
        {
            var usr = Session["Username"];
            var pw = Session["Password"];
            //to make sure you have signed in.
            if (usr?.ToString() == null || pw?.ToString() == null)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please you have to sign in first!!";
                return Redirect("Login");
            }
            if(db.EligibleMembers.Where(dbl => dbl.email.ToLower() == usr.ToString().ToLower() && dbl.password == pw.ToString()).Count() == 0)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please enter a valid email amd password combination!!";
                return Redirect("Login");
            }
            //vote once
            foreach (var voterecord in db.voteLogs)
            {
                if (voterecord.VotingSessionId == VoteSessionId && voterecord.EligibleMember.email.ToLower() == usr.ToString().ToLower())
                {
                    TempData["MessageType"] = "danger";
                    TempData["Message"] = "You can only vote once!!";
                    return Redirect("Homepage");
                }
            }
            var person = db.EligibleMembers.Where(dbl => dbl.email.ToLower() == usr.ToString().ToLower() && dbl.password == pw.ToString()).First();
            voteLog c = new voteLog
            {
                datetime = DateTime.Now,
                contestantId = contestantId,
                VotingSessionId = VoteSessionId,
                memberId = person.Id
            };
            voteLog cc = db.voteLogs.Add(c);
            db.SaveChanges();
            ViewBag.memberId = person.Id;
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your vote was successful casted!!";
            return View();
        }
        public ActionResult Votepage()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult IndexTest()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            if (Session["Username"]?.ToString() == null || Session["Password"]?.ToString() == null)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please input your username and password!!";
                return Redirect("Login");
            }
            var usr = Session["Username"];
            var pw = Session["Password"];
            return View();
        }
        [HttpPost]
        public ActionResult IndexTest(int contestantId, int VoteSessionId)
        {
            var usr = Session["Username"];
            var pw = Session["Password"];
            var mt = Session["membershipType"];
            //to make sure you have signed in.
            if(usr?.ToString() == null || pw?.ToString() == null)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please you have to sign in first!!";
                return Redirect("/Home/IndexTest#warn");
            }
            //vote once
            foreach(var voterecord in db.voteLogs)
            {
                if(voterecord.VotingSessionId == VoteSessionId && voterecord.EligibleMember.email == usr.ToString())
                {
                    TempData["MessageType"] = "danger";
                    TempData["Message"] = "You can only vote once!!";
                    return Redirect("/Home/IndexTest#warn");
                }
            }
                var person = db.EligibleMembers.Where(dbl => dbl.email == usr.ToString()).First();
                voteLog c = new voteLog
                {
                    datetime = DateTime.Now,
                    contestantId = contestantId,
                    VotingSessionId = VoteSessionId,
                    memberId = person.Id
                };
                voteLog cc = db.voteLogs.Add(c);
                db.SaveChanges();
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your vote was successful casted!!";
            return Redirect("/Home/IndexTest#warn");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}