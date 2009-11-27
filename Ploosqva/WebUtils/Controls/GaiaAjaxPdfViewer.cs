using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Ploosqva.WebUtils.Controls
{
    ///<summary>
    /// Simple pdf viewer using Gaia Ajax controls
    ///</summary>
    public class GaiaImageButtonPdfViewer : PdfViewerControlBase
    {
        private Gaia.WebWidgets.Panel pager;

        protected override void OnInit(EventArgs e)
        {
            pager = new Gaia.WebWidgets.Panel();
            imgDocumentPage = new Gaia.WebWidgets.Image();

            btnNext = new Gaia.WebWidgets.ImageButton
            {
                ID = "nextButton",
                ImageUrl = NextButtonImageUrl,
                AlternateText = NextText
            };
            (btnNext as WebControl).Style["float"] = "right";

            btnPrev = new Gaia.WebWidgets.ImageButton
            {
                ID = "prevButton",
                ImageUrl = PrevButtonImageUrl,
                AlternateText = PrevText
            };
            (btnPrev as WebControl).Style["float"] = "left";

            btnDownload = new LinkButton
            {
                ID = "downloadButton",
                Text = DownloadText
            };

            base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            HtmlGenericControl pageDiv = new HtmlGenericControl("div");
            pageDiv.Attributes["class"] = PageDivCssClass;
            pageDiv.Controls.Add(imgDocumentPage);

            pager.CssClass = PagerDivCssClass;
            pager.Controls.Add(btnPrev as Control);
            pager.Controls.Add(btnDownload as Control);
            pager.Controls.Add(btnNext as Control);

            Controls.Add(pageDiv);
            Controls.Add(pager);
        }

        ///<summary>
        /// Css class applied to the pager wrapper
        ///</summary>
        public string PagerDivCssClass
        {
            get
            {
                if (ViewState["PagerDivCssClass"] == null)
                    return string.Empty;

                return (string)ViewState["PagerDivCssClass"];
            }
            set { ViewState["PagerDivCssClass"] = value; }
        }

        ///<summary>
        /// Css class apllied to the page image wrapper
        ///</summary>
        public string PageDivCssClass
        {
            get
            {
                if (ViewState["PageDivCssClass"] == null)
                    return string.Empty;

                return (string)ViewState["PageDivCssClass"];
            }
            set { ViewState["PageDivCssClass"] = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            pager.ForceAnUpdate();

            base.Render(writer);
        }

        ///<summary>
        /// Url of the "next slide" ImageButton.ImageUrl
        ///</summary>
        [UrlProperty]
        public string NextButtonImageUrl
        {
            get
            {
                if (ViewState["NextButtonImageUrl"] == null)
                    return string.Empty;

                return (string)ViewState["NextButtonImageUrl"];
            }
            set
            {
                ViewState["NextButtonImageUrl"] = value;
            }
        }

        ///<summary>
        /// Url of the "previous slide" ImageButton.ImageUrl
        ///</summary>
        [UrlProperty]
        public string PrevButtonImageUrl
        {
            get
            {
                if (ViewState["PrevButtonImageUrl"] == null)
                    return string.Empty;

                return (string)ViewState["PrevButtonImageUrl"];
            }
            set
            {
                ViewState["PrevButtonImageUrl"] = value;
            }
        }

        ///<summary>
        /// Text displayed on the pdf file download link/button
        ///</summary>
        public string DownloadText
        {
            get
            {
                if (ViewState["DownloadText"] == null)
                    return string.Empty;

                return (string)ViewState["DownloadText"];
            }
            set
            {
                ViewState["DownloadText"] = value;
            }
        }

        ///<summary>
        /// Text displayed on the "previous slide" link/button
        ///</summary>
        public string PrevText
        {
            get
            {
                if (ViewState["PrevText"] == null)
                    return string.Empty;

                return (string)ViewState["PrevText"];
            }
            set
            {
                ViewState["PrevText"] = value;
            }
        }

        ///<summary>
        /// Text displayed on the "next slide" link/button
        ///</summary>
        public string NextText
        {
            get
            {
                if (ViewState["NextText"] == null)
                    return string.Empty;

                return (string)ViewState["NextText"];
            }
            set
            {
                ViewState["NextText"] = value;
            }
        }
    }
}
