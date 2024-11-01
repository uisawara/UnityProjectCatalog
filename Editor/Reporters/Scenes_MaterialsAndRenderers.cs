using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_MaterialsAndRenderers
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = new StringBuilder();

        log.AppendLine($"# {scene.name}");

        // すべてのRendererコンポーネントを取得
        var renderers = Object.FindObjectsOfType<Renderer>(true);

        // 使用されているMaterialごとにログをまとめる
        var materialsGroupedByRenderer = renderers
            .SelectMany(renderer => renderer.sharedMaterials
                .Where(material => material != null)
                .Select(material => new { Material = material, Renderer = renderer }))
            .GroupBy(item => item.Material);

        // Materialごとにログ出力
        foreach (var materialGroup in materialsGroupedByRenderer)
        {
            var material = materialGroup.Key;
            log.AppendLine($"- Material: {material.name}");
            log.AppendLine($"\t- Shader: {material.shader.name}");
            log.AppendLine($"\t- Render Queue: {material.renderQueue}");

            // このMaterialを使っているRendererをログ出力
            foreach (var item in materialGroup)
            {
                var renderer = item.Renderer;
                log.Append($"\t\t- Renderer: {renderer.name} (GameObject: {renderer.gameObject.name})");
                log.Append($" Enabled: {renderer.enabled}");
                log.Append($" Sorting Layer: {renderer.sortingLayerName} ({renderer.sortingLayerID})");
                log.Append($" Sorting Order: {renderer.sortingOrder}");
                log.Append($" Renderer Type: {renderer.GetType().Name}");
                log.AppendLine();
            }

            log.AppendLine("\n");
        }

        return log.ToString();
    }
}