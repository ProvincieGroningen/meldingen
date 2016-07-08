using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.Exchange.WebServices.Data;
using Common.Logging;

namespace Meldingen
{
    /// <summary>
    /// Receives a mailmessage and tries to store information in database as Melding
    /// If the mail message has attachments these will be processed and stored in bijlage.
    /// If the attachment is a photo (with) location the location is stored as well. 
    /// </summary>
    public class MailStorage
    {
        private static ILog log = LogManager.GetCurrentClassLogger();

        public enum MailStorageResult
        {
            StoredGeoReferenced,
            StoredNotGeoReferenced,
            NotStored
        }

        private MailStorage() { }

        /// <summary>
        /// Stored the message and it's attachements into database (Bijlage). 
        /// </summary>
        /// <param name="message">message to store in database</param>
        /// <returns></returns>
        public static MailStorageResult Store(EmailMessage message)
        {
            try
            {
                MailStorage mailStorage = new MailStorage();
                return mailStorage.StoreMessage(message);
            }
            catch (Exception ex)
            {
                log.Error(m => m("Error when processing e-mail message"), ex);
                return MailStorageResult.NotStored;
            }
        }

        private MailStorageResult StoreMessage(EmailMessage message)
        {
            using (DataClassesDataContext context = new DataClassesDataContext())
            {
                Melding melding = new Melding();

                melding.Bron = context.Brons.First(b => b.Naam.Equals("E-mail"));
                melding.Status = context.Status.First(s => s.Naam.Equals("Open"));

                melding.Latitude = 0;
                melding.Longitude = 0;
                melding.Onderwerp = (message.Subject == null) ? "" : message.Subject;
                melding.VerzondenOp = message.DateTimeSent;
                if (message.Sender == null)
                    melding.Melder = "Onbekend";
                else
                    melding.Melder = string.Format("{0} ({1})", message.Sender.Name, message.Sender.Address);

                melding.GewijzigdDoor = "Mailverwerker";
                melding.GewijzigdOp = System.DateTime.Now;

                message.Load(new PropertySet(EmailMessageSchema.Body));
                if ((message.Body != null) && (message.Body.Text != null))
                {
                    BijlageContent messageBijlage = new BijlageContent();
                    messageBijlage.AangemaaktDoor = "Mailverwerker";
                    messageBijlage.AangemaaktOp = DateTime.Now;
                    messageBijlage.Inhoud = new System.Data.Linq.Binary(System.Text.Encoding.UTF8.GetBytes(message.Body.Text));
                    messageBijlage.Mimetype = (message.Body.BodyType == BodyType.HTML) ? "text/html" : "text:plain";
                    messageBijlage.Naam = "Inhoud emailbericht";
                    messageBijlage.BestandsNaam = string.Format("Bericht" + ((message.Body.BodyType == BodyType.HTML) ? ".html" : ".txt"));
                    melding.BijlageContents.Add(messageBijlage);
                }

                message.Load(new PropertySet(EmailMessageSchema.Attachments));
                foreach (Attachment attachment in message.Attachments)
                {
                    ProcessAttachment(melding, attachment);
                }

                MailStorageResult result;
                if ((melding.Latitude == 0) || (melding.Longitude == 0))
                {
                    melding.Latitude = Orphan.GetLat;
                    melding.Longitude = Orphan.GetLon;
                    result = MailStorageResult.StoredNotGeoReferenced;
                    log.Warn(m => m("No geotaged photo(s) in mail from:{0} send at:{1}", message.From, message.DateTimeSent));
                }
                else
                {
                    result = MailStorageResult.StoredGeoReferenced;
                }
                setPrimaireFoto(melding);
                context.Meldings.InsertOnSubmit(melding);
                context.SubmitChanges();
                return result;
            }
        }


