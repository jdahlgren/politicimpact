using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class CaseCategoryRepository : ICaseCategoryRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<CaseCategory> All
        {
            get { return context.CaseCategories; }
        }

        public IQueryable<CaseCategory> AllIncluding(params Expression<Func<CaseCategory, object>>[] includeProperties)
        {
            IQueryable<CaseCategory> query = context.CaseCategories;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CaseCategory Find(int id)
        {
            return context.CaseCategories.Find(id);
        }

        public void InsertOrUpdate(CaseCategory casecategory)
        {
            if (casecategory.CategoryID == default(int)) {
                // New entity
                context.CaseCategories.Add(casecategory);
            } else {
                // Existing entity
                context.Entry(casecategory).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var casecategory = context.CaseCategories.Find(id);
            context.CaseCategories.Remove(casecategory);
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

    public interface ICaseCategoryRepository : IDisposable
    {
        IQueryable<CaseCategory> All { get; }
        IQueryable<CaseCategory> AllIncluding(params Expression<Func<CaseCategory, object>>[] includeProperties);
        CaseCategory Find(int id);
        void InsertOrUpdate(CaseCategory casecategory);
        void Delete(int id);
        void Save();
    }
}