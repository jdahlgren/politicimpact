using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class CaseCommentRepository : ICaseCommentRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<CaseComment> All
        {
            get { return context.CaseComments; }
        }

        public IQueryable<CaseComment> AllIncluding(params Expression<Func<CaseComment, object>>[] includeProperties)
        {
            IQueryable<CaseComment> query = context.CaseComments;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public int FindComments(int caseId)
        {
            IQueryable<CaseComment> casecomments = (from CL in context.CaseComments
                                              where CL.caseID == caseId
                                              select CL);
            return casecomments.Count();
        }
        public CaseComment Find(int id)
        {
            return context.CaseComments.Find(id);
        }

        public IQueryable<CaseComment> FindAllByCaseId(int id)
        {
            return (from cc in context.CaseComments
                    where cc.caseID == id
                    select cc);
        }

        public void InsertOrUpdate(CaseComment casecomment)
        {
            if (casecomment.commentID == default(int)) {
                // New entity
                context.CaseComments.Add(casecomment);
            } else {
                // Existing entity
                context.Entry(casecomment).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var casecomment = context.CaseComments.Find(id);
            context.CaseComments.Remove(casecomment);
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

    public interface ICaseCommentRepository : IDisposable
    {
        IQueryable<CaseComment> All { get; }
        IQueryable<CaseComment> AllIncluding(params Expression<Func<CaseComment, object>>[] includeProperties);
        CaseComment Find(int id);
        void InsertOrUpdate(CaseComment casecomment);
        void Delete(int id);
        void Save();
    }
}