namespace Sparkade.SparkTools.CustomProjectSettings.Editor
{
    using System;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
    using UnityEditorInternal;
    using UnityEngine;

    /// <summary>
    /// Injects custom settings into builds.
    /// </summary>
    public class SettingsBuildInjector : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        /// <inheritdoc/>
        public int callbackOrder => 0;

        /// <summary>
        /// Places all settings in the Resources folder, to be added to the build.
        /// </summary>
        /// <param name="report">This parameter is unused and should be ignored.</param>
        public void OnPreprocessBuild(BuildReport report = default)
        {
            Application.logMessageReceived += this.OnBuildError;

            if (!Directory.Exists(EditorSettingsManager.SettingsPath))
            {
                return;
            }

            string[] filePaths = Directory.GetFiles(EditorSettingsManager.SettingsPath);

            if (filePaths.Length == 0)
            {
                return;
            }

            if (!Directory.Exists(SettingsManager.SettingsPath))
            {
                Directory.CreateDirectory(SettingsManager.SettingsPath);
            }

            for (int i = 0; i < filePaths.Length; i += 1)
            {
                string typeName = Path.GetFileNameWithoutExtension(filePaths[i]);
                Type type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.Name == typeName);
                if (Path.GetExtension(filePaths[i]) == ".asset")
                {
                    if (type == null || !type.IsSubclassOf(typeof(SettingsAsset)))
                    {
                        Debug.LogWarning($"The SettingsAsset of type '{typeName}' does not exist, but a settings file for it exists at '{filePaths[i]}'.");
                        continue;
                    }
                    var path = Path.Combine(SettingsManager.EditorSettingsPath, Path.GetFileName(filePaths[i]));
                    Debug.Log(path);
                    ScriptableObject asset = InternalEditorUtility.LoadSerializedFileAndForget(path).FirstOrDefault() as SettingsAsset;
                    AssetDatabase.CreateAsset(asset, Path.Combine(SettingsManager.RelativeSettingsPath, Path.GetFileName(filePaths[i])));
                }
            }
        }

        /// <summary>
        /// Removes any files added through Pre-Processing.
        /// </summary>
        /// <param name="report">This parameter is unused and should be ignored.</param>
        public void OnPostprocessBuild(BuildReport report = default)
        {
            Application.logMessageReceived -= this.OnBuildError;

            if (!Directory.Exists(EditorSettingsManager.SettingsPath))
            {
                return;
            }

            string[] filePaths = Directory.GetFiles(EditorSettingsManager.SettingsPath);

            if (filePaths.Length == 0)
            {
                return;
            }

            for (int i = 0; i < filePaths.Length; i += 1)
            {
                if (Path.GetExtension(filePaths[i]) == ".asset")
                {
                    AssetDatabase.DeleteAsset(Path.Combine(SettingsManager.RelativeSettingsPath, Path.GetFileName(filePaths[i])));
                }
            }

            string relativePath = SettingsManager.RelativeSettingsPath;
            while (relativePath != "Assets")
            {
                string absolutePath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, relativePath);

                if (Directory.Exists(absolutePath))
                {
                    filePaths = Directory.GetFiles(absolutePath);
                    if (filePaths.Length == 0)
                    {
                        AssetDatabase.DeleteAsset(relativePath);
                    }
                    else
                    {
                        break;
                    }
                }

                int index = relativePath.LastIndexOf(Path.DirectorySeparatorChar);
                if (index > 0)
                {
                    relativePath = relativePath.Substring(0, index);
                }
                else
                {
                    break;
                }
            }
        }

        private void OnBuildError(string condition, string stacktrace, LogType type)
        {
            if (BuildPipeline.isBuildingPlayer && type == LogType.Error)
            {
                this.OnPostprocessBuild();
            }
        }
    }
}