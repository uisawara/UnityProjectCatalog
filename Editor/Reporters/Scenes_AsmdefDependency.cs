using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_AsmdefDependency
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = new StringBuilder();

        var sceneName = scene.name.Replace(" ", "_"); // スペースをアンダースコアに置き換え

        // シーン内のすべてのオブジェクトの依存しているスクリプトを取得
        var sceneObjects = scene.GetRootGameObjects();
        var asmdefDependencies = new HashSet<string>();

        foreach (var obj in sceneObjects)
        {
            var components = obj.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var component in components)
            {
                if (component == null)
                {
                    continue;
                }

                // スクリプトのアセンブリを取得
                var script = MonoScript.FromMonoBehaviour(component);
                var assemblyName = script.GetClass()
                    ?.Assembly?.GetName()
                    .Name;

                if (!string.IsNullOrEmpty(assemblyName))
                {
                    // Assembly名からAsmdef名を取得し、依存関係リストに追加
                    var asmdefName = AssemblyToAsmdefName(assemblyName);
                    if (!string.IsNullOrEmpty(asmdefName))
                    {
                        asmdefDependencies.Add(asmdefName);
                    }
                }
            }
        }

        // Distinct にした依存関係をソートし、Markdown形式でログに追加
        var sortedAsmdefDependencies = asmdefDependencies.OrderBy(name => name);
        log.AppendLine($"# {sceneName}");
        foreach (var asmdef in sortedAsmdefDependencies)
        {
            log.AppendLine($"- {asmdef}");
        }

        log.AppendLine();

        return log.ToString();
    }

    // アセンブリ名から対応するAsmdef名を取得するヘルパーメソッド
    private static string AssemblyToAsmdefName(string assemblyName)
    {
        var asmdefPaths = Directory.GetFiles(Application.dataPath, "*.asmdef", SearchOption.AllDirectories);
        foreach (var asmdefPath in asmdefPaths)
        {
            var json = File.ReadAllText(asmdefPath);
            var asmdefInfo = JsonUtility.FromJson<AsmdefInfo>(json);
            if (asmdefInfo.name == assemblyName)
            {
                return asmdefInfo.name;
            }
        }

        return null;
    }
}