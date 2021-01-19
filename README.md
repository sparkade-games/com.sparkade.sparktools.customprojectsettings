# SparkTools: Custom Project Settings
Allows for the creation of custom project settings, usable both in the editor and at runtime.

# Installation
It is recommened to install through the Unity Package Manager.

If you wish to manually install, clone the repository into the `Packages` folder of your project.

# How it works
- All custom settings simply inherit from `SettingsAsset` and can be used like any other `ScriptableObject`.
- When you create a setting it is, by default, stored in a `CustomSettings` folder located in the `ProjectSettings` folder of your project.
- When a setting needs to be read by the editor it is read from the file and cached.
- Cached settings will be written back to the file whenever Unity saves all scriptable objects, including its own settings, or when entering play mode.
- Settings can be edited while in play mode, but changes will not be saved.
- During runtime, if no settings file exists for a setting, default values for the setting will be used.
- During the build process all custom settings will be converted to ScriptableObjects and placed in the resources folder. They will be stripped from the editor once the build completes or fails.

# How to Use
There are two main static classes you will be using:

- `SettingsManager`: Used to read settings at runtime, located in the namespace `Sparkade.SparkTools.CustomProjectSettings`.

- `EditorSettingsManager`: Used to create, destroy, read, and edit settings in the editor, located in the namespace `Sparkade.SparkTools.CustomProjectSettings.Editor`.

All settings should inherit from `SettingsAsset`, located in `Sparkade.SparkTools.CustomProjectSettings`. Override the `Reset()` method to set default values for the settings.

# Example
Let's make a `SettingsAsset`:
```
using Sparkade.SparkTools.CustomProjectSettings;
using UnityEngine;

public class TestSetting : SettingsAsset
{
    public float Value;
    
    public override void Reset()
    {
        this.Value = 10.0f;
    }
}
```
We just need to make sure it inherits `SettingsAsset` and overrides Reset.

Now let's add a method to inspect this `SettingsAsset`:
```
using Sparkade.SparkTools.CustomProjectSettings.Editor;
using UnityEditor;
#endif

public class TestSettings : SettingsAsset
{
    public float Value;

    public override void Reset()
    {
        this.Value = 10.0f;
    }

#if UNITY_EDITOR
    [MenuItem("Edit/MySettings...")]
    private static void InspectOrCreateSettings()
    {
        EditorSettingsManager.InspectOrCreateSettings<TestSetting>();
    }
#endif
}
```
We made sure to wrap any editor specific code in compiler tags to keep it out of our build. Now we have a new option under the 'Edit' menu called 'MySettings...'. Clicking it will bring our settings up in the inspector! From here we can adjust `Value` in our `TestSettings`.

Finally, let's utilize this setting at runtime:
```
using Sparkade.SparkTools.CustomProjectSettings;
using UnityEngine;
using UnityEngine.UI;

public class TestSettingsDisplay : MonoBehaviour
{
    private Text textComponent;

    private void Awake()
    {
        this.textComponent = this.GetComponent<Text>();
    }

    private void Update()
    {
        this.textComponent.text = SettingsManager.LoadSettings<TestSetting>().Value.ToString();
    }
}
```
This simply changes a `Text` component to display `Value`. It updates every frame, so if you change `Value` at runtime the text will update to reflect this change. You could even impliment a callback for when the `Value` is changed in `TestSettings` to avoid running every frame!
