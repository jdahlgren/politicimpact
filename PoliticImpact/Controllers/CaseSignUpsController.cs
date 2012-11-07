using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class CaseSignUpsController : Controller
    {
		private readonly ICaseSignUpRepository casesignupRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseSignUpsController() : this(new CaseSignUpRepository())
        {
        }

        public CaseSignUpsController(ICaseSignUpRepository casesignupRepository)
        {
			this.casesignupRepository = casesignupRepository;
        }

        //
        // GET: /CaseSignUps/

        public ViewResult Index()
        {
            return View(casesignupRepository.All);
        }

        //
        // GET: /CaseSignUps/Details/5

        public ViewResult Details(int id)
        {
            return View(casesignupRepository.Find(id));
        }

        //
        // GET: /CaseSignUps/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CaseSignUps/Create

        [HttpPost]
        public ActionResult Create(CaseSignUp casesignup)
        {
            if (ModelState.IsValid) {
                casesignupRepository.InsertOrUpdate(casesignup);
                casesignupRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /CaseSignUps/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(casesignupRepository.Find(id));
        }

        //
        // POST: /CaseSignUps/Edit/5

        [HttpPost]
        public ActionResult Edit(CaseSignUp casesignup)
        {
            if (ModelState.IsValid) {
                casesignupRepository.InsertOrUpdate(casesignup);
                casesignupRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /CaseSignUps/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(casesignupRepository.Find(id));
        }

        //
        // POST: /CaseSignUps/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            casesignupRepository.Delete(id);
            casesignupRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                casesignupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

