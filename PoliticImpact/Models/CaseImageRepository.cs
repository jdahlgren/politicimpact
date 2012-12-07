using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class CaseImageRepository : ICaseImageRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<CaseImage> All
        {
            get { return context.CaseImages; }
        }

        public IQueryable<CaseImage> AllIncluding(params Expression<Func<CaseImage, object>>[] includeProperties)
        {
            IQueryable<CaseImage> query = context.CaseImages;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public CaseImage Find(int id)
        {
            return context.CaseImages.Find(id);
        }

        public void InsertOrUpdate(CaseImage caseimage)
        {
            if (caseimage.ImageID == default(int)) {
                // New entity
                context.CaseImages.Add(caseimage);
            } else {
                // Existing entity
                context.Entry(caseimage).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var caseimage = context.CaseImages.Find(id);
            context.CaseImages.Remove(caseimage);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public string GetImageUrl(int CaseId)
        {
            //Hämtar CaseImage som har rätt CaseID
            var img = (from l in context.CaseImages
                       where l.CaseID == CaseId
                       select l).FirstOrDefault();

            string imgUrl;
            if (img != null)
            {
                imgUrl = img.ImageUrl;
                return imgUrl;
            }
            else
            {
                return null;
            }
        }

        public string GetThumbnailUrl(int CaseId)
        {
            //Hämtar CaseImage som har rätt CaseID
            var img = (from l in context.CaseImages
                       where l.CaseID == CaseId
                       select l).FirstOrDefault();

            string thumbUrl;
            if (img != null)
            {
                thumbUrl = img.thumbnailUrl;
                return thumbUrl;
            }
            else
            {
                return null;
            }
        }
    }

    public interface ICaseImageRepository : IDisposable
    {
        IQueryable<CaseImage> All { get; }
        IQueryable<CaseImage> AllIncluding(params Expression<Func<CaseImage, object>>[] includeProperties);
        CaseImage Find(int id);
        void InsertOrUpdate(CaseImage caseimage);
        void Delete(int id);
        void Save();
        string GetImageUrl(int caseId);
        string GetThumbnailUrl(int caseId);
    }
}