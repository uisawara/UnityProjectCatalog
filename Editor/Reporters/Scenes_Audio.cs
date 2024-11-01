using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_Audio
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = new StringBuilder();
        log.AppendLine($"# {scene.name} - Scene Report");

        // 2. AudioListener の情報を取得
        log.AppendLine("\n## Audio Listeners");

        var listeners = Object.FindObjectsOfType<AudioListener>(true);
        if (listeners.Length == 0)
        {
            log.AppendLine("No AudioListeners found.");
        }
        else
        {
            foreach (var listener in listeners)
            {
                log.AppendLine($"- {listener.gameObject.name}");
            }
        }

        // 3. AudioSource の情報を取得
        log.AppendLine("\n## Audio Sources");

        var audioSources = Object.FindObjectsOfType<AudioSource>(true);
        if (audioSources.Length == 0)
        {
            log.AppendLine("No AudioSources found.");
        }
        else
        {
            foreach (var source in audioSources)
            {
                log.AppendLine($"- {source.gameObject.name}");
                log.AppendLine($"  - Output: {source.outputAudioMixerGroup?.name ?? "-"}");
                log.AppendLine($"  - playOnAwake: {source.playOnAwake}");
                log.AppendLine($"  - mute: {source.mute}");
                log.AppendLine($"  - bypassEffects: {source.bypassEffects}");
                log.AppendLine($"  - bypassListenerEffects: {source.bypassListenerEffects}");
                log.AppendLine($"  - bypassReverbZones: {source.bypassReverbZones}");
                log.AppendLine($"  - Clip: {(source.clip ? source.clip.name : "None")}");
                log.AppendLine($"  - Volume: {source.volume}");
                log.AppendLine($"  - Pitch: {source.pitch}");
                log.AppendLine($"  - Spatial Blend: {source.spatialBlend}");
                log.AppendLine($"  - Loop: {source.loop}");
            }
        }

        log.AppendLine(); // 改行を追加
        return log.ToString();
    }
}