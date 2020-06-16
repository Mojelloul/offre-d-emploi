using Microsoft.AspNet.Identity;
using offre_d_emploi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult GetJobByUser()
        {
            var UserId = User.Identity.GetUserId();
            var Jobs = db.ApplyForJobs.Where(a => a.UserId == UserId);
            return View(Jobs.ToList());
        }

        public ActionResult Edit(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                job.ApplyDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetJobByUser");
            }
            return View(job);
        }

        [Authorize]
        public ActionResult DetailsOfJob(int Id)
        {
            var job = db.ApplyForJobs.Find(Id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        public ActionResult Details(int JobId)
        {
            var job = db.Jobs.Find(JobId);
            if(job== null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = JobId;
            return View(job);
        }

        public ActionResult Delete(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {
                // TODO: Add delete logic here
                var myjob = db.ApplyForJobs.Find(job.Id);
                db.ApplyForJobs.Remove(myjob);
                db.SaveChanges();
                return RedirectToAction("GetJobByUser");
        }
        [Authorize]
        public ActionResult GetJobsByPublisher()
        {
            var UserID = User.Identity.GetUserId();
            var Jobs = from app in db.ApplyForJobs 
                      join job in db.Jobs
                      on app.JobId equals job.Id
                      where job.User.Id==UserID
                      select app;
            var grouped = from j in Jobs
                          group j by j.job.JobTitle
                          into gr
                          select new JobsViewModel
                          {
                              JobTitle = gr.Key,
                              Items = gr
                          };
            return View(grouped.ToList());
        }

        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Apply(string Message)
        {
            var UserId = User.Identity.GetUserId();
            var JobId = (int)Session["JobId"];
            var ckeck = db.ApplyForJobs.Where(a => a.JobId == JobId && a.UserId == UserId).ToList();
            if (ckeck.Count<1)
            {
                var job = new ApplyForJob();
                job.UserId = UserId;
                job.JobId = JobId;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;
                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Result = "Bien Ajouter!";
            }
            else
            {
                ViewBag.Result = "Vous avez deja postuler pour cette emploi";
            }

            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("realmed7@hotmail.fr", "*******");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("realmed7@hotmail.fr"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;
            string body = "Nom : " + contact.Name + "<br>" +
                        "Email : " + contact.Name + "<br>" +
                        "Titre : " + contact.Name + "<br>" +
                        "Message : " + contact.Name;
            mail.Body = body;
            var smrpClient = new SmtpClient("smtp.live.com", 25);
            smrpClient.EnableSsl = true;
            smrpClient.Credentials = loginInfo;
            smrpClient.Send(mail);
            return RedirectToAction("Index");
        }
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string searchName)
        {
            var result = db.Jobs.Where(a => a.JobTitle.Contains(searchName)
            || a.JobContent.Contains(searchName)
            || a.Category.CategoryName.Contains(searchName)
            || a.Category.CategoryDescription.Contains(searchName)
            ).ToList();
            return View(result);
        }

    }
}