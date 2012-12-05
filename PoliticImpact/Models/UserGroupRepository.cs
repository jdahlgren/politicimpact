using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class UserGroupRepository : IUserGroupRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<UserGroup> All
        {
            get { return context.UserGroups; }
        }

        public IQueryable<UserGroup> AllIncluding(params Expression<Func<UserGroup, object>>[] includeProperties)
        {
            IQueryable<UserGroup> query = context.UserGroups;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public UserGroup Find(int id)
        {
            return context.UserGroups.Find(id);
        }

        public void InsertOrUpdate(UserGroup usergroup)
        {
            if (usergroup.userGroupID == default(int)) {
                // New entity
                context.UserGroups.Add(usergroup);
            } else {
                // Existing entity
                context.Entry(usergroup).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var usergroup = context.UserGroups.Find(id);
            context.UserGroups.Remove(usergroup);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IUserGroupRepository : IDisposable
    {
        IQueryable<UserGroup> All { get; }
        IQueryable<UserGroup> AllIncluding(params Expression<Func<UserGroup, object>>[] includeProperties);
        UserGroup Find(int id);
        void InsertOrUpdate(UserGroup usergroup);
        void Delete(int id);
        void Save();
    }
}