using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class CaseLikesController : Controller
    {
		private readonly ICaseLikeRepository caselikeRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseLikesController() : this(new CaseLikeRepository())
        {
        }

        public CaseLikesController(ICaseLikeRepository caselikeRepository)
        {
			this.caselikeRepository = caselikeRepository;
        }

        //
        // GET: /CaseLikes/

        public ViewResult Index()
        {
            return View(caselikeRepository.All);
        }

        //
        // GET: /CaseLikes/Details/5

        public ViewResult Details(long id)
        {
            return View(caselikeRepository.Find(id));
        }

        //
        // GET: /CaseLikes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CaseLikes/Create

        [HttpPost]
        public ActionResult Create(int caseID)
        {
           // caselike.caseID = caseID;
            CaseLike caselike = new CaseLike(); 
            caselike.caseID = caseID;
            //TODO, anv�ndarens id.
            caselike.userID = 12;
            caselike.created = DateTime.Now;

            if (ModelState.IsValid) {
                caselikeRepository.InsertOrUpdate(caselike);
                caselikeRepository.Save();
              return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        [HttpPost]
        public ActionResult LikeCase(int id)
        {

          //  CaseLike caseLike = caselikeRepository.Find(id);

                  // caselike.caseID = caseID;
            CaseLike caselike = new CaseLike(); 
            caselike.caseID = id;
            //TODO, anv�ndarens id.
            caselike.userID = 12;
            caselike.created = DateTime.Now;

            if (ModelState.IsValid) {
                caselikeRepository.InsertOrUpdate(caselike);
                caselikeRepository.Save();
              return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        //
        // GET: /CaseLikes/Edit/5

        //public ActionResult Edit(long id)
        //{
        //     return View(caselikeRepository.Find(id));
        //}

        ////
        //// POST: /CaseLikes/Edit/5

        //[HttpPost]
        //public ActionResult Edit(CaseLike caselike)
        //{
        //    if (ModelState.IsValid) {
        //        caselikeRepository.InsertOrUpdate(caselike);
        //        caselikeRepository.Save();
        //        return RedirectToAction("Index");
        //    } else {
        //        return View();
        //    }
        //}

        ////
        //// GET: /CaseLikes/Delete/5

        //public ActionResult Delete(long id)
        //{
        //    return View(caselikeRepository.Find(id));
        //}

        ////
        //// POST: /CaseLikes/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    caselikeRepository.Delete(id);
        //    caselikeRepository.Save();

        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing) {
        //        caselikeRepository.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}

