using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Ploosqva.GeneralUtils
{
    ///<summary>
    /// Class used to retireve and modify settings stored in an XML file
    ///</summary>
    public class XmlSettingsManager : ISettingsManager
    {
        readonly XmlDocument xmlDoc = new XmlDocument();
        private readonly String basePath = AppDomain.CurrentDomain.BaseDirectory;
        private String path;

        ///<summary>
        /// Opens an XML file with setting. File is created if it doesn't exist
        ///</summary>
        ///<param name="settingsFile">RELATIVE (!) path to XML file</param>
        public XmlSettingsManager(string settingsFile)
        {
            path = AppDomain.CurrentDomain.BaseDirectory + settingsFile;

            CreateConfigFile();
        }

        private void CreateConfigFile()
        {
            path = Path.Combine(basePath, path);

            FileInfo file = new FileInfo(path);

            if (!Directory.Exists(file.DirectoryName))
                Directory.CreateDirectory(file.DirectoryName);

            if (!File.Exists(path))
            {
                /// The following is done to make sure file handle has been release after creation
                /// using File.Create can leave an open handle which causes an exception
                /// on the Save method
                FileStream stream = new FileStream(path, FileMode.CreateNew);
                stream.Close();
                stream.Dispose();

                xmlDoc.LoadXml("<?xml version=\"1.0\"?><appSettings></appSettings>");
                xmlDoc.Save(path);
            }
            else
                xmlDoc.Load(path);
        }

        /// <summary>
        /// Opens or creates a new configuration file named *full.assembly.name*.settings
        /// </summary>
        public XmlSettingsManager()
        {
            path = string.Format("Settings\\{0}.xml", Assembly.GetCallingAssembly().GetName().Name);

            CreateConfigFile();
        }

        /// <summary>
        /// Opens or creates a new configuration file named *full assembly name*.*language code*.settings
        /// </summary>
        /// <param name="language">CultureInfo representing the locale used</param>
        public XmlSettingsManager(CultureInfo language)
        {
            path = string.Format("Settings\\{0}.{1}.xml", 
                Assembly.GetCallingAssembly().GetName().Name,
                language);
            
            CreateConfigFile();
        }

        public string this[string settingKey]
        {
            get
            {
                return GetValue(settingKey);
            }
            set
            {
                SetValue(settingKey, value);
            }
        }

        public void Save()
        {
            xmlDoc.Save(path);
        }

        private String GetValue(String key)
        {
            XmlNode setting = xmlDoc.SelectSingleNode(string.Format("//add[@key='{0}']", key));

            return setting == null ? String.Empty : setting.Attributes["value"].Value;
        }

        private void SetValue(String key, String value)
        {
            XmlNode setting = xmlDoc.SelectSingleNode(string.Format("//add[@key='{0}']", key));

            if (setting != null)
                xmlDoc.SelectSingleNode(string.Format("//add[@key='{0}']", key)).Attributes["value"].Value = value;
            else
            {
                setting = xmlDoc.CreateElement("add");

                XmlAttribute keyAttrib = xmlDoc.CreateAttribute("key");
                keyAttrib.Value = key;

                XmlAttribute valueAttrib = xmlDoc.CreateAttribute("value");
                valueAttrib.Value = value;

                setting.Attributes.Append(keyAttrib);
                setting.Attributes.Append(valueAttrib);

                xmlDoc.SelectSingleNode("/appSettings").AppendChild(setting);
            }
        }
    }
}
