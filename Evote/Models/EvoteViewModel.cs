using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Evote.Models
{
    public class MailDetail
    {
        public string subject { get; set; }
        public string message { get; set; }
    }
    public class ChangePasswordInfo
    {
        public string username { get; set; }
        public string oldpassword { get; set; }
        public string newpassword { get; set; }
        public string confirmnewpassword { get; set; }
    }
    public enum moment
    {
        [Description("Up Coming")]
        UpComing,
        [Description("In Progress")]
        InProgress,
        [Description("Completed")]
        Completed
    }
    public class EvoteMethods
    {
        hrEntities db = new hrEntities();
        public void check()
        {
            foreach (var item in db.Contestants)
            {
                if (DateTime.Now > item.VotingSession.closeDateTime)
                { item.status = false; }
                else
                { item.status = true; }
            }
            foreach (var item in db.VotingSessions)
            {
                if (DateTime.Now > item.closeDateTime)
                { item.status = false; }
                else
                { item.status = true; }
                if (DateTime.Now < item.startDateTime)
                {
                    item.moment = moment.UpComing.ToString();//"Up Coming";
                }
                else if (DateTime.Now >= item.startDateTime && DateTime.Now <= item.closeDateTime)
                {
                    item.moment = moment.InProgress.ToString();//"In Progress";
                }
                else if (DateTime.Now > item.closeDateTime)
                {
                    item.moment = moment.Completed.ToString();//"Completed";
                }
            }
            db.SaveChanges();
        }
    }
    public class SignIn
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
    public class Vote
    {
        public int ContestantId { get; set; }
        public string email { get; set; }
    }
    public class contestantpie
    {
        public int value { get; set; }
        public string label { get; set; }
        public string formatted { get; set; }
    }
    public static class SendEmail
    {
        public static string Pass, FromEmailid, HostAdd;

        public static void Email_Without_Attachment(String ToEmail, String Subj, string Message)
        {
            //Reading sender Email credential from web.config file
            HostAdd = ConfigurationManager.AppSettings["Host"].ToString();
            FromEmailid = ConfigurationManager.AppSettings["FromMail"].ToString();
            Pass = ConfigurationManager.AppSettings["Password"].ToString();

            //creating the object of MailMessage
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(FromEmailid); //From Email Id
            mailMessage.Subject = Subj; //Subject of Email
            mailMessage.Body = Message; //body or message of Email
            mailMessage.IsBodyHtml = true;
            //Adding Multiple recipient email id logic
            string[] Multi = ToEmail.Split(','); //spiliting input Email id string with comma(,)
            foreach (string Multiemailid in Multi)
            {
                mailMessage.To.Add(new MailAddress(Multiemailid)); //adding multi reciver's Email Id
            }
            SmtpClient smtp = new SmtpClient(); // creating object of smptpclient
            smtp.Host = HostAdd; //host of emailaddress for example smtp.gmail.com etc

            //network and security related credentials
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = mailMessage.From.Address;
            NetworkCred.Password = Pass;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mailMessage); //sending Email
        }

    }
}