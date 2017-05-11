using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using XtrWebTools;
namespace ThikanaClassifieds.Utilities
{
    public class FileUploader
    {
        static NetworkCredential cred = new NetworkCredential("thikana", "xxxxxxxx1");
        public static List<string> FileUpload(ControllerContext context)
        {
            var path = "webfiles/" + context.RouteData.Values["Controller"].ToString() + "/" + context.RouteData.Values["Action"].ToString() + "/";
            var pathList = new List<string>();
            var files = context.HttpContext.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                var filedata = files[i];
                if (filedata != null && filedata.ContentLength > 0 && filedata.FileName != null)
                {
                    var filename = filedata.FileName.clearFileNameWithUniqueIdentifier();

                    if (context.HttpContext.Request.IsLocal)
                    {
                        if (XtrWebTools.XtrFtpClient.Upload(filedata, context.HttpContext.Server.MapPath("~/" + path), null, filename))
                        {
                            pathList.Add(path + filename);
                        }
                    }
                    else
                    {
                        var path2 = XtrWebTools.XtrFtpClient.FtpUrlBuilder("ftp://ftp.thikana.us", path);
                        if (XtrWebTools.XtrFtpClient.Upload(filedata, path2, cred, filename))
                        {
                            pathList.Add(path + filename);
                        }

                    }
                }
            }

            return pathList;
        }
        public static void FileUpload(ControllerContext context, string path, string filename)
        {
            DeleteFile(context, path + filename);
            var files = context.HttpContext.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                var filedata = files[i];
                if (filedata != null && filedata.ContentLength > 0 && filedata.FileName != null)
                {

                    if (context.HttpContext.Request.IsLocal)
                    {
                        XtrWebTools.XtrFtpClient.Upload(filedata, context.HttpContext.Server.MapPath("~/" + path), null, filename);

                    }
                    else
                    {
                        var path2 = XtrWebTools.XtrFtpClient.FtpUrlBuilder("ftp://ftp.thikana.us", path);
                        XtrWebTools.XtrFtpClient.Upload(filedata, path2, cred, filename);


                    }
                }
            }

        }
        public static bool DeleteFile(ControllerContext context, string path)
        {
            if (context.HttpContext.Request.IsLocal)
            {
                return XtrWebTools.XtrFtpClient.delete(context.HttpContext.Server.MapPath("~/" + path));
            }
            else
            {
                path = XtrWebTools.XtrFtpClient.FtpUrlBuilder("ftp://ftp.thikana.us", path);
                var bl = XtrWebTools.XtrFtpClient.delete(path.Remove(path.Length - 1), cred);
                return bl;
            }
        }


    }
}