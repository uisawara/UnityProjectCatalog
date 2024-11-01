using System.Linq;
using JetBrains.Annotations;
using UnityEditor;

[ProjectCatalogReport]
[UsedImplicitly]
internal sealed class Scripts_ScriptExecutionOrder
{
    public static string GenerateReport()
    {
        var log = "# Script Execution Order\n";
        var monoScripts = MonoImporter.GetAllRuntimeMonoScripts();
        var monoScriptWithOrder =
            monoScripts.Select(v => (v.name, MonoImporter.GetExecutionOrder(v)))
                .OrderBy(v2 => v2.Item2);
        foreach (var monoScript in monoScriptWithOrder)
        {
            if (monoScript.Item2 != 0)
            {
                log += $"- {monoScript.name}: {monoScript.Item2}\n";
            }
        }

        log += "\n";

        return log;
    }
}