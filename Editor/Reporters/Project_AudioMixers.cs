using System.Text;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine.Audio;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Project_AudioMixers
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();
        log.AppendLine("# Project Audio Mixers");

        // プロジェクト内のすべての Audio Mixer を検索
        var mixerGuids = AssetDatabase.FindAssets("t:AudioMixer");
        var mixerPaths = mixerGuids.Select(AssetDatabase.GUIDToAssetPath).ToList();

        if (mixerPaths.Count == 0)
        {
            log.AppendLine("No Audio Mixers found in the project.");
            return log.ToString();
        }

        // 各 Audio Mixer の詳細情報を取得
        foreach (var path in mixerPaths)
        {
            var mixer = AssetDatabase.LoadAssetAtPath<AudioMixer>(path);
            if (mixer == null) continue;

            log.AppendLine($"\n## Audio Mixer: {mixer.name}");
            log.AppendLine($"- Path: {path}");

            // 各グループの設定を取得
            var groups = mixer.FindMatchingGroups(string.Empty);
            if (groups.Length == 0)
            {
                log.AppendLine("  No groups found in this mixer.");
            }
            else
            {
                log.AppendLine("  ### Groups");
                foreach (var group in groups)
                {
                    log.AppendLine($"  - Group: {group.name}");
                    
                    // ボリュームとピッチの初期値の取得
                    float volume, pitch;
                    group.audioMixer.GetFloat("Volume", out volume);
                    group.audioMixer.GetFloat("Pitch", out pitch);
                    log.AppendLine($"    - Volume: {volume}");
                    log.AppendLine($"    - Pitch: {pitch}");
                }
            }
        }

        return log.ToString();
    }
}