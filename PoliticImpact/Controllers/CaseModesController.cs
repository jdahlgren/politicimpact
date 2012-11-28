using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class CaseModesController : Controller
    {
		private readonly ICaseModeRepository casemodeRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseModesController() : this(new CaseModeRepository())
        {
        }

        public CaseModesController(ICaseModeRepository casemodeRepository)
        {
			this.casemodeRepository = casemodeRepository;
        }

        //
        // GET: /CaseModes/

        public ViewResult Index()
        {
            return View(casemodeRepository.All);
        }

        //
        // GET: /CaseModes/Details/5

        public ViewResult Details(long id)
        {
            return View(casemodeRepository.Find(id));
        }

        //
        // GET: /CaseModes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CaseModes/Create

        [HttpPost]
        public ActionResult Create(CaseMode casemode)
        {
            if (ModelState.IsValid) {
                casemodeRepository.InsertOrUpdate(casemode);
                casemodeRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
                
        //
        // GET: /CaseModes/Edit/5
 
        //public ActionResult Edit(long id)
        //{
        //     return View(casemodeRepository.Find(id));
        //}

        ////
        //// POST: /CaseModes/Edit/5

        //[HttpPost]
        //public ActionResult Edit(CaseMode casemode)
        //{
        //    if (ModelState.IsValid) {
        //        casemodeRepository.InsertOrUpdate(casemode);
        //        casemodeRepository.Save();
        //        return RedirectToAction("Index");
        //    } else {
        //        return View();
        //    }
        //}

        ////
        //// GET: /CaseModes/Delete/5
 
        //public ActionResult Delete(long id)
        //{
        //    return View(casemodeRepository.Find(id));
        //}

        ////
        //// POST: /CaseModes/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    casemodeRepository.Delete(id);
        //    casemodeRepository.Save();

        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing) {
        //        casemodeRepository.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}

