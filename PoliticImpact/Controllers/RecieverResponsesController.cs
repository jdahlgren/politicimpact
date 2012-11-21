using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class RecieverResponsesController : Controller
    {
		private readonly IRecieverResponseRepository recieverresponseRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public RecieverResponsesController() : this(new RecieverResponseRepository())
        {
        }

        public RecieverResponsesController(IRecieverResponseRepository recieverresponseRepository)
        {
			this.recieverresponseRepository = recieverresponseRepository;
        }

        //
        // GET: /RecieverResponses/

        public ViewResult Index()
        {
            return View(recieverresponseRepository.All);
        }

        //
        // GET: /RecieverResponses/Details/5

        public ViewResult Details(int id)
        {
            return View(recieverresponseRepository.Find(id));
        }

        //
        // GET: /RecieverResponses/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /RecieverResponses/Create

        [HttpPost]
        public ActionResult Create(RecieverResponse recieverresponse)
        {
            if (ModelState.IsValid) {
                recieverresponseRepository.InsertOrUpdate(recieverresponse);
                recieverresponseRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /RecieverResponses/Edit?respCode=ONETIMECODE

        public ActionResult Edit(string respCode)
        {
            int id = new int();
            id = recieverresponseRepository.GetResponseId(respCode);

            if (id == 0)
            {
                ViewBag.caseFound = false;
                return View();
            }
            else
            {
                string caseTitle = recieverresponseRepository.GetCaseTitle(id);
                string caseText = recieverresponseRepository.GetCaseText(id);

                if (caseTitle != null)
                    ViewBag.caseTitle = caseTitle;
                else
                    return RedirectToAction("Index", "Home");

                if (caseText != null)
                    ViewBag.caseText = caseText;
                else
                    return RedirectToAction("Index", "Home");
            }

            ViewBag.caseFound = true;

            return View(recieverresponseRepository.Find(id));
        }

        //
        // POST: /RecieverResponses/Edit?respCode=ONETIMECODE

        [HttpPost]
        public ActionResult Edit(RecieverResponse recieverresponse)
        {
            int caseId = recieverresponseRepository.GetCaseId(recieverresponse.ResponseID);

            if (ModelState.IsValid && caseId != 0) {
                recieverresponseRepository.InsertOrUpdate(recieverresponse);
                recieverresponseRepository.Save();
                return RedirectToAction("Details", "CaseItems", new { id = caseId }); //Redirecta till det ärende som användaren lämnat respons på
            } else {
				return View();
			}
        }

        //
        // GET: /RecieverResponses/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(recieverresponseRepository.Find(id));
        }

        //
        // POST: /RecieverResponses/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            recieverresponseRepository.Delete(id);
            recieverresponseRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                recieverresponseRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

