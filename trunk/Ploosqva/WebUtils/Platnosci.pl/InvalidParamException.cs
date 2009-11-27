using System;

namespace Ploosqva.WebUtils.Platnosci.pl
{
    /// <summary>
    /// Thrown, when a mandatory parameter has not been set for a payment or is invalid
    /// </summary>
    class InvalidParamException : Exception
    {
        public InvalidParamException(string paramName)
            : base(string.Format("Parametr {0} jest nieprawidłowy!", paramName))
        {

        }
    }
}
