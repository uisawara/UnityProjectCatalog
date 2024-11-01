using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[SceneCatalogReport]
[UsedImplicitly]
internal sealed class Scenes_Components
{
    [UsedImplicitly]
    public static string GenerateReport(Scene scene)
    {
        var log = new StringBuilder();

        log.AppendLine($"# {scene.name}");

        // シーン内のすべてのGameObjectを取得
        var allGameObjects = scene.GetRootGameObjects();

        // 各GameObjectにアタッチされているコンポーネントを列挙
        foreach (var go in allGameObjects)
        {
            var components = go.GetComponentsInChildren<Component>(true);
            log.AppendLine($"- {go.name}");
            var filteredComponents = components
                .Where(c => c != null)
                .Select(c => c.GetType()
                    .Name)
                .Distinct()
                .OrderBy(c => c);
            foreach (var component in filteredComponents)
            {
                log.AppendLine($"  - {component}");
            }
        }

        log.AppendLine("\n");

        return log.ToString();
    }
}