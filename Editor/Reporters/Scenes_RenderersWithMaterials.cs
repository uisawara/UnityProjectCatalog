using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_RenderersWithMaterials
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = new StringBuilder();

        log.AppendLine($"# {scene.name}");

        // すべてのRendererコンポーネントを取得して、ログに出力
        var renderers = Object.FindObjectsOfType<Renderer>(true);
        foreach (var renderer in renderers)
        {
            log.AppendLine($"- {renderer.name} (GameObject: {renderer.gameObject.name})");
            log.AppendLine($"\tSorting Layer: {renderer.sortingLayerName} ({renderer.sortingLayerID})");
            log.AppendLine($"\tSorting Order: {renderer.sortingOrder}");
            log.AppendLine($"\tRenderer Type: {renderer.GetType().Name}");
            log.AppendLine($"\tEnabled: {renderer.enabled}");

            // Rendererに関連するマテリアルを列挙
            var materials = renderer.sharedMaterials;
            for (var j = 0; j < materials.Length; j++)
            {
                var material = materials[j];
                if (material != null)
                {
                    log.AppendLine($"\t\t- Material {j}: {material.name}");
                    log.AppendLine($"\t\t\t- Shader: {material.shader.name}");
                    log.AppendLine($"\t\t\t- Render Queue: {material.renderQueue}");
                }
            }

            log.AppendLine("\n");
        }

        return log.ToString();
    }
}