using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
using Microsoft.Exchange.WebServices.Data;
using Common.Logging;

namespace Meldingen
{
    public class MailClient
    {
        private static ILog log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Connect to exchange server and if it has mail returns the one (fist) mail. 
        /// Uses the Exchange Web services Managed API (http://www.microsoft.com/download/en/details.aspx?id=13480)
        /// </summary>
        /// <returns>one mail message if no mail is available null</returns>
        public static EmailMessage GetMailFromExchange()
        {
            log.Info("GetMailFromExchange()");
            Mailbox mailbox = new Mailbox(Meldingen.Properties.Settings.Default.Mailbox);
            FolderId folder = new FolderId(WellKnownFolderName.Inbox, mailbox);
            Folder inbox = Folder.Bind(GetService(), folder);
            ItemView viewTop1 = new ItemView(1);

            FindItemsResults<Item> results = inbox.FindItems(viewTop1);

            if (results.Items.Count == 0)
            {
                log.Trace("GetMailFromExchange() : no mail");
                return null;
            }

            log.Trace("GetMailFromExchange() : mail found");
            EmailMessage message = (EmailMessage)results.Items[0];
            message.Load();
            return message;
        }

        public static void Send(string recepient, string subject, string message, string report)
        {
            using (var client = new SmtpClient(Meldingen.Properties.Settings.Default.SMTPServer))
            {
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
                var mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress(Meldingen.Properties.Settings.Default.Mailbox);
                mailMessage.To.Add(recepient);
                mailMessage.Body = message;
                mailMessage.Subject = subject;
                var attachmentBytes = System.Text.Encoding.UTF8.GetBytes(report);
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(attachmentBytes), "Meldingen.xls"));
                client.Send(mailMessage);
            }
        }

        private static ExchangeService GetService()
        {
            var service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            service.Credentials = new WebCredentials(System.Net.CredentialCache.DefaultNetworkCredentials);
            service.Url = new Uri(Meldingen.Properties.Settings.Default.ExchangeServiceUrl);
            service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, Meldingen.Properties.Settings.Default.Mailbox);
            return service;
        }
    }
}