using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoliticImpact.Models;

namespace PoliticImpact.Controllers
{   
    public class GroupMembersController : Controller
    {
		private readonly IUserGroupRepository usergroupRepository;
		private readonly IGroupMemberRepository groupmemberRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public GroupMembersController() : this(new UserGroupRepository(), new GroupMemberRepository())
        {
        }

        public GroupMembersController(IUserGroupRepository usergroupRepository, IGroupMemberRepository groupmemberRepository)
        {
			this.usergroupRepository = usergroupRepository;
			this.groupmemberRepository = groupmemberRepository;
        }

        //
        // GET: /GroupMembers/

        public ViewResult Index()
        {
            return View(groupmemberRepository.All);
        }

        //
        // GET: /GroupMembers/Details/5

        public ViewResult Details(int id)
        {
            return View(groupmemberRepository.Find(id));
        }

        //
        // GET: /GroupMembers/Create

        public ActionResult Create()
        {
			ViewBag.PossibleuserGroups = usergroupRepository.All;
            return View();
        } 

        //
        // POST: /GroupMembers/Create

        [HttpPost]
        public ActionResult Create(GroupMember groupmember)
        {
            if (ModelState.IsValid) {
                groupmemberRepository.InsertOrUpdate(groupmember);
                groupmemberRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleuserGroups = usergroupRepository.All;
				return View();
			}
        }
        
        //
        // GET: /GroupMembers/Edit/5
 
        public ActionResult Edit(int id)
        {
			ViewBag.PossibleuserGroups = usergroupRepository.All;
             return View(groupmemberRepository.Find(id));
        }

        //
        // POST: /GroupMembers/Edit/5

        [HttpPost]
        public ActionResult Edit(GroupMember groupmember)
        {
            if (ModelState.IsValid) {
                groupmemberRepository.InsertOrUpdate(groupmember);
                groupmemberRepository.Save();
                return RedirectToAction("Index");
            } else {
				ViewBag.PossibleuserGroups = usergroupRepository.All;
				return View();
			}
        }

        //
        // GET: /GroupMembers/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(groupmemberRepository.Find(id));
        }

        //
        // POST: /GroupMembers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            groupmemberRepository.Delete(id);
            groupmemberRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                usergroupRepository.Dispose();
                groupmemberRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

