using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_LightingSettings
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = new StringBuilder();

        log.AppendLine($"# {scene.name}");

        // 環境設定の取得
        var lightingSettings = LightmapSettings.lightmaps;
        var environmentLighting = RenderSettings.ambientLight;
        var skyboxMaterial = RenderSettings.skybox;

        // 環境の設定をログ出力
        log.AppendLine($"- Environment Lighting: {environmentLighting}");
        log.AppendLine($"- Skybox: {(skyboxMaterial != null ? skyboxMaterial.name : "None")}");

        log.AppendLine($"- Light Probes: {(LightmapSettings.lightProbes != null ? "Enabled" : "Disabled")}");
        log.AppendLine($"- Mixed Lighting: {LightmapSettings.lightmapsMode}\n");

        log.AppendLine();

        return log.ToString();
    }
}