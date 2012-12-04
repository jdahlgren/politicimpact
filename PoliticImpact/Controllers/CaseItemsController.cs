using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;
using PoliticImpact.Models;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;


namespace PoliticImpact.Controllers
{
    public class CaseItemsController : Controller
    {
        private readonly ICaseItemRepository caseitemRepository;

        private readonly ICaseCategoryRepository casecategoryRepository;
        private readonly ICaseSignUpRepository casesignupRepository;

        private readonly ICaseVotingRepository CaseVotingRepository;
        private readonly ICaseLikeRepository caselikeRepository;


        private readonly CaseVotingRepository caseVotingRepository;
        private readonly CaseVoteRepository caseVoteRepository;


        private readonly IRecieverResponseRepository recieverresponseRepository;

        private readonly CaseCommentRepository caseCommentRepository;

        private readonly ICaseImageRepository caseimageRepository;

        private int theUser = 1414;


        // If you are using Dependency Injection, you can delete the following constructor
        public CaseItemsController() : this(new CaseItemRepository())
        {
        }

        public CaseItemsController(ICaseItemRepository caseitemRepository)
        {
            this.caseitemRepository = caseitemRepository;

            this.casecategoryRepository = new CaseCategoryRepository();

            this.CaseVotingRepository = new CaseVotingRepository();

            this.caseVotingRepository = new CaseVotingRepository();
            this.caseVoteRepository = new CaseVoteRepository();
            this.casesignupRepository = new CaseSignUpRepository();

            this.caseCommentRepository = new CaseCommentRepository();
            this.caselikeRepository = new CaseLikeRepository();

            this.recieverresponseRepository = new RecieverResponseRepository();

            this.caseimageRepository = new CaseImageRepository();
        }
        /**
         *Funktion för att användaren ska kunna Gilla ett ärende såvida den inte redan gillat. 
         */
        [HttpGet]
        public ActionResult LikeCase(int id)
        {
            //från Semones kod för signup
            //kollar så att den som är inloggad inte redan har gillat förslaget
            foreach (var item in caselikeRepository.All)
            {
                if (theUser == item.userID && id == item.caseID)
                {
                    //Detailsaction har koll på om användaren har gillat förslaget 
                    return View(); //returnerar detailsvyn.
                }

            }
            
            CaseLike caselike = new CaseLike();

            caselike.caseID = id;
            caselike.userID = theUser;
            caselike.created = DateTime.Now;



            if (ModelState.IsValid)
            {
                caselikeRepository.InsertOrUpdate(caselike);
                caselikeRepository.Save();
                return RedirectToAction("Details/" + id);
            }
            else
            {
                return View();
            }

        }



        [HttpGet]
        public ActionResult SignUp(int caseitem)
        {
            //Semone Kallin Clarke 2012-11-13

            //Hämta användaren som är inloggad, nu hårdkodad (2012-11-16)

            //Borde kolla så att den som är inloggad inte redan har signat detta caset
            foreach (var item in casesignupRepository.All)
            {
                if (theUser == item.userID && caseitem == item.CaseItemID)
                {
                    //returna någon schyst variabel till popupen
                    //Meddela användaren om att den redan har signat
                    return View();
                }

            }

            CaseSignUp casesignup = new CaseSignUp();

            casesignup.created = DateTime.Now;
            casesignup.CaseItemID = caseitem;
            casesignup.userID = theUser; //den som är inloggad.


            if (ModelState.IsValid)
            {
                casesignupRepository.InsertOrUpdate(casesignup);
                casesignupRepository.Save();
                return RedirectToAction("Details/" + caseitem);
                //Meddela användaren om att signen gick bra
            }
            else
            {
                //returna någon schyst variabel till popupen
                return View();
            }

        }

        //
        // GET: /CaseItems/

        public ViewResult Index()
        {

            System.Diagnostics.Debug.WriteLine("asdf");
            return View(caseitemRepository.All);
        }

