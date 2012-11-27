using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class CaseVotingRepository : ICaseVotingRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<CaseVoting> All
        {
            get { return context.CaseVotings; }
        }

        public IQueryable<CaseVoting> AllIncluding(params Expression<Func<CaseVoting, object>>[] includeProperties)
        {
            IQueryable<CaseVoting> query = context.CaseVotings;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CaseVoting Find(int id)
        {
            return context.CaseVotings.Find(id);
        }

        public CaseVoting FindByCaseId(int CaseId){
            //return null;// context.CaseVotings.Where(CaseVoting.Equals(aVoting));
            CaseVoting aVoting = (from l in context.CaseVotings
                                  where l.CaseID == CaseId
                                  select l).FirstOrDefault();
            return aVoting;
        }

        public int FindVotes(int id)
        {
            IQueryable<CaseVoting> casevotings = (from CVi in context.CaseVotings
                                              where CVi.CaseID == id
                                              select CVi);
            return casevotings.Count();
        }

        public void InsertOrUpdate(CaseVoting casevoting)
        {
            if (casevoting.VotingID == default(int)) {
                // New entity
                context.CaseVotings.Add(casevoting);
            } else {
                // Existing entity
                context.Entry(casevoting).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var casevoting = context.CaseVotings.Find(id);
            context.CaseVotings.Remove(casevoting);
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

    public interface ICaseVotingRepository : IDisposable
    {
        IQueryable<CaseVoting> All { get; }
        IQueryable<CaseVoting> AllIncluding(params Expression<Func<CaseVoting, object>>[] includeProperties);
        CaseVoting Find(int id);
        int FindVotes(int id);
        void InsertOrUpdate(CaseVoting casevoting);
        void Delete(int id);
        void Save();
    }
}