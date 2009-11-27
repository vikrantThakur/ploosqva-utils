using System.Web;
using System.Web.UI;

namespace Ploosqva.WebUtils
{
    /// <summary>
    /// Used to show alert javascript dialog on postback. 
    /// Does not work with AJAX
    /// </summary>
    public class JavascriptAlert
    {   
        /// <summary>
        /// Shows a client-side JavaScript alert in the browser.
        /// </summary>
        /// <param name="message">The message to appear in the alert.</param>
        public static void Show(string message)
        {
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "\'");
            string script = "<script type='text/javascript'>alert('" + cleanMessage + "');</script>";

            // Gets the executing web page
            Page page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(JavascriptAlert), "alert", script);
            }
        }
    }
}