        public ViewResult Image(int id)
        {
            byte[] imgData = caseimageRepository.GetImageDataById(id);
            if (imgData != null)
            {
                ViewBag.image = imgData;
            }

            return View();
        }

        //
        // GET: /CaseItems/Details/5

        public ViewResult Details(int id)
        {
            //Kod för att skicka eventuell respons till ett case i CaseItem-view
            string response = recieverresponseRepository.GetResponseText(id);

            if (response != null)
            {
                ViewBag.responded = true;
                ViewBag.response = response;
            }
            else
            {
                ViewBag.responded = false;
            }
            //slut på kod för respons på case

            int numberOfLikes = caselikeRepository.FindLike(id);
            ViewBag.numberOfLikes = numberOfLikes;

            Boolean UserHasVoted = false;
            CaseVoting casevoting = caseVotingRepository.FindByCaseId(id);


            if (casevoting != null)
            {
                ViewBag.voting = casevoting;
                IQueryable<CaseVote> votes = caseVoteRepository.FindAllByVotingId(casevoting.VotingID);
                ViewBag.votes = votes.Count();

                foreach (var vote in votes)
                {
                    if (vote.UserID == 1337)//TODO compare with actual fb userid
                    {
                        UserHasVoted = true;

                    }

                }
                ViewBag.userhasvoted = UserHasVoted;

            }
            
            
            IQueryable<CaseComment> casecomments = caseCommentRepository.FindAllByCaseId(id);
            ViewBag.casecomments = casecomments;
            ViewBag.nrOfComments = casecomments.Count();

            //TODO real user
            foreach (var item in casesignupRepository.All)
            {
                if (theUser == item.userID && id == item.CaseItemID)
                {
                    //returna någon schyst variabel till popupen
                    //Meddela användaren om att den redan har signat
                    ViewBag.status = "signed";
                }

            }

            foreach (var item in caselikeRepository.All)
            {
                if (theUser == item.userID && id == item.caseID)
                {
                    //returna någon schyst variabel till popupen
                    //Meddela användaren om att den redan har signat
                    ViewBag.likeStatus = "signed";
                }

            }
            return View(caseitemRepository.Find(id));
        }

        //
        // GET: /CaseItems/Create

        public ActionResult Create()
        {

            ViewBag.PossibleCategories = casecategoryRepository.All;

            return View();
        }


        /* Returnerar en "one time code" för givet CaseItem
         */
        private string GenerateResponseCode(CaseItem caseitem)
        {
            //Koden som genereras är baserad på aktuellt CaseItems ID och titel:
            string stringToCode = caseitem.ID.ToString() + caseitem.Title;
            MD5CryptoServiceProvider md5CSP = new MD5CryptoServiceProvider();

            //Skapar en array av bytes som motsvarar strängen som ska kodas
            byte[] bArr = Encoding.ASCII.GetBytes(stringToCode);
            //Krypterar med en md5 hash 
            bArr = md5CSP.ComputeHash(bArr);

            //Konverterar till string, tar bort bindestreck och returnerar den färdiga koden
            return BitConverter.ToString(bArr).Replace("-", "");
        }


        //
        // POST: /CaseItems/Create

