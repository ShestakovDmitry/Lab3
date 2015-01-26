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

        //Создание контрола
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FUploader = new FileUploadC();
                FUploader.FilePath = Server.MapPath("~/");
                FUploader.ServerPath = Server.MapPath("~/");
                PlaceHolder1.Controls.Add(FUploader);
            }
            else
            {
               FUploader = new FileUploadC();
               PlaceHolder1.Controls.Add(FUploader);
            }
        }
    }
}