using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[UsedImplicitly]
[ProjectCatalogReport]
public sealed class Project_PrefabStructure
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();

        // Mermaid グラフ出力開始
        log.AppendLine("# Prefab Variant Structure");

        // フォルダごとにプレハブのリストを保持する辞書
        var folderPrefabs = new Dictionary<string, List<string>>();

        // プロジェクト内のすべてのプレハブを検索
        var prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        foreach (var guid in prefabGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var folderPath = Path.GetDirectoryName(path);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            // プレハブリストに追加
            if (!folderPrefabs.ContainsKey(folderPath))
            {
                folderPrefabs[folderPath] = new List<string>();
            }

            folderPrefabs[folderPath]
                .Add(prefab.name);
        }

        // プレハブとバリアントの関係をsubgraphで追加
        foreach (var folder in folderPrefabs)
        {
            var folderName = folder.Key;

            // Markdown フォルダセクション
            log.AppendLine($"- **{folderName.Replace("_", "/")}**");

            foreach (var prefabName in folder.Value)
            {
                var prefabPath = $"{folder.Key}/{prefabName}.prefab";
                prefabPath = prefabPath.Replace('\\', '/');
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if (prefab == null)
                {
                    Debug.LogError($"skip : {prefabPath}");
                    continue;
                }

                // プレハブバリアントである場合、親プレハブを取得
                var prefabType = PrefabUtility.GetPrefabAssetType(prefab);
                if (prefabType == PrefabAssetType.Variant)
                {
                    var parentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
                    if (parentPrefab != null)
                    {
                        log.AppendLine($"  - {parentPrefab.name}");
                        log.AppendLine($"    - Variant: {prefabName}");
                    }
                }
                else
                {
                    // Markdown プレハブ構造を追加
                    log.AppendLine($"  - {prefabName}");
                }
            }
        }

        return log.ToString();
    }
}