using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Project_ShadersAndMaterials
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();

        // プロジェクト内のすべてのマテリアルを検索
        var materialGuids = AssetDatabase.FindAssets("t:Material");
        var allMaterials = new List<Material>();

        foreach (var guid in materialGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null)
            {
                allMaterials.Add(material);
            }
        }

        // MaterialをShaderごとにグループ化し、Shader名順にソート
        var materialsGroupedByShader = allMaterials
            .GroupBy(material => material.shader)
            .OrderBy(shaderGroup => shaderGroup.Key.name);

        // Shaderごとにログ出力
        foreach (var shaderGroup in materialsGroupedByShader)
        {
            var shader = shaderGroup.Key;
            log.AppendLine($"# {shader.name} ({shaderGroup.Count()})");

            foreach (var material in shaderGroup)
            {
                log.Append($"- Material: {material.name}");
                log.Append($" Render Queue: {material.renderQueue}");
                log.Append($" Path: {AssetDatabase.GetAssetPath(material)}");
                log.AppendLine();
            }

            log.AppendLine("\n");
        }

        return log.ToString();
    }
}