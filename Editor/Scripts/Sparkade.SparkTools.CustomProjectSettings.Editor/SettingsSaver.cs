namespace Sparkade.SparkTools.CustomProjectSettings.Editor
{
    using UnityEditor;

    /// <summary>
    /// Saves cached settings when Unity saves Project Settings.
    /// </summary>
    public class SettingsSaver : AssetModificationProcessor
    {
#pragma warning disable IDE0051
        private static string[] OnWillSaveAssets(string[] paths)
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorSettingsManager.SaveCachedSettings();
            }

            return paths;
        }
#pragma warning restore IDE0051

        [InitializeOnLoadMethod]
        private static void SubscribeToCallbacks()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    EditorSettingsManager.SaveCachedSettings();
                    SettingsManager.SettingsCache.Clear();
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    SettingsManager.SettingsCache.Clear();
                    break;
            }
        }
    }
}