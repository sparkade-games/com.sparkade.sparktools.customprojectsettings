#if UNITY_EDITOR
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Sparkade.SparkTools.CustomProjectSettings.Editor")]
#endif

namespace Sparkade.SparkTools.CustomProjectSettings
{
#if UNITY_EDITOR
    using UnityEditorInternal;
#endif

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Allows for access to custom project settings at runtime.
    /// </summary>
    public static class SettingsManager
    {
        /// <summary>
        /// An internal cache of loaded settings.
        /// </summary>
        internal static readonly Dictionary<Type, SettingsAsset> SettingsCache = new Dictionary<Type, SettingsAsset>();

        /// <summary>
        /// Gets the absolute path to where custom project settings are stored in the Resources folder.
        /// </summary>
        public static string SettingsPath => Path.Combine(Application.dataPath.Replace('/', Path.DirectorySeparatorChar), "Resources", "CustomSettings");

        /// <summary>
        /// Gets the path relative to the Resources folder where custom project settings are stored.
        /// </summary>
        public static string RelativeSettingsPath => SettingsPath.Replace(Directory.GetParent(Application.dataPath).FullName, string.Empty).Trim(Path.DirectorySeparatorChar);

#if UNITY_EDITOR
        /// <summary>
        /// Gets the absolute path to where custom project settings are stored.
        /// </summary>
        internal static string EditorSettingsPath => Path.Combine(Directory.GetParent(Application.dataPath).FullName, "ProjectSettings", "CustomSettings");
#endif

        /// <summary>
        /// Returns a custom project setting of type 'T'.
        /// </summary>
        /// <typeparam name="T">Type of SettingsAsset to be returned.</typeparam>
        /// <returns>The SettingsAsset of type 'T'. If it does not exist, null is returned.</returns>
        public static T LoadSettings<T>()
            where T : SettingsAsset
        {
            if (!SettingsCache.ContainsKey(typeof(T)) || SettingsCache[typeof(T)] == null)
            {
#if UNITY_EDITOR
                string filePath = Path.Combine(EditorSettingsPath, GetSettingsFilename<T>());

                if (!File.Exists(filePath))
                {
                    if (Application.isPlaying)
                    {
                        SettingsCache[typeof(T)] = ScriptableObject.CreateInstance<T>();
                        SettingsCache[typeof(T)].Reset();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    SettingsCache[typeof(T)] = InternalEditorUtility.LoadSerializedFileAndForget(filePath).FirstOrDefault() as SettingsAsset;
                }
#else
                int index = RelativeSettingsPath.IndexOf("Resources");
                string path = RelativeSettingsPath.Substring(index + 10);
                T settings = Resources.Load<T>(Path.Combine(path, GetSettingsName<T>()));

                if (settings == null)
                {
                    settings = ScriptableObject.CreateInstance<T>();
                    settings.Reset();
                }

                SettingsCache[typeof(T)] = settings;
#endif
            }

            return SettingsCache[typeof(T)] as T;
        }

        /// <summary>
        /// Gets the name of a custom project setting.
        /// </summary>
        /// <param name="type">The type of SettingsAsset.</param>
        /// <returns>The name of the custom project setting, or null if 'type' is not a SettingsAsset.</returns>
        public static string GetSettingsName(Type type)
        {
            return type.IsSubclassOf(typeof(SettingsAsset)) ? type.Name : null;
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

#if UNITY_EDITOR
        /// <summary>
        /// Gets the file name for a SettingsAsset of a given type.
        /// </summary>
        /// <param name="type">Type of SettingsAsset.</param>
        /// <returns>The file name of the custom project setting, or null if 'type' is not a SettingsAsset.</returns>
        internal static string GetSettingsFilename(Type type)
        {
            string settingsName = GetSettingsName(type);
            return settingsName != null ? Path.ChangeExtension(GetSettingsName(type), "asset") : null;
        }

        /// <summary>
        /// Gets the file name for a SettingsAsset of a given type.
        /// </summary>
        /// <typeparam name="T">Type of SettingsAsset.</typeparam>
        /// <returns>The file name of the custom project setting, or null if 'T' is not a SettingsAsset.</returns>
        internal static string GetSettingsFilename<T>()
        {
            return GetSettingsFilename(typeof(T));
        }
#endif
    }
}