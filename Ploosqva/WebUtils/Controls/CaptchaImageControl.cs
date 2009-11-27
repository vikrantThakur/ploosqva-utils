using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Ploosqva.WebUtils.Controls
{
    ///<summary>
    /// Represents an ImagButton, which shows a captcha image
    /// and validates it's content against user text input
    ///</summary>
    public class CaptchaImageControl : ImageButton
    {
        private TextBox inputText;

        ///<summary>
        /// Address displaying captcha images using CaptchaImageHandler
        ///</summary>
        public string CaptchaUrl
        {
            get
            {
                if (ViewState["CaptchaUrl"] == null)
                    return string.Empty;

                return (string)ViewState["CaptchaUrl"];
            }
            set
            {
                ViewState["CaptchaUrl"] = value;
            }
        }

        ///<summary>
        /// Captcha text length
        ///</summary>
        public int CaptchaLen
        {
            get
            {
                if (ViewState["CaptchaLen"] == null)
                    return 5;

                return (int)ViewState["CaptchaLen"];
            }
            set
            {
                ViewState["CaptchaLen"] = value;
            }
        }

        ///<summary>
        /// Key, which points to chache where the captcha text is stored
        ///</summary>
        public Guid CacheKey
        {
            get
            {
                if (ViewState["CacheKey"] == null)
                    return Guid.Empty;

                return (Guid)ViewState["CacheKey"];
            }
            set
            {
                ViewState["CacheKey"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            inputText = new TextBox();
            inputText.ID = UniqueID + "_text";
            inputText.Width = Width;

            /// On click event causes the text to change
            Click += CaptchaImageControl_Click;

            base.OnInit(e);
        }

        void CaptchaImageControl_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Cache.Remove(CacheKey.ToString());
            CacheKey = Guid.NewGuid();
            inputText.Text = string.Empty;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (CacheKey == Guid.Empty)
                CacheKey = Guid.NewGuid();

            if (CaptchaUrl != null) ImageUrl = string.Format("{0}?len={1}&w={2}&h={3}&g={4}",
                CaptchaUrl, CaptchaLen, Width.Value, Height.Value, CacheKey);

            base.Render(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            writer.RenderEndTag(); // BR

            inputText.RenderControl(writer);
        }

        /// <summary>
        /// Overrided, to validate posted textbox input against
        /// captha text
        /// </summary>
        protected override bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            IsValid = postCollection[inputText.UniqueID] == (string)HttpContext.Current.Cache[CacheKey.ToString()];

            inputText.Text = postCollection[inputText.UniqueID];

            return base.LoadPostData(postDataKey, postCollection);
        }

        ///<summary>
        /// Indicates user input text was correct
        ///</summary>
        public bool IsValid { get; private set; }
    }
}
