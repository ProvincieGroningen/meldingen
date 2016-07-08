using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Logging;

namespace Meldingen
{
    /// <summary>
    /// Retrieves an attachement (bijlage) from database
    /// and makes this atachment available as download
    /// </summary>
    public class GetBijlage : IHttpHandler
    {
        private static ILog log = LogManager.GetCurrentClassLogger();

        private int RequestedBijlageId
        {
            get
            {
                string bijlageId = HttpContext.Current.Request.Params["bijlageId"];
                try
                {
                    return int.Parse(bijlageId);
                }
                catch (Exception ex)
                {
                    log.Error(m => m("RequestedBijlageId invalid bijlageId={0}", bijlageId), ex);
                }
                return -1;
            }
        }

        /// <summary>
        /// Retrieves an attachement (bijlage) from database
        /// and makes this atachment available as download
        /// The request url should contain bijlageId (int) 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            log.Info(m => m("ProcessRequest(HttpContext context)={0}", context));

            try
            {
                using (DataClassesDataContext db = new DataClassesDataContext())
                {
                    BijlageContent bijlage = db.BijlageContents.First(b => b.Id == RequestedBijlageId);

                    var response = context.Response;
                    response.AddHeader("Content-Disposition", "attachment; filename=\"" + bijlage.BestandsNaam + "\"");
                    response.ContentType = bijlage.Mimetype;
                    response.BinaryWrite(bijlage.Inhoud.ToArray());
                    response.Flush();
                }
            }
            catch (Exception ex)
            {
                log.Error(m => m("ProcessRequest(HttpContext context={0})", context), ex);
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