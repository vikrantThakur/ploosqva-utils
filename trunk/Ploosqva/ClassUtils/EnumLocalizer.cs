using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace Ploosqva.ClassUtils
{
    ///<summary>
    /// Converts an Enum into localized strings using translations from a resx file.
    ///</summary>
    public class EnumLocalizer : EnumConverter
    {
        private readonly ResourceManager resourceManager;

        /// <summary>
        /// Creates new instance of EnumLocalizer
        /// </summary>
        /// <param name="type">enum type to localize</param>
        public EnumLocalizer(Type type)
            : base(type)
        {
            var attributes = type.GetCustomAttributes(typeof(ResourcePathAttribute), false);

            if (attributes.Length == 0)
                throw new Exception("Enum is missing ResourcePathAttribute");

            try
            {
                string resourcePath = attributes.Length > 0
                                          ? ((ResourcePathAttribute)attributes[0]).Path
                                          : string.Empty;

                resourceManager = new ResourceManager(resourcePath, Assembly.GetAssembly(type));
            }
            catch
            {
                throw new Exception("Cannot create ResourceManager");
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                         CultureInfo culture, object value, Type destinationType)
        {
            string resourceKey = Enum.GetName(value.GetType(), value);

            try
            {
                return resourceManager.GetString(resourceKey);
            }
            catch
            {
                return Enum.GetName(value.GetType(), value);
            }
        }

        /// <summary>
        /// Gets all values of an Enum and translates the names based on resource given
        /// via ResourcePathAttribute
        /// </summary>
        /// <returns>Localized set of enum's values</returns>
        public List<object> GetValues()
        {
            List<object> values = new List<object>();

            foreach (var value in Enum.GetValues(EnumType))
            {
                values.Add(ConvertTo(value, typeof(string)));
            }

            return values;
        }

        /// <summary>
        /// Gets enum value represented by the string
        /// </summary>
        /// <param name="text">enum text</param>
        /// <returns>enum value</returns>
        public object Parse(string text)
        {
            string resourceKey = null;

            var resourceSet = resourceManager.GetResourceSet(
                Thread.CurrentThread.CurrentUICulture, 
                true, 
                true);

            foreach (DictionaryEntry resource in resourceSet)
            {
                if ((string)resource.Value == text)
                {
                    resourceKey = (string) resource.Key;
                    break;
                }
            }

            return Enum.Parse(EnumType, resourceKey);
        }

        /// <summary>
        /// Gets string representing the enum value
        /// </summary>
        public string GetName(object value)
        {
            return (string) ConvertTo(value, typeof(string));
        }
    }
}