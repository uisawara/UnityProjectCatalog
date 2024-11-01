using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Project_AssetPrefabs
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();

        // プロジェクト内のすべてのPrefabを検索
        var prefabGuids = AssetDatabase.FindAssets("t:Prefab");

        // GUIDからアセットパスに変換して、ソート
        var prefabPaths = prefabGuids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .OrderBy(path => path)
            .ToList();

        // 前回出力されたフォルダ階層を記憶するためのリスト
        var previousPathParts = new List<string>();

        // 各Prefabのパスをフォルダ階層ごとにMarkdown形式で出力
        foreach (var path in prefabPaths)
        {
            // フォルダパスとPrefab名を分離
            var pathParts = path.Split('/')
                .Skip(1)
                .ToArray(); // "Assets" をスキップ
            var prefabName = pathParts.Last();

            // 同じフォルダ階層で既に出力されている部分はスキップ
            var commonIndex = 0;
            while (commonIndex < previousPathParts.Count && commonIndex < pathParts.Length - 1
                                                         && previousPathParts[commonIndex] == pathParts[commonIndex])
            {
                commonIndex++;
            }

            // 新しいフォルダ階層を出力
            for (var i = commonIndex; i < pathParts.Length - 1; i++)
            {
                if (i == 0)
                {
                    // 最上位階層を見出しとして出力
                    log.AppendLine($"# {pathParts[i]}");
                }
                else
                {
                    log.AppendLine($"{new string(' ', i * 2)}- {pathParts[i]}");
                }
            }

            // Prefab名を出力
            log.AppendLine($"{new string(' ', (pathParts.Length - 1) * 2)}- Prefab: {prefabName}");

            // 現在のパスを記憶
            previousPathParts = pathParts.Take(pathParts.Length - 1)
                .ToList();
        }

        return log.ToString();
    }
}