using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class CaseItemRepository : ICaseItemRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<CaseItem> All
        {
            get { return context.CaseItems; }
        }
        public IQueryable<CaseItem> FindAll()
        {
            ICaseSignUpRepository casesignupRepository = new CaseSignUpRepository();
            CaseCommentRepository caseCommentRepository = new CaseCommentRepository();
            ICaseLikeRepository caselikeRepository = new CaseLikeRepository();
            
            IQueryable<CaseItem> caseItems = context.CaseItems.OrderByDescending(c => c.ID);

            foreach (CaseItem caseItem in caseItems)
            {
                caseItem.numberOfSigns = casesignupRepository.FindSignUps(caseItem.ID);
                caseItem.numberOfLikes = caselikeRepository.FindLike(caseItem.ID);
                caseItem.numberOfComments = caseCommentRepository.FindComments(caseItem.ID);
            }

            return caseItems;
        }

        public IQueryable<CaseItem> AllIncluding(params Expression<Func<CaseItem, object>>[] includeProperties)
        {
            IQueryable<CaseItem> query = context.CaseItems;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CaseItem Find(int id)
        {
            return context.CaseItems.Find(id);
        }

public List<CaseItem> SearchItem(String searchWord)
        {
            IQueryable<CaseItem> QueryMatches = from CaseItem in context.CaseItems
                          where CaseItem.Text.Contains(searchWord) ||
                                CaseItem.Title.Contains(searchWord) ||
                                CaseItem.RecieverName.Contains(searchWord)
                          select CaseItem;

            List<CaseItem> matches = QueryMatches.OrderByDescending(c => c.ID).ToList();

            return matches;
        }


        public void InsertOrUpdate(CaseItem caseitem)
        {
            if (caseitem.ID == default(int)) {
                // New entity
                context.CaseItems.Add(caseitem);
            } else {
                // Existing entity
                context.Entry(caseitem).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var caseitem = context.CaseItems.Find(id);
            context.CaseItems.Remove(caseitem);
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

    public interface ICaseItemRepository : IDisposable
    {
        IQueryable<CaseItem> All { get; }
        IQueryable<CaseItem> AllIncluding(params Expression<Func<CaseItem, object>>[] includeProperties);
List<CaseItem> SearchItem(String searchWord);
        CaseItem Find(int id);
        void InsertOrUpdate(CaseItem caseitem);
        void Delete(int id);
        void Save();
    }
}