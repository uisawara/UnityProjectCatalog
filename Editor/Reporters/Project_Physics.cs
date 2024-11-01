using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

[ProjectCatalogReport]
[UsedImplicitly]
public static class Project_Physics
{
    [UsedImplicitly]
    public static string GenerateReport()
    {
        var log = new StringBuilder();
        log.AppendLine("# Physics Settings");

        // 重力
        log.AppendLine("\n## Gravity");
        log.AppendLine($"- Gravity: {Physics.gravity}");

        // 物理エンジンの一般設定
        log.AppendLine("\n## General Settings");
        log.AppendLine($"- Default Solver Iterations: {Physics.defaultSolverIterations}");
        log.AppendLine($"- Default Solver Velocity Iterations: {Physics.defaultSolverVelocityIterations}");
        log.AppendLine($"- Bounce Threshold: {Physics.bounceThreshold}");
        log.AppendLine($"- Sleep Threshold: {Physics.sleepThreshold}");
        log.AppendLine($"- Default Contact Offset: {Physics.defaultContactOffset}");
        log.AppendLine($"- Queries Hit Backfaces: {Physics.queriesHitBackfaces}");
        log.AppendLine($"- Queries Hit Triggers: {Physics.queriesHitTriggers}");

        // 接触ペア設定
        //log.AppendLine("\n## Contact Pair Settings");
        //log.AppendLine($"- Enable Enhanced Contact Pairs: {Physics.enhanceContacts}");

        // Time スケール設定
        log.AppendLine("\n## Time Scale Settings");
        log.AppendLine($"- Fixed Timestep: {Time.fixedDeltaTime}");
        log.AppendLine($"- Maximum Allowed Timestep: {Time.maximumDeltaTime}");

        // レイヤー衝突設定 (表形式)
        log.AppendLine("\n## Layer Collision Matrix");

        // 有効なレイヤーのみ取得
        var validLayers = new List<string>();
        for (int i = 0; i < 32; i++)
        {
            string layerName = LayerMask.LayerToName(i);
            if (!string.IsNullOrEmpty(layerName))
            {
                validLayers.Add(layerName);
            }
        }

        // ヘッダー行
        log.Append("| Layer |");
        foreach (var layer in validLayers)
        {
            log.Append($" {layer} |");
        }
        log.AppendLine();

        // 境界線行
        log.Append("|-");
        for (int i = 0; i < validLayers.Count; i++)
        {
            log.Append("|-");
        }
        log.AppendLine("|");

        // 各レイヤー行
        for (int i = 0; i < validLayers.Count; i++)
        {
            log.Append($"| {validLayers[i]} |");
            for (int j = 0; j < validLayers.Count; j++)
            {
                int layerA = LayerMask.NameToLayer(validLayers[i]);
                int layerB = LayerMask.NameToLayer(validLayers[j]);
                log.Append(Physics.GetIgnoreLayerCollision(layerA, layerB) ? " Ignored |" : " Collide |");
            }
            log.AppendLine();
        }

        return log.ToString();
    }
}
