using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class CaseCommentsController : Controller
    {
		private readonly ICaseCommentRepository casecommentRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseCommentsController() : this(new CaseCommentRepository())
        {
        }

        public CaseCommentsController(ICaseCommentRepository casecommentRepository)
        {
			this.casecommentRepository = casecommentRepository;
        }

        //
        // GET: /CaseComments/

        public ViewResult Index()
        {
            return View(casecommentRepository.All);
        }

        //
        // GET: /CaseComments/Details/5

        public ViewResult Details(int id)
        {
            return View(casecommentRepository.Find(id));
        }

        //
        // GET: /CaseComments/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CaseComments/Create

        [HttpPost]
        public ActionResult Create(CaseComment casecomment)
        {
            //Spara ner variablerna fr�n vyn

            //L�gg till i databasen

            //Uppdatera vyn

            if (ModelState.IsValid) {
                casecommentRepository.InsertOrUpdate(casecomment);
                casecommentRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        /// Till Admin-delen
        ////
        //// GET: /CaseComments/Edit/5
 
        //public ActionResult Edit(int id)
        //{
        //     return View(casecommentRepository.Find(id));
        //}

        ////
        //// POST: /CaseComments/Edit/5

        //[HttpPost]
        //public ActionResult Edit(CaseComment casecomment)
        //{
        //    if (ModelState.IsValid) {
        //        casecommentRepository.InsertOrUpdate(casecomment);
        //        casecommentRepository.Save();
        //        return RedirectToAction("Index");
        //    } else {
        //        return View();
        //    }
        //}

        ////
        //// GET: /CaseComments/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    return View(casecommentRepository.Find(id));
        //}

        ////
        //// POST: /CaseComments/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    casecommentRepository.Delete(id);
        //    casecommentRepository.Save();

        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing) {
        //        casecommentRepository.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}

