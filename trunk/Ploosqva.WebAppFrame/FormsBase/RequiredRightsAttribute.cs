using System;
using System.Collections.Generic;

namespace Ploosqva.WebAppFrame.FormsBase
{
    ///<summary>
    /// This attribute can be used to decorate objects with required right to access it.
    /// For example it can allow controll over users accessing specific classes or methods
    ///</summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class RequiredRightsAttribute : Attribute
    {
        private List<object> Rights { get; set; }

        ///<summary>
        /// Creates a new instance of RequiredRightsAttribute
        ///</summary>
        ///<param name="rights">list of right objects which will allow access to this object</param>
        public RequiredRightsAttribute(params object[] rights)
        {
            Rights = new List<object>(rights);
        }

        ///<summary>
        /// Method checks wheather the list Rights contains a right
        ///</summary>
        public bool ContainsRight(object right)
        {
            return Rights.Contains(right);
        }
    }
}
