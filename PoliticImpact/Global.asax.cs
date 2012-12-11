using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;



namespace PoliticImpact
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            EvaluateDisplayMode(); //Evaluate incoming request and update Display Mode table

            System.Threading.ThreadStart archiveThread = new System.Threading.ThreadStart(TaskLoop);
            System.Threading.Thread myTask = new System.Threading.Thread(archiveThread);
            myTask.Start();
            
        }

        static void TaskLoop()
        {
            Controllers.CaseItemsController controller = new Controllers.CaseItemsController();
            while (true)
            {                
                controller.ArchiveCaseItem();
                System.Threading.Thread.Sleep(TimeSpan.FromDays(1));
            }
        }
         

        /// <summary>
        /// Evaluates incoming request and determines and adds an entry into the Display mode table
        /// </summary>
        private static void EvaluateDisplayMode()
        {
            DisplayModeProvider.Instance.Modes.Insert(0,
                new DefaultDisplayMode("Phone")
                {  //...modify file (view that is served)
                    //Query condition
                    ContextCondition = (ctx => (
                        //look at user agent
                        (ctx.GetOverriddenUserAgent() != null) &&
                        (//...either iPhone or iPod                           
                            (ctx.GetOverriddenUserAgent().IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (ctx.GetOverriddenUserAgent().IndexOf("iPod", StringComparison.OrdinalIgnoreCase) >= 0) || 
                            (ctx.GetOverriddenUserAgent().IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0)

                        )
                ))
                });
            DisplayModeProvider.Instance.Modes.Insert(0,
                new DefaultDisplayMode("Tablet")
                {
                    ContextCondition = (ctx => (
                        (ctx.GetOverriddenUserAgent() != null) &&
                        (
                            (ctx.GetOverriddenUserAgent().IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0) ||
                            (ctx.GetOverriddenUserAgent().IndexOf("Playbook", StringComparison.OrdinalIgnoreCase) >= 0)
                        )
                ))
                });
        }
    }
   
}