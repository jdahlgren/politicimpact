using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class CaseItemsController : Controller
    {
		private readonly ICaseItemRepository caseitemRepository;
        private readonly ICaseCategoryRepository casecategoryRepository;

        private readonly ICaseVotingRepository CaseVotingRepository;
        private readonly CaseLikeRepository caselikeRepository;

        private readonly CaseVotingRepository caseVotingRepository;
        private readonly CaseVoteRepository caseVoteRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseItemsController() : this(new CaseItemRepository())
        {
        }

        public CaseItemsController(ICaseItemRepository caseitemRepository)
        {
			this.caseitemRepository = caseitemRepository;
            this.casecategoryRepository = new CaseCategoryRepository();

            this.CaseVotingRepository = new CaseVotingRepository();
            this.CaseLikeRepository = new CaseLikeRepository();

            this.caseVotingRepository = new CaseVotingRepository();
            this.caseVoteRepository = new CaseVoteRepository();

        }

        //
        // GET: /CaseItems/

        public ViewResult Index()
        {
            System.Diagnostics.Debug.WriteLine("asdf");
            return View(caseitemRepository.All);
        }

        //
        // GET: /CaseItems/Details/5

        public ViewResult Details(int id)
        {

            int numberOfLikes = caselikeRepository.FindLike(id);

            Boolean ÚserHasVoted = false;
            CaseVoting casevoting = caseVotingRepository.FindByCaseId(id);
            

            if(casevoting!=null)
            {
                IQueryable<CaseVote> votes = caseVoteRepository.FindAllByVotingId(casevoting.VotingID);
                ViewBag.votes = votes.Count();
                foreach(var vote in votes)
                {
                    if(vote.UserID==1337)//TODO compare with actual fb userid
                    {
                        ÚserHasVoted = true;

                    }

                }
                ViewBag.userhasvoted = ÚserHasVoted;

            }

            return View(caseitemRepository.Find(id));
        }

        //
        // GET: /CaseItems/Create

        public ActionResult Create()
        {
            ViewBag.PossibleCategories = casecategoryRepository.All;
            return View();
        } 

        //
        // POST: /CaseItems/Create

        [HttpPost]
        public ActionResult Create(CaseItem caseitem)
        {
            caseitem.Owner = 1337;  //TODO should be the logged in users facebook-id
            caseitem.Created = DateTime.Now;
            caseitem.LastEdited = Convert.ToDateTime("2012-01-01");
            if (ModelState.IsValid) {
                caseitemRepository.InsertOrUpdate(caseitem);
                caseitemRepository.Save();
                
                if (Request["create_voting"] != "" && Request["create_voting"] != null)
                {
                    //have the user requested a voting on this case?
                    if (Request["create_voting"] == "yes" && Request["voting_title"] != "" && Request["voting_title"]!=null)
                    {
                        CaseVoting casevoting = new CaseVoting();
                        casevoting.CaseID = caseitem.ID;
                        casevoting.Title = Request["voting_title"];
                        casevoting.StartDate = DateTime.Now;
                        casevoting.EndDate = DateTime.Now;
                        casevoting.Created = DateTime.Now;
                        caseVotingRepository.InsertOrUpdate(casevoting);
                        caseVotingRepository.Save();
                    }
                    
                }
                
                return RedirectToAction("Index");
            } else {
                ViewBag.PossibleCategories = casecategoryRepository.All;
				return View();
			}
        }
        
        //
        // GET: /CaseItems/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(caseitemRepository.Find(id));
        }

        //
        // POST: /CaseItems/Edit/5

        [HttpPost]
        public ActionResult Edit(CaseItem caseitem)
        {
            if (ModelState.IsValid) {
                caseitemRepository.InsertOrUpdate(caseitem);
                caseitemRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /CaseItems/Delete/5
        //Edited by johannes dahlgren 20/11 2012
        public ActionResult Delete(int id)
        {
            caseitemRepository.Delete(id);
            caseitemRepository.Save();

            return View();
        }

        //
        // POST: /CaseItems/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            caseitemRepository.Delete(id);
            caseitemRepository.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Publish(int id)
        {
            CaseItem caseitem = caseitemRepository.Find(id);
            caseitem.Published = true;
            caseitemRepository.InsertOrUpdate(caseitem);
            caseitemRepository.Save();
            return View();
        }

        //added by Christoffer Dahl 2012-11-07 10:32
        [HttpPost]
        public ActionResult ShareMail(int id)
        {

            CaseItem caseItem = caseitemRepository.Find(id);
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();

            try
            {
                m.From = new MailAddress("politicalimpact@gmail.com", "Politic Impact");
                m.To.Add(new MailAddress(Request["email"]));
                //m.CC.Add(new MailAddress("chrda005@student.liu.se","Display name CC"));
                m.Subject = "Political Impact: Shared Case";
                m.Body = caseItem.Title + caseItem.Text;

                //Attachment
                //FileStream fs = new FileStream("E:\\TestFolder\\test.pdf", FileMode.Open, FileAccess.Read);
                //Attachment a = new Attachment(fs, "test.pdf", MediaTypeNames.Application.Octet);

                //string str = "<html><body><h1>Picture<h/h1><br/><img src=\cid:image1\"></body></html>";
                //AlternateView av = AlternateView.CreateAlternateViewFromString(str,null,MediaTypeNames.Text.Html);
                //LinkedResource lr = new LinkedResource("E:\\Photos\\hello.jpg",MediaTypeNames.Image.Jpeg);
                //lr.ContentId = "image1";
                //av.LinkedResources.Add(lr);
                //m.AlternateViews.Add(av);

                sc.Host = "smtp.gmail.com";
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("politicalimpact@gmail.com", "pumTNM090");
                sc.EnableSsl = true;
                sc.Send(m);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View();
        }

        //added by Christoffer Dahl 2012-11-16 09:50
        public ActionResult ReportMail(int id, String pw)
        {
            if (pw == "allow")
            {
                CaseItem caseItem = caseitemRepository.Find(4);
                MailMessage m = new MailMessage();
                SmtpClient sc = new SmtpClient();

                try
                {
                    m.From = new MailAddress("politicalimpact@gmail.com", "Politic Impact");
                    m.To.Add(new MailAddress(caseItem.RecieverEmail, caseItem.RecieverName));
                    m.Subject = "Political Impact: Report on Case";
                    m.Body = caseItem.Title + caseItem.Text + "Antal likes o lite sånt shieeeet";

                    //Attachment
                    //FileStream fs = new FileStream("E:\\TestFolder\\test.pdf", FileMode.Open, FileAccess.Read);
                    //Attachment a = new Attachment(fs, "test.pdf", MediaTypeNames.Application.Octet);

                    //string str = "<html><body><h1>Picture<h/h1><br/><img src=\cid:image1\"></body></html>";
                    //AlternateView av = AlternateView.CreateAlternateViewFromString(str,null,MediaTypeNames.Text.Html);
                    //LinkedResource lr = new LinkedResource("E:\\Photos\\hello.jpg",MediaTypeNames.Image.Jpeg);
                    //lr.ContentId = "image1";
                    //av.LinkedResources.Add(lr);
                    //m.AlternateViews.Add(av);

                    sc.Host = "smtp.gmail.com";
                    sc.Port = 587;
                    sc.Credentials = new System.Net.NetworkCredential("politicalimpact@gmail.com", "pumTNM090");
                    sc.EnableSsl = true;
                    sc.Send(m);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                caseitemRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        public CaseLikeRepository CaseLikeRepository { get; set; }
    }
}

