namespace Ploosqva.GeneralUtils
{
    /// <summary>
    /// Defines a class used to retrieve and modify runtime settings
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        /// Gets or sets value of setting. When setting, and key does not exist, it should be added
        /// </summary>
        /// <param name="settingKey">setting key</param>
        /// <returns>setting value</returns>
        string this[string settingKey] { get; set; }

        /// <summary>
        /// Saves configuration overwriting all previous settings
        /// </summary>
        void Save();
    }
}
