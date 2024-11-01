using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Scripts_Modules
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        return LogGitSubmodules()
               + LogNuGetPackages()
               + LogUnityPackages();
    }

    private static string LogGitSubmodules()
    {
        var log = new StringBuilder();
        var gitModulesPath = FindGitModules(Application.dataPath, ".gitmodules");
        if (gitModulesPath == null)
        {
            return string.Empty;
        }
        if (File.Exists(gitModulesPath))
        {
            log.AppendLine("# Git Submodules");

            var lines = File.ReadAllLines(gitModulesPath);
            var path = string.Empty;
            var url = string.Empty;

            foreach (var line in lines)
            {
                // 正規表現でpathとurlの行を取得
                var pathMatch = Regex.Match(line, @"\s*path\s*=\s*(.+)");
                var urlMatch = Regex.Match(line, @"\s*url\s*=\s*(.+)");

                if (pathMatch.Success)
                {
                    path = pathMatch.Groups[1]
                        .Value;
                }
                else if (urlMatch.Success)
                {
                    url = urlMatch.Groups[1]
                        .Value;
                }

                // pathとurlが揃ったら出力
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(url))
                {
                    log.AppendLine($"- {path} <-- {url}");
                    path = string.Empty;
                    url = string.Empty;
                }
            }

            log.AppendLine();
            return log.ToString();
        }

        Debug.LogWarning(".gitmodules file not found.");

        return string.Empty;
    }

    private static string FindGitModules(string startPath, string fileName)
    {
        string currentPath = startPath;

        while (true)
        {
            string gitDirectoryPath = Path.Combine(currentPath, ".git");
            if (Directory.Exists(gitDirectoryPath))
            {
                string gitModulesPath = Path.Combine(currentPath, fileName);
                if (File.Exists(gitModulesPath))
                {
                    return gitModulesPath;
                }
                else
                {
                    return null;
                }
            }

            if (Path.GetPathRoot(currentPath) == currentPath)
            {
                break;
            }

            currentPath = Path.GetFullPath(Path.Combine(currentPath, ".."));
        }

        return null;
    }
    
    private static string LogNuGetPackages()
    {
        var log = new StringBuilder();

        var packagesConfigPath = Path.Combine(Application.dataPath, "packages.config");
        if (File.Exists(packagesConfigPath))
        {
            log.AppendLine("# NuGet Packages");

            var xml = XDocument.Load(packagesConfigPath);
            foreach (var package in xml.Descendants("package"))
            {
                var id = package.Attribute("id")
                    ?.Value;
                var version = package.Attribute("version")
                    ?.Value;

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(version))
                {
                    log.AppendLine($"- {id}: {version}");
                }
            }

            log.AppendLine();
            return log.ToString();
        }

        Debug.LogWarning("packages.config file not found in Assets folder.");

        return string.Empty;
    }

    private static string LogUnityPackages()
    {
        var log = new StringBuilder();
        var manifestPath = Path.Combine(Directory.GetCurrentDirectory(), "Packages", "manifest.json");
        if (File.Exists(manifestPath))
        {
            var manifestContent = File.ReadAllText(manifestPath);
            var manifestJson = JObject.Parse(manifestContent);

            log.AppendLine("# Unity Packages");

            if (manifestJson.ContainsKey("dependencies"))
            {
                var dependencies = (JObject)manifestJson["dependencies"];
                foreach (var dependency in dependencies)
                {
                    log.AppendLine($"- {dependency.Key}: {dependency.Value}");
                }

                log.AppendLine();
                return log.ToString();
            }

            Debug.LogWarning("No dependencies found in manifest.json.");
        }
        else
        {
            Debug.LogWarning("manifest.json file not found.");
        }

        return string.Empty;
    }
}