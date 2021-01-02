namespace Sparkade.SparkTools.CustomProjectSettings
{
    using System;
    using System.IO;
    using UnityEngine;

    /// <summary>
    /// Allows for access to custom project settings at runtime.
    /// </summary>
    public static class SettingsManager
    {
        /// <summary>
        /// Gets the absolute path to where custom project settings are stored in the Resources folder.
        /// </summary>
        public static string SettingsPath => Path.Combine(Application.dataPath.Replace('/', Path.DirectorySeparatorChar), "Resources", "CustomSettings");

        /// <summary>
        /// Gets the path relative to the Resources folder where custom project settings are stored.
        /// </summary>
        public static string RelativeSettingsPath => SettingsPath.Replace(Directory.GetParent(Application.dataPath).FullName, string.Empty).Trim(Path.DirectorySeparatorChar);

        /// <summary>
        /// Returns a custom project setting of type 'T'.
        /// </summary>
        /// <typeparam name="T">Type of SettingsAsset to be returned.</typeparam>
        /// <returns>The SettingsAsset of type 'T'. If it does not exist, null is returned.</returns>
        public static T LoadSettings<T>()
            where T : SettingsAsset
        {
            int index = RelativeSettingsPath.IndexOf("Resources");
            string path = RelativeSettingsPath.Substring(index + 10);
            return Resources.Load<T>(Path.Combine(path, GetSettingsName<T>()));
        }

        /// <summary>
        /// Gets whether a custom project setting of type 'T' exists.
        /// </summary>
        /// <typeparam name="T">The type of SettingsAsset you are checking for.</typeparam>
        /// <returns>True if the custom project setting of type 'T' exists, otherwise false.</returns>
        public static bool SettingsExists<T>()
            where T : SettingsAsset
        {
            return LoadSettings<T>() != null;
        }

        /// <summary>
        /// Gets the name of a custom project setting.
        /// </summary>
        /// <param name="type">The type of SettingsAsset.</param>
        /// <returns>The name of the custom project setting, or null if 'type' is not a SettingsAsset.</returns>
        public static string GetSettingsName(Type type)
        {
            return type.IsAssignableFrom(typeof(SettingsAsset)) ? type.Name : null;
        }

        /// <summary>
        /// Gets the name of a custom project setting.
        /// </summary>
        /// <typeparam name="T">The type of SettingsAsset.</typeparam>
        /// <returns>The name of the custom project setting.</returns>
        public static string GetSettingsName<T>()
            where T : SettingsAsset
        {
            return GetSettingsName(typeof(T));
        }
    }
}