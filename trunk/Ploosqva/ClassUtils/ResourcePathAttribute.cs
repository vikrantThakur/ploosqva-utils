using System;

namespace Ploosqva.ClassUtils
{
    /// <summary>
    /// Contains location of resource file used by EnumLocalize class
    /// </summary>
    public class ResourcePathAttribute : Attribute
    {
        internal string Path { get; set; }

        /// <summary>
        /// Creates new instance of ResourcePathAttribute attribute
        /// </summary>
        /// <param name="resourcePath">Full qualified path to resx file (as used by System.Resources.ResourceManager constructor)</param>
        public ResourcePathAttribute(string resourcePath)
        {
            Path = resourcePath;
        }
    }
}