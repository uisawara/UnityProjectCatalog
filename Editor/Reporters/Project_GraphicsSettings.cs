using JetBrains.Annotations;
using UnityEngine.Rendering;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Project_GraphicsSettings
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = "# GraphicsSettings\n";
        log +=
            $"- Render Pipeline: {(GraphicsSettings.renderPipelineAsset != null ? GraphicsSettings.renderPipelineAsset.name : "None")}\n";
        log += $"- Lights Use Linear Intensity: {GraphicsSettings.lightsUseLinearIntensity}\n";
        log += $"- Lights Use Color Temperature: {GraphicsSettings.lightsUseColorTemperature}\n";
        log += $"- Use Scriptable Render Pipeline Batchers: {GraphicsSettings.useScriptableRenderPipelineBatching}\n";
        log += "\n";
        return log;
    }
}