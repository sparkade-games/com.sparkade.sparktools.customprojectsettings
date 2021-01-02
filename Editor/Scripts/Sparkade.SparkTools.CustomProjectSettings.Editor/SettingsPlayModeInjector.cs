namespace Sparkade.SparkTools.CustomProjectSettings.Editor
{
    using UnityEditor;

    /// <summary>
    /// Injects custom project settings into play mode.
    /// </summary>
    [InitializeOnLoad]
    public static class SettingsPlayModeInjector
    {
        private static readonly SettingsBuildInjector BuildInjector = new SettingsBuildInjector();

        static SettingsPlayModeInjector()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                BuildInjector.OnPostprocessBuild();
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    BuildInjector.OnPreprocessBuild();
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    BuildInjector.OnPostprocessBuild();
                    break;

                default:
                    return;
            }
        }
    }
}