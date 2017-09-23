using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Socialbodhi.Models;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Socialbodhi.Controllers
{

    public class HomeController : Controller
    {
        SocialbodhiEntities context = new SocialbodhiEntities();
        public ActionResult Index()
        {
            ViewBag.userinfo = context.Users.FirstOrDefault();
            return View();
        }

        public ActionResult Update()
        {


            return View();
        }

        public JsonResult sendemail(string choices, string emailids)
        {
            try {
                Instance newinstance = new Instance();
                newinstance.UserId = 1; // Temporarly hard coded
                newinstance.CreatedAt = DateTime.Now;
                newinstance.Status = true;
                context.Instances.Add(newinstance);
                context.SaveChanges();
                string[] choice = choices.Split(' ');
                string[] email = emailids.Split(' ');
                foreach (var item in choice)
                {
                    if (item != "")
                    {
                        Choice newchoice = new Choice();
                        newchoice.ChoiceName = item;
                        newchoice.InstanceId = newinstance.InstanceId;
                        newchoice.CreatedAt = DateTime.Now;
                        newchoice.Status = true;
                        newchoice.UserId = 1; // temporary hard coded
                        context.Choices.Add(newchoice);
                        context.SaveChanges();
                        foreach (var item1 in email)
                        {
                            if (item1 != "")
                            {
                                Participant newparticipant = new Participant();
                                newparticipant.CreatedAt = DateTime.Now;
                                newparticipant.InstanceId = newinstance.InstanceId;
                                newparticipant.ChoiceId = newchoice.ChoiceId;
                                newparticipant.Participantsemail = item1;
                                newparticipant.Status = false; // default will be false. when particpants enters the value it will change to true.
                                context.Participants.Add(newparticipant);
                                context.SaveChanges();
                            }
                        }
                    }
                }
                foreach (var item1 in email)
                {
                    if (item1 != "")
                    {

                        // Email to the participants
                        SmtpClient client = new SmtpClient("smtp.gmail.com");
                        //Authentication    
                        client.Credentials = new NetworkCredential("rahuram777@gmail.com", "Raghu777890");
                        MailMessage mailMessage = new MailMessage("Socialbodhi@gmail.com", item1);
                        mailMessage.Subject = "Request for poll";
                        //mailMessage.Body = "http://localhost:50569/home/poll?instance=" + newinstance.InstanceId + "&email=" + item1;
                        mailMessage.Body = "http://34.208.214.144/socialbodhi/home/poll?instance=" + newinstance.InstanceId + "&email=" + item1;
                        client.Send(mailMessage);

                    }
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (WebException webex) {
             
                return Json(webex, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult about()
        {

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            //Authentication    
            client.Credentials = new NetworkCredential("rahuram777@gmail.com", "Raghu777890");

            MailMessage mailMessage = new MailMessage("Socialbodhi@gmail.com", "rahuram777@gmail.com");
            mailMessage.Subject = "Hello There";
            mailMessage.Body = "Hello my friend!";
            client.Send(mailMessage);

            return View();
        }

        [HttpGet]
        public ActionResult poll(int instance, string email)
        {
            ViewBag.choices = context.Choices.Where(x => x.InstanceId == instance).ToList();
            var participants = context.Participants.Where(x => x.InstanceId == instance).GroupBy(x => x.Participantsemail).ToList();
            List<poll> newpoll = new List<poll>();
            int? topchoicevalue = 0;
            var topchoicename = "";
            foreach (var item in participants) {
                poll newdata = new poll();
                newdata.Participantsemail = item.Key;
                newdata.participant = context.Participants.Where(x => x.Participantsemail == item.Key && x.InstanceId == instance).ToList();
                newpoll.Add(newdata);
            }

            ViewBag.participants = newpoll;
            var choicesgroup = context.Participants.Where(x => x.InstanceId == instance).GroupBy(x => x.ChoiceId).ToList();

            foreach (var choices in choicesgroup) {
                int? totalrating = 0;
                var choicename = "";
                foreach (var rating in choices)
                {
                    if (rating.Rating != null)
                    {
                        totalrating += rating.Rating;
                    } 
                    choicename = rating.Choice.ChoiceName;
                }

                if (totalrating > topchoicevalue)
                {
                    topchoicevalue = totalrating;
                    topchoicename = choicename; 
                }
            }

            ViewBag.topchoicename = topchoicename;

            ViewBag.email = email;
            return View();
        }

        [HttpPost]
        public ActionResult poll()
        {

            return RedirectToAction("home");
        }

        public ActionResult pollupdate(string[] participantId) {

            if (participantId != null)
            {
                foreach (var participantsandvalues in participantId)
                {
                    string[] partcipantIdvalue = participantsandvalues.Split(',');
                    foreach (var data in partcipantIdvalue)
                    {
                        if (data != "")
                        {
                            string[] participantid = data.Split(' ');
                            var _participantid = Convert.ToInt64(participantid[0]);
                            var value = Convert.ToInt32(participantid[1]);

                            if (_participantid != null)
                            {
                                var participantsupdate = context.Participants.Where(x => x.ParticipantsId == _participantid).FirstOrDefault();
                                participantsupdate.Rating = value;
                                participantsupdate.Status = true; // because the value has been update. Initially I would be false.
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}