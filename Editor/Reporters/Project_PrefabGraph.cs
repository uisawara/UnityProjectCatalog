using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[UsedImplicitly]
[ProjectCatalogReport]
public static class Project_PrefabGraph
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();
        log.AppendLine("# Prefab Variant Relationships");
        log.AppendLine("```mermaid");
        log.AppendLine("graph LR");

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
            var folderName = SanitizeName(folder.Key);
            //log.AppendLine($"    subgraph {folderName}");

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

                // プレハブ名を有効な形式に変換
                var sanitizedPrefabName = SanitizeName(prefabName);

                // プレハブバリアントである場合、親プレハブを取得
                var prefabType = PrefabUtility.GetPrefabAssetType(prefab);
                if (prefabType == PrefabAssetType.Variant)
                {
                    var parentPrefab = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
                    if (parentPrefab != null)
                    {
                        var parentName = SanitizeName(parentPrefab.name);
                        log.AppendLine($"        {parentName} --> {sanitizedPrefabName}");
                    }
                }
                else
                {
                    log.AppendLine($"        {sanitizedPrefabName}");
                }
            }
            //log.AppendLine("    end");
        }

        log.AppendLine("```");
        return log.ToString();
    }

    // Mermaid 形式で使用できない文字を '_' に置き換える
    private static string SanitizeName(string name)
    {
        return Regex.Replace(name, @"[^a-zA-Z0-9_]", "_");
    }
}