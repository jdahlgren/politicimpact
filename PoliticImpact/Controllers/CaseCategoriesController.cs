using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class CaseCategoriesController : Controller
    {
		private readonly ICaseCategoryRepository casecategoryRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseCategoriesController() : this(new CaseCategoryRepository())
        {
        }

        public CaseCategoriesController(ICaseCategoryRepository casecategoryRepository)
        {
			this.casecategoryRepository = casecategoryRepository;
        }

        //
        // GET: /CaseCategories/

        public ViewResult Index()
        {
            return View(casecategoryRepository.All);
        }

        //
        // GET: /CaseCategories/Details/5

        public ViewResult Details(int id)
        {
            return View(casecategoryRepository.Find(id));
        }

        //
        // GET: /CaseCategories/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CaseCategories/Create

        [HttpPost]
        public ActionResult Create(CaseCategory casecategory)
        {
            if (ModelState.IsValid) {
                casecategoryRepository.InsertOrUpdate(casecategory);
                casecategoryRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /CaseCategories/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(casecategoryRepository.Find(id));
        }

        //
        // POST: /CaseCategories/Edit/5

        [HttpPost]
        public ActionResult Edit(CaseCategory casecategory)
        {
            if (ModelState.IsValid) {
                casecategoryRepository.InsertOrUpdate(casecategory);
                casecategoryRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /CaseCategories/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(casecategoryRepository.Find(id));
        }

        //
        // POST: /CaseCategories/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            casecategoryRepository.Delete(id);
            casecategoryRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                casecategoryRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

