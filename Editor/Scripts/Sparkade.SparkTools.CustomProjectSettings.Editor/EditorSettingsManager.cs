namespace Sparkade.SparkTools.CustomProjectSettings.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Editor-only settings manager capable of creating, destroying, writing, and reading settings.
    /// </summary>
    public static class EditorSettingsManager
    {
        /// <summary>
        /// Gets the absolute path to where custom project settings are stored.
        /// </summary>
        public static string SettingsPath => SettingsManager.EditorSettingsPath;

        /// <summary>
        /// Creates a cusom project setting of type 'T'.
        /// </summary>
        /// <typeparam name="T">Type of SettingsAsset to be created.</typeparam>
        public static void CreateSettings<T>()
            where T : SettingsAsset
        {
            if (SettingsExists<T>())
            {
                throw new ArgumentException($"SettingsAsset '{SettingsManager.GetSettingsName<T>()}' already exists.", "T");
            }

            T asset = ScriptableObject.CreateInstance<T>();
            asset.Reset();

            if (!Directory.Exists(SettingsPath))
            {
                Directory.CreateDirectory(SettingsPath);
            }

            File.WriteAllText(Path.Combine(SettingsPath, SettingsManager.GetSettingsFilename<T>()), JsonUtility.ToJson(asset));
        }

        /// <summary>
        /// Destroys a cusom project setting of type 'T'.
        /// </summary>
        /// <typeparam name="T">Type of SettingsAsset to be destroyed.</typeparam>
        public static void DestroySettings<T>()
            where T : SettingsAsset
        {
            if (!SettingsExists<T>())
            {
                throw new ArgumentException($"SettingsAsset '{SettingsManager.GetSettingsName<T>()}' does not exist.", "T");
            }

            File.Delete(Path.Combine(SettingsPath, SettingsManager.GetSettingsFilename<T>()));
            SettingsManager.SettingsCache.Remove(typeof(T));
        }

        /// <summary>
        /// Returns a custom project setting of type 'T'.
        /// </summary>
        /// <typeparam name="T">Type of SettingsAsset to be returned.</typeparam>
        /// <returns>The SettingsAsset of type 'T'. If it does not exist, null is returned.</returns>
        public static T LoadSettings<T>()
            where T : SettingsAsset
        {
            return SettingsManager.LoadSettings<T>();
        }

        /// <summary>
        /// Gets whether a custom project setting of type 'T' exists.
        /// </summary>
        /// <typeparam name="T">The type of SettingsAsset you are checking for.</typeparam>
        /// <returns>True if the custom project setting of type 'T' exists, otherwise false.</returns>
        public static bool SettingsExists<T>()
            where T : SettingsAsset
        {
            return SettingsManager.LoadSettings<T>() != null;
        }

        /// <summary>
        /// Opens the SettingsAsset of type 'T' in the inspector.
        /// </summary>
        /// <typeparam name="T">The type of SettingsAsset to inspect.</typeparam>
        public static void InspectSettings<T>()
            where T : SettingsAsset
        {
            Selection.activeObject = SettingsManager.LoadSettings<T>() ?? throw new ArgumentException($"SettingsAsset '{SettingsManager.GetSettingsName<T>()}' does not exist.", "T");
        }

        /// <summary>
        /// Loads a custom project setting, and creates it if it did not exist.
        /// </summary>
        /// <typeparam name="T">Type of SettingsAsset to be returned.</typeparam>
        /// <returns>The SettingsAsset of type 'T'.</returns>
        public static T LoadOrCreateSettings<T>()
            where T : SettingsAsset
        {
            if (!SettingsExists<T>())
            {
                CreateSettings<T>();
            }

            return LoadSettings<T>();
        }

        /// <summary>
        /// Inspects a custom project setting, and creates it if it did not exist.
        /// </summary>
        /// <typeparam name="T">The type of SettingsAsset to inspect.</typeparam>
        public static void InspectOrCreateSettings<T>()
            where T : SettingsAsset
        {
            if (!SettingsExists<T>())
            {
                CreateSettings<T>();
            }

            InspectSettings<T>();
        }

        /// <summary>
        /// Manually saves all currently cached settings.
        /// </summary>
        public static void SaveCachedSettings()
        {
            if (!Directory.Exists(SettingsPath))
            {
                Directory.CreateDirectory(SettingsPath);
            }

            foreach (KeyValuePair<Type, SettingsAsset> entry in SettingsManager.SettingsCache)
            {
                File.WriteAllText(Path.Combine(SettingsPath, SettingsManager.GetSettingsFilename(entry.Key)), JsonUtility.ToJson(entry.Value));
            }
        }
    }
}