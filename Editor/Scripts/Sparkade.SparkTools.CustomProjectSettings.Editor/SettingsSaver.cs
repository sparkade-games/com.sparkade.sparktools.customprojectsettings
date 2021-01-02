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
            EditorSettingsManager.SaveCachedSettings();
            return paths;
        }
#pragma warning restore IDE0051
    }
}