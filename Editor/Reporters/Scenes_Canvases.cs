using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_Canvases
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = "";

        log += $"# {scene.name}\n";

        // Canvasの設定をログ出力
        var canvases = Object.FindObjectsOfType<Canvas>(true)
            .ToList()
            .OrderBy(canvas => canvas.sortingOrder);
        foreach (var canvas in canvases)
        {
            log += IndentUtil.GetIndentedLog(canvas.transform) + $"- {canvas.name}\n";
            //log += GetIndentedLog(canvas.transform, 1) + $"- ```\n";
            log += IndentUtil.GetIndentedLog(canvas.transform, 1) + $"  Sorting Order: {canvas.sortingOrder}\n";
            log += IndentUtil.GetIndentedLog(canvas.transform, 1) + $"  Render Mode: {canvas.renderMode}\n";
            log += IndentUtil.GetIndentedLog(canvas.transform, 1) + $"  Pixel Perfect: {canvas.pixelPerfect}\n";
            log += IndentUtil.GetIndentedLog(canvas.transform, 1) + $"  Target Display: {canvas.targetDisplay}\n";
            log += "\n";
        }

        return log;
    }
}