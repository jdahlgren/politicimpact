using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class CaseLikeRepository : ICaseLikeRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<CaseLike> All
        {
            get { return context.CaseLikes; }
        }

        public IQueryable<CaseLike> AllIncluding(params Expression<Func<CaseLike, object>>[] includeProperties)
        {
            IQueryable<CaseLike> query = context.CaseLikes;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CaseLike Find(long id)
        {
            return context.CaseLikes.Find(id);
        }

        
        public int FindLike(int caseId)
        {
            IQueryable<CaseLike> caselikes = (from CL in context.CaseLikes 
                                                  where CL.caseID == caseId
                                                  select CL);
            return caselikes.Count();
        }

        public int FindAndCountLikes(int caseId, DateTime day)
        {
            IQueryable<CaseLike> caselikes = context.CaseLikes.Where(CL => CL.caseID == caseId && CL.created.Year == day.Year && CL.created.Month == day.Month && CL.created.Day == day.Day);
     
            return caselikes.Count();
        }
        
        public int[] StatisticLikes(int caseID)
        {
            var dayLikes = new int[7];
            var today = DateTime.Now;
          
            for (var i = 0; i < 7; i++)
            {    
                dayLikes[i] = FindAndCountLikes(caseID,today.Date.AddDays(-i));
            }
           
           return dayLikes;
        }


        public void InsertOrUpdate(CaseLike caselike)
        {
            if (caselike.likeID == default(long)) {
                // New entity
                context.CaseLikes.Add(caselike);
            } else {
                // Existing entity
                context.Entry(caselike).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var caselike = context.CaseLikes.Find(id);
            context.CaseLikes.Remove(caselike);
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

    public interface ICaseLikeRepository : IDisposable
    {
        IQueryable<CaseLike> All { get; }
        IQueryable<CaseLike> AllIncluding(params Expression<Func<CaseLike, object>>[] includeProperties);
        CaseLike Find(long id);
        void InsertOrUpdate(CaseLike caselike);
        void Delete(long id);
        void Save();

        int FindLike(int id);

        int[] StatisticLikes(int id);
    }
}