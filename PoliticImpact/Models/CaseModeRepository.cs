using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class CaseModeRepository : ICaseModeRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<CaseMode> All
        {
            get { return context.CaseModes; }
        }

        public IQueryable<CaseMode> AllIncluding(params Expression<Func<CaseMode, object>>[] includeProperties)
        {
            IQueryable<CaseMode> query = context.CaseModes;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CaseMode Find(long id)
        {
            return context.CaseModes.Find(id);
        }

        public void InsertOrUpdate(CaseMode casemode)
        {
            if (casemode.caseModeID == default(long)) {
                // New entity
                context.CaseModes.Add(casemode);
            } else {
                // Existing entity
                context.Entry(casemode).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var casemode = context.CaseModes.Find(id);
            context.CaseModes.Remove(casemode);
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

    public interface ICaseModeRepository : IDisposable
    {
        IQueryable<CaseMode> All { get; }
        IQueryable<CaseMode> AllIncluding(params Expression<Func<CaseMode, object>>[] includeProperties);
        CaseMode Find(long id);
        void InsertOrUpdate(CaseMode casemode);
        void Delete(long id);
        void Save();
    }
}