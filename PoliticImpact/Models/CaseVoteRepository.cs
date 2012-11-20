using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class CaseVoteRepository : ICaseVoteRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<CaseVote> All
        {
            get { return context.CaseVotes; }
        }

        public IQueryable<CaseVote> AllIncluding(params Expression<Func<CaseVote, object>>[] includeProperties)
        {
            IQueryable<CaseVote> query = context.CaseVotes;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CaseVote Find(int id)
        {
            return context.CaseVotes.Find(id);
        }

        public IQueryable<CaseVote> FindAllByVotingId(int id)
        {
            return (from l in context.CaseVotes
                    where l.VotingID == id
                    select l);
        }

        public void InsertOrUpdate(CaseVote casevote)
        {
            if (casevote.VoteID == default(int)) {
                // New entity
                context.CaseVotes.Add(casevote);
            } else {
                // Existing entity
                context.Entry(casevote).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var casevote = context.CaseVotes.Find(id);
            context.CaseVotes.Remove(casevote);
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

    public interface ICaseVoteRepository : IDisposable
    {
        IQueryable<CaseVote> All { get; }
        IQueryable<CaseVote> AllIncluding(params Expression<Func<CaseVote, object>>[] includeProperties);
        CaseVote Find(int id);
        void InsertOrUpdate(CaseVote casevote);
        void Delete(int id);
        void Save();
    }
}