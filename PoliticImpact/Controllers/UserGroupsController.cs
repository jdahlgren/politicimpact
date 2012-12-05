using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class UserGroupsController : Controller
    {
		private readonly IUserGroupRepository usergroupRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public UserGroupsController() : this(new UserGroupRepository())
        {
        }

        public UserGroupsController(IUserGroupRepository usergroupRepository)
        {
			this.usergroupRepository = usergroupRepository;
        }

        //
        // GET: /UserGroups/

        public ViewResult Index()
        {
            return View(usergroupRepository.All);
        }

        //
        // GET: /UserGroups/Details/5

        public ViewResult Details(int id)
        {
            return View(usergroupRepository.Find(id));
        }

        //
        // GET: /UserGroups/Create

        public ActionResult Create()
        {
            //N�r man klickar p� create Group-knappen
            return View();
        } 

        //
        // POST: /UserGroups/Create

        [HttpPost]
        public ActionResult Create(UserGroup usergroup)
        {
            //�garen �r usern som �r inloggad
            usergroup.owner = 1;
            usergroup.created = DateTime.Now;
            
            //N�r man skapar en grupp blir man medlem
            //Ropa p� Create Members om man inte redan �r medlem
            //Kolla om user id finns i tabellen f�r Members
            //usergroup.listOfGroupMembers.Add()


            if (ModelState.IsValid) {
                usergroupRepository.InsertOrUpdate(usergroup);
                usergroupRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /UserGroups/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(usergroupRepository.Find(id));
        }

        //
        // POST: /UserGroups/Edit/5

        [HttpPost]
        public ActionResult Edit(UserGroup usergroup)
        {
            if (ModelState.IsValid) {
                usergroupRepository.InsertOrUpdate(usergroup);
                usergroupRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /UserGroups/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(usergroupRepository.Find(id));
        }

        //
        // POST: /UserGroups/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            usergroupRepository.Delete(id);
            usergroupRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                usergroupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

