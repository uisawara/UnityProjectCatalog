using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_ShadersAndMaterials
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = new StringBuilder();

        log.AppendLine($"# {scene.name}");

        // シェーダーの使用状況を追跡する辞書
        var shaderMaterialCount = new Dictionary<Shader, int>();

        // シーン内のすべてのRendererを取得
        var renderers = Object.FindObjectsOfType<Renderer>(true);
        foreach (var renderer in renderers)
        {
            // Rendererのマテリアルを取得
            var materials = renderer.sharedMaterials;
            foreach (var material in materials)
            {
                if (material != null)
                {
                    var shader = material.shader;
                    if (shader != null)
                    {
                        // シェーダーの個数をカウント
                        if (!shaderMaterialCount.ContainsKey(shader))
                        {
                            shaderMaterialCount[shader] = 0;
                        }

                        shaderMaterialCount[shader]++;
                    }
                }
            }
        }

        // シェーダーを名前順にソートしてログ出力
        foreach (var kvp in shaderMaterialCount.OrderBy(kvp => kvp.Key.name))
        {
            log.AppendLine($"{kvp.Value,5} : {kvp.Key.name}");
        }

        log.AppendLine(); // 改行を追加
        return log.ToString();
    }
}
