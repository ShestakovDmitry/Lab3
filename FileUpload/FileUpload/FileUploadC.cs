using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace FileUpload
{
    public class FileUploadC : WebControl
    {
        //Контролы
        private Button BtnUpload;
        private Button BtnDownload;
        private Button BtnDelete;
        private System.Web.UI.WebControls.FileUpload FUpload;
        private Image ImgFile;
        private Label StatusLabel;
        
        //Имя загруженного файла
        public string FileName
        {
            get
            {
                object o = ViewState["FileName"];
                return (o == null) ? String.Empty : (string)o;
            }

            set
            {
                ViewState["FileName"] = value;
            }
        }

        //Путь к файлу
        public string FilePath
        {
            get
            {
                object o = ViewState["FilePath"];
                return (o == null) ? "c:\\" : (string)o;
            }

            set
            {
                ViewState["FilePath"] = value;
            }
        }

        //Загружен ли файл
        public bool FileUploaded
        {
            get
            {
                object o = ViewState["FileUploaded"];
                return (o == null) ? false : (bool)o;
            }

            set
            {
                ViewState["FileUploaded"] = value;
            }
        }

        //Метод загрузки файла с сервера
        public event EventHandler DownloadFile;

        //Обработчик события нажатия кнопки BtnUpload
        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            if (FUpload.HasFile)
            {
                try
                {
                    FileName = Path.GetFileName(FUpload.FileName);
                  //  FilePath = Server.MapPath("~/");
                    string ffilename = FilePath + FileName;
                    FUpload.SaveAs(ffilename);
                    StatusLabel.Text = "Статус: Файл " + FileName + " загружен.";
                    FileUploaded = true;
                    if (FUpload.PostedFile.ContentType.Contains("image/"))
                    {
                        ImgFile.ImageUrl = FileName;
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Статус: Файл не загружен. Произошла ошибка: " + ex.Message;
                }
            }
            else
            {
                StatusLabel.Text = "Статус: Вы не выбрали файл для загрузки.";
            }

        }

        //Обработчик события нажатия кнопки BtnDownload
        protected void BtnDownload_Click(object sender, EventArgs e)
        {
            if (DownloadFile != null)
            {
                try
                {
                    this.DownloadFile(sender, e);
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Статус: Файл не загружен. Произошла ошибка: " + ex.Message;
                }
            }
            else
            {
                StatusLabel.Text = "Статус: Файл не загружен. Не определен метод DownloadFile.";
            }

        }
        //Обработчик события нажатия кнопки BtnDelete
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (File.Exists(FilePath + FileName))
            {
                try
                {
                    File.Delete(FilePath + FileName);
                    StatusLabel.Text = "Статус: Файл " + FileName + " удален.";
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Статус: Файл не удален. Произошла ошибка: " + ex.Message;
                }
            }
            else
            {
                StatusLabel.Text = "Статус: Удаляемый файл небыл найден.";
            }
            FileUploaded = false;
            FileName = "";
        }
        //Конструктор
        public FileUploadC()
        {
            Controls.Clear();

            BtnUpload = new Button();
            BtnUpload.ID = "BtnUpload";
            BtnUpload.Text = "Загрузить";
            BtnUpload.Width = 80;
            BtnUpload.Click += new EventHandler(BtnUpload_Click);

            BtnDownload = new Button();
            BtnDownload.ID = "BtnDownload";
            BtnDownload.Text = "Скачать";
            BtnDownload.Width = 80;
            BtnDownload.Click += new EventHandler(BtnDownload_Click);

            BtnDelete = new Button();
            BtnDelete.ID = "BtnDelete";
            BtnDelete.Text = "Удалить";
            BtnDelete.Width = 80;
            BtnDelete.Click += new EventHandler(BtnDelete_Click);

            FUpload = new System.Web.UI.WebControls.FileUpload();
            FUpload.ID = "FUpload";
            
            ImgFile = new Image();
            ImgFile.ID = "ImgFile";

            StatusLabel = new Label();
            StatusLabel.ID = "StatusLabel";
            StatusLabel.Text = "";

            this.Controls.Add(BtnUpload);
            this.Controls.Add(BtnDownload);
            this.Controls.Add(BtnDelete);
            this.Controls.Add(FUpload);
            this.Controls.Add(ImgFile);
            this.Controls.Add(StatusLabel);

        }
        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (!FileUploaded)
            {
                this.Controls.Add(FUpload);
                this.Controls.Add(BtnUpload);
                this.Controls.Add(StatusLabel);
            }
            else
            {
                BtnDelete.Text = "Удалить";
                this.Controls.Add(ImgFile);
                this.Controls.Add(BtnDownload);
                this.Controls.Add(BtnDelete);
                this.Controls.Add(StatusLabel);
            }


            base.CreateChildControls();
        }
        //Рендер
        protected override void Render(HtmlTextWriter writer)
        {
            AddAttributesToRender(writer);
            if (!FileUploaded)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Table);

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "185px");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            FUpload.RenderControl(writer);
                        writer.RenderEndTag();
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "85px");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            BtnUpload.RenderControl(writer);
                        writer.RenderEndTag();
                    writer.RenderEndTag();

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            StatusLabel.RenderControl(writer);
                        writer.RenderEndTag();
                    writer.RenderEndTag();

                writer.RenderEndTag();
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Table);

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "185px");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            BtnDownload.RenderControl(writer);
                        writer.RenderEndTag();
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "85px");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            BtnDelete.RenderControl(writer);
                        writer.RenderEndTag();
                        writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, "2");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "90px");
                            ImgFile.RenderControl(writer);
                        writer.RenderEndTag();
                    writer.RenderEndTag();

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            StatusLabel.RenderControl(writer);
                        writer.RenderEndTag();
                    writer.RenderEndTag();

                writer.RenderEndTag();
            }
        }
        //Методы для сохранения и загрузки состояния контрола
        protected override void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        protected override object SaveControlState()
        {
            object baseState = base.SaveControlState();

            //create an array to hold the base control’s state 
            //and this control’s state.
            object thisState = new object[] { baseState, this.FileUploaded, this.FileName, this.FilePath};
            return thisState;
        }

        protected override void LoadControlState(object state)
        {
            object[] stateLastRequest = (object[])state;

            //Grab the state for the base class 
            //and give it to it.
            object baseState = stateLastRequest[0];
            base.LoadControlState(baseState);

            //Now load this control’s state.
            FileUploaded = (bool)stateLastRequest[1];
            FileName = (string)stateLastRequest[2];
            FilePath = (string)stateLastRequest[3];
        }
    }
}