namespace Sparkade.SparkTools.CustomProjectSettings
{
    using UnityEngine;

    /// <summary>
    /// A custom project setting.
    /// </summary>
    public abstract class SettingsAsset : ScriptableObject
    {
        /// <summary>
        /// Called when the SettingsAsset is first created.
        /// Also called in the editor when 'Reset' is pressed.
        /// </summary>
        public virtual void Reset()
        {
        }
    }
}