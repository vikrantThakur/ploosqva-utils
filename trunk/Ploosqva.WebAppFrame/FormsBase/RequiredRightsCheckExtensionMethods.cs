namespace Ploosqva.WebAppFrame.FormsBase
{
    ///<summary>
    /// Contains extension method, which allow checking, wheather extended objects
    /// are decorated with RequiredRightsAttribute and this attribute contains
    /// certain objects
    ///</summary>
    public static class RequiredRightsCheckExtensionMethods
    {
        ///<summary>
        /// Checks for rights in class's RequiredRightsAttribute
        ///</summary>
        ///<returns>true if class is not decorated with RequiredRightsAttribute or the attribute contains required right or false if it does not</returns>
        public static bool CheckRequiredRights<T>(this object o, T right)
        {
            var attributes = o.GetType().GetCustomAttributes(typeof(RequiredRightsAttribute), true);
            if (attributes.Length != 0)
            {
                var attr = (RequiredRightsAttribute)attributes[0];

                return attr.ContainsRight(right);
            }

            return true;
        }
    }
}
