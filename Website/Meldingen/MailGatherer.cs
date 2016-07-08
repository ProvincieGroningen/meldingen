using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Common.Logging;

namespace Meldingen
{
    /// <summary>
    /// Start a BackgroundWorker in order to process mails
    /// When an new mail is discoverd it processed by HandleMail()
    /// This process once in a while also calls PingServer
    /// </summary>
    public class MailGatherer
    {
        private static ILog log = LogManager.GetCurrentClassLogger();
        private string ServerPath { get; set; }


        private BackgroundWorker worker;
        private string Id = System.Guid.NewGuid().ToString();
        private static int MailsHandled { get; set; }
        public bool IsRunning { get; set; }


        /// <summary>
        /// Creates a background worker process and registers events
        /// </summary>
        /// <param name="serverPath">Root path to where this application is deployed 
        /// This is needed to locate ping server. The value can be obtained by  
        /// Server.MapPath(null)</param>
        public MailGatherer(string serverPath)
        {
            this.ServerPath = serverPath;

            log.Trace(m => m("MailGatherer()"));
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += this.DoWork;
        }

        /// <summary>
        /// Starts the worker, i.e. mail processing
        /// </summary>
        public void StartGathering()
        {
            log.Info(m => m("StartGathering() Mailprocessing is starting"));
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Stops the worker, i.e. mail processing
        /// </summary>
        public void StopGathering()
        {
            log.Info(m => m("StopGathering() : Mailprocessing is stopping"));
            worker.CancelAsync();
            while (worker.IsBusy)
            {
                System.Threading.Thread.Sleep(100);
            }
            worker.Dispose();
        }

        /// <summary>
        /// Starts the woker loop
        /// </summary>
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            log.Info("Mailprocessing is started");
            MailsHandled = 0;
            IsRunning = true;
            Gather();
            IsRunning = false;
            log.Info("Mailprocessing stopped");
        }


        /// <summary>
        /// Worker loop
        /// </summary>
        private void Gather()
        {
            DateTime lastPing = DateTime.Now.AddDays(-1);
            bool mailHandled = false;
            while (true)
            {
                try
                {
                    HandleReport();
                }
                catch (Exception ex)
                {
                    log.Error(m => m("Gather() : Error during Report handling"), ex);
                }

                try
                {
                    mailHandled = HandleMail();
                }
                catch (Exception ex)
                {
                    log.Error(m => m("Gather() : Error during Mailprocessing"), ex);
                }

                if (!mailHandled)
                {
                    // Sleep if no new mail
                    for (int i = 0; i < 600; i++)
                    {
                        System.Threading.Thread.Sleep(100);
                        if (worker.CancellationPending) break;
                        if (DateTime.Now.Subtract(lastPing).Minutes >= 30)
                        {
                            log.Info(m => m("Mailprocessing is active (nr: {0} mails processed in this session).", MailsHandled));
                            lastPing = DateTime.Now;
                        }
                    }
                    PingServer();
                }
                if (worker.CancellationPending)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Pings the local host. This prevents IIS from unloading/stopping the webapplication (applicationpool).
        /// This is needed otherwise the mail processing ends when webapplication (applicationpool) ends
        /// </summary>
        private void PingServer()
        {
            string pingUrl = ServerPath + Meldingen.Properties.Settings.Default.PingUrl;
            try
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    client.DownloadString(pingUrl);
                }
            }
            catch (Exception ex)
            {
                log.Error(m => m("PingServer()"), ex);
            }
        }

        /// <summary>
        /// Retrieves the mail from ExchangeServer and stores in in the database. 
        /// </summary>
        /// <returns>
        /// false if the mail message is emtpy true 
        /// true when mail is handled succesfull         
        /// </returns>
        public static bool HandleMail()
        {
            Microsoft.Exchange.WebServices.Data.EmailMessage message = MailClient.GetMailFromExchange();
            if (message == null)
            {
                return false;
            }

            string sender = string.Format("{0} ({1})", message.Sender.Name, message.Sender.Address);
            string subject = message.Subject;
            log.Info(m => m("Start processing mail message '{1}' van {0}", sender, subject));

            var mailStoreResult = MailStorage.Store(message);
            switch (mailStoreResult)
            {
                case MailStorage.MailStorageResult.StoredGeoReferenced:
                    log.Trace(m => m("HandleMail() : StoredGeoReferenced"));
                    message.Delete(Microsoft.Exchange.WebServices.Data.DeleteMode.MoveToDeletedItems);
                    break;
                case MailStorage.MailStorageResult.StoredNotGeoReferenced:
                    log.Trace(m => m("HandleMail() : StoredNotGeoReferenced"));
                    message.Delete(Microsoft.Exchange.WebServices.Data.DeleteMode.MoveToDeletedItems);
                    break;
                case MailStorage.MailStorageResult.NotStored:
                    log.Trace(m => m("HandleMail() : NotStored"));
                    break;
                default:
                    {
                        log.Error(m => m("HandleMail() unkown MailStorageRusult={0}", mailStoreResult));
                        throw new System.InvalidOperationException();
                    }
            }
            MailsHandled++;
            log.Info(m => m("Mail message processed '{1}' from {0}", sender, subject));
            return true;
        }

        /// <summary>
        /// Sends a Maandrapport every first day of the month
        /// </summary>
        private DateTime reportHandled = DateTime.MinValue;
        private void HandleReport()
        {
            using (Meldingen.DataClassesDataContext db = new DataClassesDataContext())
            {
                if (reportHandled == DateTime.MinValue)
                    reportHandled = db.Configuraties.MaandrapportLastSent();
                if (DateTime.Now.Year * 100 + DateTime.Now.Month <= reportHandled.Year * 100 + reportHandled.Month)
                    return;

                Maandrapport.Verstuur(db);
                db.Configuraties.MaandrapportLastSent(reportHandled = DateTime.Now.Date);
                db.SubmitChanges();
            }
        }
    }
}