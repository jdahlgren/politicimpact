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

  

        [HttpGet]
        public ActionResult LikeCase(int id)
        {

            //  CaseLike caseLike = caselikeRepository.Find(id);

            // caselike.caseID = caseID;
            CaseLike caselike = new CaseLike();

            theUser = Int64.Parse(Session["uid"].ToString());
            caselike.caseID = id;
            caselike.userID = theUser;
            caselike.created = DateTime.Now;

            //från Semones kod för signup
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

        public long theUser { get; set; }
    }
}

