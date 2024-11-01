#if UNITYPROJECTCATALOG_USE_ZENJECT
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Zenject;

[UsedImplicitly]
[ProjectCatalogReport]
internal static class Scripts_ZenjectContexts
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();

        // ProjectContext の情報を取得
        var projectContextPath = AssetDatabase.FindAssets("t:ProjectContext");
        if (projectContextPath.Length > 0)
        {
            var projectContext =
 AssetDatabase.LoadAssetAtPath<ProjectContext>(AssetDatabase.GUIDToAssetPath(projectContextPath[0]));
            if (projectContext != null)
            {
                log.AppendLine("# ProjectContext");
                AppendInstallerInfo(log, projectContext);
            }
        }

        // プロジェクト内の "Assets" フォルダに含まれるすべてのシーンを取得
        var scenePathes = AssetDatabase.FindAssets("t:Scene")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(path => path.StartsWith("Assets/")) // "Assets/" フォルダ内のシーンに限定
            .ToList();
        
        foreach (var scenePath in scenePathes)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            // シーン内の SceneContext オブジェクトを取得
            var sceneContextObjects = GameObject.FindObjectsOfType<SceneContext>();
            foreach (var sceneContext in sceneContextObjects)
            {
                var sceneName = sceneContext.gameObject.scene.name;
                log.AppendLine($"# {sceneName} SceneContext");
                AppendInstallerInfo(log, sceneContext);
            }

            // シーンをアンロードしてメモリを解放
            EditorSceneManager.CloseScene(scene, true);
        }

        return log.ToString();
    }

    private static void AppendInstallerInfo(StringBuilder log, Context context)
    {
        log.AppendLine("## Installers");
        var installers = context.Installers;
        foreach (var installer in installers)
        {
            if (installer == null) continue;
            log.AppendLine($"- {installer.GetType().Name}");
        }

        log.AppendLine("## ScriptableObject Installers");
        var scriptableInstallers = context.ScriptableObjectInstallers;
        foreach (var installer in scriptableInstallers)
        {
            if (installer == null) continue;
            log.AppendLine($"- {installer.GetType().Name}");
        }

        log.AppendLine("## MonoInstallers");
        var monoInstallers = context.GetComponents<MonoInstaller>();
        foreach (var installer in monoInstallers)
        {
            if (installer == null) continue;
            log.AppendLine($"- {installer.GetType().Name}");
        }

        log.AppendLine();
    }
}
#endif