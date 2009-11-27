using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;

namespace Ploosqva.WebUtils.Controls
{
    /// <summary>
    /// Abstract class for building a (simple) pdf viewer containing a single pdf page
    /// and prev/next navigation buttons. To create a simpliest functioning viewer, inherit this class
    /// and instantiate protected fields
    /// </summary>
    public abstract class PdfViewerControlBase : CompositeControl
    {
        protected Image imgDocumentPage;
        protected IButtonControl btnNext;
        protected IButtonControl btnPrev;
        protected IButtonControl btnDownload;

        /// <summary>
        /// Address of a PdfPageImageHandler as set in web.config
        /// </summary>
        [UrlProperty]
        public string PdfPageUrl
        {
            get
            {
                if (ViewState["PdfPageUrl"] == null)
                    return string.Empty;

                return (string)ViewState["PdfPageUrl"];
            }
            set
            {
                ViewState["PdfPageUrl"] = value;
            }
        }

        /// <summary>
        /// Self-explanatory: gets or sets currently displayed page
        /// </summary>
        public int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                    return 0;

                return (int)ViewState["CurrentPage"];
            }
            set
            {
                ViewState["CurrentPage"] = value;

                if (CurrentPage < 0)
                    CurrentPage = 0;

                if (CurrentPage >= PagesCount)
                    CurrentPage = PagesCount;
            }
        }

        /// <summary>
        /// Gets number of pages in document
        /// </summary>
        public int PagesCount
        {
            get
            {
                if (string.IsNullOrEmpty(PdfDocPath))
                    throw new ArgumentException("PdfDocPath");

                return new PdfReader(PdfDocPath).NumberOfPages;
            }
        }

        /// <summary>
        /// Gets or rest physical of the pdf file
        /// </summary>
        [UrlProperty]
        public string PdfDocPath
        {
            get
            {
                if (ViewState["PdfDocPath"] == null)
                    return string.Empty;

                return (string)ViewState["PdfDocPath"];
            }
            set
            {
                ViewState["PdfDocPath"] = value;
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Add(btnPrev as Control);
            Controls.Add(imgDocumentPage);
            Controls.Add(btnNext as Control);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (PdfDocPath != null)
                imgDocumentPage.ImageUrl = string.Format("{0}?page={2}&h={3}&w={4}&doc={1}", PdfPageUrl,
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(PdfDocPath)),
                    CurrentPage, PageHeight.Value, PageWidth.Value);
            (btnNext as WebControl).Enabled = CurrentPage != PagesCount - 1;
            (btnPrev as WebControl).Enabled = CurrentPage != 0;

            base.Render(writer);
        }

        ///<summary>
        /// Width of the generated pdf page image
        ///</summary>
        public Unit PageWidth
        {
            get
            {
                if (ViewState["PageWidth"] == null)
                    return 300;

                return (Unit)ViewState["PageWidth"];
            }
            set { ViewState["PageWidth"] = value; }
        }

        ///<summary>
        /// Height of the generated pdf page image
        ///</summary>
        public Unit PageHeight
        {
            get
            {
                if (ViewState["PageHeight"] == null)
                    return 300;

                return (Unit)ViewState["PageHeight"];
            }
            set { ViewState["PageHeight"] = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            btnNext.Click += IncrementPage;
            btnPrev.Click += DecrementPage;
            btnDownload.Click += delegate { DownloadPdfDocument(); };

            base.OnLoad(e);
        }

        void DecrementPage(object sender, EventArgs e)
        {
            CurrentPage--;
        }

        void IncrementPage(object sender, EventArgs e)
        {
            CurrentPage++;
        }

        /// <summary>
        /// Makes control render as div instead of span
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        /// <summary>
        /// Sends the pdf file to the current Response
        /// </summary>
        public void DownloadPdfDocument()
        {
            try
            {
                Page.Response.ClearContent();
                Page.Response.WriteFile(PdfDocPath);
                Page.Response.ContentType = "application/pdf";
                Page.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.pdf", PdfTitle));
                Page.Response.AddHeader("Content-Length", new FileInfo(PdfDocPath).Length.ToString());
                Page.Response.End();
            }
            catch (Exception)
            {
            }
        }

        ///<summary>
        /// Document's title
        ///</summary>
        public string PdfTitle
        {
            get
            {
                if (ViewState["PdfTitle"] == null)
                    return string.Empty;

                return (string)ViewState["PdfTitle"];
            }
            set
            {
                ViewState["PdfTitle"] = value;
            }
        }
    }
}
