using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[ProjectCatalogReport]
internal sealed class Project_AssetAudioSources
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();
        log.AppendLine("# Project Audio Mixers and Asset Audio Sources");

        // Assets フォルダ内のプレハブに含まれる AudioSource 情報を取得
        log.AppendLine("\n## Prefab Audio Sources in Assets");
        var prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        foreach (var guid in prefabGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var audioSources = prefab.GetComponentsInChildren<AudioSource>(true);

            if (audioSources.Length > 0)
            {
                log.AppendLine($"### Prefab: {prefab.name}");
                log.AppendLine($"- Path: {path}");

                foreach (var source in audioSources)
                {
                    log.AppendLine($"  - AudioSource on GameObject: {source.gameObject.name}");
                    log.AppendLine($"    - Clip: {(source.clip ? source.clip.name : "None")}");
                    log.AppendLine($"    - Volume: {source.volume}");
                    log.AppendLine($"    - Pitch: {source.pitch}");
                    log.AppendLine($"    - Spatial Blend: {source.spatialBlend}");
                    log.AppendLine($"    - Loop: {source.loop}");
                }
            }
        }

        return log.ToString();
    }
}