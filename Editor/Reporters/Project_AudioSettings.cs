using System;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Project_AudioSettings
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();

        // Audio設定の取得
        log.AppendLine("# Project Audio Settings");
        AudioSettings.GetDSPBufferSize(out var bufferLength, out var numBuffers);
        log.AppendLine($"- Buffer Length: {bufferLength}");
        log.AppendLine($"- Number of Buffers: {numBuffers}");
        log.AppendLine($"- Default Speaker Mode: {AudioSettings.speakerMode}");
        log.AppendLine($"- Sample Rate: {AudioSettings.outputSampleRate} Hz");

        // Ambisonic設定
        log.AppendLine("\n## Ambisonic Settings");
        var ambisonicPluginName = GetInternalPluginName("GetAmbisonicDecoderPluginName");
        var ambisonicDecoderEnabled = !string.IsNullOrEmpty(ambisonicPluginName);
        log.AppendLine($"- Ambisonic Decoder Enabled: {ambisonicDecoderEnabled}");
        log.AppendLine($"- Ambisonic Decoder Plugin: {(ambisonicDecoderEnabled ? ambisonicPluginName : "None")}");

        // Spatializer設定
        log.AppendLine("\n## Spatializer Settings");
        var spatializerPluginName = GetInternalPluginName("GetSpatializerPluginName");
        var spatializerEnabled = !string.IsNullOrEmpty(spatializerPluginName);
        log.AppendLine($"- Spatializer Enabled: {spatializerEnabled}");
        log.AppendLine($"- Spatializer Plugin: {(spatializerEnabled ? spatializerPluginName : "None")}");

        // AudioManagerの設定を取得
        var audioManager =
            new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/AudioManager.asset")[0]);
        log.AppendLine($"- Volume: {audioManager.FindProperty("m_Volume").floatValue}");
        log.AppendLine($"- Rolloff Scale: {audioManager.FindProperty("Rolloff Scale").floatValue}");
        log.AppendLine($"- Doppler Factor: {audioManager.FindProperty("m_DopplerFactor")?.floatValue ?? 0}");
        log.AppendLine($"- Default Max Distance: {audioManager.FindProperty("m_DefaultMaxDistance")?.floatValue ?? 0}");
        log.AppendLine($"- Default Min Distance: {audioManager.FindProperty("m_DefaultMinDistance")?.floatValue ?? 0}");
        log.AppendLine($"- Virtualize Effects: {audioManager.FindProperty("m_VirtualizeEffects")?.boolValue ?? false}");
        log.AppendLine($"- Disable Unity Audio: {audioManager.FindProperty("m_DisableAudio")?.boolValue ?? false}");

        // Voice Limit の設定
        log.AppendLine($"- Voice Limit: {audioManager.FindProperty("m_DefaultDSPBufferSize")?.intValue ?? 0}");
        log.AppendLine($"- Virtual Voice Count: {audioManager.FindProperty("m_VirtualVoiceCount")?.intValue ?? 0}");
        log.AppendLine($"- Real Voice Count: {audioManager.FindProperty("m_RealVoiceCount")?.intValue ?? 0}");

        return log.ToString();
    }

    private static string GetInternalPluginName(string methodName)
    {
        try
        {
            var audioSettingsType = typeof(AudioSettings);
            var method = audioSettingsType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (method != null)
            {
                return method.Invoke(null, null) as string;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error accessing {methodName}: {e.Message}");
        }

        return null;
    }
}