        [HttpPost]
        public ActionResult Create(CaseItem caseitem, HttpPostedFileBase image, HttpPostedFileBase document)
        {
            RecieverResponse resp = new RecieverResponse();
            resp.ResponseCode = GenerateResponseCode(caseitem);
            recieverresponseRepository.InsertOrUpdate(resp);
            recieverresponseRepository.Save();

            if (Session["uid"] != null)
            {
                //Hämta session-id
                long userId = Convert.ToInt64(Session["uid"].ToString());
                caseitem.Owner = userId;
            }
            else
            {
                caseitem.Owner = 4;
            }
            caseitem.caseMode = 0;
            caseitem.Created = DateTime.Now;
            caseitem.LastEdited = Convert.ToDateTime("2012-01-01");
            caseitem.ResponseID = resp.ResponseID;

            if (ModelState.IsValid)
            {
                caseitemRepository.InsertOrUpdate(caseitem);
                caseitemRepository.Save();

                //validering och sparning av bild
                if (image != null)
                {
                    switch (image.ContentType)
                    {
                        //tillåtna filtyper:
                        case "image/jpeg":
                        case "image/png":
                        case "image/gif":
                            CaseImage img = new CaseImage();
                            img.CaseID = caseitem.ID;
                 
                            img.ImageBytes = new byte[image.ContentLength];

                            image.InputStream.Read(img.ImageBytes, 0, image.ContentLength);

                            caseimageRepository.InsertOrUpdate(img);
                            caseimageRepository.Save();

                            caseitem.AttachedImage = true;
                            break;
                        default: //Otillåten filtyp
                            caseitem.AttachedImage = false;
                            /* Lägg till validerings-errormeddelande i vy:
                             * ModelState.AddModelError("key", "message");
                             * i view: @Html.ValidationSummary(false)
                             */
                            break;
                    }

                }
                else
                {
                    caseitem.AttachedImage = false;
                }
                //slut på validering och sparning bild

                //dokumentuppladdning:
                if (document != null)
                {
                    caseitem.documentMimeType = document.ContentType;
                    caseitem.documentName = document.FileName;
                    string location =   "~/Content/uploadedDocuments/" + document.FileName;
                    caseitem.documentUrl = location;
                    document.SaveAs(Server.MapPath(location));
                }

                resp.ResponseCode = GenerateResponseCode(caseitem);
                recieverresponseRepository.InsertOrUpdate(resp);
                recieverresponseRepository.Save();

                if (Request["create_voting"] != "" && Request["create_voting"] != null)
                {
                    //have the user requested a voting on this case?
                    if (Request["create_voting"] == "yes" && Request["voting_title"] != "" && Request["voting_title"] != null)
                    {
                        CaseVoting casevoting = new CaseVoting();
                        casevoting.CaseID = caseitem.ID;
                        casevoting.Title = Request["voting_title"];
                        casevoting.StartDate = DateTime.Now;
                        casevoting.EndDate = DateTime.Now;
                        casevoting.Created = DateTime.Now;
                        caseVotingRepository.InsertOrUpdate(casevoting);
                        caseVotingRepository.Save();
                    }

                }


            }
            else
            {
                ViewBag.PossibleCategories = casecategoryRepository.All;

            }


            caseitem.caseComment = new List<CaseComment>();
            if (ModelState.IsValid)
            {
                caseitemRepository.InsertOrUpdate(caseitem);
                caseitemRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateComment(CaseComment caseComment)
        {
            if (ModelState.IsValid)
            {
                var casecommentRepository = new CaseCommentRepository();
                casecommentRepository.InsertOrUpdate(caseComment);
                casecommentRepository.Save();

                return RedirectToAction("Details", new { id = caseComment.caseID });
            }
            return RedirectToAction("Index");
        }


        //
        // GET: /CaseItems/Edit/5

        public ActionResult Edit(int id)
        {
            return View(caseitemRepository.Find(id));
        }

        //
        // POST: /CaseItems/Edit/5

        [HttpPost]
        public ActionResult Edit(CaseItem caseitem)
        {
            if (ModelState.IsValid)
            {
                caseitemRepository.InsertOrUpdate(caseitem);
                caseitemRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /CaseItems/Delete/5

        //Edited by johannes dahlgren 20/11 2012
        public ActionResult Delete(int id)
        {
            caseitemRepository.Delete(id);
            caseitemRepository.Save();

            return View();

        }

        //
        // POST: /CaseItems/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            caseitemRepository.Delete(id);
            caseitemRepository.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Publish(int id)
        {
            CaseItem caseitem = caseitemRepository.Find(id);
            caseitem.Published = true;
            caseitemRepository.InsertOrUpdate(caseitem);
            caseitemRepository.Save();
            return View();
        }
        /**
         * public ActionResult Archive
         * En action som arkiverar ett ärende.
         * Sätter boolean "Archived" till true när funktionen anropas
         * och updaterar databasen.
         */
        [HttpPost]
        public ActionResult Archive(int id)
        {
            CaseItem caseitem = caseitemRepository.Find(id);
            caseitem.Archived = true;
            caseitemRepository.InsertOrUpdate(caseitem);
            caseitemRepository.Save();
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        /**
         * SubmitSearch - en funktion som hämtar sökvariabel och skickar den till caseitemRepository,
         * returnerar en view.
         */
        [HttpPost]
        public ActionResult SubmitSearch(FormCollection collection)
        {
            string searchWord = collection.Get("search");
            ViewBag.result = caseitemRepository.SearchItem(searchWord);
            ViewBag.word = searchWord;

            return View();
        }

        [HttpPost]
        public ActionResult ShareMail(int id)
        {

            CaseItem caseItem = caseitemRepository.Find(id);
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();

            try
            {
                m.From = new MailAddress("politicalimpact@gmail.com", "Politic Impact");
                m.To.Add(new MailAddress(Request["email"]));
                //m.CC.Add(new MailAddress("chrda005@student.liu.se","Display name CC"));
                m.Subject = "Political Impact: Shared Case";
                m.Body = caseItem.Title + caseItem.Text;

                //Attachment
                //FileStream fs = new FileStream("E:\\TestFolder\\test.pdf", FileMode.Open, FileAccess.Read);
                //Attachment a = new Attachment(fs, "test.pdf", MediaTypeNames.Application.Octet);

                //string str = "<html><body><h1>Picture<h/h1><br/><img src=\cid:image1\"></body></html>";
                //AlternateView av = AlternateView.CreateAlternateViewFromString(str,null,MediaTypeNames.Text.Html);
                //LinkedResource lr = new LinkedResource("E:\\Photos\\hello.jpg",MediaTypeNames.Image.Jpeg);
                //lr.ContentId = "image1";
                //av.LinkedResources.Add(lr);
                //m.AlternateViews.Add(av);

                sc.Host = "smtp.gmail.com";
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("politicalimpact@gmail.com", "pumTNM090");
                sc.EnableSsl = true;
                sc.Send(m);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View();
        }

        /**
        * ReportMail – En funktion som sammanställer en rapport och skickar ut till mottagaren av ett förslag.
        */
        public ActionResult ReportMail(int id)
        {
            CaseItem caseItem = caseitemRepository.Find(id);
            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();

            try
            {
                m.From = new MailAddress("politicalimpact@gmail.com", "Politic Impact");
                m.To.Add(new MailAddress(caseItem.RecieverEmail, caseItem.RecieverName));
                m.Subject = "Politic Impact: Rapport på "+caseItem.Title;
                //m.Body = "Behövs inte om man har AlternativeView";

                //Attachment
                //FileStream fs = new FileStream("E:\\TestFolder\\test.pdf", FileMode.Open, FileAccess.Read);
                //Attachment a = new Attachment(fs, "test.pdf", MediaTypeNames.Application.Octet);
                int numberOfLikes = caselikeRepository.FindLike(id);
                int numberOfSignUps = casesignupRepository.FindSignUps(id);
                int numberOfVotes = caseVotingRepository.FindVotes(id);
                IQueryable<CaseComment> comments = caseCommentRepository.FindAllByCaseId(id);
                List<CaseComment> comments2  = comments.ToList<CaseComment>();
                comments2.Reverse();
                int numberOfComments = comments.Count();
                string commentsString; 
                if (numberOfComments > 5)
                {
                    commentsString = "<b>Förslagets fem senaste kommentarer:</b><br>";
                    int i = 0;
                    foreach (var comment in comments2)
                    {
                        commentsString += "<p>" + comment.commentStr + "</p>";
                        i++;
                        if (i >= 5)
                        {
                            break;
                        }
                }
                }
                else
                {
                    commentsString = "<b>Förslagets " + numberOfComments + " kommentarer:</b><br>";
                    foreach (var comment in comments2)
                    {
                        commentsString += "<p>" + comment.commentStr + "</p>";
                    }
                }
                string link;
                //OBS! Länken är just nu hårdkodad till min localhost, bör ändras till azure senare.
                link = "<" + "a href=" + "http://" + "localhost:56397/CaseItems/PrintCase/" + id + ">" + " Klicka här" + "</a>";
                string str = @"<html><body><h1> Rapport på " + caseItem.Title + " </h1>" +
                    "Du får den här rapporten eftersom du har blivit uppsatt som mottagare på det här förslaget på Politic Impact<br>" +
                    "<h3>Förslagsbeskrivning: </h3>" + caseItem.Text + "<br>" +
                    "<h3>Skapad: </h3>" + caseItem.Created + "<br>" +
                    "<h3>Deadline: </h3>" + caseItem.Deadline + "<br>" +
                    "<h3>Statistik:</h3>" +
                    "<b>Antal gillanden:</b> " + numberOfLikes + "<br>" +
                    "<b>Antal underskrifter:</b> " + numberOfSignUps + "<br>" +
                    "<b>Antal röster:</b> " + numberOfVotes + "<br>" +
                    "<b>Antal kommentarer:</b> " + numberOfComments + "<br>" + commentsString +
                    "Ser det här mailet konstigt ut? " + link +  
                    "</body></html>";
                AlternateView av = AlternateView.CreateAlternateViewFromString(str,null,MediaTypeNames.Text.Html);
                //LinkedResource lr = new LinkedResource("E:\\Photos\\hello.jpg",MediaTypeNames.Image.Jpeg);
                //lr.ContentId = "image1";
                //av.LinkedResources.Add(lr);
                m.AlternateViews.Add(av);

                sc.Host = "smtp.gmail.com";
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("politicalimpact@gmail.com", "pumTNM090");
                sc.EnableSsl = true;
                sc.Send(m);
                ViewBag.message = "Rapportmail skickat!";
            }
            catch (Exception ex)
            {
                ViewBag.message = "Mailet kunde tyvärr inte skickas iväg! Felmeddelande: " + ex.Message;
            }
            return View();
        }

        /**
        * PrintCase – Sammanställer data och skickar vidare till vyn för utskrift av förslag.
        */
        public ActionResult PrintCase(int id)
        {
            IQueryable<CaseComment> comments = caseCommentRepository.FindAllByCaseId(id);
            List<CaseComment> comments2  = comments.ToList<CaseComment>();
            comments2.Reverse();
            ViewBag.casecomments = comments2;
            ViewBag.nrOfComments = comments.Count();


            int numberOfLikes = caselikeRepository.FindLike(id);
            int numberOfSignUps = casesignupRepository.FindSignUps(id);
            int numberOfVotes = caseVotingRepository.FindVotes(id);

            ViewBag.numberOfLikes = numberOfLikes;
            ViewBag.numberOfSignUps = numberOfSignUps;
            ViewBag.numberOfVotes = numberOfVotes;

            return View(caseitemRepository.Find(id));
        }

        public ActionResult TheStatistics(int id)
        {
            int[] theLikes = caselikeRepository.StatisticLikes(id);
            ViewBag.theLikes = theLikes;
            return View();
        }

        //private JsonResult theStatisticsJSON
        //{ 
        
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                caseitemRepository.Dispose();
            }
            base.Dispose(disposing);
        }


        public CaseLikeRepository CaseLikeRepository { get; set; }
    }


}

