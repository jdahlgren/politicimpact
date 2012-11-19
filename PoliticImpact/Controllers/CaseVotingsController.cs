using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class CaseVotingsController : Controller
    {
		private readonly ICaseVotingRepository casevotingRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseVotingsController() : this(new CaseVotingRepository())
        {
        }

        public CaseVotingsController(ICaseVotingRepository casevotingRepository)
        {
			this.casevotingRepository = casevotingRepository;
        }

        //
        // GET: /CaseVotings/

        public ViewResult Index()
        {
            return View(casevotingRepository.All);
        }

        //
        // GET: /CaseVotings/Details/5

        public ViewResult Details(int id)
        {
            return View(casevotingRepository.Find(id));
        }

        //
        // GET: /CaseVotings/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CaseVotings/Create

        [HttpPost]
        public ActionResult Create(CaseVoting casevoting)
        {
            if (ModelState.IsValid) {
                casevotingRepository.InsertOrUpdate(casevoting);
                casevotingRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /CaseVotings/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(casevotingRepository.Find(id));
        }

        //
        // POST: /CaseVotings/Edit/5

        [HttpPost]
        public ActionResult Edit(CaseVoting casevoting)
        {
            if (ModelState.IsValid) {
                casevotingRepository.InsertOrUpdate(casevoting);
                casevotingRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /CaseVotings/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(casevotingRepository.Find(id));
        }

        //
        // POST: /CaseVotings/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            casevotingRepository.Delete(id);
            casevotingRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                casevotingRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

