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

        //[HttpPost]
        //public ActionResult Create(int caseID)
        //{
        //    CaseLike caselike = new CaseLike(); 
        //    caselike.caseID = caseID;
        //    //TODO, användarens id.
        //    caselike.userID = 12;
        //    caselike.created = DateTime.Now;

        //    if (ModelState.IsValid) {
        //        caselikeRepository.InsertOrUpdate(caselike);
        //        caselikeRepository.Save();
        //      return RedirectToAction("Index");
        //    } else {
        //        return View();
        //    }
        //}

        [HttpGet]
        public ActionResult LikeCase(int id)
        {

            //  CaseLike caseLike = caselikeRepository.Find(id);

            // caselike.caseID = caseID;
            CaseLike caselike = new CaseLike();

            theUser = 16;
            caselike.caseID = id;
            caselike.userID = theUser;
            caselike.created = DateTime.Now;

            //från Semone
            //Borde kolla så att den som är inloggad inte redan har signat detta caset
            foreach (var item in caselikeRepository.All)
            {
                if (theUser == item.userID && id == item.caseID)
                {
                    //returna någon schyst variabel till popupen
                    //Meddela användaren om att den redan har signat
                    return View();
                }

            }

            if (ModelState.IsValid)
            {
                caselikeRepository.InsertOrUpdate(caselike);
                caselikeRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
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

        public long theUser { get; set; }
    }
}

