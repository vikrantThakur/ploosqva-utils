using System;
using System.Collections.Generic;

namespace Ploosqva.Security
{
    ///<summary>
    /// This attribute can be used to decorate objects with required right to access it.
    /// For example it can allow control over users accessing specific classes or methods
    ///</summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class RequiredRightsAttribute : Attribute
    {
        internal List<object> Rights { get; set; }
        internal Type RightType { get; set; }

        ///<summary>
        /// Creates a new instance of RequiredRightsAttribute
        ///</summary>
        ///<param name="rights">list of right objects which will allow access to this object</param>
        public RequiredRightsAttribute(params object[] rights)
        {
            if(rights.Length == 0)
                throw new ArgumentException("rights cannot be empty");

            Rights = new List<object>(rights);
            RightType = rights[0].GetType();
        }
    }
}