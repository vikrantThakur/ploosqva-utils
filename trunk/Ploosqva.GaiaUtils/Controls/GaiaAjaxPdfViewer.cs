using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Gaia.WebWidgets;
using Ploosqva.PdfViewer;
using LinkButton = System.Web.UI.WebControls.LinkButton;

namespace Ploosqva.GaiaUtils.Controls
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
            if (!string.IsNullOrEmpty(UpdateControl))
                (btnNext as Gaia.WebWidgets.ImageButton).Aspects.Add(new AspectUpdateControl(UpdateControl));
            (btnNext as WebControl).Style["float"] = "right";

            btnPrev = new Gaia.WebWidgets.ImageButton
                          {
                              ID = "prevButton",
                              ImageUrl = PrevButtonImageUrl,
                              AlternateText = PrevText
                          };
            if (!string.IsNullOrEmpty(UpdateControl))
                (btnPrev as Gaia.WebWidgets.ImageButton).Aspects.Add(new AspectUpdateControl(UpdateControl));
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
            pager.Controls.Add(new Literal { Text = "<table style=\"width:100%\"><tr><td>" });
            pager.Controls.Add(btnPrev as Control);
            pager.Controls.Add(new Literal { Text = "</td><td style=\"text-align:center\">" });
            pager.Controls.Add(btnDownload as Control);
            pager.Controls.Add(new Literal { Text = "</td><td style=\"text-align:right\">" });
            pager.Controls.Add(btnNext as Control);
            pager.Controls.Add(new Literal { Text = "</td></tr></table>" });

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

        ///<summary>
        /// ID of control used as update control for visualising ajax postbacks
        ///</summary>
        public string UpdateControl
        {
            get
            {
                if (ViewState["UpdateControl"] == null)
                    return string.Empty;

                return (string)ViewState["UpdateControl"];
            }
            set { ViewState["UpdateControl"] = value; }
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