using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class CaseSignUpRepository : ICaseSignUpRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<CaseSignUp> All
        {
            get { return context.CaseSignUps; }
        }

        public IQueryable<CaseSignUp> AllIncluding(params Expression<Func<CaseSignUp, object>>[] includeProperties)
        {
            IQueryable<CaseSignUp> query = context.CaseSignUps;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CaseSignUp Find(int id)
        {
            return context.CaseSignUps.Find(id);
        }

        public IQueryable<CaseSignUp> FindAllByCaseId(int id)
        {
            return (from cs in context.CaseSignUps
                    where cs.caseID == id
                    select cs);
        }

        public void InsertOrUpdate(CaseSignUp casesignup)
        {
            if (casesignup.caseID == default(int)) {
                // New entity
                context.CaseSignUps.Add(casesignup);
            } else {
                // Existing entity
                context.Entry(casesignup).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var casesignup = context.CaseSignUps.Find(id);
            context.CaseSignUps.Remove(casesignup);
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

    public interface ICaseSignUpRepository : IDisposable
    {
        IQueryable<CaseSignUp> All { get; }
        IQueryable<CaseSignUp> AllIncluding(params Expression<Func<CaseSignUp, object>>[] includeProperties);
        IQueryable<CaseSignUp> FindAllByCaseId(int id);
        CaseSignUp Find(int id);
        void InsertOrUpdate(CaseSignUp casesignup);
        void Delete(int id);
        void Save();
    }
}