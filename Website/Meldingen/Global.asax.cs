using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.ComponentModel;
using Common.Logging;

namespace Meldingen
{
    /// <summary>
    /// Responsible for making the MailGatherer (thread) when the application starts
    /// and shut it down when the application ends. 
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private MailGatherer mailGatherer;
        private static ILog log = LogManager.GetCurrentClassLogger();

        public static string GetFullRootUrl() {
            var url = HttpContext.Current.Request.Url;
            return url.AbsoluteUri.Replace(url.AbsolutePath, string.Empty) + HttpContext.Current.Request.ApplicationPath; 
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            log.Info(m => m("Application_Start(object sender={0}, EventArgs e={1})", sender, e));
            log.Info(m => m("ServerName = {0}", Server.MachineName));
            try
            {
                if (Meldingen.Properties.Settings.Default.StartMailverwerking)
                {
                    string url = GetFullRootUrl();
                    
                    mailGatherer = new MailGatherer(url);
                    mailGatherer.StartGathering();
                }
            }
            catch (Exception ex)
            {
                log.Fatal(m => m("Application_Start(object sender={0}, EventArgs e={1})", sender, e), ex);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Url.AbsolutePath.ToLower().EndsWith("beheer"))
              Response.Redirect("beheer.aspx"); 
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (sender.Equals(this)) return;
            if (Meldingen.Properties.Settings.Default.StartMailverwerking)
            {
                log.Fatal(m => m("Application_Error(object sender={0}, EventArgs e={1})", sender, e));
            }
            else
            {
                log.Fatal(m => m("Application_Error(object sender={0}, EventArgs e={1})", sender, e));
                throw new Exception(e.ToString());
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (Meldingen.Properties.Settings.Default.StartMailverwerking)
            {
                mailGatherer.StopGathering();
            }
        }

        public override void Dispose()
        {
            if ((mailGatherer != null) && (Meldingen.Properties.Settings.Default.StartMailverwerking) && (mailGatherer.IsRunning))
            {
                mailGatherer.StopGathering();
            }
            base.Dispose();
        }
    }
}