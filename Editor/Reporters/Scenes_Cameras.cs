using System.Collections.Generic;
using System.Linq;
using Domains.mmzkworks.unity.workflow.UnityProjectCatalog.Editor.Miscs;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_Cameras
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = "";

        log += $"# {scene.name}\n";

        var cameras = Object.FindObjectsOfType<Camera>(true)
            .ToList()
            .OrderBy(camera => camera.depth);
        foreach (var camera in cameras)
        {
            log += IndentUtil.GetIndentedLog(camera.transform) + $"- {camera.name}\n";
            //log += IndentUtil.GetIndentedLog(camera.transform, 1) + $"- ```\n";
            log += IndentUtil.GetIndentedLog(camera.transform, 1) + $"  Depth: {camera.depth}\n";
            log += IndentUtil.GetIndentedLog(camera.transform, 1) + $"  Clear Flags: {camera.clearFlags}\n";
            log += IndentUtil.GetIndentedLog(camera.transform, 1) +
                   $"  Background Color: {camera.backgroundColor}\n";
            log += IndentUtil.GetIndentedLog(camera.transform, 1) +
                   $"  Culling Mask: {GetCullingMaskString(camera.cullingMask)}\n";
            log += IndentUtil.GetIndentedLog(camera.transform, 1) + $"  Field of View: {camera.fieldOfView}\n";
            log += "\n";
        }

        return log;
    }

    private static string GetCullingMaskString(int cullingMask)
    {
        var layerNames = new List<string>();
        for (var i = 0; i < 32; i++)
        {
            if ((cullingMask & (1 << i)) != 0)
            {
                layerNames.Add(LayerMask.LayerToName(i));
            }
        }

        return string.Join(", ", layerNames);
    }
}