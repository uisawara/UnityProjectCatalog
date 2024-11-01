using JetBrains.Annotations;
using UnityEngine;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Project_QualitySettings
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = "# QualitySettings\n";
        log += $"- Active Quality Level: {QualitySettings.names[QualitySettings.GetQualityLevel()]}\n";
        log += $"- Pixel Light Count: {QualitySettings.pixelLightCount}\n";
        log += $"- Shadow Quality: {QualitySettings.shadows}\n";
        log += $"- Shadow Resolution: {QualitySettings.shadowResolution}\n";
        log += $"- Shadow Projection: {QualitySettings.shadowProjection}\n";
        log += $"- Shadow Distance: {QualitySettings.shadowDistance}\n";
        log += $"- Shadow Cascades: {QualitySettings.shadowCascades}\n";
        log += $"- LOD Bias: {QualitySettings.lodBias}\n";
        log += $"- Anisotropic Filtering: {QualitySettings.anisotropicFiltering}\n";
        log += $"- Anti Aliasing: {QualitySettings.antiAliasing}\n";
        log += $"- Soft Particles: {QualitySettings.softParticles}\n";
        log += $"- VSync Count: {QualitySettings.vSyncCount}\n";
        log += $"- Max Queued Frames: {QualitySettings.maxQueuedFrames}\n";
        log += $"- Realtime Reflection Probes: {QualitySettings.realtimeReflectionProbes}\n";
        log += $"- Billboards Face Camera Position: {QualitySettings.billboardsFaceCameraPosition}\n";
        log += $"- Resolution Scaling Fixed DPI Factor: {QualitySettings.resolutionScalingFixedDPIFactor}\n";
        log += "\n";
        return log;
    }
}