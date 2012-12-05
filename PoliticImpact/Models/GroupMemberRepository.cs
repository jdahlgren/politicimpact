using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class GroupMemberRepository : IGroupMemberRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<GroupMember> All
        {
            get { return context.GroupMembers; }
        }

        public IQueryable<GroupMember> AllIncluding(params Expression<Func<GroupMember, object>>[] includeProperties)
        {
            IQueryable<GroupMember> query = context.GroupMembers;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public GroupMember Find(int id)
        {
            return context.GroupMembers.Find(id);
        }

        public void InsertOrUpdate(GroupMember groupmember)
        {
            if (groupmember.groupMemberID == default(int)) {
                // New entity
                context.GroupMembers.Add(groupmember);
            } else {
                // Existing entity
                context.Entry(groupmember).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var groupmember = context.GroupMembers.Find(id);
            context.GroupMembers.Remove(groupmember);
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

    public interface IGroupMemberRepository : IDisposable
    {
        IQueryable<GroupMember> All { get; }
        IQueryable<GroupMember> AllIncluding(params Expression<Func<GroupMember, object>>[] includeProperties);
        GroupMember Find(int id);
        void InsertOrUpdate(GroupMember groupmember);
        void Delete(int id);
        void Save();
    }
}