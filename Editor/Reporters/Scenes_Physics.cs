using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
public static class Scenes_Physics
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = new StringBuilder();
        log.AppendLine($"# {scene.name}");

        // シーン内のすべての Rigidbody 情報
        log.AppendLine("\n## Rigidbodies");
        var rigidbodies = Object.FindObjectsOfType<Rigidbody>();
        if (rigidbodies.Length == 0)
        {
            log.AppendLine("No Rigidbodies found in the scene.");
        }
        else
        {
            foreach (var rb in rigidbodies)
            {
                log.AppendLine($"- {rb.gameObject.name}");
                log.AppendLine($"  - Mass: {rb.mass}");
                log.AppendLine($"  - Drag: {rb.drag}");
                log.AppendLine($"  - Angular Drag: {rb.angularDrag}");
                log.AppendLine($"  - Use Gravity: {rb.useGravity}");
                log.AppendLine($"  - Is Kinematic: {rb.isKinematic}");
                log.AppendLine($"  - Interpolation: {rb.interpolation}");
                log.AppendLine($"  - Collision Detection: {rb.collisionDetectionMode}");
            }
        }

        // シーン内のすべての Collider 情報
        log.AppendLine("\n## Colliders");
        var colliders = Object.FindObjectsOfType<Collider>();
        if (colliders.Length == 0)
        {
            log.AppendLine("No Colliders found in the scene.");
        }
        else
        {
            foreach (var col in colliders)
            {
                log.AppendLine($"- {col.gameObject.name}");
                log.AppendLine($"  - Collider Type: {col.GetType().Name}");
                log.AppendLine($"  - Is Trigger: {col.isTrigger}");
                if (col.sharedMaterial != null)
                {
                    log.AppendLine($"  - Physics Material: {col.sharedMaterial.name}");
                    log.AppendLine($"    - Bounciness: {col.sharedMaterial.bounciness}");
                    log.AppendLine(
                        $"    - Friction: {col.sharedMaterial.dynamicFriction} (Dynamic), {col.sharedMaterial.staticFriction} (Static)");
                    log.AppendLine($"    - Friction Combine: {col.sharedMaterial.frictionCombine}");
                    log.AppendLine($"    - Bounce Combine: {col.sharedMaterial.bounceCombine}");
                }
                else
                {
                    log.AppendLine("  - Physics Material: None");
                }
            }
        }

        return log.ToString();
    }
}