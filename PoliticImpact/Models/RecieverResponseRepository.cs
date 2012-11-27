using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PoliticImpact.Models
{ 
    public class RecieverResponseRepository : IRecieverResponseRepository
    {
        PoliticImpactContext context = new PoliticImpactContext();

        public IQueryable<RecieverResponse> All
        {
            get { return context.RecieverResponses; }
        }

        public IQueryable<RecieverResponse> AllIncluding(params Expression<Func<RecieverResponse, object>>[] includeProperties)
        {
            IQueryable<RecieverResponse> query = context.RecieverResponses;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public RecieverResponse Find(int id)
        {
            return context.RecieverResponses.Find(id);
        }

        public void InsertOrUpdate(RecieverResponse recieverresponse)
        {
            if (recieverresponse.ResponseID == default(int)) {
                // New entity
                context.RecieverResponses.Add(recieverresponse);
            } else {
                // Existing entity
                context.Entry(recieverresponse).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var recieverresponse = context.RecieverResponses.Find(id);
            context.RecieverResponses.Remove(recieverresponse);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        /* Returnerar ResponseID från RecieverResponse-tabellen som överensstämmer 
         * med given ResponseCode från RecieverResponse-tabellen.
         */
        public int GetResponseId(string respCode)
        {
            //Hämtar RecieverResponse som har ResponseCode = respCode
            var resp = (from l in context.RecieverResponses
                        where l.ResponseCode == respCode
                        select l).FirstOrDefault();

            int id;
            if (resp != null) //Om det finns en RecieverResponse med given ResponseCode returneras ResponseID för denna
            {
                id = resp.ResponseID;
                return id;
            }
            else //Om det inte finns någon RecieverResponse returneras 0
            {
                return 0;
            }
        }

        /* Returnerar ResponseText från RecieverResponse-tabellen som
         * överensstämmer med givet ID från CaseItem-tabellen.
         */
        public string GetResponseText(int caseId)
        {
            var caseItem = (from p in context.CaseItems
                            where p.ID == caseId
                            select p).SingleOrDefault();

            int respId;
            if (caseItem != null)
            {
                respId = caseItem.ResponseID;

                var resp = (from l in context.RecieverResponses
                            where l.ResponseID == respId
                            select l).FirstOrDefault();

                string respText;
                if (resp != null)
                {
                    respText = resp.ResponseText;
                    return respText;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /* Returnerar Title från CaseItem-tabellen som överensstämmer 
         * med givet ResponseID från RecieverResponse-tabellen
         * ResponseID är en foreign key i CaseItem
         */
        public string GetCaseTitle(int respId)
        {
            string caseTitle = (from p in context.CaseItems
                             where p.ResponseID == respId
                             select p.Title).SingleOrDefault();

            if (caseTitle != null)
            {
                caseTitle = caseTitle.ToString();
            }

            return caseTitle;
        }

        /* Returnerar Text från CaseItem-tabellen som överensstämmer 
         * med givet ResponseID från RecieverResponse-tabellen
         * ResponseID är en foreign key i CaseItem
         */
        public string GetCaseText(int respId)
        {
            string caseText = (from b in context.CaseItems
                            where b.ResponseID == respId
                            select b.Text).SingleOrDefault();

            if(caseText != null)
            {
                caseText = caseText.ToString();
            }

            return caseText;
        }

        /* Returnerar ID från CaseItem-tabellen som överensstämmer 
         * med givet ResponseID från RecieverResponse-tabellen
         * ResponseID är en foreign key i CaseItem
         */
        public int GetCaseId(int respId)
        {
            var caseItem = (from q in context.CaseItems
                        where q.ResponseID == respId
                        select q).FirstOrDefault();

            int id;
            if (caseItem != null)
            {
                id = caseItem.ID;
                return id;
            }
            else
            {
                return 0;
            }
        }
    }

    public interface IRecieverResponseRepository : IDisposable
    {
        IQueryable<RecieverResponse> All { get; }
        IQueryable<RecieverResponse> AllIncluding(params Expression<Func<RecieverResponse, object>>[] includeProperties);
        RecieverResponse Find(int id);
        void InsertOrUpdate(RecieverResponse recieverresponse);
        void Delete(int id);
        void Save();
        int GetResponseId(string respCode);
        string GetCaseTitle(int respId);
        string GetCaseText(int respId);
        int GetCaseId(int respId);
        string GetResponseText(int caseId);
    }
}