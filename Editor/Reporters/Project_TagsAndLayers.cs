using JetBrains.Annotations;
using UnityEditorInternal;
using UnityEngine;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Project_TagsAndLayers
{
    public static string GenerateReport()
    {
        var log = "";

        log += "# Tags\n";
        foreach (var tag in InternalEditorUtility.tags)
        {
            log += $"- Tag: {tag}\n";
        }

        log += "\n";

        log += "# Sorting Layers\n";
        var sortingLayers = SortingLayer.layers;
        foreach (var sortingLayer in sortingLayers)
        {
            log += $"- {sortingLayer.name}, ID: {sortingLayer.id}\n";
        }

        log += "\n";

        log += "# Layers\n";
        for (var i = 0; i < 32; i++)
        {
            var layerName = InternalEditorUtility.GetLayerName(i);
            if (!string.IsNullOrEmpty(layerName))
            {
                log += $"- Layer {i}: {layerName}\n";
            }
        }

        log += "\n";

        return log;
    }
}