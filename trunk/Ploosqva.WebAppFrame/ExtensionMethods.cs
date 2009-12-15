using System;
using System.Reflection;
using Ploosqva.WebAppFrame.FormsBase;

namespace Ploosqva.WebAppFrame
{
    ///<summary>
    /// Contains extension method, which allow checking, wheather extended objects
    /// are decorated with RequiredRightsAttribute and this attribute contains
    /// certain objects
    ///</summary>
    public static class ExtensionMethods
    {
        ///<summary>
        /// Checks for rights in class's RequiredRightsAttribute
        ///</summary>
        ///<returns>true if class is not decorated with RequiredRightsAttribute or one of 
        /// the attributes contains required right or false if none of them do</returns>
        public static bool CheckRequiredRights<T>(this object o, T right)
        {
            RequiredRightsAttribute[] attributes;
                
            if(o is MemberInfo)
                attributes = (RequiredRightsAttribute[]) Attribute.GetCustomAttributes(o as MemberInfo, typeof(RequiredRightsAttribute), true);
            else
                attributes = (RequiredRightsAttribute[])o.GetType().GetCustomAttributes(typeof(RequiredRightsAttribute), true);

            foreach (RequiredRightsAttribute attribute in attributes)
            {
                if(attribute.RightType != typeof(T))
                    continue;

                return attribute.Rights.Exists(right.Equals);
            }
            
            return true;
        }
    }
}
