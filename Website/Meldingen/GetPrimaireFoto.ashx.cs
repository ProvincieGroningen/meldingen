using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Logging;

namespace Meldingen
{
    /// <summary>
    /// Retrieves Primary Photo 
    /// input paramters for http request:
    /// -meldingId 
    /// -size : enum Size, 
    /// </summary>
    public class GetPrimaireFoto : IHttpHandler
    {
        private static ILog log = LogManager.GetCurrentClassLogger();

        private int RequestedMeldingId()
        {
            return int.Parse(HttpContext.Current.Request.Params["meldingId"]);
        }

        private enum Size
        {
            ThumbBig,
            ThumbSmall,
            Normal
        }

        private Size RequestedSize()
        {
            return (Size)Enum.Parse(typeof(Size), HttpContext.Current.Request.Params["size"], true);
        }

        public void ProcessRequest(HttpContext context)
        {
            log.Info(m => m("ProcessRequest(HttpContext context={0})", context));
            try
            {
                WritePrimaireFoto(context.Response);
            }
            catch (Exception ex)
            {
                log.Trace(m => m("ProcessRequest : WritePrimaireFoto"), ex);
                context.Response.StatusCode = 403;
            }
            context.Response.Flush();
            context.Response.End();
        }

        private void WritePrimaireFoto(HttpResponse response)
        {
            log.Trace(m => m("ProcessRequest : WritePrimaireFoto"));

            using (DataClassesDataContext db = new DataClassesDataContext())
            {
                BijlageContent bijlage = db.BijlageContents.First(b =>
                    b.IdMelding == RequestedMeldingId() && b.IsPrimaireFoto);
                var size = this.RequestedSize();
                switch (size)
                {
                    case Size.Normal:
                        response.ContentType = bijlage.Mimetype;
                        response.BinaryWrite(bijlage.Inhoud.ToArray());
                        break;
                    case Size.ThumbBig:
                        response.ContentType = "image/png";
                        response.BinaryWrite(bijlage.ThumbnailBig.ToArray());
                        break;
                    case Size.ThumbSmall:
                        response.ContentType = "image/png";
                        response.BinaryWrite(bijlage.ThumbnailSmall.ToArray());
                        break;
                    default:
                        log.Error(m => m("WritePrimaireFoto(HttpResponse response),  Unkown Size={0} in Request param size!", size));
                        response.StatusCode = 400; // Bad Request http://msdn.microsoft.com/en-us/library/aa383887.aspx
                        break;
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}