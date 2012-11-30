using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class CaseVotesController : Controller
    {
		private readonly ICaseVoteRepository casevoteRepository;

        private long theUser = 1414;

		// If you are using Dependency Injection, you can delete the following constructor
        public CaseVotesController() : this(new CaseVoteRepository())
        {
        }

        public CaseVotesController(ICaseVoteRepository casevoteRepository)
        {
			this.casevoteRepository = casevoteRepository;
        }

        //
        // GET: /CaseVotes/

        public ViewResult Index()
        {
            return View(casevoteRepository.All);
        }

        //
        // GET: /CaseVotes/Details/5

        public ViewResult Details(int id)
        {
            return View(casevoteRepository.Find(id));
        }

        //
        // GET: /CaseVotes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CaseVotes/Create

        [HttpPost]
        public ActionResult Create(int id)
        {
            if (Session["uid"] != null)
            {
                //Hämta session-id
                theUser = Convert.ToInt64(Session["uid"].ToString());
                
            }
            CaseVote casevote = new CaseVote();
            casevote.VotingID = id;
            if(Request["Vote"]=="yes"){
                casevote.Vote = true;
            }
            else
            {
                casevote.Vote = false;
            }

            casevote.UserID = theUser;//TODO: fix so that this is the actual user fb-id
            
            if (ModelState.IsValid) {
                casevoteRepository.InsertOrUpdate(casevote);
                casevoteRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /CaseVotes/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(casevoteRepository.Find(id));
        }

        //
        // POST: /CaseVotes/Edit/5

        [HttpPost]
        public ActionResult Edit(CaseVote casevote)
        {
            if (ModelState.IsValid) {
                casevoteRepository.InsertOrUpdate(casevote);
                casevoteRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /CaseVotes/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(casevoteRepository.Find(id));
        }

        //
        // POST: /CaseVotes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            casevoteRepository.Delete(id);
            casevoteRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                casevoteRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

