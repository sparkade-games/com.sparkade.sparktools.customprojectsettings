# SparkTools: Custom Project Settings
Allows for the creation of custom project settings, usable both in the editor and at runtime.

# Installation
It is recommened to install through the Unity Package Manager.

If you wish to manually install, clone the repository into the `Packages` folder of your project.

# How it works
- All custom settings simply inherit from `SettingsAsset` and can be used like any other `ScriptableObject`.
- When you create a setting it is, by default, stored in a `CustomSettings` folder located in the `ProjectSettings` folder of your project.
- When a setting needs to be read by the editor it is read from the file and cached.
- Cached settings will be written back to the file whenever Unity saves all scriptable objects, including its own settings.
- During the build process all custom settings will be converted to ScriptableObjects and placed in the resources folder. They will be stripped from the editor once the build completes.
- Similarly to building, starting play mode will also place all settings in your resources folder, and strip them out when play mode is exited.

# How to Use
There are two main static classes you will be using:

- `SettingsManager`: Used to read settings at runtime, located in the namespace `Sparkade.SparkTools.CustomProjectSettings`.

- `EditorSettingsManager`: Used to create, delete, read, and edit settings in the editor, located in the namespace `Sparkade.SparkTools.CustomProjectSettings.Editor`.

## SettingsManager
- `LoadSettings<T>()`: Returns the `SettingsAsset` of type 'T'. If none exists, it will return null.

- `SettingsExist<T>()`: Returns whether or not the `SettingsAsset` of type 'T' exists.

- `GetSettingsName<T>()`: Returns the `SettingsAsset`'s name as a string.

## EditorSettingsManager
- `CreateSettings<T>()`: Creates a new `SettingsAsset` of type 'T'.

- `DestroySettings<T>()`: Deletes a `SettingsAsset` of type 'T'. Use with caution, it cannot be undone.

- `LoadSettings<T>()`: Returns the `SettingsAsset` of type 'T'. If none exists, it will return null.

- `SettingsExists<T>()`: Returns whether or not the `SettingsAsset` of type 'T' exists.

- `InspectSettings<T>()`: Brings up the `SettingsAsset` of type 'T' in the inspector window.

- `LoadOrCreateSettings<T>()`: Returns the `SettingsAsset` of type 'T', and creates one if it doesn't exist.

- `InspectOrCreateSettings<T>()`: Brings up the `SettingsAsset` of type 'T' in the inspector window, and creates one if it doesn't exist.

- `SaveCachedSettings()`: If you need to manually save all cached settings, use this.

## SettingsAsset
- `Reset()`: Resets the `SettingsAsset` to default values. This is also called if you reset from the inspector window.