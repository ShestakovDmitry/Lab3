using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace FileUpload
{
    public partial class test : System.Web.UI.Page
    {   
        //Контрол загрузчик файлов
        public FileUploadC FUploader;

        //Метод возвращающий ContentType по расширению файла
        private string GetFileExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

        //Метод загрузки файла с сервера
        protected void downloadf(object obj, EventArgs e)
        {
            var _filepath = Server.MapPath(FUploader.FileName);//FilePath + FileName;
            FileInfo file = new FileInfo(_filepath);
            if (file.Exists)
            {
                Response.ClearHeaders();
                Response.ClearContent();
                var FN = file.Name.Replace(" ", "_");
                Response.AddHeader("Content-Disposition", "attachment; filename=" + FN);
                Response.AddHeader("Content-Length", file.Length.ToString());
                var ct = GetFileExtension(file.Extension.ToLower());
                Response.ContentType = ct;
                Response.TransmitFile(file.FullName);
                Response.End();
            }
        }

        //Создание контрола
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FUploader = new FileUploadC();
                FUploader.FilePath = Server.MapPath("~/");
                FUploader.DownloadFile += new EventHandler(downloadf);
                PlaceHolder1.Controls.Add(FUploader);
            }
            else
            {
               FUploader = new FileUploadC();
               FUploader.DownloadFile += new EventHandler(downloadf);
               PlaceHolder1.Controls.Add(FUploader);
            }
        }
    }
}