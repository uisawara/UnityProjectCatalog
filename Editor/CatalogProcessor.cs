using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CatalogProcessor
{
    [MenuItem("Tools/Generate Project Catalog")]
    public static void GenerateAllReports()
    {
        Debug.Log("Begin generating project catalog");

        // レポートの保存先を設定
        var reportPath = Path.Combine(Application.dataPath, "..", "Catalog");
        Directory.CreateDirectory(reportPath);

        ReportBasicCatalogReports(reportPath);
        ReportSceneCatalogReports(reportPath);

        var reportUri = new Uri(reportPath).AbsoluteUri;
        Debug.Log($"Generated report: <a href=\"{reportUri}\">{reportPath}</a>");

        Debug.Log("Finished generating project catalog");
    }

    private static void ReportBasicCatalogReports(string reportPath)
    {
        // 同じアセンブリ内のクラスを取得し、CatalogReport属性がついたクラスをフィルタ
        var reportClasses = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<ProjectCatalogReportAttribute>() != null);

        foreach (var reportClass in reportClasses)
        {
            // GenerateReportメソッドを探して実行
            var generateMethod = reportClass.GetMethod("GenerateReport", BindingFlags.Static | BindingFlags.Public);
            if (generateMethod != null)
            {
                var reportContent = generateMethod.Invoke(null, null) as string;
                var fileName = $"{reportClass.Name}.md";
                var filePath = Path.Combine(reportPath, fileName);
                File.WriteAllText(filePath, reportContent);
                Debug.Log($"Generated report: {fileName}");
            }
            else
            {
                Debug.LogWarning($"{reportClass.Name} is missing a static GenerateReport method.");
            }
        }
    }

    private static void ReportSceneCatalogReports(string reportPath)
    {
        // 同じアセンブリ内のクラスを取得し、CatalogReport属性がついたクラスをフィルタ
        var reportClasses = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<SceneCatalogReportAttribute>() != null);

        // プロジェクト内の "Assets" フォルダに含まれるすべてのシーンを取得
        var scenePathes = AssetDatabase.FindAssets("t:Scene")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(path => path.StartsWith("Assets/")) // "Assets/" フォルダ内のシーンに限定
            .ToList();

        var logMap = new Dictionary<string, StringBuilder>();
        foreach (var reportClass in reportClasses)
        {
            logMap[reportClass.FullName] = new StringBuilder();
        }

        var currentScenePath = SceneManager.GetActiveScene()
            .path;
        foreach (var scenePath in scenePathes)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            var sceneName = scene.name.Replace(" ", "_"); // スペースをアンダースコアに置き換え

            foreach (var reportClass in reportClasses)
            {
                var log = logMap[reportClass.FullName];

                // GenerateReportメソッドを探して実行
                var generateMethod = reportClass.GetMethod("GenerateReport", BindingFlags.Static | BindingFlags.Public);
                if (generateMethod != null)
                {
                    var reportContent = generateMethod.Invoke(null, new object[] { scene }) as string;
                    log.Append(reportContent);
                }
                else
                {
                    Debug.LogWarning($"{reportClass.Name} is missing a static GenerateReport method.");
                }
            }
        }

        // 元のシーンに戻す
        EditorSceneManager.OpenScene(currentScenePath);

        foreach (var reportClass in reportClasses)
        {
            var fileName = $"{reportClass.Name}.md";
            var filePath = Path.Combine(reportPath, fileName);
            File.WriteAllText(filePath, logMap[reportClass.FullName]
                .ToString());
            Debug.Log($"Generated report: {fileName}");
        }
    }
}