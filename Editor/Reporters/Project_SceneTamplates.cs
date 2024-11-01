using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneTemplate;

[ProjectCatalogReport]
[UsedImplicitly]
public static class Project_SceneTamplates
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();
        log.AppendLine("# Scene Templates");

        // Scene Template フォルダをスキャン
        var sceneTemplateGuids = AssetDatabase.FindAssets("t:SceneTemplateAsset");
        if (sceneTemplateGuids.Length == 0)
        {
            log.AppendLine("No scene templates found in Assets/SceneTemplates.");
            return log.ToString();
        }

        // 各シーンテンプレートの情報を取得
        foreach (var guid in sceneTemplateGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var sceneTemplate = AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>(path);

            log.AppendLine($"- {sceneTemplate.name}");
            log.AppendLine($"  - {path}");

        /*
            // Scene のアセット情報取得
            var dependencies = AssetDatabase.GetDependencies(path, true);
            log.AppendLine("  - Dependencies:");
            foreach (var dependency in dependencies)
            {
                log.AppendLine($"    - {dependency}");
            }
        */

            log.AppendLine();
        }

        return log.ToString();
    }
}
