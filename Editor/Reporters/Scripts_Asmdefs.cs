using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Scripts_Asmdefs
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        // 設定を読み込み。設定ファイルが存在しない場合、除外リストを空に設定
        var settings =
            AssetDatabase.LoadAssetAtPath<UnityProjectCatalogSettings>(
                "Assets/Settings/UnityProjectCatalogSettings.asset");
        var excludedAssemblies = settings ? settings.excludedAssemblies : new List<string>();

        var asmdefPaths = Directory.GetFiles(Application.dataPath, "*.asmdef", SearchOption.AllDirectories);
        var asmdefInfos = new List<AsmdefInfo>();

        foreach (var asmdefPath in asmdefPaths)
        {
            var json = File.ReadAllText(asmdefPath);
            var asmdefInfo = JsonUtility.FromJson<AsmdefInfo>(json);
            var queryPath = asmdefPath.Replace(Application.dataPath, "Assets/");
            asmdefInfo.guid = AssetDatabase.AssetPathToGUID(queryPath); // GUIDを取得して格納
            asmdefInfo.filePath = asmdefPath;

            // 除外リストにないアセンブリのみ追加
            if (!excludedAssemblies.Contains(asmdefInfo.name))
            {
                asmdefInfos.Add(asmdefInfo);
            }
        }

        var mermaidGraph = "graph LR\n";

        foreach (var asmdef in asmdefInfos)
        {
            if (asmdef.references != null && asmdef.references.Length > 0)
            {
                foreach (var referenceGUID in asmdef.references)
                {
                    var referenceName = GUIDToAsmdefName(referenceGUID);

                    // 参照先も除外リストに含まれていないことを確認
                    if (!string.IsNullOrEmpty(referenceName) && !excludedAssemblies.Contains(referenceName))
                    {
                        // Mermaid形式で依存関係を記述
                        mermaidGraph += $"    {GetAsmdefName(asmdef)} --> {referenceName}\n";
                    }
                }
            }
            else
            {
                mermaidGraph += $"    {GetAsmdefName(asmdef)}\n";
            }
        }

        return "# Assembly Dependency\n"
               + "```mermaid\n"
               + mermaidGraph
               + "```\n";
    }

    // GUIDを名前に変換するメソッド
    private static string GUIDToAsmdefName(string guid)
    {
        // GUIDからアセットのパスを取得
        guid = guid.Replace("GUID:", "");
        var asmdefPath = AssetDatabase.GUIDToAssetPath(guid);
        if (!string.IsNullOrEmpty(asmdefPath))
            // パスからasmdefのファイル名を取得（拡張子は除く）
        {
            return Path.GetFileNameWithoutExtension(asmdefPath);
        }

        return null;
    }

    // asmdef名を取得するメソッド
    private static string GetAsmdefName(AsmdefInfo asmdef)
    {
        return Path.GetFileNameWithoutExtension(asmdef.filePath);
    }
}