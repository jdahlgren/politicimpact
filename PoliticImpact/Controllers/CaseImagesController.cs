using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class CaseImagesController : Controller
    {
		private readonly ICaseImageRepository caseimageRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseImagesController() : this(new CaseImageRepository())
        {
        }

        public CaseImagesController(ICaseImageRepository caseimageRepository)
        {
			this.caseimageRepository = caseimageRepository;
        }

        //
        // GET: /CaseImages/

        public ViewResult Index()
        {
            return View(caseimageRepository.All);
        }

        //
        // GET: /CaseImages/Details/5

        public ViewResult Details(int id)
        {
            return View(caseimageRepository.Find(id));
        }

        //
        // GET: /CaseImages/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CaseImages/Create

        [HttpPost]
        public ActionResult Create(CaseImage caseimage)
        {
            if (ModelState.IsValid) {
                caseimageRepository.InsertOrUpdate(caseimage);
                caseimageRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /CaseImages/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(caseimageRepository.Find(id));
        }

        //
        // POST: /CaseImages/Edit/5

        [HttpPost]
        public ActionResult Edit(CaseImage caseimage)
        {
            if (ModelState.IsValid) {
                caseimageRepository.InsertOrUpdate(caseimage);
                caseimageRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /CaseImages/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(caseimageRepository.Find(id));
        }

        //
        // POST: /CaseImages/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            caseimageRepository.Delete(id);
            caseimageRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                caseimageRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

