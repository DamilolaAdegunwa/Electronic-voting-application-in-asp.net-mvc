using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Microsoft.Office.Interop.Excel;
using System.Text;
using Evote.Models;
using System.Net;

namespace Evote.Controllers
{
    public class AdminController : Controller
    {
        hrEntities db = new hrEntities();
        //(1) This page was used in testing the template used in the layout
        public ActionResult Test()
        {
            return View();
        }
        //(2) This page is with the intent of viewing each voting summary
        public ActionResult Dashboard()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(3) This is definitely the entry page to the administrative end 
        public ActionResult Logout()
        {
            #region logout
            Session["Username"] = "";
            Session["Password"] = "";
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            TempData["MessageType"] = "success";
            TempData["Message"] = "You have successfully logged out!!";
            return RedirectToAction("Login");
            #endregion
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
            #region login
            Session["Username"] = "";
            Session["Password"] = "";
            foreach (var person in db.Admins)
            {
                if (model.Name.ToLower() == person.name.ToLower() && model.Password == person.password)
                {
                    Session["Username"] = person.name;
                    Session["Password"] = person.password;
                    return RedirectToAction("Dashboard");
                }
            }
            TempData["MessageType"] = "danger";
            TempData["Message"] = "Invalid Username or Password!!";
            return View(model);
            #endregion
        }
        //(4a) This page is used to get eligible members registered
        public ActionResult RegisterMember()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(4b) This page is used to get eligible members registered
        [HttpPost]
        public ActionResult RegisterMember(EligibleMember model)
        {
            #region personal picture validation
            if (Request.Files[0].ContentLength == 0 || Request.Files[0].FileName == "")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please input your postal image!!";
                return View(model);
            }
            if (Request.Files[0].ContentLength >= 20000000)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "File too large!!";
                return View(model);
            }
            if (Request.Files[0].ContentType.Substring(0, 5) != "image")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid file type!!";
                return View(model);
            }
            #endregion

            #region member data validation
            foreach (var item in db.EligibleMembers)
            {
                if (model.email.ToLower() == item.email.ToLower())
                {
                    TempData["MessageType"] = "danger";
                    TempData["Message"] = "The email has already been used!!";
                    return View(model);
                }
                if (model.phone_number == item.phone_number)
                {
                    TempData["MessageType"] = "danger";
                    TempData["Message"] = "The phone number has already been used!!";
                    return View(model);
                }
                if (model.membership_number.ToLower() == item.membership_number.ToLower())
                {
                    TempData["MessageType"] = "danger";
                    TempData["Message"] = "Someone already has that membership number!!";
                    return View(model);
                }
            }
            if (model.phone_number.Trim().Length < 7 || model.phone_number.Trim().Length > 15)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please input a valid phone number!!";
                return View(model);
            }
            if (model.password != model.confirmPassword)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Your password and confirm password information does not tally!!";
                return View(model);
            }
            if (model.password.Length < 8)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Your password should be at least 8 characters!!";
                return View(model);
            }
            if (model.password.Length > 15)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Your password should be at most 15 characters!!";
                return View(model);
            }
            #endregion

            #region save member data
            EligibleMember c = new EligibleMember
            {
                fullname = model.fullname,
                email = model.email,
                phone_number = model.phone_number,
                membership_number = model.membership_number,
                password = model.password,
            };

            var f = Request.Files[0];
            c.ImageName = f.FileName;
            c.ImageFileType = f.ContentType;

            using (var reader = new BinaryReader(f.InputStream))
            {
                c.ImageContent = reader.ReadBytes(f.ContentLength);
            }
            EligibleMember cc = db.EligibleMembers.Add(c);
            db.SaveChanges();
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your Registration was successful!!";
            return View();
            #endregion
        }
        //(4c) This page is used to get eligible members registered
        public ActionResult BulkRegisterMember()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(4d) This page is used to get eligible members registered
        
        //(5) This page would be used to get the members of the committee registered
        public ActionResult CommitteeMembersList()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(6a) To register contestants
        public ActionResult RegisterContestant()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(6b) To register contestants
        [HttpPost]
        public ActionResult RegisterContestant(Contestant model, string email, string password)
        {
            #region contestants data validation
            if (db.EligibleMembers.Where(dbl => dbl.email.ToLower() == email.ToLower() && dbl.password == password).Count() == 0)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid email and password combination!!";
                return View(model);
            }
            if (db.VotingSessions.Where(dbl => dbl.PositionId == model.PositionId && dbl.status == true).Count() == 0)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "The position you picked is not up for a vote!!";
                return View(model);
            }
            if (db.VotingSessions.Where(dbl => dbl.PositionId == model.PositionId && dbl.status == true).Count() > 1)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Multiple Voting sessions for that position. Please contact the admin!!";
                return View(model);
            }
            //////////////////////////////////////////////////////////////////////////
            if (Request.Files[0].ContentLength == 0 || Request.Files[0].FileName == "")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please input your postal image!!";
                return View(model);
            }
            if (Request.Files[0].ContentLength >= 20000000)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "File too large!!";
                return View(model);
            }
            if (Request.Files[0].ContentType.Substring(0, 5) != "image")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid file type!!";
                return View(model);
            }
            var vsId = db.VotingSessions.Where(dbl => dbl.PositionId == model.PositionId && dbl.status == true).First().Id;
            var memberId = db.EligibleMembers.Where(dbl => dbl.email.ToLower() == model.email.ToLower() && dbl.password == model.password).First().Id;
            if (db.Contestants.Where(dbl => dbl.MemberId == memberId && dbl.status == true).Count() != 0)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "You can't register in multiple voting sessions!!";
                return View(model);
            }
            #endregion

            #region save contestant data
            Contestant c = new Contestant
            {
                manifesto = model.manifesto,
                PositionId = model.PositionId,
                VotingSessionId = vsId,
                status = true,
                MemberId = memberId

            };
            var g = Request.Files[0];
            c.CampaignPictureName = g.FileName;
            c.CampaignPictureFileType = g.ContentType;

            using (var reader = new BinaryReader(g.InputStream))
            {
                c.CampaignPictureContent = reader.ReadBytes(g.ContentLength);
            }
            Contestant cc = db.Contestants.Add(c);
            db.SaveChanges();
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your Registration was successful!!";
            return View();
            #endregion
        }
        //(7) To log of virtually all the major activities on the application
        public ActionResult ActivityLog()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(8) A List of all ICAN members on the application
        public ActionResult MembersList()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        public ActionResult EligibleMembersList()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(9a) A List of all ICAN Committee members on the application
        public ActionResult RegisterCommitteeMember()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(9b) A List of all ICAN Committee members on the application
        [HttpPost]
        public ActionResult RegisterCommitteeMember(Committee_member model, string email, string password)
        {
            #region committee member data validation
            if (db.EligibleMembers.Where(dbl => dbl.email.ToLower() == email.ToLower() && dbl.password == password).Count() == 0)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid email and password combination!!";
                return View(model);
            }
            var memberid = db.EligibleMembers.Where(dbl => dbl.email == model.EligibleMember.email).First().Id;
            #endregion

            #region saving committee member data
            Committee_member c = new Committee_member
            {
                MemberId = memberid,
                cmvn = model.cmvn
            };
            Committee_member cc = db.Committee_member.Add(c);
            db.SaveChanges();
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your Registration was successful!!";
            return View();
            #endregion
        }
        //(10) A List of all contestants on the application (both active and inactive)
        public ActionResult ContestantsList()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(11) This page would enable us view the profile of each of the ICAN members
        public ActionResult Profile4EligibleMembers(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            ViewBag.Id = Id;
            return View();
        }
        //(12) This page would enable us view the profile of each of the ICAN Committee members
        public ActionResult Profile4CommitteeMembers(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            ViewBag.Id = Id;
            return View();
        }
        //(13) This page would enable us view the profile of each of the contestants
        public ActionResult Profile4Contestants(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult profile4Positions(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult EligibleMembersGallery()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(12) This page would enable us view the profile of each of the ICAN Committee members
        public ActionResult CommitteeMembersGallery()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(13) This page would enable us view the profile of each of the contestants
        public ActionResult ContestantsGallery()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        public ActionResult AllMembersGallery()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        //(14) A form that takes the details of all the information needed for a vote 
        public ActionResult CreateVotingSession()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        [HttpPost]
        public ActionResult CreateVotingSession(VotingSession model, string subject, string message)
        {
            #region voting session data validation 
            var EmailString = "";
            foreach (var item in db.EligibleMembers)
            {
                EmailString += item.email + ",";
            }
            EmailString = EmailString.Remove(EmailString.Length - 1);
            var toEmail = EmailString;
            var EmailSubj = subject;
            var EmailMsg = message;
            //passing parameter to Email Method
            //SendEmail.Email_Without_Attachment(toEmail, EmailSubj, EmailMsg);

            var sdate = Convert.ToDateTime(model.startDate).ToShortDateString();
            var stime = Convert.ToDateTime(model.startTime.ToString()).ToLongTimeString();
            var sdatetime = sdate + " " + stime;
            DateTime sDATEtime = Convert.ToDateTime(sdatetime);

            var cdate = Convert.ToDateTime(model.closeDate).ToShortDateString();
            var ctime = Convert.ToDateTime(model.closeTime.ToString()).ToLongTimeString();
            var cdatetime = cdate + " " + ctime;
            DateTime cDATEtime = Convert.ToDateTime(cdatetime);

            if (sDATEtime > cDATEtime)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "The start time is ahead of the close time!!";
                return View(model);
            }
            if (db.VotingSessions.Where(dbl => dbl.PositionId == model.PositionId && dbl.status == true).Count() != 0)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "You already have an active voting session for that position!!";
                return View(model);
            }
            if (DateTime.Now > cDATEtime)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "The time given has already elapsed!!";
                return View(model);
            }
            var moment = "";
            if (DateTime.Now < sDATEtime)
            {
                moment = "Up Coming";
            }
            else if (DateTime.Now > sDATEtime && DateTime.Now < cDATEtime)
            {
                moment = "In Progress";
            }
            else if (DateTime.Now > cDATEtime)
            {
                moment = "Completed";
            }
            #endregion

            #region saving session data
            VotingSession c = new VotingSession
            {
                startDate = model.startDate,
                startTime = model.startTime,
                closeDate = model.closeDate,
                closeTime = model.closeTime,
                startDateTime = sDATEtime,
                closeDateTime = cDATEtime,
                PositionId = model.PositionId,
                status = true, // active or inactive!!
                moment = moment,
            };
            VotingSession cc = db.VotingSessions.Add(c);
            db.SaveChanges();
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your Registration was successful!!";
            return View();
            #endregion
        }
        
        public ActionResult VoteLog()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        
        public ActionResult SendMail()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        [HttpPost]
        public ActionResult SendMail(MailDetail model)
        {
            #region send mail
            var EmailString = "";
            foreach (var item in db.EligibleMembers)
            {
                EmailString += item.email + ",";
            }
            EmailString = EmailString.Remove(EmailString.Length - 1);
            var toEmail = EmailString;
            var EmailSubj = model.subject;
            var EmailMsg = model.message;
            SendEmail.Email_Without_Attachment(toEmail, EmailSubj, EmailMsg);
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your Registration was successful!!";
            return View();
            #endregion
        }
        public ActionResult VotingSessionList()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        public ActionResult Report(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            ViewBag.Id = Id;
            foreach(var item in db.VotingSessions)
            {
                if(item.Id == Id)
                {
                    return View();
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        public ActionResult RegisterPosition()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        [HttpPost]
        public ActionResult RegisterPosition(Position model)
        {
            Position c = new Position
            {
                Post = model.Post,
                Description = model.Description
            };
            var cc = db.Positions.Add(c);
            db.SaveChanges();
            return View();
        }
        public ActionResult PositionList()
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        public ActionResult EditContestants(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            var model = db.Contestants.Where(dbl => dbl.Id == Id).First(); 
            return View(model);
        }
        [HttpPost]
        public ActionResult EditContestants(Contestant model)
        {
            if (Request.Files[0].ContentLength == 0 || Request.Files[0].FileName == "")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please input your personal image!!";
                return View(model);
            }
            if (Request.Files[0].ContentLength >= 20000000)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "File too large!!";
                return View(model);
            }
            if (Request.Files[0].ContentType.Substring(0, 5) != "image")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid file type!!";
                return View(model);
            }
            var c = db.Contestants.Find(model.Id);

            c.manifesto = model.manifesto;
            c.PositionId = model.PositionId;
            var g = Request.Files[0];
            c.CampaignPictureName = g.FileName;
            c.CampaignPictureFileType = g.ContentType;

            using (var reader = new BinaryReader(g.InputStream))
            {
                c.CampaignPictureContent = reader.ReadBytes(g.ContentLength);
            }
            db.SaveChanges();
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your Registration was successful!!";
            return RedirectToAction("EditContestants", new { Id = model.Id });
        }
        public ActionResult EditCommitteeMember(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            var model = db.Committee_member.Where(dbl => dbl.Id == Id).First();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditCommitteeMember(Committee_member model)
        {
            if (Request.Files[0].ContentLength == 0 || Request.Files[0].FileName == "")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please input your postal image!!";
                return View(model);
            }
            if (Request.Files[0].ContentLength >= 20000000)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "File too large!!";
                return View(model);
            }
            if (Request.Files[0].ContentType.Substring(0, 5) != "image")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid file type!!";
                return View(model);
            }
            var c = db.Committee_member.Find(model.Id);
            c.MemberId = model.EligibleMember.Id;
            c.cmvn = model.cmvn;
            Committee_member cc = db.Committee_member.Add(c);
            db.SaveChanges();
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your Registration was successful!!";
            //return RedirectToAction("EditCommitteeMember", new { Id = model.Id });
            return RedirectToAction("CommitteeMembersList");
        }
        public ActionResult EditEligibleMember(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            var model = db.EligibleMembers.Where(dbl => dbl.Id == Id).First();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditEligibleMember(EligibleMember model)
        {
            if (Request.Files[0].ContentLength == 0 || Request.Files[0].FileName == "")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Please input your postal image!!";
                return View(model);
            }
            if (Request.Files[0].ContentLength >= 20000000)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "File too large!!";
                return View(model);
            }
            if (Request.Files[0].ContentType.Substring(0, 5) != "image")
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid file type!!";
                return View(model);
            }
            //var c = db.EligibleMembers.Find(model.Id);
            var c = db.EligibleMembers.Where(dbl => dbl.Id == model.Id).First();
            //EligibleMember c = new EligibleMember
            //{
            c.fullname = model.fullname;
            c.email = model.email;
            c.phone_number = model.phone_number;
            c.membership_number = model.membership_number;
            c.password = model.password;
            //};

            var f = Request.Files[0];
            c.ImageName = f.FileName;
            c.ImageFileType = f.ContentType;

            using (var reader = new BinaryReader(f.InputStream))
            {
                c.ImageContent = reader.ReadBytes(f.ContentLength);
            }
            //EligibleMember cc = db.EligibleMembers.Add(c);
            db.SaveChanges();
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your Registration was successful!!";
            //return RedirectToAction("EditEligibleMember", new { Id = model.Id });
            return RedirectToAction("EligibleMembersList");
        }
        public ActionResult EditPostion(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            var model = db.Positions.Where(dbl => dbl.Id == Id).First();
            return View(model);
        }
        [HttpPost]
        public ActionResult EditPostion(Position model)
        {
            return View();
        }
        public FileContentResult GetFile(int id)
        {
            var img = db.EligibleMembers.Find(id);
            return File(img.ImageContent, img.ImageFileType);
        }
        public FileContentResult GetCommitteeFile(int id)
        {
            var img = db.Committee_member.Find(id);
            return File(img.EligibleMember.ImageContent, img.EligibleMember.ImageFileType);
        }
        public FileContentResult GetContestantFile(int id)
        {
            var img = db.Contestants.Find(id);
            return File(img.EligibleMember.ImageContent, img.EligibleMember.ImageFileType);
        }
        [HttpPost]
        public ActionResult BulkRegisterMember(HttpPostedFileBase file)
        {
            //try
            //{
            //    var extension = Path.GetExtension(file.FileName);
            //    var path = Server.MapPath("~/Uploads/");

            //    if (!Directory.Exists(path))
            //    {
            //        Directory.CreateDirectory(path);
            //    }

            //    var filePath = path + file.FileName;

            //    var fInfo = new FileInfo(filePath);
            //    //if (fInfo.Exists)
            //    //{
            //    //    fInfo.Delete();
            //    //}
            //    file.SaveAs(path + Path.GetFileName(file.FileName));
            //    /////////////////////////////////////
            //    Application xlApp = new Application();
            //    Workbook xlbook = xlApp.Workbooks.Open(filePath);
            //    Worksheet xlsheet = xlbook.Sheets[1];
            //    Range xlrange = xlsheet.UsedRange;
            //    var rows = (xlrange.Count/8);
            //    //xlApp.Workbooks.Close();
            //    for (var i = 2; i <= rows; i++)
            //    {
            //        EligibleMember tk = new EligibleMember
            //        {
            //            fullname = Convert.ToString(xlrange.Cells[i, 1].Value2),
            //            phone_number = Convert.ToString(xlrange.Cells[i, 2].Value2),
            //            membership_number = Convert.ToString(xlrange.Cells[i, 3].Value2),
            //            email = Convert.ToString(xlrange.Cells[i, 4].Value2),
            //            password = Convert.ToString(xlrange.Cells[i, 5].Value2),
            //            ImageName = Convert.ToString(xlrange.Cells[i, 6].Value2),
            //            ImageContent = Encoding.ASCII.GetBytes(xlrange.Cells[i, 7].Value2),
            //            ImageFileType = Convert.ToString(xlrange.Cells[i, 8].Value2),
            //        };
            //        var cc = db.EligibleMembers.Add(tk);
            //    }
            //    db.SaveChanges();
            //    xlApp.Workbooks.Close();
            //    TempData["MessageType"] = "success";
            //    TempData["Message"] = "Tickets successfully added!";
            //    return View();
            //}
            //catch (Exception ex)
            //{

            //    TempData["MessageType"] = "danger";
            //    TempData["Message"] = ex.Message;
            //    return View();
            //}
            return View();
        }
        public ActionResult EditPosition(int Id)
        {
            EvoteMethods EvoteMethod = new EvoteMethods();
            EvoteMethod.check();
            return View();
        }
        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAdmin(SignIn model)
        {
            var person = db.EligibleMembers.Where(dbl => dbl.email.ToLower() == model.Name.ToLower() && dbl.password == model.Password);
            if(db.Admins.Where(dbl => dbl.name.ToLower() == model.Name.ToLower()).Count() > 0)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "This username already exist. If this is you, please login with your given credentials!!";
                return View(model);
            }
            var cp = person.Count();
            if (person.Count() == 1)
            {
                var MemberId = db.EligibleMembers.Where(dbl => dbl.email.ToLower() == model.Name.ToLower() && dbl.password == model.Password).First().Id;
                Admin ad = new Admin
                {
                    name = model.Name,
                    password = model.Password
                };
                var cc = db.Admins.Add(ad);
                db.SaveChanges();
                var fullname = db.EligibleMembers.Find(MemberId).fullname;
                var toEmail = model.Name;
                var EmailSubj = "Evoting Admin Credentials";
                var g = Guid.NewGuid();
                var passwordAdmin = g.ToString().Remove(10);
                var usernameAdmin = toEmail;
                var EmailMsg = "Hello " + fullname + ",\n\nYou have been successfully registered as an admin to the Evoting Application.\n\n" +
                "Username: " + toEmail + "\n\nPassword: " + passwordAdmin + "\n\nClick the link below to login as an admin.\n\nhttp://localhost:49296/Admin/Login";
                //passing parameter to Email Method
                SendEmail.Email_Without_Attachment(toEmail, EmailSubj, EmailMsg);

                TempData["MessageType"] = "success";
                TempData["Message"] = db.EligibleMembers.Find(MemberId).fullname + ", you have been successfully registered as an admin." +
                " Please check your email to get your admin username and password!!";
                return View();
            }
            else if(person.Count() == 0)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid username and password combination!!";
                return View(model);
            }
            else
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Duplicate Membership Information in the database. Please resolve immediately!!";
                return View(model);
            }
        }
        public ActionResult EditRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(SignIn model, int PageId)
        {
            if (db.Admins.Where(dbl => dbl.name.ToLower() == model.Name.ToLower()).Count() == 0)
            {
                TempData["MessageType"] = "success";
                TempData["Message"] = "Your request has been successfully logged and an email would have been sent to your email address if you " +
                "are an administrator to this platform. Please check your email to get your updated credentials!!";
                if(PageId == 1)
                {
                    return RedirectToAction("AddAdmin");
                }else
                {
                    return RedirectToAction("Login");
                }
                
            }
            if (db.Admins.Where(dbl => dbl.name.ToLower() == model.Name.ToLower()).Count() > 1)
            {
                TempData["MessageType"] = "success";
                TempData["Message"] = "Your request has been successfully logged and an email would have been sent to your email address if you " +
                "are an administrator to this platform. Please check your email to get your updated credentials!!";
                if (PageId == 1)
                {
                    return RedirectToAction("AddAdmin");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            var newpassword = Guid.NewGuid().ToString().Remove(10);
            var admin = db.Admins.Where(dbl => dbl.name.ToLower() == model.Name.ToLower()).FirstOrDefault();
            var adminId = admin.Id;
            db.Admins.Find(adminId).password = newpassword;
            db.SaveChanges();
            var fullname = db.EligibleMembers.Where(dbl => dbl.email.ToLower() == model.Name.ToLower()).First().fullname;
            var toEmail = model.Name;
            var EmailSubj = "Change of Your Admin Password";
            var EmailMsg = "Hello " + fullname + ",\n\nYour password has been regenerated successfully.\n\n" +
            "Username: " + toEmail + "\n\nPassword: " + newpassword + "\n\nClick the link below to login as an admin.\n\nhttp://localhost:49296/Admin/Login";
            SendEmail.Email_Without_Attachment(toEmail, EmailSubj, EmailMsg);
            TempData["MessageType"] = "success";
            TempData["Message"] = "Your request has been successfully logged and an email would have been sent to your email address if you " +
            "are an administrator to this platform. Please check your email to get your updated credentials!!";
            if (PageId == 1)
            {
                return RedirectToAction("AddAdmin");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordInfo model, int PageId)
        {
            if(model.newpassword != model.confirmnewpassword)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "The password and Confirm password should be the same!!";
                if(PageId == 1)
                {
                    return RedirectToAction("AddAdmin", model);
                }
                else
                {
                    return RedirectToAction("Login", model);
                }
            }
            if(db.Admins.Where(dbl => dbl.name.ToLower() == model.username.ToLower() && dbl.password == model.oldpassword).Count() == 1)
            {
                var AdminId = db.Admins.Where(dbl => dbl.name.ToLower() == model.username.ToLower() && dbl.password == model.oldpassword).FirstOrDefault().Id;
                db.Admins.Find(AdminId).password = model.newpassword;
                db.SaveChanges();
                var fullname = db.EligibleMembers.Where(dbl => dbl.email.ToLower() == model.username.ToLower()).First().fullname;
                var toEmail = model.username;
                var EmailSubj = "Change of Your Admin Password";
                var EmailMsg = "Hello " + fullname + ",\n\nYour password has been successfully changed.\n\n" +
                "Username: " + toEmail + "\n\nPassword: " + model.newpassword + "\n\nClick the link below to login as an admin.\n\nhttp://localhost:49296/Admin/Login";
                SendEmail.Email_Without_Attachment(toEmail, EmailSubj, EmailMsg);
                TempData["MessageType"] = "success";
                TempData["Message"] = "Your password has been successfully changed!!";
                if(PageId == 1)
                {
                    return RedirectToAction("AddAdmin");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            if (db.Admins.Where(dbl => dbl.name.ToLower() == model.username.ToLower() && dbl.password == model.oldpassword).Count() == 0)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid login credentials!!";
                if(PageId == 1)
                {
                    return RedirectToAction("AddAdmin", model);
                }
                else
                {
                    return RedirectToAction("Login", model);
                }
            }
            else
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Multiple login credentials for the same admin. Contact support!!";
                if (PageId == 1)
                {
                    return RedirectToAction("AddAdmin", model);
                }
                else
                {
                    return RedirectToAction("Login", model);
                }
            }
            
        }
    }
}