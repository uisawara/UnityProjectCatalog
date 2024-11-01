#if UNITYPROJECTCATALOG_USE_POSTPROCESSING
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.Rendering.PostProcessing;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_PostProcessVolumes
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = "";
        var currentScenePath = SceneManager.GetActiveScene().path;

        // プロジェクト内の "Assets" フォルダに含まれるすべてのシーンを取得
        var scenePathes = AssetDatabase.FindAssets("t:Scene")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(path => path.StartsWith("Assets/")) // "Assets/" フォルダ内のシーンに限定
            .ToList();
        
        foreach (var scenePath in scenePathes)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            
            //
            log += $"# {scene.name}\n";
            //
            var volumes = GameObject.FindObjectsOfType<PostProcessVolume>(true);
            foreach (var volume in volumes)
            {
                log += GetIndentedLog(volume.transform) + $"- PostProcessVolume: {volume.name}\n";
                log += GetIndentedLog(volume.transform, 1) + $"  Is Global: {volume.isGlobal}\n";
                log += GetIndentedLog(volume.transform, 1) + $"  Priority: {volume.priority}\n";
                log += GetIndentedLog(volume.transform, 1) + $"  Blend Distance: {volume.blendDistance}\n";
                //
                if (volume.profile != null)
                    foreach (var effect in volume.profile.settings)
                    {
                        log += GetIndentedLog(volume.transform, 2) + $"  - {effect.GetType().Name} Settings:\n";
                        foreach (var prop in effect.GetType().GetProperties())
                            log += GetIndentedLog(volume.transform, 3) + $"    {prop.Name}: {prop.GetValue(effect)}\n";
                    }

                log += "\n";
            }
        }

        // 元のシーンに戻す
        EditorSceneManager.OpenScene(currentScenePath);
        return log;
    }
}
#endif