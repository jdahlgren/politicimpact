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

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseItemsController() : this(new CaseItemRepository())
        {
        }

        public CaseItemsController(ICaseItemRepository caseitemRepository)
        {
			this.caseitemRepository = caseitemRepository;
        }

        //
        // GET: /CaseItems/

        public ViewResult Index()
        {
            return View(caseitemRepository.All);
        }

        //
        // GET: /CaseItems/Details/5

        public ViewResult Details(int id)
        {
            //var model = caseitemRepository.Find(id);
            //using (var casecommentRepository = new CaseCommentRepository()){
            //    model.caseComment = casecommentRepository.AllIncluding(comment => comment.caseID == id).ToList();
            //}
            return View(caseitemRepository.Find(id));
        }

        //
        // GET: /CaseItems/Create

        public ActionResult Create()
        {
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
            caseitem.caseComment = new List<CaseComment>();
            if (ModelState.IsValid) {
                caseitemRepository.InsertOrUpdate(caseitem);
                caseitemRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        [HttpPost]
        public ActionResult CreateComment(CaseComment caseComment)
        {
            if (ModelState.IsValid)
            {
                var casecommentRepository = new CaseCommentRepository();
                casecommentRepository.InsertOrUpdate(caseComment);
                casecommentRepository.Save();

                return RedirectToAction("Details", new { id = caseComment.caseID });
            }
            return RedirectToAction("Index");
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
 
        public ActionResult Delete(int id)
        {
            return View(caseitemRepository.Find(id));
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

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                caseitemRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