        private static void setPrimaireFoto(Melding melding)
        {
            foreach (BijlageContent bijlagecontent in melding.BijlageContents)
            {
                if (bijlagecontent.Melding.Latitude != 0 && bijlagecontent.Melding.Longitude != 0 && bijlagecontent.IsPrimaireFoto == false
                  && bijlagecontent.Mimetype.Equals("image/png"))
                {
                    bijlagecontent.IsPrimaireFoto = true;
                    return;
                }
            }

            foreach (BijlageContent bijlagecontent in melding.BijlageContents)
            {
                if (bijlagecontent.IsPrimaireFoto == false && bijlagecontent.Mimetype.Equals("image/png"))
                {
                    bijlagecontent.IsPrimaireFoto = true;
                    return;
                }
            }
        }

        private static void ProcessAttachment(Melding melding, Attachment attachment)
        {
            if (!(attachment is FileAttachment)) return;
            FileAttachment fileAttachment = (FileAttachment)attachment;
            fileAttachment.Load();

            BijlageContent bijlage = new BijlageContent();
            melding.BijlageContents.Add(bijlage);
            bijlage.Naam = attachment.Name;
            bijlage.AangemaaktDoor = melding.Melder;
            bijlage.AangemaaktOp = System.DateTime.Now;
            bijlage.BestandsNaam = string.IsNullOrEmpty(fileAttachment.FileName) ? attachment.Name : fileAttachment.FileName;
            bijlage.IsPrimaireFoto = false;
            ProcessAttachmentDocument(bijlage, fileAttachment);
        }

        private static void ProcessAttachmentDocument(BijlageContent bijlage, FileAttachment fileAttachment)
        {
            fileAttachment.Load();
            try
            {
                System.IO.MemoryStream stream = new MemoryStream(fileAttachment.Content);
                System.Drawing.Image image = System.Drawing.Bitmap.FromStream(stream, true, true);
                bijlage.Inhoud = toPng(image);
                bijlage.ThumbnailSmall = getThumb(image, 45);
                bijlage.ThumbnailBig = getThumb(image, 160);
                bijlage.Mimetype = "image/png";
            }
            catch(Exception ex)
            {
                // Geen foto
                bijlage.Mimetype = GetMimeType(fileAttachment);
                bijlage.Inhoud = new System.Data.Linq.Binary(fileAttachment.Content);
            }
            if (bijlage.Melding.BijlageContents.Count(b => b.IsPrimaireFoto) == 0)
            {
                try
                {
                    ExifLib.JpegInfo info = ExifLib.ExifReader.ReadBytes(fileAttachment.Content);
                    bijlage.Melding.Latitude = (decimal)(info.GpsLatitude[0] + info.GpsLatitude[1] / 60 + info.GpsLatitude[2] / 3600);
                    bijlage.Melding.Longitude = (decimal)(info.GpsLongitude[0] + info.GpsLongitude[1] / 60 + info.GpsLongitude[2] / 3600);
                }
                catch(Exception ex)
                { //foto kon niet worden gegeotagged
                    log.Error(m => m("Photo could not be geotagged"), ex);
                }
            }
        }

        private static string GetMimeType(FileAttachment fileAttachment)
        {
            return fileAttachment.ContentType == null ? "application/octet-stream" : fileAttachment.ContentType;
        }

        private static byte[] getThumb(System.Drawing.Image image, int maxHeightAndWidth)
        {
            decimal ratio = (decimal)image.Height / (decimal)image.Width;
            int height;
            int width;
            if (image.Height > image.Width)
            {
                height = maxHeightAndWidth;
                width = (int)Decimal.Truncate(maxHeightAndWidth / ratio);
            }
            else
            {
                height = (int)Decimal.Truncate(maxHeightAndWidth * ratio);
                width = maxHeightAndWidth;
            }
            System.Drawing.Image thumbSmall = image.GetThumbnailImage(width, height, null, new IntPtr());
            return toPng(thumbSmall);
        }

        private static byte[] toPng(System.Drawing.Image image)
        {
            using (MemoryStream thumbStream = new MemoryStream())
            {
                using (BinaryWriter thumbWriter = new BinaryWriter(thumbStream))
                {
                    image.Save(thumbStream, System.Drawing.Imaging.ImageFormat.Png);
                    return thumbStream.ToArray();
                }
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}