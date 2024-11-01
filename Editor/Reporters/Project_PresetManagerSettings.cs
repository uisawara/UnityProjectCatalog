using System.Text;
using JetBrains.Annotations;
using UnityEditor;

[ProjectCatalogReport]
[UsedImplicitly]
public static class Project_PresetManagerSettings
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();
        log.AppendLine("# Preset Manager Settings");

        // PresetManager の設定を取得
        var presetManager =
            new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/PresetManager.asset")[0]);
        var defaultPresetsArray = presetManager.FindProperty("m_DefaultPresets");

        if (defaultPresetsArray == null || defaultPresetsArray.arraySize == 0)
        {
            log.AppendLine("No default presets found in the Preset Manager.");
            return log.ToString();
        }

        // 各プリセットの情報を取得
        for (var i = 0; i < defaultPresetsArray.arraySize; i++)
        {
            var presetProperty = defaultPresetsArray.GetArrayElementAtIndex(i);

            // ターゲットタイプの情報を取得
            var nativeTypeID = presetProperty.FindPropertyRelative("first.m_NativeTypeID")
                .intValue;
            var managedTypeGUID = presetProperty.FindPropertyRelative("first.m_ManagedTypePPtr.guid")
                ?.stringValue;
            var managedTypeFallback = presetProperty.FindPropertyRelative("first.m_ManagedTypeFallback")
                .stringValue;

            log.AppendLine($"\n## Preset {i + 1}");
            log.AppendLine($"- Native Type ID: {nativeTypeID}");
            log.AppendLine($"- Managed Type GUID: {managedTypeGUID}");
            log.AppendLine(
                $"- Managed Type Fallback: {(string.IsNullOrEmpty(managedTypeFallback) ? "None" : managedTypeFallback)}");

            // 各プリセットエントリの詳細を取得
            var presetsArray = presetProperty.FindPropertyRelative("second");
            log.AppendLine("  - Presets:");
            if (presetsArray != null && presetsArray.arraySize > 0)
            {
                for (var j = 0; j < presetsArray.arraySize; j++)
                {
                    var presetEntry = presetsArray.GetArrayElementAtIndex(j);
                    var presetGUID = presetEntry.FindPropertyRelative("m_Preset.guid")
                        ?.stringValue;
                    var filter = presetEntry.FindPropertyRelative("m_Filter")
                        .stringValue;
                    var isDisabled = presetEntry.FindPropertyRelative("m_Disabled")
                        .intValue == 1;

                    log.AppendLine($"    - Preset GUID: {presetGUID}");
                    log.AppendLine($"      - Filter: {filter}");
                    log.AppendLine($"      - Disabled: {isDisabled}");
                }
            }
            else
            {
                log.AppendLine("    None");
            }
        }

        return log.ToString();
    }
}